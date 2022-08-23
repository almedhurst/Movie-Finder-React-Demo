using MovieFinder.Core.Dtos;

namespace MovieFinder.Core.Tests.Dtos;

public class PaginationMetaDtoTests
{
    [Theory]
    [InlineData(16,49,719,15)]
    [InlineData(9,17,597,36)]
    [InlineData(5,41,152,4)]
    [InlineData(9,26,509,20)]
    [InlineData(17,32,502,16)]
    [InlineData(7,41,209,6)]
    [InlineData(17,19,942,50)]
    [InlineData(4,32,538,17)]
    [InlineData(17,32,618,20)]
    [InlineData(9,41,377,10)]
    [InlineData(19,24,582,25)]
    [InlineData(13,49,579,12)]
    [InlineData(6,25,102,5)]
    [InlineData(10,46,828,18)]
    [InlineData(18,31,551,18)]

    public void PaginationMetaDto_Constructor(int pageIndex, int pageSize, int count, int expectedPageCount)
    {
        //Arrange
        var data = new PaginationMetaDto(pageIndex, pageSize, count);
        
        //Assert
        data.PageIndex.ShouldBe(pageIndex);
        data.PageSize.ShouldBe(pageSize);
        data.Count.ShouldBe(count);
        data.PageCount.ShouldBe(expectedPageCount);
    }
}