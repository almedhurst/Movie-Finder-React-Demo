using System.Globalization;

namespace MovieFinder.Core.Helpers;

public static class StringHelpers
{
    private const string CharList = "0123456789abcdefghijklmnopqrstuvwxyz";
    private static Random random = new Random();
    
    public static string toBase36(this long input)
    {
        if (input < 0) throw new ArgumentOutOfRangeException("input", input, "input cannot be negative");

        char[] clistArr = CharList.ToCharArray();
        var result = new Stack<char>();
        while (input != 0)
        {
            result.Push(clistArr[input % 36]);
            input /= 36;
        }
        return $"{new string(result.ToArray())}-{RandomString(6)}";
    }
    
    public static string RandomString(int length)
    {
        return new string(Enumerable.Repeat(CharList, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    public static string toTitleCase(this string input)
    {
        TextInfo textInfo = new CultureInfo("en-US",false).TextInfo;
        return textInfo.ToTitleCase(input);
    }
}