using UnityEngine;

namespace Rascar.Toolbox.Serialization
{
    /// <summary>
    /// Wrapper used to reference objects that implement a specific interface.
    /// </summary>
    [System.Serializable]
    public struct SerializableInterface<TInterface> where TInterface : class
    {
        [SerializeField] private Object _object;

        public readonly TInterface Get()
        {
            return _object as TInterface;
        }

        public readonly bool TryGet(out TInterface resultInterfaceObject)
        {
            resultInterfaceObject = null;

            if (_object is TInterface interfaceObject)
            {
                resultInterfaceObject = interfaceObject;
            }

            return resultInterfaceObject != null;
        }
    }
}
