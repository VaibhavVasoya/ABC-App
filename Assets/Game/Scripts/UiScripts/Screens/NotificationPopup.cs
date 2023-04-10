using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Master.UIKit;
using UnityEngine;
using UnityEngine.UI;

public class NotificationPopup : MonoBehaviour
{
    [SerializeField] MovePanelAnimate movePanelAnimate;
    [SerializeField] Text txtMsg;
    Canvas _canvas;
    //bool isReadyToHide;
    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
    }
    public void Show(string str)
    {
        //UIController.instance.getCurrentScreen().
        //isReadyToHide = true;
        StartCoroutine("BackKeyForNotification");
        UIController.instance.getScreen(UIController.instance.getCurrentScreen()).isBackWorking = false;
        txtMsg.text = str;
        _canvas.enabled = true;
        movePanelAnimate.ShowAnimation();
        //Invoke("Hide", 4);
    }

    void Hide()
    {
        movePanelAnimate.HideAnimation(() => _canvas.enabled = false);
        UIController.instance.getScreen(UIController.instance.getCurrentScreen()).isBackWorking = true;
        //isReadyToHide = false;
        StopCoroutine("BackKeyForNotification");
    }

    public void HideNotification()
    {
        //CancelInvoke("Hide");
        Hide();
    }

    public void OnClickOk()
    {
        UIController.instance.getScreen(UIController.instance.getCurrentScreen()).isBackWorking = true;
        if (UIController.instance.getCurrentScreen() == ScreenType.PoiDetails)
        {
            UIController.instance.getScreen(ScreenType.PoiDetails).GetComponent<ScreenPoiDetail>().SetPoiDetails();
        }
        else if (UIController.instance.GetLastOpenScreen() == ScreenType.Menu)
        {
            MenuHide();
        }
        else
        {
            UIController.instance.ShowNextScreen(ScreenType.PoiDetails);
        }
        Hide();
    }

    async void MenuHide()
    {
        UIController.instance.HideScreen(ScreenType.Menu);
        await Task.Delay(TimeSpan.FromSeconds(.5f));
        UIController.instance.ShowNextScreen(ScreenType.PoiDetails);
    }

    IEnumerator BackKeyForNotification()
    {
        while (true)
        {

            if (Input.GetKeyDown(KeyCode.Escape))// if (Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                Hide();
                yield return new WaitForSeconds(1f);
            }

            yield return null;
        }
    }
}
