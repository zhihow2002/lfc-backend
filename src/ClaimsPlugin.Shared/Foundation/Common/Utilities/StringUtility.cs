using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using ClaimsPlugin.Shared.Foundation.Features.Validation.Simple;

namespace ClaimsPlugin.Shared.Foundation.Common.Utilities;

public static class StringUtility
{
    private const char DefaultMaskChar = '*';
    private const MaskOption DefaultMaskOption = MaskOption.InTheMiddleOfString;
    private const int DefaultPercentToApply = 25;

    public enum MaskOption : byte
    {
        AtTheBeginningOfString = 1,
        InTheMiddleOfString = 2,
        AtTheEndOfString = 3
    }

    public static string EmptyStringIfNull(this string? value)
    {
        return value.IsNullOrEmpty() ? string.Empty : value;
    }

    public static string EmptyStringIfNullOrWhiteSpace(this string? value)
    {
        return value.IsNullOrWhiteSpace() ? string.Empty : value;
    }

    /// <summary>
    ///  Get the first characters of string, e.g. `This is a test` returns `tiat`
    /// </summary>
    /// <param name="value"></param>
    /// <param name="limitLength"></param>
    /// <returns></returns>
    public static string GetAllFirstCharactersOfString(this string value, int? limitLength = null)
    {
        if (value.IsNullOrWhiteSpace())
        {
            return string.Empty;
        }

        // Loop string only once.
        IEnumerable<char> firstCharacters = value
            .Where((ch, index) => ch != ' ' && (index == 0 || value[index - 1] == ' '))
            .ToArray();

        if (limitLength.HasValue)
        {
            firstCharacters = firstCharacters.Take(limitLength.Value);
        }

        StringBuilder builder = new();

        foreach (char character in firstCharacters)
        {
            builder.Append(character);
        }

        return builder.ToString();
    }

    /// <summary>
    /// Get the last characters of string, e.g. `This is a test` returns `ssat`
    /// </summary>
    /// <param name="value"></param>
    /// <param name="limitLength"></param>
    /// <returns></returns>
    public static string GetAllLastCharactersOfString(this string value, int? limitLength = null)
    {
        if (value.IsNullOrWhiteSpace())
        {
            return string.Empty;
        }

        // Reverse the string and then get the first characters (which were originally the last characters).
        IEnumerable<char> lastCharacters = value
            .Reverse()
            .Where((ch, index) => ch != ' ' && (index == 0 || value[index - 1] == ' '))
            .ToArray();

        if (limitLength.HasValue)
        {
            lastCharacters = lastCharacters.Take(limitLength.Value);
        }

        StringBuilder builder = new();

        foreach (char character in lastCharacters.Reverse())
        {
            builder.Append(character);
        }

        return builder.ToString();
    }

    public static string GetTheFirstCharacterOfString(this string value)
    {
        return value.IsNullOrWhiteSpace() ? string.Empty : value[..1];
    }

    public static string GetTheLastCharacterOfString(this string value)
    {
        return value.IsNullOrWhiteSpace() ? string.Empty : value[^1].ToString();
    }

    /// <summary>
    ///  Get the last number of characters, e.g. x = 4, `This is a test` returns `This`
    /// </summary>
    /// <param name="value"></param>
    /// <param name="x"></param>
    /// <returns></returns>
    public static string GetFirstNumberOfCharacters(this string value, int x)
    {
        if (string.IsNullOrEmpty(value) || x <= 0)
        {
            return string.Empty;
        }

        return value.Length <= x ? value : value[..x];
    }

    /// <summary>
    ///  Get the last number of characters, e.g. x = 4, `This is a test` returns `test`
    /// </summary>
    /// <param name="value"></param>
    /// <param name="x"></param>
    /// <returns></returns>
    public static string GetLastNumberOfCharacters(this string value, int x)
    {
        if (string.IsNullOrEmpty(value) || x <= 0)
        {
            return string.Empty;
        }

        return value.Length <= x ? value : value.Substring(value.Length - x, x);
    }

    public static string RemoveWhitespaces(this string? input)
    {
        return new string(input?.ToCharArray().Where(c => !char.IsWhiteSpace(c)).ToArray());
    }

    public static string[] SplitStringByCharacters(this string text, params char[] characters)
    {
        return text.Split(characters, StringSplitOptions.RemoveEmptyEntries);
    }

    public static string ToUpperFirstCharacter(this string value)
    {
        if (value.IsNullOrWhiteSpace())
        {
            return value;
        }

        string[] words = value.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        for (int index = 0; index < words.Length; index++)
        {
            string s = words[index];

            if (s.Length > 0)
            {
                words[index] = s[0].ToString().ToUpper() + (s[1..]);
            }
        }

        return string.Join(" ", words);
    }

    public static string ToLowerFirstCharacter(this string value)
    {
        if (value.IsNullOrWhiteSpace())
        {
            return value;
        }

        string[] words = value.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        for (int index = 0; index < words.Length; index++)
        {
            string s = words[index];

            if (s.Length > 0)
            {
                words[index] = s[0].ToString().ToLower() + (s[1..]);
            }
        }

        return string.Join(" ", words);
    }

    public static string ToCamelCase(this string value)
    {
        if (value.IsNullOrWhiteSpace())
        {
            return value;
        }

        return char.ToLowerInvariant(value[0]) + value[1..];
    }

    public static string ToPascalCase(this string value)
    {
        // If there are 0 or 1 characters, just return the string.
        if (value.IsNullOrWhiteSpace())
        {
            return value;
        }

        if (value.Length < 2)
        {
            return value.ToUpper();
        }

        // Split the string into words.
        string[] words = value.Split(new char[] { }, StringSplitOptions.RemoveEmptyEntries);

        // Combine the words using StringBuilder.
        StringBuilder resultBuilder = new StringBuilder();
        foreach (string word in words)
        {
            resultBuilder.Append(word.Substring(0, 1).ToUpper()).Append(word.Substring(1));
        }
        string result = resultBuilder.ToString();

        return result;
    }

    public static string ToKebabCase(this string input)
    {
        if (input.IsNullOrWhiteSpace())
        {
            return input;
        }

        string kebab = Regex.Replace(
            input,
            @"(?<!^)([A-Z][a-z]|(?<=[a-z])[A-Z0-9])",
            "-$1",
            RegexOptions.Compiled,
            TimeSpan.FromMilliseconds(100)
        );

        return kebab.ToLowerInvariant();
    }

    public static string ToSnakeCase(this string input)
    {
        if (input.IsNullOrWhiteSpace())
        {
            return input;
        }

        string snake = Regex.Replace(
            input,
            @"(?<!^)([A-Z][a-z]|(?<=[a-z])[A-Z0-9])",
            "_$1",
            RegexOptions.Compiled,
            TimeSpan.FromMilliseconds(100)
        );

        return snake.ToLowerInvariant();
    }

    public static string Mask(this string input)
    {
        return Mask(input, DefaultMaskChar);
    }

    public static string Mask(this string input, char maskChar)
    {
        return Mask(input, maskChar, DefaultPercentToApply);
    }

    public static string Mask(this string input, int percentToApply)
    {
        return Mask(input, DefaultMaskChar, percentToApply);
    }

    public static string Mask(this string input, MaskOption maskOption)
    {
        return Mask(input, DefaultMaskChar, DefaultPercentToApply, maskOption);
    }

    public static string Mask(this string input, char maskChar, int percentToApply)
    {
        return Mask(input, maskChar, percentToApply, DefaultMaskOption);
    }

    public static string Mask(this string input, char maskChar, MaskOption maskOption)
    {
        return Mask(input, maskChar, DefaultPercentToApply, maskOption);
    }

    public static string Mask(this string input, int percentToApply, MaskOption maskOption)
    {
        return Mask(input, DefaultMaskChar, percentToApply, maskOption);
    }

    public static string Mask(
        this string input,
        char maskChar,
        int percentToApply,
        MaskOption maskOptions
    )
    {
        if (input.Length == 0 || percentToApply < 1)
            return input;
        if (percentToApply >= 100)
            return new string(maskChar, input.Length);

        int maskLength = Math.Max((int)Math.Round(percentToApply * input.Length / 100m), 1);
        string mask = new(maskChar, maskLength);

        switch (maskOptions)
        {
            case MaskOption.AtTheBeginningOfString:
                input = string.Concat(mask, input.AsSpan(maskLength));
                break;
            case MaskOption.AtTheEndOfString:
                input = input[..^maskLength] + mask;
                break;
            case MaskOption.InTheMiddleOfString:
                int maskStart = (input.Length - maskLength) / 2;

                input = input[..maskStart] + mask + input[(maskStart + maskLength)..];
                break;
            default:
                throw new ArgumentOutOfRangeException(
                    nameof(maskOptions),
                    maskOptions,
                    $"Unsupported mask option '{maskOptions}'."
                );
        }

        return input;
    }

    public static string GetUniqueString(
        int size,
        string lookUpCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890"
    )
    {
        char[] chars = lookUpCharacters.ToCharArray();

        byte[] data = new byte[4 * size];
        using (var crypto = RandomNumberGenerator.Create())
        {
            crypto.GetBytes(data);
        }

        StringBuilder result = new(size);

        for (int i = 0; i < size; i++)
        {
            var rnd = BitConverter.ToUInt32(data, i * 4);
            var idx = rnd % chars.Length;

            result.Append(chars[idx]);
        }

        return result.ToString();
    }

    public static string ToHtmlEncoded(this string value)
    {
        if (value.IsNullOrWhiteSpace())
        {
            return value;
        }

        return WebUtility.HtmlEncode(value);
    }

    public static string ToHtmlDecoded(this string value)
    {
        if (value.IsNullOrWhiteSpace())
        {
            return value;
        }

        return WebUtility.HtmlDecode(value);
    }

    public static string ToUrlEncoded(this string value)
    {
        if (value.IsNullOrWhiteSpace())
        {
            return value;
        }

        return WebUtility.UrlEncode(value);
    }

    public static string ToUrlDecoded(this string value)
    {
        if (value.IsNullOrWhiteSpace())
        {
            return value;
        }

        return WebUtility.UrlDecode(value);
    }
    public static List<string> SplitIntoDistinctList(string value, string separator = ",")
    {
        return value
            .Split(separator)
            .Select(x => x.Trim())
            .Distinct()
            .Where(x => x.IsNotNullOrWhiteSpace())
            .ToList();
    }

    public static bool AllBlank(string?[] array)
    {
        return array.All(x => x.IsNullOrWhiteSpace());
    }

    public static string AmountToWords(decimal value, string currency)
    {
        string valueString = DecimalString(value);
        string[] split = valueString.Split(".");
        int dollars = int.Parse(split[0]);
        int cents = int.Parse(split[1]);
        string dollarWords = NumberToWords(dollars);
        string centWords = cents == 0 ? "" : NumberToWords(cents);
        string final = cents == 0 ? dollarWords : dollarWords + " and cents " + centWords;
        final = currency + " " + final + " only";
        return final.ToUpper();
    }

    public static string DecimalString(decimal value)
    {
        return value.ToString("0.00");
    }

    public static string NumberToWords(int number)
    {
        if (number == 0)
        {
            return "zero";
        }

        if (number < 0)
        {
            return "minus " + NumberToWords(Math.Abs(number));
        }

        string words = "";
        const int million = 1000000;
        const int thousand = 1000;
        const int hundred = 100;
        const int twenty = 20;
        const int ten = 10;

        if ((number / million) > 0)
        {
            words += NumberToWords(number / million) + " million ";
            number %= million;
        }

        if ((number / thousand) > 0)
        {
            words += NumberToWords(number / thousand) + " thousand ";
            number %= thousand;
        }

        if ((number / hundred) > 0)
        {
            words += NumberToWords(number / hundred) + " hundred ";
            number %= hundred;
        }

        if (number > 0)
        {
            if (words != "")
            {
                words += "and ";
            }

            string[] unitsMap =
            {
                "zero",
                "one",
                "two",
                "three",
                "four",
                "five",
                "six",
                "seven",
                "eight",
                "nine",
                "ten",
                "eleven",
                "twelve",
                "thirteen",
                "fourteen",
                "fifteen",
                "sixteen",
                "seventeen",
                "eighteen",
                "nineteen"
            };
            string[] tensMap =
            {
                "zero",
                "ten",
                "twenty",
                "thirty",
                "forty",
                "fifty",
                "sixty",
                "seventy",
                "eighty",
                "ninety"
            };

            if (number < twenty)
            {
                words += unitsMap[number];
            }
            else
            {
                words += tensMap[number / ten];

                if ((number % ten) > 0)
                {
                    words += "-" + unitsMap[number % ten];
                }
            }
        }

        return words;
    }

    public static string AddedPrefix(string value, string prefix, int totalLength)
    {
        StringBuilder returnValue = new StringBuilder(value);
        int prefixCount = (totalLength - returnValue.Length) / prefix.Length;

        for (int i = 0; i < prefixCount; i++)
        {
            returnValue.Insert(0, prefix);
        }

        return returnValue.ToString();
    }

    public static string AdvancedMask(
        string input,
        int visibleCharacterCount,
        char maskCharacter = 'x',
        MaskOption maskOption = MaskOption.AtTheBeginningOfString
    )
    {
        if (input.IsNullOrWhiteSpace())
        {
            return input;
        }

        input = input.Trim();

        if (visibleCharacterCount >= input.Length)
        {
            return input;
        }

        int visibleFrontCount =
            maskOption == MaskOption.AtTheEndOfString ? visibleCharacterCount : 0;
        int visibleEndCount =
            maskOption == MaskOption.AtTheBeginningOfString ? visibleCharacterCount : 0;

        if (maskOption == MaskOption.InTheMiddleOfString)
        {
            visibleFrontCount = visibleCharacterCount / 2;
            visibleEndCount = visibleCharacterCount - visibleFrontCount;
        }

        string frontString =
            visibleFrontCount <= 0
                ? ""
                : GetSubstringExcludingWhiteSpace(
                    input,
                    visibleFrontCount,
                    visibleFrontCount,
                    true
                );
        string endString =
            visibleEndCount <= 0
                ? ""
                : GetSubstringExcludingWhiteSpace(input, visibleEndCount, visibleEndCount, false);
        string maskString = "";
        string lowerMask = "" + maskCharacter;
        string upperMask = lowerMask.ToUpper();
        string toMask = input.Substring(frontString.Length);
        StringBuilder maskStringBuilder = new StringBuilder();

        foreach (char character in toMask)
        {
            maskStringBuilder.Append(AddMaskCharacter(character, lowerMask, upperMask));
        }

        maskString = maskStringBuilder.ToString();

        return frontString + maskString + endString;
    }

    private static string GetSubstringExcludingWhiteSpace(
        string input,
        int length,
        int expectedLength,
        bool fromFront
    )
    {
        string result = fromFront
            ? input.Substring(0, length)
            : input.Substring(input.Length - length);

        if (result.Replace(" ", "").Length == expectedLength)
        {
            return result;
        }

        return GetSubstringExcludingWhiteSpace(input, length + 1, expectedLength, fromFront);
    }

    private static string AddMaskCharacter(char character, string lowerMask, string upperMask)
    {
        if (char.IsWhiteSpace(character))
        {
            return " ";
        }

        if (char.IsUpper(character))
        {
            return upperMask;
        }

        return lowerMask;
    }
}
