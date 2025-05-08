using UnityEngine;
using UnityEngine.InputSystem;

namespace Rascar.Toolbox.Debugging
{
    public class DebugTimeScaler : MonoBehaviour
    {
        [SerializeField] private InputActionReference _speedUpInput;
        [SerializeField] private InputActionReference _slowDownInput;
        [SerializeField] private InputActionReference _resetInput;
        [SerializeField][Range(1f, 5f)] private float _multiplierOnInput = 2f;
        [SerializeField][Range(0.01f, 10f)] private float _timeScale = 1f;

        private float _initialFixedDeltaTime;

        private void SpeedUpInput_Performed(InputAction.CallbackContext _)
        {
            _timeScale *= _multiplierOnInput;
        }

        private void SlowDownInput_Performed(InputAction.CallbackContext _)
        {
            _timeScale /= _multiplierOnInput;
        }

        private void ResetSpeed_Performed(InputAction.CallbackContext _)
        {
            _timeScale = 1f;
        }

        private void Awake()
        {
            _initialFixedDeltaTime = Time.fixedDeltaTime;

            if (_speedUpInput != null)
            {
                _speedUpInput.action.performed += SpeedUpInput_Performed;
            }

            if (_slowDownInput != null)
            {
                _slowDownInput.action.performed += SlowDownInput_Performed;
            }

            if (_resetInput != null)
            {
                _resetInput.action.performed += ResetSpeed_Performed;
            }
        }

        private void LateUpdate()
        {
            Time.timeScale = _timeScale;
            Time.fixedDeltaTime = _initialFixedDeltaTime * _timeScale;
        }
    }
}
