using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rascar.Toolbox.Utilities
{
    public class RomanNumberUtils
    {
        public static readonly SortedList<int, string> ROMAN_NUMBERS = new()
        {
            {
                1,
                "I"
            },
            {
                4,
                "IV"
            },
            {
                5,
                "V"
            },
            {
                9,
                "IX"
            },
            {
                10,
                "X"
            },
            {
                40,
                "XL"
            },
            {
                50,
                "L"
            },
            {
                90,
                "XC"
            },
            {
                100,
                "C"
            },
            {
                400,
                "CD"
            },
            {
                500,
                "D"
            },
            {
                900,
                "CM"
            },
            {
                1000,
                "M"
            }
        };

        public static string IntToRoman(int number)
        {
            StringBuilder romanResult = new();

            foreach ((int romanNumber, string romanText) in ROMAN_NUMBERS.Reverse())
            {
                if (number <= 0)
                {
                    break;
                }

                while (number >= romanNumber)
                {
                    romanResult.Append(romanText);
                    number -= romanNumber;
                }
            }

            return romanResult.ToString();
        }
    }
}
