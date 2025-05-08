using DG.Tweening;
using UnityEngine;

namespace Rascar.Toolbox.Tweens
{
    public sealed class FloatTweenLerper : BaseTweenLerper<float>
    {
        protected override void Setup(float startLerpValue, float endLerpValue, float duration, float startingProgress, out Tween lerpTween)
        {
            _currentLerpValue = Mathf.Lerp(startLerpValue, endLerpValue, startingProgress);
            lerpTween = DOTween.To(() => _currentLerpValue, newValue => _currentLerpValue = newValue, endLerpValue, duration);
        }
    }
}
