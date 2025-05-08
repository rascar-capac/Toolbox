using System;
using UnityEditor;
using UnityEditor.IMGUI.Controls;

namespace Rascar.Toolbox.Editor.Dropdowns.Items
{
    public sealed class TypeDropdownItem : AdvancedDropdownItem
    {
        public Type Type { get; }

        public TypeDropdownItem(Type type) : base(ObjectNames.NicifyVariableName(type.Name))
        {
            Type = type;
        }
    }
}
