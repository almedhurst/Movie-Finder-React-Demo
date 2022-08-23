using Ardalis.Specification;
using Microsoft.AspNetCore.Identity;
using MovieFinder.Core.Dtos;
using MovieFinder.Core.Entities;
using MovieFinder.Core.Repositories;
using MovieFinder.Core.Services;
using MovieFinder.Infrastructure.Services;
using NSubstitute.Core;
using NSubstitute.Extensions;

namespace MovieFinder.Infrastructure.Tests.Services;

public class UserServiceTests
{
    [Fact]
    public async void Login_WhenCalled_UserManagerFindByNameAsyncIsCalled()
    {
        //Arrange
        var mockTitleRepo = Substitute.For<IGenericRepository<Title>>();
        var mockFavTitleRepo = Substitute.For<IGenericRepository<UserFavouriteTitle>>();
        var mockTokenService = Substitute.For<ITokenService>();
        var mockUserManager = GetMockUserManager();
        var subject = Substitute.For<UserService>(mockUserManager,
            mockTokenService,
            mockFavTitleRepo,
            mockTitleRepo);

        //Act
        await subject.Login(new LoginDto());

        //Assert
        await mockUserManager.Received().FindByNameAsync(Arg.Any<string>());
    }

    [Fact]
    public async void Login_WhenUserFound_UserManagerCheckPasswordAsyncIsCalled()
    {
        //Arrange
        var mockTitleRepo = Substitute.For<IGenericRepository<Title>>();
        var mockFavTitleRepo = Substitute.For<IGenericRepository<UserFavouriteTitle>>();
        var mockTokenService = Substitute.For<ITokenService>();
        var mockUserManager = GetMockUserManager();
        mockUserManager.FindByNameAsync(Arg.Any<string>()).Returns(new User
        {
            Id = "User1",
            UserName = "UserOne",
            GivenName = "User One",
            Email = "userone@test.com"
        });
        var subject = Substitute.For<UserService>(mockUserManager,
            mockTokenService,
            mockFavTitleRepo,
            mockTitleRepo);

        //Act
        await subject.Login(new LoginDto());

        //Assert
        await mockUserManager.Received().FindByNameAsync(Arg.Any<string>());
        await mockUserManager.Received().CheckPasswordAsync(Arg.Any<User>(), Arg.Any<string>());
    }

    [Fact]
    public async void Login_WhenUserNotFound_ReturnNull()
    {
        //Arrange
        var mockTitleRepo = Substitute.For<IGenericRepository<Title>>();
        var mockFavTitleRepo = Substitute.For<IGenericRepository<UserFavouriteTitle>>();
        var mockTokenService = Substitute.For<ITokenService>();
        var mockUserManager = GetMockUserManager();
        var subject = Substitute.For<UserService>(mockUserManager,
            mockTokenService,
            mockFavTitleRepo,
            mockTitleRepo);

        //Act
        var result = await subject.Login(new LoginDto());

        //Arrange
        result.ShouldBeNull();
    }

    [Fact]
    public async void Login_WhenUserFound_GetUserDtoIsCalled()
    {
        //Arrange
        var loginData = new LoginDto {Username = "User1", Password = "Password"};

        var mockTitleRepo = Substitute.For<IGenericRepository<Title>>();
        var mockFavTitleRepo = Substitute.For<IGenericRepository<UserFavouriteTitle>>();
        mockFavTitleRepo.ListAsync(Arg.Any<ISpecification<UserFavouriteTitle>>())
            .Returns(new List<UserFavouriteTitle>());
        var mockTokenService = Substitute.For<ITokenService>();
        var mockUserManager = GetMockUserManager();
        mockUserManager.FindByNameAsync(Arg.Any<string>()).Returns(new User
        {
            Id = "User1",
            UserName = loginData.Username,
            GivenName = "User One",
            Email = "userone@test.com"
        });
        mockUserManager.CheckPasswordAsync(Arg.Any<User>(), Arg.Any<string>()).Returns(true);
        var subject = Substitute.For<UserService>(mockUserManager,
            mockTokenService,
            mockFavTitleRepo,
            mockTitleRepo);
        subject.GetUserDto(Arg.Any<User>()).Returns(new UserDto());

        //Act
        await subject.Login(loginData);

        //Assert
        await subject.Received().GetUserDto(Arg.Any<User>());
    }

    [Fact]
    public async void Login_WhenUserFound_ReturnsTypoOfUserDto()
    {
        //Arrange
        var loginData = new LoginDto {Username = "User1", Password = "Password"};

        var mockTitleRepo = Substitute.For<IGenericRepository<Title>>();
        var mockFavTitleRepo = Substitute.For<IGenericRepository<UserFavouriteTitle>>();
        mockFavTitleRepo.ListAsync(Arg.Any<ISpecification<UserFavouriteTitle>>())
            .Returns(new List<UserFavouriteTitle>());
        var mockTokenService = Substitute.For<ITokenService>();
        var mockUserManager = GetMockUserManager();
        mockUserManager.FindByNameAsync(Arg.Any<string>()).Returns(new User
        {
            Id = "User1",
            UserName = loginData.Username,
            GivenName = "User One",
            Email = "userone@test.com"
        });
        mockUserManager.CheckPasswordAsync(Arg.Any<User>(), Arg.Any<string>()).Returns(true);
        var subject = Substitute.For<UserService>(mockUserManager,
            mockTokenService,
            mockFavTitleRepo,
            mockTitleRepo);
        subject.GetUserDto(Arg.Any<User>()).Returns(new UserDto());
        
        // Act
        var result = await subject.Login(loginData);
        
        //Assert
        result.ShouldNotBeNull();
        result.ShouldBeAssignableTo<UserDto>();
    }

    [Fact]
    public async void GetCurrentUser_WhenCalled_GetUserDtoIsCalled()
    {
        //Arrange
        var username = "UserOne";
        var mockTitleRepo = Substitute.For<IGenericRepository<Title>>();
        var mockFavTitleRepo = Substitute.For<IGenericRepository<UserFavouriteTitle>>();
        mockFavTitleRepo.ListAsync(Arg.Any<ISpecification<UserFavouriteTitle>>())
            .Returns(new List<UserFavouriteTitle>());
        var mockTokenService = Substitute.For<ITokenService>();
        var mockUserManager = GetMockUserManager();
        mockUserManager.FindByNameAsync(Arg.Any<string>()).Returns(new User
        {
            Id = "User1",
            UserName = username,
            GivenName = "User One",
            Email = "userone@test.com"
        });
        var subject = Substitute.For<UserService>(mockUserManager,
            mockTokenService,
            mockFavTitleRepo,
            mockTitleRepo);
        subject.GetUserDto(Arg.Any<User>()).Returns(new UserDto());
        
        //Act
        await subject.GetCurrentUser(username);
        
        //Assert
        await subject.Received().GetUserDto(Arg.Any<User>());
    }
    
    [Fact]
    public async void GetCurrentUser_WhenCalled_UserManagerFindByNameSyncIsCalled()
    {
        //Arrange
        var username = "UserOne";
        var mockTitleRepo = Substitute.For<IGenericRepository<Title>>();
        var mockFavTitleRepo = Substitute.For<IGenericRepository<UserFavouriteTitle>>();
        mockFavTitleRepo.ListAsync(Arg.Any<ISpecification<UserFavouriteTitle>>())
            .Returns(new List<UserFavouriteTitle>());
        var mockTokenService = Substitute.For<ITokenService>();
        var mockUserManager = GetMockUserManager();
        mockUserManager.FindByNameAsync(Arg.Any<string>()).Returns(new User
        {
            Id = "User1",
            UserName = username,
            GivenName = "User One",
            Email = "userone@test.com"
        });
        var subject = Substitute.For<UserService>(mockUserManager,
            mockTokenService,
            mockFavTitleRepo,
            mockTitleRepo);
        subject.GetUserDto(Arg.Any<User>()).Returns(new UserDto());
        
        //Act
        await subject.GetCurrentUser(username);
        
        //Assert
        await mockUserManager.FindByNameAsync(Arg.Any<string>());
    }
    
    [Fact]
    public async void GetCurrentUser_WhenCalled_ReturnsTypoOfUserDto()
    {
        //Arrange
        var username = "userone";

        var mockTitleRepo = Substitute.For<IGenericRepository<Title>>();
        var mockFavTitleRepo = Substitute.For<IGenericRepository<UserFavouriteTitle>>();
        mockFavTitleRepo.ListAsync(Arg.Any<ISpecification<UserFavouriteTitle>>())
            .Returns(new List<UserFavouriteTitle>());
        var mockTokenService = Substitute.For<ITokenService>();
        var mockUserManager = GetMockUserManager();
        mockUserManager.FindByNameAsync(Arg.Any<string>()).Returns(new User
        {
            Id = "User1",
            UserName = username,
            GivenName = "User One",
            Email = "userone@test.com"
        });
        var subject = Substitute.For<UserService>(mockUserManager,
            mockTokenService,
            mockFavTitleRepo,
            mockTitleRepo);
        subject.GetUserDto(Arg.Any<User>()).Returns(new UserDto());
        
        // Act
        var result = await subject.GetCurrentUser(username);
        
        //Assert
        result.ShouldNotBeNull();
        result.ShouldBeAssignableTo<UserDto>();
    }

    [Fact]
    public async void GetUserFavouriteMovies_WhenCalled_GetUserDtoIsCalled()
    {
        //Arrange
        var username = "UserOne";
        var mockTitleRepo = Substitute.For<IGenericRepository<Title>>();
        var mockFavTitleRepo = Substitute.For<IGenericRepository<UserFavouriteTitle>>();
        mockFavTitleRepo.ListAsync(Arg.Any<ISpecification<UserFavouriteTitle>>())
            .Returns(new List<UserFavouriteTitle>());
        var mockTokenService = Substitute.For<ITokenService>();
        var mockUserManager = GetMockUserManager();
        mockUserManager.FindByNameAsync(Arg.Any<string>()).Returns(new User
        {
            Id = "User1",
            UserName = username,
            GivenName = "User One",
            Email = "userone@test.com"
        });
        var subject = Substitute.For<UserService>(mockUserManager,
            mockTokenService,
            mockFavTitleRepo,
            mockTitleRepo);
        subject.GetUserDto(Arg.Any<User>()).Returns(new UserDto
        {
            FavouriteMovies = new ()
        });
        
        //Act
        await subject.GetUserFavouriteMovies(username);
        
        //Assert
        await subject.Received().GetUserDto(Arg.Any<User>());
    }
    
    [Fact]
    public async void GetUserFavouriteMovies_WhenCalled_UserManagerFindByNameSyncIsCalled()
    {
        //Arrange
        var username = "UserOne";
        var mockTitleRepo = Substitute.For<IGenericRepository<Title>>();
        var mockFavTitleRepo = Substitute.For<IGenericRepository<UserFavouriteTitle>>();
        mockFavTitleRepo.ListAsync(Arg.Any<ISpecification<UserFavouriteTitle>>())
            .Returns(new List<UserFavouriteTitle>());
        var mockTokenService = Substitute.For<ITokenService>();
        var mockUserManager = GetMockUserManager();
        mockUserManager.FindByNameAsync(Arg.Any<string>()).Returns(new User
        {
            Id = "User1",
            UserName = username,
            GivenName = "User One",
            Email = "userone@test.com"
        });
        var subject = Substitute.For<UserService>(mockUserManager,
            mockTokenService,
            mockFavTitleRepo,
            mockTitleRepo);
        subject.GetUserDto(Arg.Any<User>()).Returns(new UserDto
        {
            FavouriteMovies = new ()
        });
        
        //Act
        await subject.GetUserFavouriteMovies(username);
        
        //Assert
        await mockUserManager.FindByNameAsync(Arg.Any<string>());
    }
    
    [Fact]
    public async void GetUserFavouriteMovies_WhenCalled_ReturnsTypoOfListFavouriteMovieDto()
    {
        //Arrange
        var username = "userone";

        var mockTitleRepo = Substitute.For<IGenericRepository<Title>>();
        var mockFavTitleRepo = Substitute.For<IGenericRepository<UserFavouriteTitle>>();
        mockFavTitleRepo.ListAsync(Arg.Any<ISpecification<UserFavouriteTitle>>())
            .Returns(new List<UserFavouriteTitle>());
        var mockTokenService = Substitute.For<ITokenService>();
        var mockUserManager = GetMockUserManager();
        mockUserManager.FindByNameAsync(Arg.Any<string>()).Returns(new User
        {
            Id = "User1",
            UserName = username,
            GivenName = "User One",
            Email = "userone@test.com"
        });
        var subject = Substitute.For<UserService>(mockUserManager,
            mockTokenService,
            mockFavTitleRepo,
            mockTitleRepo);
        subject.WhenForAnyArgs(x => x.GetUserDto(Arg.Any<User>())).DoNotCallBase();
        subject.GetUserDto(Arg.Any<User>()).Returns(new UserDto
        {
            FavouriteMovies = new ()
        });
        
        // Act
        var result = await subject.GetUserFavouriteMovies(username);
        
        //Assert
        result.ShouldNotBeNull();
        result.ShouldBeAssignableTo<List<FavouriteMovieDto>>();
    }

    [Fact]
    public async void AddFavouriteMovie_WhenCalled_UserManagerFindByNameAsyncIsCalled()
    {
        //Arrange
        var username = "userone";
        var modelData = new AddRemoveFavouriteMovieDto {TitleId = "Title1"};

        var mockTitleRepo = Substitute.For<IGenericRepository<Title>>();
        var mockFavTitleRepo = Substitute.For<IGenericRepository<UserFavouriteTitle>>();
        mockFavTitleRepo.ListAsync(Arg.Any<ISpecification<UserFavouriteTitle>>())
            .Returns(new List<UserFavouriteTitle>());
        var mockTokenService = Substitute.For<ITokenService>();
        var mockUserManager = GetMockUserManager();
        mockUserManager.FindByNameAsync(Arg.Any<string>()).Returns(new User
        {
            Id = "User1",
            UserName = username,
            GivenName = "User One",
            Email = "userone@test.com"
        });
        var subject = Substitute.For<UserService>(mockUserManager,
            mockTokenService,
            mockFavTitleRepo,
            mockTitleRepo);
        subject.WhenForAnyArgs(x => x.GetUserDto(Arg.Any<User>())).DoNotCallBase();
        subject.GetUserDto(Arg.Any<User>()).Returns(new UserDto
        {
            FavouriteMovies = new ()
        });
        
        //Act
        await subject.AddFavouriteMovie(username, modelData);
        
        //Assert
        await mockUserManager.Received().FindByNameAsync(Arg.Any<string>());
    }
    
    [Fact]
    public async void AddFavouriteMovie_WhenCalled_UserFavTitleRepoFirstOrDefaultAsyncIsCalled()
    {
        //Arrange
        var username = "userone";
        var modelData = new AddRemoveFavouriteMovieDto {TitleId = "Title1"};

        var mockTitleRepo = Substitute.For<IGenericRepository<Title>>();
        var mockFavTitleRepo = Substitute.For<IGenericRepository<UserFavouriteTitle>>();
        mockFavTitleRepo.ListAsync(Arg.Any<ISpecification<UserFavouriteTitle>>())
            .Returns(new List<UserFavouriteTitle>());
        var mockTokenService = Substitute.For<ITokenService>();
        var mockUserManager = GetMockUserManager();
        mockUserManager.FindByNameAsync(Arg.Any<string>()).Returns(new User
        {
            Id = "User1",
            UserName = username,
            GivenName = "User One",
            Email = "userone@test.com"
        });
        var subject = Substitute.For<UserService>(mockUserManager,
            mockTokenService,
            mockFavTitleRepo,
            mockTitleRepo);
        subject.WhenForAnyArgs(x => x.GetUserDto(Arg.Any<User>())).DoNotCallBase();
        subject.GetUserDto(Arg.Any<User>()).Returns(new UserDto
        {
            FavouriteMovies = new ()
        });
        
        //Act
        await subject.AddFavouriteMovie(username, modelData);
        
        //Assert
        await mockFavTitleRepo.Received().FirstOrDefaultAsync(Arg.Any<ISpecification<UserFavouriteTitle>>());
    }
    
    [Fact]
    public async void AddFavouriteMovie_WhenFavTitleExists_UserFavTitleRepoUpdateAsyncIsCalled()
    {
        //Arrange
        var username = "userone";
        var oldData = new UserFavouriteTitle {TitleId = "Title1", UserId = "User1"};
        var newData = new AddRemoveFavouriteMovieDto {TitleId = "Title1", Comments = "Some comment"};
        var expectedData = new UserFavouriteTitle {TitleId = "Title1", UserId = "User1", Comments = "Some comment"};
        var captechedData = new UserFavouriteTitle();

        var mockTitleRepo = Substitute.For<IGenericRepository<Title>>();
        var mockFavTitleRepo = Substitute.For<IGenericRepository<UserFavouriteTitle>>();
        mockFavTitleRepo.ListAsync(Arg.Any<ISpecification<UserFavouriteTitle>>())
            .Returns(new List<UserFavouriteTitle>());
        mockFavTitleRepo.FirstOrDefaultAsync(Arg.Any<ISpecification<UserFavouriteTitle>>()).Returns(oldData);
        await mockFavTitleRepo.UpdateAsync(Arg.Do<UserFavouriteTitle>(x => captechedData = x));
        var mockTokenService = Substitute.For<ITokenService>();
        var mockUserManager = GetMockUserManager();
        mockUserManager.FindByNameAsync(Arg.Any<string>()).Returns(new User
        {
            Id = "User1",
            UserName = username,
            GivenName = "User One",
            Email = "userone@test.com"
        });
        var subject = Substitute.For<UserService>(mockUserManager,
            mockTokenService,
            mockFavTitleRepo,
            mockTitleRepo);
        subject.WhenForAnyArgs(x => x.GetUserDto(Arg.Any<User>())).DoNotCallBase();
        subject.GetUserDto(Arg.Any<User>()).Returns(new UserDto
        {
            FavouriteMovies = new ()
        });
        
        //Act
        await subject.AddFavouriteMovie(username, newData);
        
        //Assert
        await mockFavTitleRepo.Received().UpdateAsync(Arg.Any<UserFavouriteTitle>());
        captechedData.Comments.ShouldBeSameAs(expectedData.Comments);
    }
    
    [Fact]
    public async void AddFavouriteMovie_WhenFavTitleDoesNotExists_UserFavTitleRepoAddAsyncIsCalled()
    {
        //Arrange
        var username = "userone";
        var newData = new AddRemoveFavouriteMovieDto {TitleId = "Title1", Comments = "Some comment"};
        var expectedData = new UserFavouriteTitle {TitleId = "Title1", UserId = "User1", Comments = "Some comment"};
        var captechedData = new UserFavouriteTitle();

        var mockTitleRepo = Substitute.For<IGenericRepository<Title>>();
        var mockFavTitleRepo = Substitute.For<IGenericRepository<UserFavouriteTitle>>();
        mockFavTitleRepo.ListAsync(Arg.Any<ISpecification<UserFavouriteTitle>>())
            .Returns(new List<UserFavouriteTitle>());
        await mockFavTitleRepo.AddAsync(Arg.Do<UserFavouriteTitle>(x => captechedData = x));
        var mockTokenService = Substitute.For<ITokenService>();
        var mockUserManager = GetMockUserManager();
        mockUserManager.FindByNameAsync(Arg.Any<string>()).Returns(new User
        {
            Id = "User1",
            UserName = username,
            GivenName = "User One",
            Email = "userone@test.com"
        });
        var subject = Substitute.For<UserService>(mockUserManager,
            mockTokenService,
            mockFavTitleRepo,
            mockTitleRepo);
        subject.WhenForAnyArgs(x => x.GetUserDto(Arg.Any<User>())).DoNotCallBase();
        subject.GetUserDto(Arg.Any<User>()).Returns(new UserDto
        {
            FavouriteMovies = new ()
        });
        
        //Act
        await subject.AddFavouriteMovie(username, newData);
        
        //Assert
        await mockFavTitleRepo.Received().AddAsync(Arg.Any<UserFavouriteTitle>());
        captechedData.TitleId.ShouldBe(expectedData.TitleId);
        captechedData.UserId.ShouldBe(expectedData.UserId);
        captechedData.Comments.ShouldBe(expectedData.Comments);
    }
    
    [Fact]
    public async void AddFavouriteMovie_WhenCalled_ReturnTypeListFavouriteMovieDto()
    {
        //Arrange
        var username = "userone";
        var newData = new AddRemoveFavouriteMovieDto {TitleId = "Title1", Comments = "Some comment"};

        var mockTitleRepo = Substitute.For<IGenericRepository<Title>>();
        var mockFavTitleRepo = Substitute.For<IGenericRepository<UserFavouriteTitle>>();
        mockFavTitleRepo.ListAsync(Arg.Any<ISpecification<UserFavouriteTitle>>())
            .Returns(new List<UserFavouriteTitle>());
        var mockTokenService = Substitute.For<ITokenService>();
        var mockUserManager = GetMockUserManager();
        mockUserManager.FindByNameAsync(Arg.Any<string>()).Returns(new User
        {
            Id = "User1",
            UserName = username,
            GivenName = "User One",
            Email = "userone@test.com"
        });
        var subject = Substitute.For<UserService>(mockUserManager,
            mockTokenService,
            mockFavTitleRepo,
            mockTitleRepo);
        subject.WhenForAnyArgs(x => x.GetUserDto(Arg.Any<User>())).DoNotCallBase();
        subject.GetUserDto(Arg.Any<User>()).Returns(new UserDto
        {
            FavouriteMovies = new ()
        });
        
        //Act
        var result = await subject.AddFavouriteMovie(username, newData);
        
        //Assert
        result.ShouldNotBeNull();
        result.ShouldBeAssignableTo<List<FavouriteMovieDto>>();
    }

    [Fact]
    public async void RemoveFavouriteMovie_WhenCalled_UserManagerFindByNameAsyncIsCalled()
    {
        //Arrange
        var username = "userone";
        var modelData = new AddRemoveFavouriteMovieDto {TitleId = "Title1"};

        var mockTitleRepo = Substitute.For<IGenericRepository<Title>>();
        var mockFavTitleRepo = Substitute.For<IGenericRepository<UserFavouriteTitle>>();
        mockFavTitleRepo.ListAsync(Arg.Any<ISpecification<UserFavouriteTitle>>())
            .Returns(new List<UserFavouriteTitle>());
        var mockTokenService = Substitute.For<ITokenService>();
        var mockUserManager = GetMockUserManager();
        mockUserManager.FindByNameAsync(Arg.Any<string>()).Returns(new User
        {
            Id = "User1",
            UserName = username,
            GivenName = "User One",
            Email = "userone@test.com"
        });
        var subject = Substitute.For<UserService>(mockUserManager,
            mockTokenService,
            mockFavTitleRepo,
            mockTitleRepo);
        subject.WhenForAnyArgs(x => x.GetUserDto(Arg.Any<User>())).DoNotCallBase();
        subject.GetUserDto(Arg.Any<User>()).Returns(new UserDto
        {
            FavouriteMovies = new ()
        });
        
        //Act
        await subject.RemoveFavouriteMovie(username, modelData);
        
        //Assert
        await mockUserManager.Received().FindByNameAsync(Arg.Any<string>());
    }
    
    [Fact]
    public async void RemoveFavouriteMovie_WhenCalled_UserFavTitleRepoFirstOrDefaultAsyncIsCalled()
    {
        //Arrange
        var username = "userone";
        var modelData = new AddRemoveFavouriteMovieDto {TitleId = "Title1"};

        var mockTitleRepo = Substitute.For<IGenericRepository<Title>>();
        var mockFavTitleRepo = Substitute.For<IGenericRepository<UserFavouriteTitle>>();
        mockFavTitleRepo.ListAsync(Arg.Any<ISpecification<UserFavouriteTitle>>())
            .Returns(new List<UserFavouriteTitle>());
        var mockTokenService = Substitute.For<ITokenService>();
        var mockUserManager = GetMockUserManager();
        mockUserManager.FindByNameAsync(Arg.Any<string>()).Returns(new User
        {
            Id = "User1",
            UserName = username,
            GivenName = "User One",
            Email = "userone@test.com"
        });
        var subject = Substitute.For<UserService>(mockUserManager,
            mockTokenService,
            mockFavTitleRepo,
            mockTitleRepo);
        subject.WhenForAnyArgs(x => x.GetUserDto(Arg.Any<User>())).DoNotCallBase();
        subject.GetUserDto(Arg.Any<User>()).Returns(new UserDto
        {
            FavouriteMovies = new ()
        });
        
        //Act
        await subject.RemoveFavouriteMovie(username, modelData);
        
        //Assert
        await mockFavTitleRepo.Received().FirstOrDefaultAsync(Arg.Any<ISpecification<UserFavouriteTitle>>());
    }
    
    [Fact]
    public async void RemoveFavouriteMovie_WhenFavTitleExists_UserFavTitleRepoDeleteAsyncIsCalled()
    {
        //Arrange
        var username = "userone";
        var modelData = new AddRemoveFavouriteMovieDto {TitleId = "Title1"};
        var mockData = new UserFavouriteTitle {TitleId = "Title1", UserId = "User1"};

        var mockTitleRepo = Substitute.For<IGenericRepository<Title>>();
        var mockFavTitleRepo = Substitute.For<IGenericRepository<UserFavouriteTitle>>();
        mockFavTitleRepo.ListAsync(Arg.Any<ISpecification<UserFavouriteTitle>>())
            .Returns(new List<UserFavouriteTitle>());
        mockFavTitleRepo.FirstOrDefaultAsync(Arg.Any<ISpecification<UserFavouriteTitle>>()).Returns(mockData);
        var mockTokenService = Substitute.For<ITokenService>();
        var mockUserManager = GetMockUserManager();
        mockUserManager.FindByNameAsync(Arg.Any<string>()).Returns(new User
        {
            Id = "User1",
            UserName = username,
            GivenName = "User One",
            Email = "userone@test.com"
        });
        var subject = Substitute.For<UserService>(mockUserManager,
            mockTokenService,
            mockFavTitleRepo,
            mockTitleRepo);
        subject.WhenForAnyArgs(x => x.GetUserDto(Arg.Any<User>())).DoNotCallBase();
        subject.GetUserDto(Arg.Any<User>()).Returns(new UserDto
        {
            FavouriteMovies = new ()
        });
        
        //Act
        await subject.RemoveFavouriteMovie(username, modelData);
        
        //Assert
        await mockFavTitleRepo.Received().DeleteAsync(Arg.Any<UserFavouriteTitle>());
    }
    
    [Fact]
    public async void RemoveFavouriteMovie_WhenFavTitleDoesNotExists_UserFavTitleRepoDeleteAsyncIsNotCalled()
    {
        //Arrange
        var username = "userone";
        var modelData = new AddRemoveFavouriteMovieDto {TitleId = "Title1"};
        var mockData = new UserFavouriteTitle {TitleId = "Title1", UserId = "User1"};

        var mockTitleRepo = Substitute.For<IGenericRepository<Title>>();
        var mockFavTitleRepo = Substitute.For<IGenericRepository<UserFavouriteTitle>>();
        mockFavTitleRepo.ListAsync(Arg.Any<ISpecification<UserFavouriteTitle>>())
            .Returns(new List<UserFavouriteTitle>());
        var mockTokenService = Substitute.For<ITokenService>();
        var mockUserManager = GetMockUserManager();
        mockUserManager.FindByNameAsync(Arg.Any<string>()).Returns(new User
        {
            Id = "User1",
            UserName = username,
            GivenName = "User One",
            Email = "userone@test.com"
        });
        var subject = Substitute.For<UserService>(mockUserManager,
            mockTokenService,
            mockFavTitleRepo,
            mockTitleRepo);
        subject.WhenForAnyArgs(x => x.GetUserDto(Arg.Any<User>())).DoNotCallBase();
        subject.GetUserDto(Arg.Any<User>()).Returns(new UserDto
        {
            FavouriteMovies = new ()
        });
        
        //Act
        await subject.RemoveFavouriteMovie(username, modelData);
        
        //Assert
        await mockFavTitleRepo.DidNotReceive().DeleteAsync(Arg.Any<UserFavouriteTitle>());
    }
    
    [Fact]
    public async void RemoveFavouriteMovie_WhenCalled_ReturnTypeListFavouriteMovieDto()
    {
        //Arrange
        var username = "userone";
        var newData = new AddRemoveFavouriteMovieDto {TitleId = "Title1", Comments = "Some comment"};

        var mockTitleRepo = Substitute.For<IGenericRepository<Title>>();
        var mockFavTitleRepo = Substitute.For<IGenericRepository<UserFavouriteTitle>>();
        mockFavTitleRepo.ListAsync(Arg.Any<ISpecification<UserFavouriteTitle>>())
            .Returns(new List<UserFavouriteTitle>());
        var mockTokenService = Substitute.For<ITokenService>();
        var mockUserManager = GetMockUserManager();
        mockUserManager.FindByNameAsync(Arg.Any<string>()).Returns(new User
        {
            Id = "User1",
            UserName = username,
            GivenName = "User One",
            Email = "userone@test.com"
        });
        var subject = Substitute.For<UserService>(mockUserManager,
            mockTokenService,
            mockFavTitleRepo,
            mockTitleRepo);
        subject.WhenForAnyArgs(x => x.GetUserDto(Arg.Any<User>())).DoNotCallBase();
        subject.GetUserDto(Arg.Any<User>()).Returns(new UserDto
        {
            FavouriteMovies = new ()
        });
        
        //Act
        var result = await subject.RemoveFavouriteMovie(username, newData);
        
        //Assert
        result.ShouldNotBeNull();
        result.ShouldBeAssignableTo<List<FavouriteMovieDto>>();
    }

    [Fact]
    public async void RegisterUser_WhenCalled_UserManagerCreateAsyncIsCalled()
    {
        //Arrange
        var modelData = new RegisterDto
            {Email = "test@test.com", GivenName = "Test User", Password = "Password", Username = "test"};

        var mockTitleRepo = Substitute.For<IGenericRepository<Title>>();
        var mockFavTitleRepo = Substitute.For<IGenericRepository<UserFavouriteTitle>>();
        mockFavTitleRepo.ListAsync(Arg.Any<ISpecification<UserFavouriteTitle>>())
            .Returns(new List<UserFavouriteTitle>());
        var mockTokenService = Substitute.For<ITokenService>();
        var mockUserManager = GetMockUserManager();
        var subject = Substitute.For<UserService>(mockUserManager,
            mockTokenService,
            mockFavTitleRepo,
            mockTitleRepo);
        subject.WhenForAnyArgs(x => x.GetUserDto(Arg.Any<User>())).DoNotCallBase();
        subject.GetUserDto(Arg.Any<User>()).Returns(new UserDto
        {
            FavouriteMovies = new ()
        });
        
        //Act
        await subject.RegisterUser(modelData);
    }

    [Fact]
    public async void RegisterUser_WhenCalled_ReturnTypeOfIdentityResult()
    {
        //Arrange
        var modelData = new RegisterDto
            {Email = "test@test.com", GivenName = "Test User", Password = "Password", Username = "test"};

        var mockTitleRepo = Substitute.For<IGenericRepository<Title>>();
        var mockFavTitleRepo = Substitute.For<IGenericRepository<UserFavouriteTitle>>();
        mockFavTitleRepo.ListAsync(Arg.Any<ISpecification<UserFavouriteTitle>>())
            .Returns(new List<UserFavouriteTitle>());
        var mockTokenService = Substitute.For<ITokenService>();
        var mockUserManager = GetMockUserManager();
        mockUserManager.CreateAsync(Arg.Any<User>(), Arg.Any<string>()).Returns(new IdentityResult());
        var subject = Substitute.For<UserService>(mockUserManager,
            mockTokenService,
            mockFavTitleRepo,
            mockTitleRepo);
        subject.WhenForAnyArgs(x => x.GetUserDto(Arg.Any<User>())).DoNotCallBase();
        subject.GetUserDto(Arg.Any<User>()).Returns(new UserDto
        {
            FavouriteMovies = new ()
        });
        
        //Act
        var result = await subject.RegisterUser(modelData);
        
        //Assert
        result.ShouldBeAssignableTo<IdentityResult>();
    }
    
    [Fact]
    public async void RegisterUser_WhenCalled_ShouldCreateNewUserModel()
    {
        //Arrange
        var modelData = new RegisterDto
            {Email = "test@test.com", GivenName = "Test User", Password = "Password", Username = "test"};
        var capturedData = new User();
        var capturedPassword = "";

        var mockTitleRepo = Substitute.For<IGenericRepository<Title>>();
        var mockFavTitleRepo = Substitute.For<IGenericRepository<UserFavouriteTitle>>();
        mockFavTitleRepo.ListAsync(Arg.Any<ISpecification<UserFavouriteTitle>>())
            .Returns(new List<UserFavouriteTitle>());
        var mockTokenService = Substitute.For<ITokenService>();
        var mockUserManager = GetMockUserManager();
        await mockUserManager.CreateAsync(Arg.Do<User>(x => capturedData = x),
            Arg.Do<string>(x => capturedPassword = x));
        mockUserManager.CreateAsync(Arg.Any<User>(), Arg.Any<string>()).Returns(new IdentityResult());
        var subject = Substitute.For<UserService>(mockUserManager,
            mockTokenService,
            mockFavTitleRepo,
            mockTitleRepo);
        subject.WhenForAnyArgs(x => x.GetUserDto(Arg.Any<User>())).DoNotCallBase();
        subject.GetUserDto(Arg.Any<User>()).Returns(new UserDto
        {
            FavouriteMovies = new ()
        });
        
        //Act
        var result = await subject.RegisterUser(modelData);
        
        //Assert
        capturedData.GivenName.ShouldBe(modelData.GivenName);
        capturedData.UserName.ShouldBe(modelData.Username);
        capturedData.Email.ShouldBe(modelData.Email);
        capturedPassword.ShouldBe(modelData.Password);
    }
    
    [Fact]
    public async void GetUserDto_WhenCalled_UserFavTitleRepoListAsyncIsCalled()
    {
        //Arrange
        var userData = new User {Id = "User1", UserName = "User1", GivenName = "User One", Email = "user1@test.com"};

        var mockTitleRepo = Substitute.For<IGenericRepository<Title>>();
        var mockFavTitleRepo = Substitute.For<IGenericRepository<UserFavouriteTitle>>();
        mockFavTitleRepo.ListAsync(Arg.Any<ISpecification<UserFavouriteTitle>>())
            .Returns(new List<UserFavouriteTitle>());
        var mockTokenService = Substitute.For<ITokenService>();
        var mockUserManager = GetMockUserManager();
        var subject = Substitute.For<UserService>(mockUserManager,
            mockTokenService,
            mockFavTitleRepo,
            mockTitleRepo);
        subject.WhenForAnyArgs(x => x.GetUserDto(Arg.Any<User>())).CallBase();

        //Act
        await subject.GetUserDto(userData);

        //Assert
        await mockFavTitleRepo.Received().ListAsync(Arg.Any<ISpecification<UserFavouriteTitle>>());
    }
    
    [Fact]
    public async void GetUserDto_WhenFavMovieFound_TitleRepoListAsyncIsCalled()
    {
        //Arrange
        var userData = new User {Id = "User1", UserName = "User1", GivenName = "User One", Email = "user1@test.com"};

        var mockTitleRepo = Substitute.For<IGenericRepository<Title>>();
        mockTitleRepo.ListAsync(Arg.Any<ISpecification<Title>>()).Returns(new List<Title>());
        var mockFavTitleRepo = Substitute.For<IGenericRepository<UserFavouriteTitle>>();
        mockFavTitleRepo.ListAsync(Arg.Any<ISpecification<UserFavouriteTitle>>())
            .Returns(new List<UserFavouriteTitle>{new UserFavouriteTitle{ TitleId = "Title1"}});
        var mockTokenService = Substitute.For<ITokenService>();
        var mockUserManager = GetMockUserManager();
        var subject = Substitute.For<UserService>(mockUserManager,
            mockTokenService,
            mockFavTitleRepo,
            mockTitleRepo);
        subject.WhenForAnyArgs(x => x.GetUserDto(Arg.Any<User>())).CallBase();

        //Act
        await subject.GetUserDto(userData);

        //Assert
        await mockTitleRepo.Received().ListAsync(Arg.Any<ISpecification<Title>>());
    }
    
    [Fact]
    public async void GetUserDto_WhenCalled_TokenServicdeGenerateTokenIsCalled()
    {
        //Arrange
        var userData = new User {Id = "User1", UserName = "User1", GivenName = "User One", Email = "user1@test.com"};

        var mockTitleRepo = Substitute.For<IGenericRepository<Title>>();
        var mockFavTitleRepo = Substitute.For<IGenericRepository<UserFavouriteTitle>>();
        mockFavTitleRepo.ListAsync(Arg.Any<ISpecification<UserFavouriteTitle>>())
            .Returns(new List<UserFavouriteTitle>( ));
        var mockTokenService = Substitute.For<ITokenService>();
        var mockUserManager = GetMockUserManager();
        var subject = Substitute.For<UserService>(mockUserManager,
            mockTokenService,
            mockFavTitleRepo,
            mockTitleRepo);
        subject.WhenForAnyArgs(x => x.GetUserDto(Arg.Any<User>())).CallBase();

        //Act
        await subject.GetUserDto(userData);

        //Assert
        mockTokenService.Received().GenerateToken(Arg.Any<User>());
    }

    [Fact]
    public async void GetUserDto_WhenCalled_ReturnTypeOfUserDto()
    {
        //Arrange
        var userData = new User {Id = "User1", UserName = "User1", GivenName = "User One", Email = "user1@test.com"};

        var mockTitleRepo = Substitute.For<IGenericRepository<Title>>();
        var mockFavTitleRepo = Substitute.For<IGenericRepository<UserFavouriteTitle>>();
        mockFavTitleRepo.ListAsync(Arg.Any<ISpecification<UserFavouriteTitle>>())
            .Returns(new List<UserFavouriteTitle>( ));
        var mockTokenService = Substitute.For<ITokenService>();
        var mockUserManager = GetMockUserManager();
        var subject = Substitute.For<UserService>(mockUserManager,
            mockTokenService,
            mockFavTitleRepo,
            mockTitleRepo);
        subject.WhenForAnyArgs(x => x.GetUserDto(Arg.Any<User>())).CallBase();

        //Act
        var result = await subject.GetUserDto(userData);

        //Assert
        result.ShouldNotBeNull();
        result.ShouldBeAssignableTo<UserDto>();
    }
    
    [Fact]
    public async void GetUserDto_WhenFavMovieFound_ReturnFavouriteMovieShouldNotBeEmpty()
    {
        //Arrange
        var userData = new User {Id = "User1", UserName = "User1", GivenName = "User One", Email = "user1@test.com"};

        var mockTitleRepo = Substitute.For<IGenericRepository<Title>>();
        mockTitleRepo.ListAsync(Arg.Any<ISpecification<Title>>()).Returns(new List<Title>{new Title{Id = "Title1"}});
        var mockFavTitleRepo = Substitute.For<IGenericRepository<UserFavouriteTitle>>();
        mockFavTitleRepo.ListAsync(Arg.Any<ISpecification<UserFavouriteTitle>>())
            .Returns(new List<UserFavouriteTitle>{new UserFavouriteTitle{ TitleId = "Title1"}});
        var mockTokenService = Substitute.For<ITokenService>();
        var mockUserManager = GetMockUserManager();
        var subject = Substitute.For<UserService>(mockUserManager,
            mockTokenService,
            mockFavTitleRepo,
            mockTitleRepo);
        subject.WhenForAnyArgs(x => x.GetUserDto(Arg.Any<User>())).CallBase();

        //Act
        var result = await subject.GetUserDto(userData);

        //Assert
        result.ShouldNotBeNull();
        result.FavouriteMovies.ShouldNotBeNull();
        result.FavouriteMovies.ShouldBeAssignableTo<List<FavouriteMovieDto>>();
        result.FavouriteMovies.ShouldNotBeEmpty();
    }

    private UserManager<User> GetMockUserManager()
    {
        var mockUserStore = Substitute.For<IUserStore<User>>();
        var mockUserManager =
            Substitute.For<UserManager<User>>(mockUserStore, null, null, null, null, null, null, null, null);
        return mockUserManager;
    }
}