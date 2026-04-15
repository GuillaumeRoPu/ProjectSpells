using System;
using System.Collections;
using UnityEngine;

namespace Core
{
    namespace Menu
    {
        public class Page : MonoBehaviour
        {
            public static readonly string FLAG_ON = "On";
            public static readonly string FLAG_OFF = "Off";
            public static readonly string FLAG_NONE = "None";

            [SerializeField] private PageType _type;
            public PageType Type { get { return _type; } set { _type = value; } }

            [SerializeField] private bool _startOn = true;
            public bool StartOn { get { return _startOn; } set { _startOn = value; } }

            [SerializeField] private bool _debug;
            public bool MyDebug { get { return _debug; } set { _debug = value; } }
                
            [SerializeField] private bool _useAnimation;
            public bool UseAnimation { get { return _useAnimation; } set { _useAnimation = value; } }

            private string _targetState;
            public string TargetState { get { return _targetState; } }

            private Animator _Animator;

            public Action TurnedPageOn;
            public Action TurningPageOff;


            #region Unity Functions

            private void OnEnable()
            {
                CheckAnimatiorIntergrity();
                TurnedPageOn?.Invoke();
            }

            #endregion

            #region Public Functions

            public void Animate(bool on, bool animate = true)
            {
                if (_useAnimation && animate)
                {
                    _Animator.SetBool("on", on);

                    StopCoroutine(nameof(AwaitAnimation));
                    StartCoroutine(nameof(AwaitAnimation), on);
                }
                else
                {
                    if (!on)
                    {
                        TurningPageOff?.Invoke();
                        gameObject.SetActive(false);
                    }
                }
            }

            #endregion

            #region Private Functions

            private IEnumerator AwaitAnimation(bool on)
            {
                _targetState = on ? FLAG_ON : FLAG_OFF;

                if (!on)
                {
                    TurningPageOff?.Invoke();
                }

                // wait for the animator to reach the target state
                while (!_Animator.GetCurrentAnimatorStateInfo(0).IsName(TargetState))
                {
                    yield return null;
                }

                // wait for the animator to finish animating
                while (_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
                {
                    yield return null;
                }

                _targetState = FLAG_NONE;

                Log("Page [" + _type + "] finished transitioning to " + (on ? "on" : "off"));

                if (!on)
                {
                    gameObject.SetActive(false);
                }
            }

            public void CheckAnimatiorIntergrity()
            {
                if (_useAnimation && !_Animator)
                {
                    TryGetComponent(out _Animator);
                    if (!_Animator)
                    {
                        LogWarning("You tried to animate a page [" + _type + "], but there isn't any Animator component on this object !!!");
                    }
                }
            }

            private void Log(string msg)
            {
                if (!_debug)
                {
                    return;
                }
                Debug.Log("[Page] : " + msg);
            }

            private void LogWarning(string msg)
            {
                if (!_debug)
                {
                    return;
                }
                Debug.LogWarning("[Page] : " + msg);
            }

            #endregion
        }

    }
}
