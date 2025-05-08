using System;
using System.Diagnostics;

namespace Rascar.Toolbox.Attributes
{
    /// <summary>
    /// Hides the target class from the asset creation dropdown from CreateAssetContextMenu, just for convenience.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    [Conditional("UNITY_EDITOR")]
    public sealed class HideInCreateAssetDropdownAttribute : Attribute { }
}
