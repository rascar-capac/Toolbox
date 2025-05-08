using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Rascar.Toolbox.Tweens
{
    public abstract class BaseTweenLerper<TValue> : MonoBehaviour
    {
        [SerializeField] private float _duration = 1f;
        [SerializeField] private TValue _startLerpValue;
        [SerializeField] private TValue _endLerpValue;
        [SerializeField] private Ease _easing = Ease.OutQuint;

        private readonly List<OnUpdateCallback> _onUpdateCallbacks = new();

        protected TValue _currentLerpValue;
        private Tween _lerpTween;

        private float _durationModifier = 1f;

        public delegate void OnUpdateCallback(TValue lerpValue);

        protected abstract void Setup(TValue startLerpValue, TValue endLerpValue, float duration, float startingProgress, out Tween lerpTween);

        public Tween StartLerp(float startingProgress = 0f, bool inversed = false)
        {
            Stop();

            TValue startLerpValue = inversed ? _endLerpValue : _startLerpValue;
            TValue endLerpValue = inversed ? _startLerpValue : _endLerpValue;

            float duration = (1f - startingProgress) * _duration * _durationModifier;
            Setup(startLerpValue, endLerpValue, duration, startingProgress, out _lerpTween);

            _lerpTween
                .SetEase(_easing)
                .OnUpdate(InvokeCallbacks)
                .OnComplete(() => Stop());

            return _lerpTween;
        }

        public void Stop()
        {
            if (_lerpTween != null)
            {
                _lerpTween.Kill();
                _lerpTween = null;
            }
        }

        public void SetDuration(float duration)
        {
            _duration = duration;
        }

        public void SetDurationModifier(float modifier)
        {
            _durationModifier = modifier;
        }

        public void SetStartValue(TValue startLerpValue)
        {
            _startLerpValue = startLerpValue;
        }

        public void SetEndValue(TValue endLerpValue)
        {
            _endLerpValue = endLerpValue;
        }

        public void AddUpdateCallback(OnUpdateCallback onUpdateCallback)
        {
            _onUpdateCallbacks.Add(onUpdateCallback);
        }

        public void RemoveUpdateCallback(OnUpdateCallback onUpdateCallback)
        {
            _onUpdateCallbacks.Remove(onUpdateCallback);
        }

        public void ClearUpdateCallbacks()
        {
            _onUpdateCallbacks.Clear();
        }

        private void InvokeCallbacks()
        {
            foreach (OnUpdateCallback onUpdateCallback in _onUpdateCallbacks)
            {
                onUpdateCallback.Invoke(_currentLerpValue);
            }
        }

        private void OnDestroy()
        {
            Stop();
        }
    }
}
