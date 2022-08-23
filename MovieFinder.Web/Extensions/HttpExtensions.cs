using System.Text.Json;
using MovieFinder.Core.Dtos;

namespace MovieFinder.Web.Extensions;

public static class HttpExtensions
{
    public static void AddPaginationHeader(this HttpResponse response, PaginationMetaDto paginationData)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var jsonString = JsonSerializer.Serialize(paginationData, options);
        
        response.Headers.Add("Pagination", jsonString);
        response.Headers.Add("Access-Control-Expose-Headers","Pagination");
    }
}