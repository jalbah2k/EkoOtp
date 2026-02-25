using System;
using System.Text;

/// <summary>
/// Summary description for StringExtensions
/// </summary>
static public class StringExtensions
{
    /*
    /// <summary>
    /// Returns a new string in which all occurrences of a specified string in the current instance are replaced with another specified string.
    /// </summary>
    /// <param name="str">The string performing the replace method.</param>
    /// <param name="oldValue">The string to be replaced.</param>
    /// <param name="newValue">The string replace all occurrances of oldValue.</param>
    /// <param name="comparisonType">Type of the comparison.</param>
    /// <returns></returns>
    public static string Replace(this string str, string oldValue, string @newValue, StringComparison comparisonType)
    {
        @newValue = @newValue ?? string.Empty;
        if (string.IsNullOrEmpty(str) || string.IsNullOrEmpty(oldValue) || oldValue.Equals(@newValue, comparisonType))
        {
            return str;
        }
        int foundAt;
        while ((foundAt = str.IndexOf(oldValue, 0, comparisonType)) != -1)
        {
            str = str.Remove(foundAt, oldValue.Length).Insert(foundAt, @newValue);
        }
        return str;
    }
    */

    /// <summary>
    /// Returns a new string in which all occurrences of a specified string in the current instance are replaced with another specified string.
    /// </summary>
    /// <param name="original">The string performing the replace method.</param>
    /// <param name="oldValue">The string to be replaced.</param>
    /// <param name="newValue">The string replace all occurrances of oldValue.</param>
    /// <param name="comparisonType">Type of the comparison.</param>
    /// <returns></returns>
    static public string Replace(this string original, string oldValue, string newValue, StringComparison comparisonType, int stringBuilderInitialSize = -1)
    {
        if (original == null)
        {
            return null;
        }

        if (String.IsNullOrEmpty(oldValue))
        {
            return original;
        }


        int posCurrent = 0;
        int lenPattern = oldValue.Length;
        int idxNext = original.IndexOf(oldValue, comparisonType);
        StringBuilder result = new StringBuilder(stringBuilderInitialSize < 0 ? Math.Min(4096, original.Length) : stringBuilderInitialSize);

        while (idxNext >= 0)
        {
            result.Append(original, posCurrent, idxNext - posCurrent);
            result.Append(newValue);

            posCurrent = idxNext + lenPattern;

            idxNext = original.IndexOf(oldValue, posCurrent, comparisonType);
        }

        result.Append(original, posCurrent, original.Length - posCurrent);

        return result.ToString();
    }
}