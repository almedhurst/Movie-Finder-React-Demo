using MovieFinder.Core.Dtos;

namespace MovieFinder.Core.Test.Dtos;

public class PaginatedDtoTests
{
    [Theory]
    [InlineData(4,38,400)]
    [InlineData(15,32,938)]
    [InlineData(12,13,730)]
    [InlineData(3,46,324)]
    [InlineData(4,10,210)]
    [InlineData(2,31,496)]
    [InlineData(14,15,846)]
    [InlineData(11,42,983)]
    [InlineData(6,41,224)]
    [InlineData(5,25,123)]
    [InlineData(7,31,717)]
    [InlineData(13,37,449)]
    [InlineData(18,39,818)]
    [InlineData(9,15,356)]
    [InlineData(11,25,746)]
    public void PaginatedDto_Constructor(int pageIndex, int pageSize, int count)
    {
        //Arrange
        var data = new PaginatedDto<object>(pageIndex, pageSize, count, new List<Object>());
        
        //Assert
        data.PaginationMeta.PageIndex.ShouldBe(pageIndex);
        data.PaginationMeta.PageSize.ShouldBe(pageSize);
        data.PaginationMeta.Count.ShouldBe(count);
        data.Data.ShouldNotBeNull();
    }
}