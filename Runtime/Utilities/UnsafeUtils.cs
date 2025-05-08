using Unity.Collections.LowLevel.Unsafe;

namespace Rascar.Toolbox.Utilities
{
    public static class UnsafeUtils
    {
        public static TUnmanaged AsUnmanaged<TValue, TUnmanaged>(TValue value)
            where TValue : unmanaged
            where TUnmanaged : unmanaged
        {
            if (UnsafeUtility.SizeOf<TUnmanaged>() != UnsafeUtility.SizeOf<TValue>())
            {
                throw new System.Exception($"type mismatch: cannot convert {typeof(TValue)} to {typeof(TUnmanaged)}");
            }

            return UnsafeUtility.As<TValue, TUnmanaged>(ref value);
        }

        public static TValue FromUnmanaged<TValue, TUnmanaged>(TUnmanaged value)
            where TValue : unmanaged
            where TUnmanaged : unmanaged
        {
            if (UnsafeUtility.SizeOf<TValue>() != UnsafeUtility.SizeOf<TUnmanaged>())
            {
                throw new System.Exception($"type mismatch: cannot convert {typeof(TUnmanaged)} to {typeof(TValue)}");
            }

            return UnsafeUtility.As<TUnmanaged, TValue>(ref value);
        }
    }
}
