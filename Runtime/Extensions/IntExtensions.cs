using System.Collections.Generic;
using Rascar.Toolbox.Utilities;
using UnityEngine;

namespace Rascar.Toolbox.Extensions
{
    public static class IntExtensions
    {
        /// <summary>
        /// Returns the bit indices equal to 1 in the bitmask.<br/>
        /// Layers are stored in a bitmask, where each layer has its own bit and can thus be translated in the power of 2 corresponding to that bit, allowing to get from that single integer a combination of multiple layers.<br/>
        /// For instance, if the bitmask 68 is 64 + 4 then the corresponding layers are 3 and 6.
        /// </summary>
        public static IEnumerable<int> GetBitMaskIndices(this int bitMask)
        {
            for (int bitIndex = 0; bitMask >= Mathf.Pow(2, bitIndex); bitIndex++)
            {
                if ((bitMask & (1 << bitIndex)) != 0)
                {
                    yield return bitIndex;
                }
            }
        }

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
