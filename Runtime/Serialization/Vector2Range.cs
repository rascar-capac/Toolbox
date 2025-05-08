using System;
using UnityEngine;

namespace Rascar.Toolbox.Serialization
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class Vector2Range : PropertyAttribute
    {
        private readonly float _minValue;
        private readonly float _maxValue;

        public float MinValue => _minValue;
        public float MaxValue => _maxValue;

        public Vector2Range(float minValue, float maxValue)
        {
            _minValue = minValue;
            _maxValue = maxValue;
        }
    }
}
