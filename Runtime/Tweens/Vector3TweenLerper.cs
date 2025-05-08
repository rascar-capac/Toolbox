using DG.Tweening;
using UnityEngine;

namespace Rascar.Toolbox.Tweens
{
    public sealed class Vector3TweenLerper : BaseTweenLerper<Vector3>
    {
        protected override void Setup(Vector3 startLerpValue, Vector3 endLerpValue, float duration, float startingProgress, out Tween lerpTween)
        {
            _currentLerpValue = Vector3.Lerp(startLerpValue, endLerpValue, startingProgress);
            lerpTween = DOTween.To(() => _currentLerpValue, newValue => _currentLerpValue = newValue, endLerpValue, duration);
        }
    }

}
