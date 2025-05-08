using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Rascar.Toolbox.Extensions
{
    public static class StringExtensions
    {
        public static int CountOccurence(this string input, string pattern)
        {
            int count = 0;
            int previousIndex = 0;

            if (!string.IsNullOrEmpty(pattern))
            {
                while ((previousIndex = input.IndexOf(pattern, previousIndex)) != -1)
                {
                    ++previousIndex;
                    ++count;
                }
            }

            return count;
        }

        public static string Capitalize(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return null;
            }

            if (input.Length > 1)
            {
                return char.ToUpper(input[0]) + input[1..];
            }

            return input.ToUpper();
        }

        public static string SafeFormat(this string format, params object[] args)
        {
            if (!string.IsNullOrEmpty(format))
            {
                return string.Format(format, args);
            }

            string result = string.Empty;
            bool firstArg = true;

            foreach (object arg in args)
            {
                if (!firstArg)
                {
                    result += " - ";
                }

                if (arg != null)
                {
                    result += arg.ToString();
                }

                firstArg = false;
            }

            return result;
        }

        /// <summary>
        /// Checks if two strings are equal, no matter the case.
        /// </summary>
        /// <param name="mustIgnoreSpacing">
        /// If set to true, spacing will not be taken in account, whether it's expressed with " ", "_", or "-".
        public static bool EqualsInsensitive(this string first, string second, bool mustIgnoreSpacing)
        {
            if (string.IsNullOrEmpty(first) || string.IsNullOrEmpty(second))
            {
                return string.IsNullOrEmpty(first) == string.IsNullOrEmpty(second);
            }

            if (mustIgnoreSpacing)
            {
                first = Regex.Replace(first, @"[_\-\s]", "");
                second = Regex.Replace(second, @"[_\-\s]", "");
            }

            return first.Equals(second, StringComparison.OrdinalIgnoreCase);
        }

        public static string ReplaceMultiple(this string template, IEnumerable<KeyValuePair<string, string>> replaceParameters, RegexOptions regexOptions)
        {
            string result = template;

            foreach ((string replaceKey, string replaceValue) in replaceParameters)
            {
                result = Regex.Replace(result, replaceKey, replaceValue, regexOptions);
            }

            return result;
        }

        public static string RemoveDiacritics(this string input)
        {
            StringBuilder stringBuilder = new();
            string normalizedString = input.Normalize(NormalizationForm.FormD);

            foreach (char character in normalizedString)
            {
                UnicodeCategory unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(character);

                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(character);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        public static string RemoveNonAlphaNumericCharacters(this string input, bool removeSpaces = true, bool removeSpecialCharacters = true, bool ignoreUnderscoresAndHyphens = true)
        {
            StringBuilder stringBuilder = new();

            foreach (char character in input)
            {
                if (character.IsWhitespace())
                {
                    if (removeSpaces)
                    {
                        continue;
                    }
                }
                else if (!char.IsLetterOrDigit(character))
                {
                    if (removeSpecialCharacters)
                    {
                        if (character == '_' || character == '-')
                        {
                            if (!ignoreUnderscoresAndHyphens)
                            {
                                continue;
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                }

                stringBuilder.Append(character);
            }

            return stringBuilder.ToString();
        }

        public static bool IsWhitespace(this char character)
        {
            switch (character)
            {
                case '\u0020':
                case '\u00A0':
                case '\u1680':
                case '\u2000':
                case '\u2001':
                case '\u2002':
                case '\u2003':
                case '\u2004':
                case '\u2005':
                case '\u2006':
                case '\u2007':
                case '\u2008':
                case '\u2009':
                case '\u200A':
                case '\u202F':
                case '\u205F':
                case '\u3000':
                case '\u2028':
                case '\u2029':
                case '\u0009':
                case '\u000A':
                case '\u000B':
                case '\u000C':
                case '\u000D':
                case '\u0085':
                {
                    return true;
                }

                default:
                {
                    return false;
                }
            }
        }

        public static StringBuilder TrimEnd(this StringBuilder builder)
        {
            if (builder == null || builder.Length == 0)
            {
                return builder;
            }

            int charIndex;

            for (charIndex = builder.Length - 1; charIndex >= 0; charIndex--)
            {
                if (!char.IsWhiteSpace(builder[charIndex]))
                {
                    break;
                }
            }

            if (charIndex < builder.Length - 1)
            {
                builder.Length = charIndex + 1;
            }

            return builder;
        }
    }
}
