using MovieFinder.Core.Entities;

namespace MovieFinder.Core.Tests.Entities;

public class BaseEntityTests
{
    [Fact]
    public void BaseEntity_Constructor()
    {
        //Act
        var data = new BaseEntity();
        
        //Assert
        data.Id.ShouldNotBeNullOrWhiteSpace();
    }
}