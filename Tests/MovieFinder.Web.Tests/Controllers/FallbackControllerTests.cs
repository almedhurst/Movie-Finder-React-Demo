using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using MovieFinder.Web.Controllers;

namespace MovieFinder.Web.Tests.Controllers;

public class FallbackControllerTests
{
    [Fact]
    public void Index_WhenCalled_ShouldReturnPhysicalFileResult()
    {
        //Arrange
        var subject = new FallbackController();
        
        //Act
        var result = subject.Index();
        
        //Assert
        result.ShouldBeOfType<PhysicalFileResult>();
        ((PhysicalFileResult)result).FileName.ShouldEndWith("index.html");
        ((PhysicalFileResult)result).ContentType.ShouldBe(MediaTypeNames.Text.Html);
    }
}