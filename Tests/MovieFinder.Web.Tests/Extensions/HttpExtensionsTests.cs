using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MovieFinder.Core.Dtos;
using MovieFinder.Web.Extensions;

namespace MovieFinder.Web.Tests.Extensions;

public class HttpExtensionsTests
{
    [Fact]
    public void AddPaginationHeader_AddedPaginationMetaDtoToResponseHeader()
    {
        //Arrange
        var subject = Substitute.For<HttpResponse>();
        var paginationData = new PaginationMetaDto(1, 10, 40);
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        var expectedHeaderString = JsonSerializer.Serialize(paginationData, options);
        
        //Act
        subject.AddPaginationHeader(paginationData);
        
        //Assert
        subject.Received().Headers.Add("Pagination", expectedHeaderString);
        subject.Received().Headers.Add("Access-Control-Expose-Headers", "Pagination");

    }
}