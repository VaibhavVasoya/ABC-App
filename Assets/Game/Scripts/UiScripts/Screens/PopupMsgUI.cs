using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Master.UIKit;
using UnityEngine.UI;
using System;
using System.Threading.Tasks;

namespace Master.UI
{
    public class PopupMsgUI : UIScreenView
    {
        [SerializeField] Text txtTitle;
        [SerializeField] Text txtMsg;
        [SerializeField] Text btnText;

        Action callBack = null;
        public override void OnScreenShowCalled()
        {
            base.OnScreenShowCalled();
            UIController.instance.getScreen(UIController.instance.getCurrentScreen()).ToggleInteraction(false);
        }
        public override void OnScreenHideAnimationCompleted()
        {
            base.OnScreenHideAnimationCompleted();
            txtMsg.text = txtTitle.text = "";
            UIController.instance.getScreen(UIController.instance.getCurrentScreen()).ToggleInteraction(true);
            callBack?.Invoke();
        }
        //public override void OnBack()
        //{
        //    base.OnBack();
        //    ClosePopup();
        //}

        public void SetMsg(string title, string msg,string buttonText, Action callback = null)
        {
            //Debug.LogError("====>>>> Popup Msg Set" + transform.localScale);
            txtTitle.text = title;
            txtMsg.text = msg;
            btnText.text = buttonText;
            callBack = callback;
            //Debug.LogError("====>>>> Popup Msg set completed");
        }

        public void ClosePopup()
        {
            //ClosePopUpWait();
            //callBack?.Invoke();
            UIController.instance.HidePopup(ScreenType.PopupMSG);
        }

        async void ClosePopUpWait()
        {
            UIController.instance.HidePopup(ScreenType.PopupMSG);
            await Task.Delay(TimeSpan.FromSeconds(1f));
            //callBack?.Invoke();
        }
    }
    }