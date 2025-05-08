using UnityEngine;

namespace Rascar.Toolbox.SerializableComparables
{
    public enum EComparisonOperator
    {
        [InspectorName("!=")]
        NotEqual = 0,
        [InspectorName("==")]
        Equal = 1,
        [InspectorName("<")]
        Lower = 2,
        [InspectorName("<=")]
        LowerOrEqual = 3,
        [InspectorName(">=")]
        HigherOrEqual = 4,
        [InspectorName(">")]
        Higher = 5,
    }
}
