using MovieFinder.Core.Dtos;
using MovieFinder.Core.Entities;
using MovieFinder.Infrastructure.Extensions;

namespace MovieFinder.Infrastructure.Tests.Extensions;

public class CategoryMovieDtoExtensionsTests
{
    [Fact]
    public void ToCategoryMovieDtoList_WhenNullObjectProvided_ReturnTypeOfEnumerableCategoryMovieDto()
    {
        //Arrange
        List<Category> data = null;

        //Act
        var result = data.ToCategoryMovieDtoList();
        
        //Assert
        result.ShouldBeAssignableTo<IEnumerable<CategoryMovieDto>>();
    }
    
    [Fact]
    public void ToCategoryMovieDtoList_WhenValidObjectProvided_ReturnTypeOfEnumerableCategoryMovieDto()
    {
        //Arrange
        var data = new List<Category>();

        //Act
        var result = data.ToCategoryMovieDtoList();
        
        //Assert
        result.ShouldBeAssignableTo<IEnumerable<CategoryMovieDto>>();
    }
    
    [Fact]
    public void ToCategoryMovieDtoList_WhenValidObjectProvided_DataReturnedMatches()
    {
        //Arrange
        var data = new List<Category>
        {
            new Category() { Id = "Category1", Name = "Category One"},
            new Category() { Id = "Category2", Name = "Category Two"},
            new Category() { Id = "Category3", Name = "Category Thre"},
        };

        //Act
        var result = data.ToCategoryMovieDtoList();
        
        //Assert
        result.Count().ShouldBe(data.Count);
        foreach (var item in data)
        {
            result.ShouldContain(x => x.Id == item.Id && x.Name == item.Name);
        }
    }

}