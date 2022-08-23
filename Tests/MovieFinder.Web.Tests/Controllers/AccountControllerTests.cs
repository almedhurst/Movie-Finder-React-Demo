using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MovieFinder.Core.Dtos;
using MovieFinder.Core.Services;
using MovieFinder.Web.Controllers;
using NSubstitute.ReceivedExtensions;
using NSubstitute.ReturnsExtensions;

namespace MovieFinder.Web.Tests.Controllers;

public class AccountControllerTests
{
    [Fact]
    public async void Login_WhenCalled_UserServiceLoginIsCalled()
    {
        //Arrange
        var mockUserService = Substitute.For<IUserService>();
        mockUserService.Login(Arg.Any<LoginDto>()).ReturnsNull();
        var subject = new AccountController(mockUserService);
        
        //Act
        await subject.Login(new LoginDto{Username = "username", Password = "password"});
        
        //Assert
        await mockUserService.Received().Login(Arg.Any<LoginDto>());
    }

    [Fact]
    public async void Login_WhenUserServiceLoginReturnsNull_ReturnUnauthorizedResult()
    {
        //Arrange
        var mockUserService = Substitute.For<IUserService>();
        mockUserService.Login(Arg.Any<LoginDto>()).ReturnsNull();
        var subject = new AccountController(mockUserService);

        //Act
        var result = await subject.Login(new LoginDto());

        //Assert
        result.Result.ShouldBeOfType<UnauthorizedResult>();
    }

    [Fact]
    public async void Login_WhenUserServiceLoginReturnValidUserData_ReturnOKResult()
    {
        //Arrange
        var mockUserService = Substitute.For<IUserService>();
        mockUserService.Login(Arg.Any<LoginDto>()).Returns(new UserDto());
        var subject = new AccountController(mockUserService);
        
        //Act
        var result = await subject.Login(new LoginDto());
        
        //Assert
        result.Result.ShouldBeOfType<OkObjectResult>();
    }

    [Fact]
    public async void GetCurrentUser_WhenCalled_UserServiceGetCurrentUserIsCalled()
    {
        //Arrange
        var mockUserService = Substitute.For<IUserService>();
        mockUserService.GetCurrentUser(Arg.Any<string>()).Returns(new UserDto());
        var mockClaimsPrinciple = GetMockClaimsPrinciple();
        var subject = new AccountController(mockUserService);
        subject.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext() {User = mockClaimsPrinciple}
        };
        
        //Arrange
        await subject.GetCurrentUser();
        
        //Act
        await mockUserService.Received().GetCurrentUser(Arg.Any<string>());

    }

    [Fact]
    public async void GetCurrentUser_WhenCalled_ReturnOKResult()
    {
        //Arrange
        var mockUserService = Substitute.For<IUserService>();
        mockUserService.GetCurrentUser(Arg.Any<string>()).Returns(new UserDto());
        var mockClaimsPrinciple = GetMockClaimsPrinciple();
        var subject = new AccountController(mockUserService);
        subject.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext() {User = mockClaimsPrinciple}
        };
        
        //Arrange
        var result = await subject.GetCurrentUser();
        
        //Act
        result.Result.ShouldBeOfType<OkObjectResult>();
    }

    [Fact]
    public async void AddFavouriteMovie_WhenCalled_UserServiceAddFavouriteMovieIsCalled()
    {
        //Arrange
        var mockUserService = Substitute.For<IUserService>();
        mockUserService.AddFavouriteMovie(Arg.Any<string>(), Arg.Any<AddRemoveFavouriteMovieDto>())
            .Returns(new List<FavouriteMovieDto>());
        var mockClaimsPrinciple = GetMockClaimsPrinciple();
        var subject = new AccountController(mockUserService);
        subject.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext() {User = mockClaimsPrinciple}
        };
        
        //Arrange
        await subject.AddFavouriteMovie(new AddRemoveFavouriteMovieDto());
        
        //Act
        await mockUserService.Received().AddFavouriteMovie(Arg.Any<string>(), Arg.Any<AddRemoveFavouriteMovieDto>());

    }

    [Fact]
    public async void AddFavouriteMovie_WhenCalled_ReturnOKResult()
    {
        //Arrange
        var mockUserService = Substitute.For<IUserService>();
        mockUserService.AddFavouriteMovie(Arg.Any<string>(), Arg.Any<AddRemoveFavouriteMovieDto>())
            .Returns(new List<FavouriteMovieDto>());
        var mockClaimsPrinciple = GetMockClaimsPrinciple();
        var subject = new AccountController(mockUserService);
        subject.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext() {User = mockClaimsPrinciple}
        };
        
        //Arrange
        var result = await subject.AddFavouriteMovie(new AddRemoveFavouriteMovieDto());
        
        //Act
        result.Result.ShouldBeOfType<OkObjectResult>();
    }

    [Fact]
    public async void DeleteFavouriteMovie_WhenCalled_UserServiceRemoveFavouriteMovieIsCalled()
    {
        //Arrange
        var mockUserService = Substitute.For<IUserService>();
        mockUserService.RemoveFavouriteMovie(Arg.Any<string>(), Arg.Any<AddRemoveFavouriteMovieDto>())
            .Returns(new List<FavouriteMovieDto>());
        var mockClaimsPrinciple = GetMockClaimsPrinciple();
        var subject = new AccountController(mockUserService);
        subject.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext() {User = mockClaimsPrinciple}
        };
        
        //Arrange
        await subject.DeleteFavouriteMovie(new AddRemoveFavouriteMovieDto());
        
        //Act
        await mockUserService.Received().RemoveFavouriteMovie(Arg.Any<string>(), Arg.Any<AddRemoveFavouriteMovieDto>());

    }

    [Fact]
    public async void DeleteFavouriteMovie_WhenCalled_ReturnOKResult()
    {
        //Arrange
        var mockUserService = Substitute.For<IUserService>();
        mockUserService.RemoveFavouriteMovie(Arg.Any<string>(), Arg.Any<AddRemoveFavouriteMovieDto>())
            .Returns(new List<FavouriteMovieDto>());
        var mockClaimsPrinciple = GetMockClaimsPrinciple();
        var subject = new AccountController(mockUserService);
        subject.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext() {User = mockClaimsPrinciple}
        };
        
        //Arrange
        var result = await subject.DeleteFavouriteMovie(new AddRemoveFavouriteMovieDto());
        
        //Act
        result.Result.ShouldBeOfType<OkObjectResult>();
    }

    [Fact]
    public async void Register_WhenCalled_UserServiceRegisterUserIsCalled()
    {
        //Arrange
        var mockUserService = Substitute.For<IUserService>();
        mockUserService.RegisterUser(Arg.Any<RegisterDto>()).Returns(new IdentityResult());
        var subject = new AccountController(mockUserService);
        
        //Act
        await subject.Register(new RegisterDto());
        
        //Assert
        await mockUserService.Received().RegisterUser(Arg.Any<RegisterDto>());

    }

    [Fact]
    public async void Register_WhenRegistrationFails_ReturnValidationProblemResult()
    {
        //Arrange
        var mockUserService = Substitute.For<IUserService>();
        mockUserService.RegisterUser(Arg.Any<RegisterDto>()).Returns(IdentityResult.Failed(
            new IdentityError(){Code = "ErrorCode1", Description = "Error code description"}));
        var subject = new AccountController(mockUserService);
        
        //Act
        var result = await subject.Register(new RegisterDto());
        
        //Assert
        result.ShouldBeOfType<ObjectResult>();
        ((ObjectResult) result).Value.ShouldBeOfType<ValidationProblemDetails>();
        ((ValidationProblemDetails)((ObjectResult)result).Value).Errors
            .ShouldContain(x => 
                x.Key == "ErrorCode1" && 
                x.Value[0] == "Error code description");
    }
    
    [Fact]
    public async void Register_WhenRegistrationIsSuccessful_ReturnStatusCodeOfTwoZeroOne()
    {
        //Arrange
        var mockUserService = Substitute.For<IUserService>();
        mockUserService.RegisterUser(Arg.Any<RegisterDto>()).Returns(IdentityResult.Success);
        var subject = new AccountController(mockUserService);
        
        //Act
        var result = await subject.Register(new RegisterDto());
        
        //Assert
        result.ShouldBeOfType<StatusCodeResult>();
        ((StatusCodeResult)result).StatusCode.ShouldBe(201);
    }

    private ClaimsPrincipal GetMockClaimsPrinciple()
    {
  
        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.Name, "example name"),
            new Claim(ClaimTypes.NameIdentifier, "1"),
        }, "mock"));

        return claimsPrincipal;
    }
}