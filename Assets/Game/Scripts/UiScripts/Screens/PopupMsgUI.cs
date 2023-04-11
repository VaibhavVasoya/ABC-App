using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Master.UIKit;
using UnityEngine.UI;
using System;

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

        }
        public override void OnScreenHideAnimationCompleted()
        {
            base.OnScreenHideAnimationCompleted();
            txtMsg.text = txtTitle.text = "";
        }
        //public override void OnBack()
        //{
        //    base.OnBack();
        //    ClosePopup();
        //}

        public void SetMsg(string title, string msg,string buttonText, Action callback = null)
        {
            txtTitle.text = title;
            txtMsg.text = msg;
            btnText.text = buttonText;
            callBack = callback;
        }

        public void ClosePopup()
        {
            callBack?.Invoke();
            UIController.instance.HideScreen(ScreenType.PopupMSG);
        }
    }
}