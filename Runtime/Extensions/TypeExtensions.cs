using System;
using System.Collections.Generic;
using Rascar.Toolbox.Utilities;

namespace Rascar.Toolbox.Extensions
{
    public static class TypeExtensions
    {
        public static IEnumerable<Type> GetImplementations(this Type baseType, Func<Type, bool> implementationDelegate = null)
        {
            return ReflectionUtils.GetImplementations(baseType, implementationDelegate);
        }
    }
}
