using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Master.UIKit
{
    public class UIScreenView : UIBase
    {
        [HideInInspector]
        public Image Background;
        [HideInInspector]
        public RectTransform Parent;
        public bool isBackWorking = true;

        UIAnimator _uiAnimator;
        public override void OnAwake()
        {
            base.OnAwake();
            Background = transform.Find(BACKGROUND).GetComponent<Image>();
            Parent = transform.Find(PARENT).GetComponent<RectTransform>();
            _uiAnimator = GetComponent<UIAnimator>();
        }

        public override void OnScreenShowCalled()
        {
            base.OnScreenShowCalled();
            ToggleCanvas(true);
        }

        public override void OnScreenShowAnimationCompleted()
        {
            base.OnScreenShowAnimationCompleted();
            ToggleRaycaster(true);
            StartStopBackKey(true);
        }
        public override void OnBack()
        {
            base.OnBack();
        }
        Coroutine BackKeyRoutine;

        public override void OnScreenHideCalled()
        {
            base.OnScreenHideCalled();
            ToggleRaycaster(false);
            StartStopBackKey(false);

            if (_uiAnimator == null)
            {
                OnScreenHideAnimationCompleted();
            }
        }

        public override void OnScreenHideAnimationCompleted()
        {
            base.OnScreenHideAnimationCompleted();
            ToggleCanvas(false);
        }

        IEnumerator BackKeyUpdateRoutine()
        {
            yield return new WaitForSeconds(.5f);
            while (true)
            {

                if (Input.GetKeyDown(KeyCode.Escape) && isBackWorking)// if (Keyboard.current.escapeKey.wasPressedThisFrame)
                {
                    OnBack();

                    if(BackKeyRoutine!=null)
                    {
                        StopCoroutine(BackKeyRoutine);
                    }
                    yield return new WaitForSeconds(1f);
                }

                yield return null;
            }
        }

        public void StartStopBackKey(bool isActive)
        {
            if (BackKeyRoutine != null)
            {
                StopCoroutine(BackKeyRoutine);
                BackKeyRoutine = null;
            }
            if (isActive)
                BackKeyRoutine = StartCoroutine(BackKeyUpdateRoutine());
        }
    }
}