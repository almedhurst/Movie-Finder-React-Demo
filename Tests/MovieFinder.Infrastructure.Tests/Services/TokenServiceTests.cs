using Microsoft.Extensions.Configuration;
using MovieFinder.Core.Entities;
using MovieFinder.Infrastructure.Services;

namespace MovieFinder.Infrastructure.Tests.Services;

public class TokenServiceTests
{
    [Fact]
    public void GenerateToken_WhenCalled_ShouldReturnTokenString()
    {
        //Arrange
        var userData = new User {Id = "User1", UserName = "UserOne", GivenName = "User One", Email = "test@test.com"};
        
        var mockConfiguration = Substitute.For<IConfiguration>();
        mockConfiguration["JWTSettings:TokenKey"] = "SomeReallyReallyReallyLongKeyTheMakesAbsolutelyNoSense";

        var subject = Substitute.For<TokenService>(mockConfiguration);
        
        //Act
        var result = subject.GenerateToken(userData);
        
        //
        result.ShouldNotBeNullOrEmpty();
        result.ShouldNotBeNullOrWhiteSpace();
    }
}