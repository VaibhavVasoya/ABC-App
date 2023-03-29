using System.Collections;
using System.Collections.Generic;
using Master.UIKit;
using UnityEngine;
using UnityEngine.UI;

public class ScreenEventPoiMapUI : UIScreenView
{
    [SerializeField] ImageLoader eventPoiImg;
    [SerializeField] Text txtEventPoiName;
    [SerializeField] Text txtEventPoiSubDesc;

    [SerializeField] OnlineMapsRawImageTouchForwarder onlineMapsRawImageTouchForwarder;
    [SerializeField] float Zoom_offset = 0;

    SculptureEvent eventPoi;
    private void Start()
    {
        //Zoom_offset = (1 - UIController.AspectRatio) * -2.5f;
    }
    public override void OnScreenShowCalled()
    {
        base.OnScreenShowCalled();
        LoadMapScean();
    }
    public override void OnScreenHideCalled()
    {
        base.OnScreenHideCalled();
        RemoveMapScean();
    }
    public override void OnBack()
    {
        base.OnBack();
        BackToEventDetailsScreen();
    }
    public void BackToEventDetailsScreen()
    {
        UIController.instance.ShowNextScreen(ScreenType.EventsDetails);
    }

    public async void LoadMapScean()
    {
        eventPoi = TrailsHandler.instance.sculptureEvent;
        if (eventPoi == null)
        {
            UIController.instance.ShowPopupMsg("Oops!!", "Unable to determine trail location.");
            return;
        }
        eventPoiImg.Downloading(eventPoi.num, eventPoi.main_image);
        txtEventPoiName.text = eventPoi.name;
        txtEventPoiSubDesc.text = eventPoi.short_desc;

        await MapController.instance.LoadMapScean(onlineMapsRawImageTouchForwarder, new Vector2(float.Parse(eventPoi.logitude), float.Parse(eventPoi.latitude)), Zoom_offset);
    }
    void RemoveMapScean()
    {
        MapController.instance.RemoveMapScean();
    }
    public void ResetZoom()
    {
        MapController.instance.ResetZoom(Zoom_offset);
    }

    public void OpenMapViaLink()
    {
        MapController.instance.OpenMapViaLink(eventPoi.logitude, eventPoi.latitude);
    }


}
