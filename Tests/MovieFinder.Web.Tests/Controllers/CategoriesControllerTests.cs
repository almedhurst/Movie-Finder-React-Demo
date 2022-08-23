using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieFinder.Core.Dtos;
using MovieFinder.Core.Services;
using MovieFinder.Web.Controllers;

namespace MovieFinder.Web.Tests.Controllers;

public class CategoriesControllerTests
{
    [Fact]
    public async void GetCategories_WhenCalled_CategoryServiceGetCategoriesIsCalled()
    {
        //Arrange
        var mockCategoryService = Substitute.For<ICategoryService>();
        mockCategoryService.GetCategories().Returns(new List<NameDto>());
        var subject = new CategoriesController(mockCategoryService);
        
        //Act
        await subject.GetCategories();
        
        //Assert
        await mockCategoryService.Received().GetCategories();

    }
    [Fact]
    public async void GetCategories_WhenCalled_ReturnsOkReturn()
    {
        //Arrange
        var mockCategoryService = Substitute.For<ICategoryService>();
        mockCategoryService.GetCategories().Returns(new List<NameDto>());
        var subject = new CategoriesController(mockCategoryService);
        
        //Act
        var result = await subject.GetCategories();
        
        //Assert
        result.Result.ShouldBeOfType<OkObjectResult>();

    }
}