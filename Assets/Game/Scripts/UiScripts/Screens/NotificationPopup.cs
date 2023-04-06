using System.Collections;
using System.Collections.Generic;
using Master.UIKit;
using UnityEngine;
using UnityEngine.UI;

public class NotificationPopup : MonoBehaviour
{
    [SerializeField] MovePanelAnimate movePanelAnimate;
    [SerializeField] Text txtMsg;
    Canvas _canvas;
    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
    }
    public void Show(string str)
    {
        txtMsg.text = str;
        _canvas.enabled = true;
        movePanelAnimate.ShowAnimation();
        //Invoke("Hide", 4);
    }

    void Hide()
    {
        movePanelAnimate.HideAnimation(() => _canvas.enabled = false );
    }

    public void HideNotification()
    {
        //CancelInvoke("Hide");
        Hide();
    }

    public void OnClickOk()
    {
        UIController.instance.ShowNextScreen(ScreenType.PoiDetails);
        Hide();
    }
}
