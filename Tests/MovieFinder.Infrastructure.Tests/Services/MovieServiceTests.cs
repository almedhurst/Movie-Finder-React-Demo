

using Ardalis.Specification;
using MovieFinder.Core.Dtos;
using MovieFinder.Core.Entities;
using MovieFinder.Core.Repositories;
using MovieFinder.Core.RequestHelpers;
using MovieFinder.Infrastructure.Services;
using NSubstitute.Core;

namespace MovieFinder.Infrastructure.Tests.Services;

public class MovieServiceTests
{
    [Fact]
    public async void GetMovies_WhenCalled_GetDataFromRepo()
    {
        //Arrange
        var mockTitleRepo = Substitute.For<IGenericRepository<Title>>();
        var mockCategoryRepo = Substitute.For<IGenericRepository<Category>>();
        var subject = Substitute.For<MovieService>(mockTitleRepo, mockCategoryRepo);

        //Act
        await subject.GetMovies(new MovieParams());

        //Asset
        await mockTitleRepo.Received().ListAsync(Arg.Any<ISpecification<Title>>());
        await mockTitleRepo.Received().CountAsync(Arg.Any<ISpecification<Title>>());
    }

    [Fact]
    public async void GetMovies_WhenCalled_ReturnTypeOfPaginatedDtoMovieDto()
    {
        //Arrange
        var mockTitleRepo = Substitute.For<IGenericRepository<Title>>();
        var mockCategoryRepo = Substitute.For<IGenericRepository<Category>>();
        var subject = Substitute.For<MovieService>(mockTitleRepo, mockCategoryRepo);
        
        //Act
        var result = await subject.GetMovies(new MovieParams());
        
        //Asset
        result.ShouldBeAssignableTo<PaginatedDto<MovieDto>>();

    }

    [Fact]
    public async void GetMovies_WhenCalled_ShouldNotReturnEmpty()
    {
        //Arrange
        var mockTitleRepo = Substitute.For<IGenericRepository<Title>>();
        mockTitleRepo.ListAsync(Arg.Any<ISpecification<Title>>()).Returns(new List<Title>
        {
            new() {Id = "Title1", Name = "Title One"},
            new() {Id = "Title2", Name = "Title Two"}
        });
        mockTitleRepo.CountAsync(Arg.Any<ISpecification<Title>>()).Returns(2);
        var mockCategoryRepo = Substitute.For<IGenericRepository<Category>>();
        var subject = Substitute.For<MovieService>(mockTitleRepo, mockCategoryRepo);
        
        //Act
        var result = await subject.GetMovies(new MovieParams());
        
        //Asset
        result.Data.ShouldNotBeEmpty();
        result.PaginationMeta.Count.ShouldBe(2);
    }

    [Fact]
    public async void GetMovies_WhenPageIndexAndPageSizeDefined_ReturnedPageIndexAndPageSizeMatched()
    {
        //Arrange
        var mockTitleRepo = Substitute.For<IGenericRepository<Title>>();
        var mockCategoryRepo = Substitute.For<IGenericRepository<Category>>();
        var subject = Substitute.For<MovieService>(mockTitleRepo, mockCategoryRepo);

        var pageIndex = 3;
        var pageSize = 20;
        
        //Act
        var result = await subject.GetMovies(new MovieParams(), pageIndex, pageSize);
        
        //Asset
        result.PaginationMeta.PageIndex.ShouldBe(pageIndex);
        result.PaginationMeta.PageSize.ShouldBe(pageSize);
    }

    [Fact]
    public async void GetMovie_WhenCalled_GetDataFromRepo()
    {
        //Arrange
        var mockTitleRepo = Substitute.For<IGenericRepository<Title>>();
        var mockCategoryRepo = Substitute.For<IGenericRepository<Category>>();
        var subject = Substitute.For<MovieService>(mockTitleRepo, mockCategoryRepo);

        var movieId = "Title1";
        
        //Act
        await subject.GetMovie(movieId);
        
        //Asset
        await mockTitleRepo.Received().FirstOrDefaultAsync(Arg.Any<ISpecification<Title>>());
        
    }
    
    [Fact]
    public async void GetMovie_WhenNoMovieFound_ShouldBeNull()
    {
        //Arrange
        var mockTitleRepo = Substitute.For<IGenericRepository<Title>>();
        var mockCategoryRepo = Substitute.For<IGenericRepository<Category>>();
        var subject = Substitute.For<MovieService>(mockTitleRepo, mockCategoryRepo);

        var movieId = "Title1";
        
        //Act
        var result = await subject.GetMovie(movieId);
        
        //Asset
        result.ShouldBeNull();
        
    }
    
    [Fact]
    public async void GetMovie_WhenMovieFound_ReturnTypeOfToMovieDto()
    {
        //Arrange
        var movieId = "Title1";
        
        var mockTitleRepo = Substitute.For<IGenericRepository<Title>>();
        mockTitleRepo.FirstOrDefaultAsync(Arg.Any<ISpecification<Title>>()).Returns(new Title
        {
            Id = movieId,
            Name = "Movie One"
        });
        var mockCategoryRepo = Substitute.For<IGenericRepository<Category>>();
        var subject = Substitute.For<MovieService>(mockTitleRepo, mockCategoryRepo);

        
        //Act
        var result = await subject.GetMovie(movieId);
        
        //Asset
        result.ShouldNotBeNull();
        result.ShouldBeAssignableTo<MovieDto>();
        
    }

    [Fact]
    public async void GetRandomMoviesByRandomCategories_WhenCalled_GetDataFromRepo()
    {
        //Arrange
        var mockTitleRepo = Substitute.For<IGenericRepository<Title>>();
        var mockCategoryRepo = Substitute.For<IGenericRepository<Category>>();
        mockCategoryRepo.ListAsync(Arg.Any<ISpecification<Category>>()).Returns(new List<Category>
        {
            new() {Id = "Category1"},
            new() {Id = "Category2"}
        });
        var subject = Substitute.For<MovieService>(mockTitleRepo, mockCategoryRepo);
        
        //Act
        await subject.GetRandomMoviesByRandomCategories();
        
        //Assert
        await mockCategoryRepo.Received().ListAsync(Arg.Any<ISpecification<Category>>());
        await mockTitleRepo.Received().ListAsync(Arg.Any<ISpecification<Title>>());
    }
    
    [Fact]
    public async void GetRandomMoviesByRandomCategories_WhenCalled_ReturnTypeOfEnumberableCategoryMovieDto()
    {
        //Arrange
        var mockTitleRepo = Substitute.For<IGenericRepository<Title>>();
        var mockCategoryRepo = Substitute.For<IGenericRepository<Category>>();
        mockCategoryRepo.ListAsync(Arg.Any<ISpecification<Category>>()).Returns(new List<Category>
        {
            new() {Id = "Category1"},
            new() {Id = "Category2"}
        });
        var subject = Substitute.For<MovieService>(mockTitleRepo, mockCategoryRepo);
        
        //Act
        var result = await subject.GetRandomMoviesByRandomCategories();
        
        //Assert
        result.ShouldBeAssignableTo<IEnumerable<CategoryMovieDto>>();
        foreach (var item in result)
        {
            item.Movies.ShouldBeAssignableTo<IEnumerable<MovieDto>>();
        }
    }
    
    [Fact]
    public async void GetRandomMoviesByRandomCategories_WhenResultFound_ShouldNotBeNullOrEmpty()
    {
        //Arrange
        var mockTitleRepo = Substitute.For<IGenericRepository<Title>>();
        var mockCategoryRepo = Substitute.For<IGenericRepository<Category>>();
        mockCategoryRepo.ListAsync(Arg.Any<ISpecification<Category>>()).Returns(new List<Category>
        {
            new() {Id = "Category1"},
            new() {Id = "Category2"}
        });
        var subject = Substitute.For<MovieService>(mockTitleRepo, mockCategoryRepo);
        
        //Act
        var result = await subject.GetRandomMoviesByRandomCategories();
        
        //Assert
        result.ShouldNotBeNull();
        result.ShouldNotBeEmpty();
    }
    
    
    
    [Fact]
    public async void GetRandomMoviesByRandomCategories_WhenNoResultFound_ShouldNotBeNull()
    {
        //Arrange
        var mockTitleRepo = Substitute.For<IGenericRepository<Title>>();
        var mockCategoryRepo = Substitute.For<IGenericRepository<Category>>();
        mockCategoryRepo.ListAsync(Arg.Any<ISpecification<Category>>()).Returns(new List<Category>
        {
            new() {Id = "Category1"},
            new() {Id = "Category2"}
        });
        var subject = Substitute.For<MovieService>(mockTitleRepo, mockCategoryRepo);
        
        //Act
        var result = await subject.GetRandomMoviesByRandomCategories();
        
        //Assert
        result.ShouldNotBeNull();
    }
}