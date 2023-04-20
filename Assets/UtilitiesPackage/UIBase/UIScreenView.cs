using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Master.UIKit
{
    public class UIScreenView : UIBase
    {
        [HideInInspector]
        public Image Background;
        [HideInInspector]
        public RectTransform Parent;
        public bool BackKeyActive = false;

        public UIScreenView previousScreen = null;

        UIAnimator _uiAnimator;
        public override void OnAwake()
        {
            base.OnAwake();
            Background = transform.Find(BACKGROUND).GetComponent<Image>();
            Parent = transform.Find(PARENT).GetComponent<RectTransform>();
            _uiAnimator = GetComponent<UIAnimator>();
            Events.OnScreenChange += ToggleInteraction;
        }
        private void OnDestroy()
        {
            Events.OnScreenChange -= ToggleInteraction;
        }
        public override void OnScreenShowCalled()
        {
            base.OnScreenShowCalled();
            ToggleCanvas(true);
        }

        public override void OnScreenShowAnimationCompleted()
        {
            base.OnScreenShowAnimationCompleted();
            ToggleInteraction(true);
        }
        public override void OnBack()
        {
            base.OnBack();
            Debug.Log(transform.name + " Screen view back call.");
        }
        Coroutine BackKeyRoutine;

        public override void OnScreenHideCalled()
        {
            base.OnScreenHideCalled();
            ToggleInteraction(false);

            if (_uiAnimator == null)
            {
                OnScreenHideAnimationCompleted();
            }
        }

        public override void OnScreenHideAnimationCompleted()
        {
            base.OnScreenHideAnimationCompleted();
            ToggleCanvas(false);
            if (previousScreen != null)
            {
                previousScreen.ToggleInteraction(true);
                previousScreen = null;
            }
        }

        IEnumerator BackKeyUpdateRoutine()
        {
            yield return new WaitForSeconds(.5f);
            while (BackKeyActive)
            {

                if (Input.GetKeyDown(KeyCode.Escape) && BackKeyActive)// if (Keyboard.current.escapeKey.wasPressedThisFrame)
                {
                    BackKeyActive = false;
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
        public void ToggleInteraction(bool isActive)
        {
            ToggleRaycaster(isActive);
            BackKeyActive = isActive;
            if (isActive) 
                BackKeyRoutine = StartCoroutine(BackKeyUpdateRoutine());
        }
       
    }
}