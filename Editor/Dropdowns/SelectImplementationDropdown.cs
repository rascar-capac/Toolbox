using System;
using System.Collections.Generic;
using Rascar.Toolbox.Editor.Dropdowns.Items;
using Rascar.Toolbox.Extensions;
using UnityEditor.IMGUI.Controls;

namespace Rascar.Toolbox.Editor.Dropdowns
{
    /// <summary>
    /// Dropdown that allows to search and select an implementation of a given type.
    /// </summary>
    public class SelectImplementationDropdown : AdvancedDropdown
    {
        protected readonly IEnumerable<Type> _implementations;

        private readonly Type _baseType;

        public event Action<Type> OnImplementationSelected;

        public SelectImplementationDropdown(AdvancedDropdownState state, Type baseType, Func<Type, bool> implementationDelegate) : base(state)
        {
            _baseType = baseType;
            _implementations = _baseType.GetImplementations(implementationDelegate);
        }

        protected override AdvancedDropdownItem BuildRoot()
        {
            AdvancedDropdownItem root = new("Root");

            foreach (Type type in _implementations)
            {
                root.AddChild(new TypeDropdownItem(type));
            }

            return root;
        }

        protected override void ItemSelected(AdvancedDropdownItem item)
        {
            if (item is TypeDropdownItem dropdownItem)
            {
                OnImplementationSelected?.Invoke(dropdownItem.Type);
            }
        }
    }

    public class SelectImplementationDropdown<TBase> : SelectImplementationDropdown
    {
        public SelectImplementationDropdown(AdvancedDropdownState state, Func<Type, bool> implementationDelegate) : base(state, typeof(TBase), implementationDelegate) { }
    }
}
