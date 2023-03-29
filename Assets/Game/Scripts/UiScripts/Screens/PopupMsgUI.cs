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

        Action callBack = null;

        public override void OnScreenHideAnimationCompleted()
        {
            base.OnScreenHideAnimationCompleted();
            txtMsg.text = txtTitle.text = "";
        }
        public override void OnBack()
        {
            base.OnBack();
            ClosePopup();
        }

        public void SetMsg(string title, string msg, Action callback = null)
        {
            txtTitle.text = title;
            txtMsg.text = msg;
            callBack = callback;
        }

        public void ClosePopup()
        {
            callBack?.Invoke();
            UIController.instance.HideScreen(ScreenType.PopupMSG);
        }
    }
}