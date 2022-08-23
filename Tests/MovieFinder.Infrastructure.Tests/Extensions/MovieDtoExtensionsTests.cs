using MovieFinder.Core.Dtos;
using MovieFinder.Core.Entities;
using MovieFinder.Infrastructure.Extensions;

namespace MovieFinder.Infrastructure.Tests.Extensions;

public class MovieDtoExtensionsTests
{
    [Fact]
    public void ToMovieDToList_WhenNullObjectProvided_ReturnTypeOfEnumerableMovieDto()
    {
        //Arrange
        List<Title> data = new List<Title>();
        
        //Act
        var result = data.ToMovieDtoList();
        
        //Assert
        result.ShouldBeAssignableTo<IEnumerable<MovieDto>>();
    }
    
    [Fact]
    public void ToMovieDToList_WhenValidObjectProvided_ReturnTypeOfEnumerableMovieDto()
    {
        //Arrange
        var data = TitleListData;
        
        //Act
        var result = data.ToMovieDtoList();
        
        //Assert
        result.ShouldBeAssignableTo<IEnumerable<MovieDto>>();
    }

    [Fact]
    public void ToMovieDToList_WhenValidObjectProvided_DataReturnMatches()
    {
        //Arrange
        var data = TitleListData;
        
        //Act
        var result = data.ToMovieDtoList();
        
        //Assert
        result.Count().ShouldBe(data.Count);
        foreach (var item in data)
        {
            result.ShouldContain(x =>
                x.Id == item.Id &&
                x.Name == item.Name &&
                x.Year == item.Year &&
                x.RunTime == item.Runtime &&
                x.ReleaseDate == item.ReleaseDate &&
                x.StoryLine == item.StoryLine);

            var resultItem = result.FirstOrDefault(x => x.Id == item.Id);
            resultItem.ShouldNotBeNull();
            if (resultItem != null)
            {
                resultItem.Categories.Count().ShouldBe(item.TitleCategories.Count);
                foreach (var categoryItem in item.TitleCategories)
                {
                    resultItem.Categories.ShouldContain(x =>
                        x.Id == categoryItem.Category.Id &&
                        x.Name == categoryItem.Category.Name);
                }
                
                resultItem.Directors.Count().ShouldBe(item.TitleDirectors.Count);
                foreach (var directorItem in item.TitleDirectors)
                {
                    resultItem.Directors.ShouldContain(x =>
                        x.Id == directorItem.Director.Id &&
                        x.Name == directorItem.Director.Name);
                }
                
                resultItem.Writers.Count().ShouldBe(item.TitleWriters.Count);
                foreach (var writerItem in item.TitleWriters)
                {
                    resultItem.Writers.ShouldContain(x =>
                        x.Id == writerItem.Writer.Id &&
                        x.Name == writerItem.Writer.Name);
                }
                
                resultItem.Actors.Count().ShouldBe(item.TitleActors.Count);
                foreach (var actorItem in item.TitleActors)
                {
                    resultItem.Actors.ShouldContain(x =>
                        x.Id == actorItem.Actor.Id &&
                        x.Name == actorItem.Actor.Name);
                }
            }

        }
    }
    
    
    [Fact]
    public void ToMovieDto_WhenNullObjectProvided_ReturnTypeOfNull()
    {
        //Arrange
        Title data = null;
        
        //Act
        var result = data.ToMovieDto();
        
        //Assert
        result.ShouldBeNull();
    }

    [Fact]
    public void ToMovieDto_WhenValidObjectProvided_ReturnTypeOfMovieDto()
    {
        //Arrange
        var data = TitleData;
        
        //Act
        var result = data.ToMovieDto();
        
        //Arrange
        result.ShouldBeAssignableTo<MovieDto>();
    }

    [Fact]
    public void ToMovieDto_WhenValidObjectProvided_DataReturnMatches()
    {
        //Arrange
        var data = TitleData;
        
        //Act
        var result = data.ToMovieDto();
        
        //Arrange

        result.Id.ShouldBe(data.Id);
        result.Name.ShouldBe(data.Name);
        result.Year.ShouldBe(data.Year);
        result.RunTime.ShouldBe(data.Runtime);
        result.ReleaseDate.ShouldBe(data.ReleaseDate);
        result.StoryLine.ShouldBe(data.StoryLine);
        
        result.Categories.Count().ShouldBe(data.TitleCategories.Count);
        foreach (var categoryItem in data.TitleCategories)
        {
            result.Categories.ShouldContain(x =>
                x.Id == categoryItem.Category.Id &&
                x.Name == categoryItem.Category.Name);
        }
            
        result.Directors.Count().ShouldBe(data.TitleDirectors.Count);
        foreach (var directorItem in data.TitleDirectors)
        {
            result.Directors.ShouldContain(x =>
                x.Id == directorItem.Director.Id &&
                x.Name == directorItem.Director.Name);
        }
            
        result.Writers.Count().ShouldBe(data.TitleWriters.Count);
        foreach (var writerItem in data.TitleWriters)
        {
            result.Writers.ShouldContain(x =>
                x.Id == writerItem.Writer.Id &&
                x.Name == writerItem.Writer.Name);
        }
            
        result.Actors.Count().ShouldBe(data.TitleActors.Count);
        foreach (var actorItem in data.TitleActors)
        {
            result.Actors.ShouldContain(x =>
                x.Id == actorItem.Actor.Id &&
                x.Name == actorItem.Actor.Name);
        }
    }
    
    private List<Title> TitleListData => new List<Title>
    {
        new Title
        {
            Id = "Title1",
            Name = "Movie One",
            ReleaseDate = new DateTime(2022, 04, 12),
            Year = 2022,
            Runtime = 120,
            StoryLine =
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Praesent eget dignissim nunc, vitae porttitor erat. Morbi dictum magna vel dui sagittis porttitor. In hac habitasse platea dictumst. Etiam pretium porttitor massa in pulvinar.",
            TitleActors = new List<TitleActor>
            {
                new TitleActor {ActorId = "Actor1", Actor = new Actor {Id = "Actor1", Name = "Actor One"}},
                new TitleActor {ActorId = "Actor2", Actor = new Actor {Id = "Actor2", Name = "Actor Two"}},
                new TitleActor {ActorId = "Actor3", Actor = new Actor {Id = "Actor3", Name = "Actor Three"}},
                new TitleActor {ActorId = "Actor4", Actor = new Actor {Id = "Actor4", Name = "Actor Four"}}
            },
            TitleDirectors = new List<TitleDirector>
            {
                new TitleDirector
                    {DirectorId = "Director1", Director = new Director {Id = "Director1", Name = "Director One"}},
                new TitleDirector
                    {DirectorId = "Director2", Director = new Director {Id = "Director2", Name = "Director Two"}}
            },
            TitleWriters = new List<TitleWriter>
            {
                new TitleWriter {WriterId = "Writer1", Writer = new Writer {Id = "Writer1", Name = "Writer One"}},
                new TitleWriter {WriterId = "Writer2", Writer = new Writer {Id = "Writer2", Name = "Writer Two"}}
            },
            TitleCategories = new List<TitleCategory>
            {
                new TitleCategory
                    {CategoryId = "Category1", Category = new Category {Id = "Category1", Name = "Category One"}},
                new TitleCategory
                    {CategoryId = "Category3", Category = new Category {Id = "Category3", Name = "Category Three"}},
                new TitleCategory
                    {CategoryId = "Category2", Category = new Category {Id = "Category2", Name = "Category Two"}},
                new TitleCategory
                    {CategoryId = "Category4", Category = new Category {Id = "Category4", Name = "Category Four"}}
            }
        },
        new Title
        {
            Id = "Title2",
            Name = "Movie Two",
            ReleaseDate = new DateTime(2021, 04, 12),
            Year = 2021,
            Runtime = 100,
            StoryLine =
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Praesent eget dignissim nunc, vitae porttitor erat. Morbi dictum magna vel dui sagittis porttitor. In hac habitasse platea dictumst. Etiam pretium porttitor massa in pulvinar.",
            TitleActors = new List<TitleActor>
            {
                new TitleActor {ActorId = "Actor3", Actor = new Actor {Id = "Actor3", Name = "Actor Three"}},
                new TitleActor {ActorId = "Actor4", Actor = new Actor {Id = "Actor4", Name = "Actor Four"}}
            },
            TitleDirectors = new List<TitleDirector>
            {
                new TitleDirector
                    {DirectorId = "Director2", Director = new Director {Id = "Director2", Name = "Director Two"}}
            },
            TitleWriters = new List<TitleWriter>
            {
                new TitleWriter {WriterId = "Director1", Writer = new Writer {Id = "Writer1", Name = "Writer One"}}
            },
            TitleCategories = new List<TitleCategory>
            {
                new TitleCategory
                    {CategoryId = "Category2", Category = new Category {Id = "Category2", Name = "Category Two"}},
                new TitleCategory
                    {CategoryId = "Category4", Category = new Category {Id = "Category4", Name = "Category Four"}}
            }
        },
        new Title
        {
            Id = "Title3",
            Name = "Movie Three",
            ReleaseDate = new DateTime(2020, 04, 12),
            Year = 2020,
            Runtime = 180,
            StoryLine =
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Praesent eget dignissim nunc, vitae porttitor erat. Morbi dictum magna vel dui sagittis porttitor. In hac habitasse platea dictumst. Etiam pretium porttitor massa in pulvinar.",
            TitleActors = new List<TitleActor>
            {
                new TitleActor {ActorId = "Actor1", Actor = new Actor {Id = "Actor1", Name = "Actor One"}},
                new TitleActor {ActorId = "Actor4", Actor = new Actor {Id = "Actor4", Name = "Actor Four"}}
            },
            TitleDirectors = new List<TitleDirector>
            {
                new TitleDirector
                    {DirectorId = "Director2", Director = new Director {Id = "Director2", Name = "Director Two"}}
            },
            TitleWriters = new List<TitleWriter>
            {
                new TitleWriter {WriterId = "Director1", Writer = new Writer {Id = "Writer1", Name = "Writer One"}},
                new TitleWriter {WriterId = "Director2", Writer = new Writer {Id = "Writer2", Name = "Writer Two"}}
            },
            TitleCategories = new List<TitleCategory>
            {
                new TitleCategory
                    {CategoryId = "Category2", Category = new Category {Id = "Category2", Name = "Category Two"}},
                new TitleCategory
                    {CategoryId = "Category4", Category = new Category {Id = "Category4", Name = "Category Four"}}
            }
        }
    };

    private Title TitleData => new Title
    {
        Id = "Title1",
        Name = "Movie One",
        ReleaseDate = new DateTime(2022, 04, 12),
        Year = 2022,
        Runtime = 120,
        StoryLine =
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Praesent eget dignissim nunc, vitae porttitor erat. Morbi dictum magna vel dui sagittis porttitor. In hac habitasse platea dictumst. Etiam pretium porttitor massa in pulvinar.",
        TitleActors = new List<TitleActor>
        {
            new TitleActor {ActorId = "Actor1", Actor = new Actor {Id = "Actor1", Name = "Actor One"}},
            new TitleActor {ActorId = "Actor2", Actor = new Actor {Id = "Actor2", Name = "Actor Two"}},
            new TitleActor {ActorId = "Actor3", Actor = new Actor {Id = "Actor3", Name = "Actor Three"}},
            new TitleActor {ActorId = "Actor4", Actor = new Actor {Id = "Actor4", Name = "Actor Four"}}
        },
        TitleDirectors = new List<TitleDirector>
        {
            new TitleDirector
                {DirectorId = "Director1", Director = new Director {Id = "Director1", Name = "Director One"}},
            new TitleDirector
                {DirectorId = "Director2", Director = new Director {Id = "Director2", Name = "Director Two"}}
        },
        TitleWriters = new List<TitleWriter>
        {
            new TitleWriter {WriterId = "Writer1", Writer = new Writer {Id = "Writer1", Name = "Writer One"}},
            new TitleWriter {WriterId = "Writer2", Writer = new Writer {Id = "Writer2", Name = "Writer Two"}}
        },
        TitleCategories = new List<TitleCategory>
        {
            new TitleCategory
                {CategoryId = "Category1", Category = new Category {Id = "Category1", Name = "Category One"}},
            new TitleCategory
                {CategoryId = "Category3", Category = new Category {Id = "Category3", Name = "Category Three"}},
            new TitleCategory
                {CategoryId = "Category2", Category = new Category {Id = "Category2", Name = "Category Two"}},
            new TitleCategory
                {CategoryId = "Category4", Category = new Category {Id = "Category4", Name = "Category Four"}}
        }
    };
}