using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieFinder.Core.Dtos;
using MovieFinder.Core.RequestHelpers;
using MovieFinder.Core.Services;
using MovieFinder.Web.Controllers;

namespace MovieFinder.Web.Tests.Controllers;

public class MoviesControllerTests
{
    [Fact]
    public async void GetMovies_WhenPageNameIsLessThenOne_ShouldChangeToOne()
    {
        //Arrange
        var mockMovieService = Substitute.For<IMovieService>();
        mockMovieService.GetMovies(Arg.Any<MovieParams>(),Arg.Any<int>(),Arg.Any<int>()).Returns(
            new PaginatedDto<MovieDto>(1,1,1,new List<MovieDto>()));
        var subject = new MoviesController(mockMovieService);
        subject.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };
        
        //Act
        await subject.GetMovies(new MovieParams(), 0);
        
        //Arrange
        await mockMovieService.Received().GetMovies(Arg.Any<MovieParams>(), 1, 50);
    }
    
    [Fact]
    public async void GetMovies_WhenPageSizeIsMoreThenFifty_ShouldChangeToFifty()
    {
        //Arrange
        var mockMovieService = Substitute.For<IMovieService>();
        mockMovieService.GetMovies(Arg.Any<MovieParams>(),Arg.Any<int>(),Arg.Any<int>()).Returns(
            new PaginatedDto<MovieDto>(1,1,1,new List<MovieDto>()));
        var subject = new MoviesController(mockMovieService);
        subject.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };
        
        //Act
        await subject.GetMovies(new MovieParams(), 1, 51);
        
        //Arrange
        await mockMovieService.Received().GetMovies(Arg.Any<MovieParams>(), 1, 50);
    }
    
    [Fact]
    public async void GetMovies_WhenPageSizeIsLessThenThree_ShouldChangeToThree()
    {
        //Arrange
        var mockMovieService = Substitute.For<IMovieService>();
        mockMovieService.GetMovies(Arg.Any<MovieParams>(),Arg.Any<int>(),Arg.Any<int>()).Returns(
            new PaginatedDto<MovieDto>(1,1,1,new List<MovieDto>()));
        var subject = new MoviesController(mockMovieService);
        subject.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };
        
        //Act
        await subject.GetMovies(new MovieParams(), 1, 2);
        
        //Arrange
        await mockMovieService.Received().GetMovies(Arg.Any<MovieParams>(), 1, 3);
    }
    
    [Fact]
    public async void GetMovies_WhenCalled_MovieServiceGetMoviesIsCalled()
    {
        //Arrange
        var mockMovieService = Substitute.For<IMovieService>();
        mockMovieService.GetMovies(Arg.Any<MovieParams>(),Arg.Any<int>(),Arg.Any<int>()).Returns(
            new PaginatedDto<MovieDto>(1,1,1,new List<MovieDto>()));
        var subject = new MoviesController(mockMovieService);
        subject.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };
        
        //Act
        await subject.GetMovies(new MovieParams());
        
        //Arrange
        await mockMovieService.Received().GetMovies(Arg.Any<MovieParams>(), Arg.Any<int>(), Arg.Any<int>());
    }
    
    [Fact]
    public async void GetMovies_WhenCalled_ReturnOKResult()
    {
        //Arrange
        var mockMovieService = Substitute.For<IMovieService>();
        mockMovieService.GetMovies(Arg.Any<MovieParams>(),Arg.Any<int>(),Arg.Any<int>()).Returns(
            new PaginatedDto<MovieDto>(1,1,1,new List<MovieDto>()));
        var subject = new MoviesController(mockMovieService);
        subject.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };
        
        //Act
        var result = await subject.GetMovies(new MovieParams());
        
        //Arrange
        result.Result.ShouldBeOfType<OkObjectResult>();
    }

    [Fact]
    public async void GetMovie_WhenCalled_MovieServiceGetMovieIsCalled()
    {
        //Arrange
        var mockMovieService = Substitute.For<IMovieService>();
        mockMovieService.GetMovie(Arg.Any<string>()).Returns(new MovieDto());
        var subject = new MoviesController(mockMovieService);
        
        //Act
        await subject.GetMovie("Title1");
        
        //Arrange
        await mockMovieService.Received().GetMovie(Arg.Any<string>());
    }

    [Fact]
    public async void GetMovie_WhenCalled_ReturnsOKResult()
    {
        //Arrange
        var mockMovieService = Substitute.For<IMovieService>();
        mockMovieService.GetMovie(Arg.Any<string>()).Returns(new MovieDto());
        var subject = new MoviesController(mockMovieService);
        
        //Act
        var result = await subject.GetMovie("Title1");
        
        //Arrange
        result.Result.ShouldBeOfType<OkObjectResult>();
    }

    [Fact]
    public async void GetRandomMoviesByRandomCategories_WhenCalled_MovieServiceGetRandomMoviesByRandomCategoriesIsCalled()
    {
        //Arrange
        var mockMovieService = Substitute.For<IMovieService>();
        mockMovieService.GetRandomMoviesByRandomCategories().Returns(new List<CategoryMovieDto>());
        var subject = new MoviesController(mockMovieService);
        
        //Act
        await subject.GetRandomMoviesByRandomCategories();
        
        //Arrange
        await mockMovieService.Received().GetRandomMoviesByRandomCategories();
    }

    [Fact]
    public async void GetRandomMoviesByRandomCategories_WhenCalled_ReturnsOKResult()
    {
        //Arrange
        var mockMovieService = Substitute.For<IMovieService>();
        mockMovieService.GetRandomMoviesByRandomCategories().Returns(new List<CategoryMovieDto>());
        var subject = new MoviesController(mockMovieService);
        
        //Act
        var result = await subject.GetRandomMoviesByRandomCategories();
        
        //Arrange
        result.Result.ShouldBeOfType<OkObjectResult>();
    }
}