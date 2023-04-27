using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Master.UIKit;
using UnityEngine;
using UnityEngine.UI;

public class NotificationPopup : UIScreenView
{
    //[SerializeField] MovePanelAnimate movePanelAnimate;
    [SerializeField] Text txtMsg;
    [SerializeField] AudioSource audioSource;
    //Canvas _canvas;
    //bool isReadyToHide;
    //private void Awake()
    //{
    //    _canvas = GetComponent<Canvas>();
    //}
    public void SetMsg(string title)
    {
        txtMsg.text = title;
    }

    [EasyButtons.Button]
    public override void OnScreenShowCalled()
    {
        base.OnScreenShowCalled();
        audioSource.Play();
        //UIController.instance.getScreen(UIController.instance.getCurrentScreen()).ToggleInteraction(false);
    }

    public override void OnScreenHideAnimationCompleted()
    {
        base.OnScreenHideAnimationCompleted();
        txtMsg.text = "";
        //UIController.instance.getScreen(UIController.instance.getCurrentScreen()).ToggleInteraction(true);
        //callBack?.Invoke();
    }
    public override void OnBack()
    {
        base.OnBack();
        HideNotification();
    }
    //public void Show(string str)
    //{
    //    //UIController.instance.getCurrentScreen().
    //    //isReadyToHide = true;
    //    StartCoroutine("BackKeyForNotification");
    //    UIController.instance.getScreen(UIController.instance.getCurrentScreen()).ToggleInteraction(false);
    //    txtMsg.text = str;
    //    //_canvas.enabled = true;
    //    movePanelAnimate.ShowAnimation();
    //    //Invoke("Hide", 4);
    //}

    //void Hide()
    //{
    //    //movePanelAnimate.HideAnimation(() => _canvas.enabled = false);
    //    UIController.instance.getScreen(UIController.instance.getCurrentScreen()).ToggleInteraction(true);
    //    //isReadyToHide = false;
    //    StopCoroutine("BackKeyForNotification");
    //}

    public void HideNotification()
    {
        //CancelInvoke("Hide");
        UIController.instance.HidePopup(ScreenType.notificationPopUp);
    }

    public void OnClickOk()
    {
        //UIController.instance.getScreen(UIController.instance.getCurrentScreen()).ToggleInteraction(true);
        if (UIController.instance.getCurrentScreen() == ScreenType.PoiDetails)
        {
            UIController.instance.getScreen(ScreenType.PoiDetails).GetComponent<ScreenPoiDetail>().SetPoiDetails();
        }
        else if (UIController.instance.IsPopupEnable(ScreenType.Menu))
        {
            MenuHide();
        }
        else
        {
            UIController.instance.ShowNextScreen(ScreenType.PoiDetails);
        }
        //Hide();
        UIController.instance.HidePopup(ScreenType.notificationPopUp);
    }

    async void MenuHide()
    {
        UIController.instance.HidePopup(ScreenType.Menu);
        await Task.Delay(TimeSpan.FromSeconds(.5f));
        UIController.instance.ShowNextScreen(ScreenType.PoiDetails);
    }

    //IEnumerator BackKeyForNotification()
    //{
    //    while (true)
    //    {

    //        if (Input.GetKeyDown(KeyCode.Escape))// if (Keyboard.current.escapeKey.wasPressedThisFrame)
    //        {
    //            Hide();
    //            yield return new WaitForSeconds(1f);
    //        }

    //        yield return null;
    //    }
    //}
}
