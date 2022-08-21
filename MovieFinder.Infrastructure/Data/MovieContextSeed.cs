using System.Data;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using MovieFinder.Core.Entities;
using MovieFinder.Core.Helpers;

namespace MovieFinder.Infrastructure.Data;

public sealed class MovieContextSeed : IDisposable
{
    private readonly MovieContext _context;
    private readonly ILoggerFactory _loggerFactory;

    private List<Actor> _actors = new();
    private List<Category> _categories = new();
    private List<Director> _directors = new();
    private List<Title> _titles = new();
    private List<Writer> _writers = new();

    public MovieContextSeed(MovieContext context, ILoggerFactory loggerFactory)
    {
        _loggerFactory = loggerFactory;
        _context = context;
    }

    public void StartSeed()
    {
        var logger = _loggerFactory.CreateLogger<MovieContextSeed>();
        try
        {
            if (_context.Actors.Any() || _context.Categories.Any() || _context.Directors.Any() ||
                _context.Writers.Any() || _context.Titles.Any())
            {
                logger.LogInformation("Seeding skipped.");
            }
            else
            {
                logger.LogInformation("Seeding started.");
                var assembly = Assembly.GetExecutingAssembly();
                var resources = assembly.GetManifestResourceNames().Where(x => x.EndsWith(".json"));


                foreach (var item in resources)
                {
                    string? fileData = null;
                    using (Stream stream = assembly.GetManifestResourceStream(item))
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        fileData = reader.ReadToEnd();
                    }

                    logger.LogInformation($"Seeding file {item}");
                    if (!string.IsNullOrWhiteSpace(fileData))
                    {
                        var data = JsonSerializer.Deserialize<ResourceJson>(fileData);

                        var titleData = new Title();
                        titleData.Name = data.Name;
                        titleData.Year = data.Year;
                        titleData.Runtime = data.Runtime;
                        titleData.ReleaseDate = FormatDate(data.ReleaseDate);
                        titleData.StoryLine = data.StoryLine;
                        titleData.TitleCategories = ProcessCategories(data.Categories);
                        titleData.TitleDirectors = ProcessDirectors(data.Director);
                        titleData.TitleWriters = ProcessWriters(data.Writer);
                        titleData.TitleActors = ProcessActors(data.Actors);
                        
                        _titles.Add(titleData);
                    }
                }

                CommitData();

                logger.LogInformation("Seeding completed.");
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
        }
    }

    private void CommitData()
    {
        _context.Actors.AddRange(_actors);
        _context.Categories.AddRange(_categories);
        _context.Directors.AddRange(_directors);
        _context.Writers.AddRange(_writers);
        _context.Titles.AddRange(_titles);
        _context.SaveChanges();
    }

    private DateTime FormatDate(string input)
    {
        var dateParts = input.Split("-");
        return new DateTime(Int32.Parse(dateParts[0]), Int32.Parse(dateParts[1]), Int32.Parse(dateParts[2]));
    }

    private List<TitleCategory> ProcessCategories(object categories)
    {
        List<TitleCategory> returnList = new List<TitleCategory>();
        
        var valueKind = ((JsonElement) categories).ValueKind;
        var value = ((JsonElement) categories).ToString();
        
        switch (valueKind)
        {
            case JsonValueKind.String:
                var formattedValue = value.Trim().toTitleCase();
                if (!_categories.Any(x => x.Name == formattedValue))
                {
                    var newCategory = new Category();
                    newCategory.Name = formattedValue;
                    _categories.Add(newCategory);

                    var newTitleCategory = new TitleCategory();
                    newTitleCategory.CategoryId = newCategory.Id;
                    newTitleCategory.Category = newCategory;
                    returnList.Add(newTitleCategory);
                }
                else
                {
                    var categoryData = _categories.FirstOrDefault(x => x.Name == formattedValue);
                    var newTitleCategory = new TitleCategory();
                    newTitleCategory.CategoryId = categoryData.Id;
                    newTitleCategory.Category = categoryData;
                    returnList.Add(newTitleCategory);
                }
                break;
            case JsonValueKind.Array:
                var arrayData = JsonSerializer.Deserialize<string[]>(value);
                foreach (var category in arrayData)
                {
                    var formattedValue2 = category.Trim().toTitleCase();
                    if (!_categories.Any(x => x.Name == formattedValue2))
                    {
                        var newCategory = new Category();
                        newCategory.Name = formattedValue2;
                        _categories.Add(newCategory);

                        var newTitleCategory = new TitleCategory();
                        newTitleCategory.CategoryId = newCategory.Id;
                        newTitleCategory.Category = newCategory;
                        returnList.Add(newTitleCategory);
                    }
                    else
                    {
                        var categoryData = _categories.FirstOrDefault(x => x.Name == formattedValue2);
                        var newTitleCategory = new TitleCategory();
                        newTitleCategory.CategoryId = categoryData.Id;
                        newTitleCategory.Category = categoryData;
                        returnList.Add(newTitleCategory);
                    }
                }
                break;
        }

        return returnList;
    }
    
    private object GetPropValue(object src, string propName)
    {
        return src.GetType().GetProperty(propName).GetValue(src, null);
    }

    private List<TitleDirector> ProcessDirectors(object director)
    {
        List<TitleDirector> returnList = new List<TitleDirector>();
        
        var valueKind = ((JsonElement) director).ValueKind;
        var value = ((JsonElement) director).ToString();

        switch (valueKind)
        {
            case JsonValueKind.String:
                var formattedValue = value.Trim().toTitleCase();
                if (!_directors.Any(x => x.Name == formattedValue))
                {
                    var newDirector = new Director();
                    newDirector.Name = formattedValue;
                    _directors.Add(newDirector);
                    var newTitleDirector = new TitleDirector();
                    newTitleDirector.DirectorId = newDirector.Id;
                    newTitleDirector.Director = newDirector;
                    returnList.Add(newTitleDirector);
                }
                else
                {
                    var directorData = _directors.FirstOrDefault(x => x.Name == formattedValue);
                    var newTitleDirector = new TitleDirector();
                    newTitleDirector.DirectorId = directorData.Id;
                    newTitleDirector.Director = directorData;
                    returnList.Add(newTitleDirector);
                }
                break;
            case JsonValueKind.Array:
                var arrayData = JsonSerializer.Deserialize<string[]>(value);
                foreach (var data in arrayData)
                {
                    var formattedValue2 = data.Trim().toTitleCase();
                    if (!_directors.Any(x => x.Name == formattedValue2))
                    {
                        var newDirector = new Director();
                        newDirector.Name = data;
                        _directors.Add(newDirector);
                        var newTitleDirector = new TitleDirector();
                        newTitleDirector.DirectorId = newDirector.Id;
                        newTitleDirector.Director = newDirector;
                        returnList.Add(newTitleDirector);
                    }
                    else
                    {
                        var directorData = _directors.FirstOrDefault(x => x.Name == formattedValue2);
                        var newTitleDirector = new TitleDirector();
                        newTitleDirector.DirectorId = directorData.Id;
                        newTitleDirector.Director = directorData;
                        returnList.Add(newTitleDirector);
                    }
                }
                break;
        }
        
        return returnList;
    }

    public void Dispose()
    {
        _context.Dispose();
        _loggerFactory.Dispose();
        _actors = new();
        _categories = new();
        _directors = new();
        _titles = new();
        _writers = new();
    }

    private List<TitleWriter> ProcessWriters(object writers)
    {
        List<TitleWriter> returnList = new List<TitleWriter>();
        
        var valueKind = ((JsonElement) writers).ValueKind;
        var value = ((JsonElement) writers).ToString();

        switch (valueKind)
        {
            case JsonValueKind.String:
                var formattedValue = value.Trim().toTitleCase();
                if (!_writers.Any(x => x.Name == formattedValue))
                {
                    var newWriter = new Writer();
                    newWriter.Name = formattedValue;
                    _writers.Add(newWriter);

                    var newTitleWriter = new TitleWriter();
                    newTitleWriter.WriterId = newWriter.Id;
                    newTitleWriter.Writer = newWriter;
                    returnList.Add(newTitleWriter);
                }
                else
                {
                    var writerData = _writers.FirstOrDefault(x => x.Name == formattedValue);
                    var newTitleWriter = new TitleWriter();
                    newTitleWriter.WriterId = writerData.Id;
                    newTitleWriter.Writer = writerData;
                    returnList.Add(newTitleWriter);
                }
                break;
            case JsonValueKind.Array:
                var arrayData = JsonSerializer.Deserialize<string[]>(value);
                foreach (var writer in arrayData)
                {
                    var formattedValue2 = writer.Trim().toTitleCase();
                    if (!_writers.Any(x => x.Name == formattedValue2))
                    {
                        var newWriter = new Writer();
                        newWriter.Name = formattedValue2;
                        _writers.Add(newWriter);

                        var newTitleWriter = new TitleWriter();
                        newTitleWriter.WriterId = newWriter.Id;
                        newTitleWriter.Writer = newWriter;
                        returnList.Add(newTitleWriter);
                    }
                    else
                    {
                        var writerData = _writers.FirstOrDefault(x => x.Name == formattedValue2);
                        var newTitleWriter = new TitleWriter();
                        newTitleWriter.WriterId = writerData.Id;
                        newTitleWriter.Writer = writerData;
                        returnList.Add(newTitleWriter);
                    }
                }
                break;
        }
        
        

        return returnList;
    }

    private List<TitleActor> ProcessActors(object actors)
    {
        List<TitleActor> returnList = new List<TitleActor>();
        
        var valueKind = ((JsonElement) actors).ValueKind;
        var value = ((JsonElement) actors).ToString();

        switch (valueKind)
        {
            case JsonValueKind.String:
                var formattedValue = value.Trim().toTitleCase();
                if (!_actors.Any(x => x.Name == formattedValue))
                {
                    var newActor = new Actor();
                    newActor.Name = value;
                    _actors.Add(newActor);

                    var newTitleActor = new TitleActor();
                    newTitleActor.ActorId = newActor.Id;
                    newTitleActor.Actor = newActor;
                    returnList.Add(newTitleActor);
                }
                else
                {
                    var actorData = _actors.FirstOrDefault(x => x.Name == formattedValue);
                    var newTitleActor = new TitleActor();
                    newTitleActor.ActorId = actorData.Id;
                    newTitleActor.Actor = actorData;
                    returnList.Add(newTitleActor);
                }
                break;
            case JsonValueKind.Array:
                var arrayData = JsonSerializer.Deserialize<string[]>(value);
                foreach (var actor in arrayData)
                {
                    var formattedValue2 = actor.Trim().toTitleCase();
                    if (!_actors.Any(x => x.Name == formattedValue2))
                    {
                        var newActor = new Actor();
                        newActor.Name = formattedValue2;
                        _actors.Add(newActor);

                        var newTitleActor = new TitleActor();
                        newTitleActor.ActorId = newActor.Id;
                        newTitleActor.Actor = newActor;
                        returnList.Add(newTitleActor);
                    }
                    else
                    {
                        var actorData = _actors.FirstOrDefault(x => x.Name == formattedValue2);
                        var newTitleActor = new TitleActor();
                        newTitleActor.ActorId = actorData.Id;
                        newTitleActor.Actor = actorData;
                        returnList.Add(newTitleActor);
                    }
                }
                break;
        }
        
        

        return returnList;
    }


    public class ResourceJson
    {
        [JsonPropertyName("name")] public string Name { get; set; }

        [JsonPropertyName("year")] public int Year { get; set; }

        [JsonPropertyName("runtime")] public int Runtime { get; set; }

        [JsonPropertyName("categories")] public object Categories { get; set; }

        [JsonPropertyName("release-date")] public string ReleaseDate { get; set; }

        [JsonPropertyName("director")] public object Director { get; set; }

        [JsonPropertyName("writer")] public object Writer { get; set; }

        [JsonPropertyName("actors")] public object Actors { get; set; }

        [JsonPropertyName("storyline")] public string StoryLine { get; set; }
    }
}