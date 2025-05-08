using UnityEngine;

namespace Rascar.Toolbox.Attributes
{
    /// <summary>
    /// Displays the content of the referenced scriptable object.
    /// </summary>
    public class ExposedScriptableObjectAttribute : PropertyAttribute
    {
        public bool _itMustDisplayObjectSelector = true;

        public ExposedScriptableObjectAttribute(bool itMustDisplayObjectSelector = true)
        {
            _itMustDisplayObjectSelector = itMustDisplayObjectSelector;
        }
    }
}
