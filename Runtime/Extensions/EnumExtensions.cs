using System;
using System.ComponentModel;
using System.Reflection.Emit;

namespace Rascar.Toolbox.Extensions
{
    public static class EnumExtensions
    {
        /// <summary>
        /// This is a fast and allocation free alternative to <see cref="Enum.HasFlag"/>. It is faster than Enum.HasFlag but slower than equivalent bitwise operations.<br/>
        /// <see href="https://devblogs.microsoft.com/premier-developer/dissecting-new-generics-constraints-in-c-7-3/"/>
        /// </summary>
        public static bool HasFlags<TEnum>(this TEnum left, TEnum right) where TEnum : unmanaged, Enum
        {
            return (CreateConvertToLong<TEnum>()(left) & CreateConvertToLong<TEnum>()(right)) != 0;
        }

        private static Func<TEnum, long> CreateConvertToLong<TEnum>() where TEnum : struct, Enum
        {
            DynamicMethod method = new
            (
                name: "ConvertToLong",
                returnType: typeof(long),
                parameterTypes: new[] { typeof(TEnum) },
                m: typeof(EnumConverter).Module,
                skipVisibility: true
            );

            ILGenerator ilGenerator = method.GetILGenerator();
            {
                ilGenerator.Emit(OpCodes.Ldarg_0);
                ilGenerator.Emit(OpCodes.Conv_I8);
                ilGenerator.Emit(OpCodes.Ret);
            }

            return (Func<TEnum, long>)method.CreateDelegate(typeof(Func<TEnum, long>));
        }
    }
}
