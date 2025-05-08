#if UNITY_LOCALIZATION_INSTALLED
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.SmartFormat.Core.Extensions;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

namespace Rascar.Toolbox.Localization.CustomVariables
{
    public abstract class VariableListVariable<TVariable> : IVariable where TVariable : IVariable
    {
        [SerializeField] private List<TVariable> _items;

        public List<TVariable> Value
        {
            get => _items;
            set => _items = value;
        }

        public object GetSourceValue(ISelectorInfo selector)
        {
            return _items.Select(item => item.GetSourceValue(selector)).ToList();
        }
    }

    [DisplayName("List/Localized String List")]
    public class LocalizedStringListVariable : VariableListVariable<LocalizedString> { }
}
#endif