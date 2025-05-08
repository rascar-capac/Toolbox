#if UNITY_LOCALIZATION_INSTALLED
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.SmartFormat.Core.Extensions;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using PersistentVariables = UnityEngine.Localization.SmartFormat.PersistentVariables;

namespace Rascar.Toolbox.Localization.CustomVariables
{
    public abstract class ListVariable<TItem, TVariable> : IVariable where TVariable : PersistentVariables.Variable<TItem>, new()
    {
        [SerializeField] private List<TItem> _items;

        private List<TVariable> _variableItems;

        public List<TItem> Value
        {
            get => _items;
            set
            {
                _items = value;
                UpdateVariableItems();
            }
        }

        public object GetSourceValue(ISelectorInfo selector)
        {
            if (_variableItems == null)
            {
                UpdateVariableItems();
            }

            return _variableItems.Select(item => item.GetSourceValue(selector)).ToList();
        }

        private void UpdateVariableItems()
        {
            _variableItems ??= new List<TVariable>();
            _variableItems.Clear();

            foreach (TItem item in _items)
            {
                _variableItems.Add(new TVariable { Value = item });
            }
        }
    }

    [DisplayName("List/Boolean List")]
    public class BoolListVariable : ListVariable<bool, BoolVariable> { }

    [DisplayName("List/Signed Byte List")]
    public class SByteListVariable : ListVariable<sbyte, SByteVariable> { }

    [DisplayName("List/Byte List")]
    public class ByteListVariable : ListVariable<byte, ByteVariable> { }

    [DisplayName("List/Short List")]
    public class ShortListVariable : ListVariable<short, ShortVariable> { }

    [DisplayName("List/Unsigned Short List")]
    public class UShortListVariable : ListVariable<ushort, UShortVariable> { }

    [DisplayName("List/Integer List")]
    public class IntListVariable : ListVariable<int, IntVariable> { }

    [DisplayName("List/Unsigned Integer List")]
    public class UIntListVariable : ListVariable<uint, UIntVariable> { }

    [DisplayName("List/Long List")]
    public class LongListVariable : ListVariable<long, LongVariable> { }

    [DisplayName("List/Unsigned Long List")]
    public class ULongListVariable : ListVariable<ulong, ULongVariable> { }

    [DisplayName("List/String List")]
    public class StringListVariable : ListVariable<string, StringVariable> { }

    [DisplayName("List/Float List")]
    public class FloatListVariable : ListVariable<float, FloatVariable> { }

    [DisplayName("List/Double List")]
    public class DoubleListVariable : ListVariable<double, DoubleVariable> { }

    [DisplayName("List/Object Reference List")]
    public class ObjectListVariable : ListVariable<Object, ObjectVariable> { }

}
#endif