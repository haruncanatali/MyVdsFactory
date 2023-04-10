using Microsoft.IdentityModel.Tokens;

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

    public static List<string> GetMonthsByTurkishName()
    {
        return new List<string>
        {
            "Ocak",
            "Şubat",
            "Mart",
            "Nisan",
            "Mayıs",
            "Haziran",
            "Temmuz",
            "Ağustos",
            "Eylül",
            "Ekim",
            "Kasım",
            "Aralık"
        };
    }
    
    public static string SafeTrim(this string source, char val = ' ')
    {
        try
        {
            if(source.IsNullOrEmpty()) return string.Empty;

            return source.Trim(val);
        }
        catch
        {
            return String.Empty;
        }
    }

    public static string SafeLeftTrim(this string source, char val = ' ')
    {
        try
        {
            if(source.IsNullOrEmpty()) return String.Empty;

            return source.TrimStart(val);
        }
        catch
        {
            return String.Empty;
        }
    }

    public static string SafeRightTrim(this string source, char val = ' ')
    {
        try
        {
            if(source.IsNullOrEmpty()) return String.Empty;

            return source.TrimEnd(val);
        }
        catch
        {
            return String.Empty;
        }
    }

    public static string SafeSubstring(this string source, int startIndex, int slice)
    {
        try
        {
            if(source.IsNullOrEmpty()) return String.Empty;
            
            int length = source.Length;

            if ( (startIndex < 0) || (startIndex > length - 1))
            {
                return source;
            }
            
            if ( (slice < 0) || (slice > length))
            {
                return source;
            }

            return source.Substring(startIndex, slice);

        }
        catch
        {
            if (!source.IsNullOrEmpty()) return source;
            return String.Empty;
        }
    }
}