using MovieFinder.Core.Helpers;

namespace MovieFinder.Core.Tests.Helpers;

public class StringHelpersTests
{
    [Theory]
    [InlineData("2020-03-10")]
    [InlineData("2021-04-09")]
    [InlineData("2020-01-20")]
    [InlineData("2020-08-24")]
    [InlineData("2020-05-27")]
    [InlineData("2022-05-14")]
    [InlineData("2021-11-22")]
    [InlineData("2022-02-12")]
    [InlineData("2020-03-03")]
    [InlineData("2020-06-18")]
    [InlineData("2022-05-03")]
    [InlineData("2020-10-05")]
    [InlineData("2021-02-25")]
    [InlineData("2021-08-02")]
    [InlineData("2021-06-14")]
    public void ToBase36_WhenInputValid_ReturnCorrectStringFormat(string input)
    {
        //Arrange
        string allowedCharactors = "0123456789abcdefghijklmnopqrstuvwxyz";
        var inputArr = input.Split("-");
        var dateTime = new DateTime(Int32.Parse(inputArr[0]), Int32.Parse(inputArr[1]), Int32.Parse(inputArr[2]));
        var dateTimeTicks = dateTime.Ticks;

        //Act
        var result = dateTimeTicks.toBase36();

        //Assert
        var resultArr = result.Split("-");
        resultArr.Length.ShouldBe(2);
        resultArr[0].All(c => allowedCharactors.Contains(c)).ShouldBe(true);
        resultArr[1].Length.ShouldBe(6);
        
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-71)]
    [InlineData(-6)]
    [InlineData(-36)]
    public void ToBase36_WhenInputValid_ErrorIsThrow(long input)
    {
        //Act & Assert
        Should.Throw<ArgumentOutOfRangeException>(() => input.toBase36());
    }

    [Theory]
    [InlineData(23)]
    [InlineData(10)]
    [InlineData(42)]
    [InlineData(10)]
    [InlineData(50)]
    [InlineData(71)]
    [InlineData(84)]
    [InlineData(100)]
    public void RandomString_WhenLengthValid_ShouldReturnRequestedLength(int length)
    {
        //Arrange
        string allowedCharactors = "0123456789abcdefghijklmnopqrstuvwxyz";
        
        //Act
        var result = StringHelpers.RandomString(length);
        
        //Assert
        result.Length.ShouldBe(length);
        result.All(c => allowedCharactors.Contains(c)).ShouldBe(true);
    }
    
    [Theory]
    [InlineData(-1)]
    [InlineData(-71)]
    [InlineData(-6)]
    [InlineData(-36)]
    public void RandomString_WhenLengthInvalid_ErrorIsThrow(int length)
    {
        //Act & Assert
        Should.Throw<ArgumentOutOfRangeException>(() => StringHelpers.RandomString(length));
    }

    [Theory]
    [InlineData("pAR FoR thE COuRse", "Par For The Course")]
    [InlineData("a cOLD fish", "A Cold Fish")]
    [InlineData("A BIte aT ThE cHeRRy", "A Bite At The Cherry")]
    [InlineData("tHERe's no i In tEAM", "There's No I In Team")]
    [InlineData("PLaYING fOR KEePs", "Playing For Keeps")]
    [InlineData("tHRow In tHE toweL", "Throw In The Towel")]
    [InlineData("a DOg in tHE mAnGeR", "A Dog In The Manger")]
    [InlineData("LonG in THe tootH", "Long In The Tooth")]
    [InlineData("swinGING foR the fENCES", "Swinging For The Fences")]
    [InlineData("kEEp YoUR eyeS PeELeD", "Keep Your Eyes Peeled")]
    public void ToTitleCase_ReturnExpectedOutput(string input, string expected)
    {
        //Act
        var result = input.toTitleCase();
        
        //Assert
        result.ShouldBe(expected);
    }
}