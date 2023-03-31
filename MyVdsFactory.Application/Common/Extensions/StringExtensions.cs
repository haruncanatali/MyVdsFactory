namespace MyVdsFactory.Application.Common.Extensions;

public static class StringExtensions
{
    public static string ReplaceTurkishCharacters(this string turkishWord,bool isLower=true)
    {
        string source = "ığüşöçĞÜŞİÖÇ";
        string destination = "igusocGUSIOC";
 
        string result = isLower ? turkishWord.ToLower() : turkishWord.ToUpper();
 
        for (int i = 0; i < source.Length; i++)
        {
            result = result.Replace(source[i], destination[i]);
        }
 
        return result;
    }
}