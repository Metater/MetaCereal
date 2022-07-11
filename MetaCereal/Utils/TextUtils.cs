namespace MetaCereal.Utils;

public static class TextUtils
{
    public static string ToPascal(string input)
    {
        string output = "";
        string[] segments = input.Split('-');
        foreach (string segment in segments)
        {
            output += char.ToUpper(segment[0]);
            output += segment[1..];
        }
        return output;
    }

    public static string ToCamel(string input)
    {
        string pascal = ToPascal(input);
        string end = pascal[1..];
        char start = char.ToLower(pascal[0]);
        return start + end;
    }

    public static string ToSnake(string input)
    {
        return input.Replace('-', '_');
    }

    public static string ToUpper(string input)
    {
        return ToSnake(input).ToUpper();
    }

    public static int CountLeadingTabs(string input)
    {
        int count = 0;
        foreach (char c in input)
        {
            if (c == '\t')
            {
                count++;
            }
            else
            {
                break;
            }
        }
        return count;

    }
}