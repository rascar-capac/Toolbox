using System.Collections.Generic;
using UnityEngine;

namespace Rascar.Toolbox.Utilities
{
    public static class BitUtils
    {
        /// <summary>
        /// Returns the bit indices equal to 1 in a bitmask.<br/>
        /// Layers are stored in a bitmask, where each layer has its own bit and can thus be translated in the power of 2 corresponding to that bit, allowing to get from that single integer a combination of multiple layers.<br/>
        /// For instance, if the bitmask 68 is 64 + 4 then the corresponding layers are 3 and 6.
        /// </summary>
        public static IEnumerable<int> GetBitMaskIndices(int bitMask)
        {
            for (int bitIndex = 0; bitMask >= Mathf.Pow(2, bitIndex); bitIndex++)
            {
                if (BitMaskIndexIsTrue(bitMask, bitIndex))
                {
                    yield return bitIndex;
                }
            }
        }

        /// <summary>
        /// Returns whether the provided index is set to true in a bitmask.<br/>
        /// Layers are stored in a bitmask, where each layer has its own bit and can thus be translated in the power of 2 corresponding to that bit, allowing to get from that single integer a combination of multiple layers.<br/>
        /// For instance, if the bitmask 68 is 64 + 4 then the corresponding layers are 3 and 6.
        /// </summary>
        public static bool BitMaskIndexIsTrue(int bitMask, int index)
        {
            return bitMask == (bitMask | 1 << index);
        }

        public static bool IsSameSign(this int input, int other, bool zeroIsSame = false)
        {
            return MathUtils.IsSameSign(input, other, zeroIsSame);
        }
    }
}
