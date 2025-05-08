using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Rascar.Toolbox.Utilities
{
    public static class ReflectionUtils
    {
        public static bool IsStaticClass(Type type)
        {
            return type.IsClass && type.IsSealed && type.IsAbstract;
        }

        public static bool TryGetEnumerableElementType(Type listType, out Type elementType)
        {
            if (listType.IsArray)
            {
                elementType = listType.GetElementType();

                return true;
            }

            if (listType.IsGenericType && IsAssignableToGenericType(listType, typeof(IEnumerable<>)))
            {
                elementType = listType.GetGenericArguments()[0];

                return true;
            }

            elementType = null;

            return false;
        }

        public static bool IsAssignableToGenericType(Type type, Type genericType)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition().Equals(genericType))
            {
                return true;
            }

            if (genericType.IsInterface)
            {
                Type[] interfaceTypes = type.GetInterfaces();

                foreach (Type interfaceType in interfaceTypes)
                {
                    if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == genericType)
                    {
                        return true;
                    }
                }
            }

            Type baseType = type.BaseType;

            while (baseType != null)
            {
                if (baseType.IsGenericType && baseType.GetGenericTypeDefinition().Equals(genericType))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Gets all types that implement the given type and match the given delegate.
        /// </summary>
        public static IEnumerable<Type> GetImplementations(Type baseType, Func<Type, bool> implementationDelegate = null)
        {
            return AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => baseType.IsAssignableFrom(type) && (implementationDelegate == null || implementationDelegate(type)));
        }


        /// <inheritdoc cref="GetImplementations"/>
        public static IEnumerable<Type> GetImplementations<TBase>(Func<Type, bool> implementationDelegate = null)
        {
            return GetImplementations(typeof(TBase), implementationDelegate);
        }

        /// <summary>
        /// Instantiates all implementations of a type found in the solution that match the given delegate.
        /// </summary>
        public static IEnumerable<TBase> CreateInstances<TBase>(object[] parameters = null, Func<Type, bool> implementationDelegate = null) where TBase : class
        {
            IEnumerable<Type> implementations = GetImplementations<TBase>(type => CanCreateInstanceOfImplementation(type, implementationDelegate));
            IEnumerable<TBase> instances;

            if (parameters == null)
            {
                instances = implementations.Select(type => CreateInstance<TBase>(type));
            }
            else
            {
                instances = implementations.Select(type => CreateInstance<TBase>(type, parameters));
            }

            return instances.Where(instance => instance != null);
        }

        private static TBase CreateInstance<TBase>(Type type) where TBase : class
        {
            try
            {
                return (TBase)Activator.CreateInstance(type);
            }
            catch (Exception exception)
            {
                Debug.LogError($"Unable to activate instance of type {type.Name}: {exception.Message}\n{exception.StackTrace}");

                return null;
            }
        }

        private static TBase CreateInstance<TBase>(Type type, object[] parameters) where TBase : class
        {
            Type[] parameterTypes = parameters.Select(systemObject => systemObject.GetType()).ToArray();

            try
            {
                ConstructorInfo constructorInfo = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, CallingConventions.HasThis, parameterTypes, null);

                return (TBase)constructorInfo.Invoke(parameters);
            }
            catch (Exception exception)
            {
                Debug.LogError($"Unable to activate instance of type {type.Name}: {exception.Message}");

                return null;
            }
        }

        private static bool CanCreateInstanceOfImplementation(Type type, Func<Type, bool> implementationDelegate)
        {
            return !type.IsInterface
                && !type.IsAbstract
                && !type.ContainsGenericParameters
                && (implementationDelegate == null || implementationDelegate(type));
        }
    }
}
