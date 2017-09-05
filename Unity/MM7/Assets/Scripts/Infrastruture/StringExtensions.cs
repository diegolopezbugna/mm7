using System;
using System.Text.RegularExpressions;

public static class StringExtensions
{
    public static string ToSentenceCase(this string str)
    {
//        return Regex.Replace(str, "[a-z][A-Z]", m => $"{m.Value[0]} {char.ToLower(m.Value[1])}");
        var sentenced = Regex.Replace(str, "[a-z][A-Z]", m => m.Value[0] + " " + char.ToLower(m.Value[1]));
        return sentenced.Substring(0, 1).ToUpper() + sentenced.Substring(1);
    }

    public static TEnum ToEnum<TEnum>(this string strEnumValue, TEnum defaultValue)
    {
        if (!Enum.IsDefined(typeof(TEnum), strEnumValue))
            return defaultValue;

        return (TEnum)Enum.Parse(typeof(TEnum), strEnumValue);
    }
}

