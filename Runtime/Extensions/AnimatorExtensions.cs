using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor.Animations;
#endif
using UnityEngine;

// TODO: Instead of still relying on strings (and thus not gaining any perf or ease of use), we could provide an enum value as the argument and internally cache the parameters' ids in a map.
// Either we generate that enum class according to the animator's parameters, or we just update it manually (it has to be in Samples to be editable)
namespace Rascar.Toolbox.Extensions
{
    public static class AnimatorExtensions
    {
        private static readonly Dictionary<string, int> _parameterNameIdMap = new();

        public static bool HasParameter(this Animator animator, string parameterName)
        {
            foreach (AnimatorControllerParameter param in animator.parameters)
            {
                if (param.name == parameterName)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool HasParameter(this Animator animator, int parameterHashId)
        {
            foreach (AnimatorControllerParameter param in animator.parameters)
            {
                if (param.nameHash == parameterHashId)
                {
                    return true;
                }
            }

            return false;
        }

        public static void ResetAllTriggerParameters(this Animator animator)
        {
            foreach (AnimatorControllerParameter parameter in animator.parameters)
            {
                if (parameter.type == AnimatorControllerParameterType.Trigger)
                {
                    animator.ResetTriggerParameter(parameter.name);
                }
            }
        }

        public static void SetTriggerParameter(this Animator animator, string cachedParameterName)
        {
            animator.SetTrigger(animator.GetCachedAnimatorParameterId(cachedParameterName));
        }

        public static void ResetTriggerParameter(this Animator animator, string cachedParameterName)
        {
            animator.ResetTrigger(animator.GetCachedAnimatorParameterId(cachedParameterName));
        }

        public static bool GetBooleanParameter(this Animator animator, string cachedParameterName)
        {
            return animator.GetBool(animator.GetCachedAnimatorParameterId(cachedParameterName));
        }

        public static void SetBooleanParameter(this Animator animator, string cachedParameterName, bool itIsTrue)
        {
            animator.SetBool(animator.GetCachedAnimatorParameterId(cachedParameterName), itIsTrue);
        }

        public static float GetFloatParameter(this Animator animator, string cachedParameterName)
        {
            return animator.GetFloat(animator.GetCachedAnimatorParameterId(cachedParameterName));
        }

        public static void SetFloatParameter(this Animator animator, string cachedParameterName, float value)
        {
            animator.SetFloat(animator.GetCachedAnimatorParameterId(cachedParameterName), value);
        }

        public static float GetIntegerParameter(this Animator animator, string cachedParameterName)
        {
            return animator.GetInteger(animator.GetCachedAnimatorParameterId(cachedParameterName));
        }

        public static void SetIntegerParameter(this Animator animator, string cachedParameterName, int value)
        {
            animator.SetInteger(animator.GetCachedAnimatorParameterId(cachedParameterName), value);
        }

        public static void ToggleBooleanParameter(this Animator animator, string cachedParameterName)
        {
            int parameterId = animator.GetCachedAnimatorParameterId(cachedParameterName);

            animator.SetBool(parameterId, !animator.GetBool(parameterId));
        }

#if UNITY_EDITOR
        public static void ClearAllTransitions(this Animator animator)
        {
            AnimatorController animationController = animator.runtimeAnimatorController as AnimatorController;

            if (animationController == null)
            {
                Debug.LogWarning($"{animator.name} : No AnimationController set.");

                return;
            }

            foreach (AnimatorControllerLayer layerToClear in animationController.layers)
            {
                foreach (ChildAnimatorState state in layerToClear.stateMachine.states)
                {
                    foreach (AnimatorStateTransition transition in state.state.transitions)
                    {
                        transition.hasExitTime = false;
                        transition.exitTime = 1f;

                        transition.hasFixedDuration = false;
                        transition.duration = 0f;
                    }
                }
            }
        }
#endif

        private static int GetCachedAnimatorParameterId(this Animator animator, string parameterName)
        {
            if (!_parameterNameIdMap.TryGetValue(parameterName, out int parameterValue))
            {
                parameterValue = Animator.StringToHash(parameterName);
                _parameterNameIdMap.Add(parameterName, parameterValue);
            }

            Debug.Assert(animator.HasParameter(parameterValue), $"Animator '{animator.name}' has no parameters '{parameterName}'");

            return parameterValue;
        }
    }
}
