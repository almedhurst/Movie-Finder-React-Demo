using Ardalis.Specification;
using MovieFinder.Core.Dtos;
using MovieFinder.Core.Entities;
using MovieFinder.Core.Repositories;
using MovieFinder.Infrastructure.Services;

namespace MovieFinder.Infrastructure.Tests.Services;

public class CategoryServiceTest
{
    [Fact]
    public async void CategoryService_WhenCalled_GetDataFromRepo()
    {
        //Arrange
        var repo = Substitute.For<IGenericRepository<Category>>();
        var subject = Substitute.For<CategoryService>(repo);
        
        //Act
        await subject.GetCategories();

        //Assert
        await repo.Received().ListAsync(Arg.Any<ISpecification<Category>>());
    }
    
    [Fact]
    public async void CategoryService_WhenCalled_ReturnTypeOfEnumableNameDto()
    {
        //Arrange
        var repo = Substitute.For<IGenericRepository<Category>>();
        var subject = Substitute.For<CategoryService>(repo);
        
        //Act
        var result = await subject.GetCategories();

        //Assert
        result.ShouldBeAssignableTo<IEnumerable<NameDto>>();
    }
    
    [Fact]
    public async void CategoryService_WhenCalled_ShouldNotReturnEmpty()
    {
        //Arrange
        var repo = Substitute.For<IGenericRepository<Category>>();
        repo.ListAsync(Arg.Any<ISpecification<Category>>()).Returns(new List<Category>
        {
            new () {Id = "Category1", Name = "Category One"},
            new () {Id = "Category2", Name = "Category Two"},
            new () {Id = "Category3", Name = "Category Three"},
        });
        var subject = Substitute.For<CategoryService>(repo);
        
        //Act
        var result = await subject.GetCategories();

        //Assert
        result.ShouldNotBeEmpty();
        result.Count().ShouldBe(3);
    }
}