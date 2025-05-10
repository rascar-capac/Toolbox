using System.Collections.Generic;
using Rascar.Toolbox.Utilities;
using UnityEngine;

namespace Rascar.Toolbox.Extensions
{
    public static class IntExtensions
    {
        public static bool IsSameSign(this int input, int other, bool zeroIsSame = false)
        {
            return MathUtils.IsSameSign(input, other, zeroIsSame);
        }

        public static string GetCompactFormat(this int number)
        {
            return ((float)number).GetCompactFormat();
        }
    }
}
