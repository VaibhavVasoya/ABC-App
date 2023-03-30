using System.Collections;
using System.Collections.Generic;
using Master.UIKit;
using UnityEngine;
using UnityEngine.UI;

public class ScreenMapUI : UIScreenView
{
    [SerializeField] ImageLoader PoiImg;
    [SerializeField] Text txtEventPoiName;
    [SerializeField] Text txtEventPoiSubDesc;

    [SerializeField] OnlineMapsRawImageTouchForwarder onlineMapsRawImageTouchForwarder;
    [SerializeField] float Zoom_offset = 0;

    Poi eventPoi;
    
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
        UIController.instance.ShowNextScreen(ScreenType.PoiDetails);
    }

    public async void LoadMapScean()
    {
        eventPoi = TrailsHandler.instance.CurrentTrailPoi;
        if (eventPoi == null)
        {
            UIController.instance.ShowPopupMsg("Oops!!", "Unable to determine trail location.");
            return;
        }
        PoiImg.Downloading(eventPoi.num, eventPoi.thumbnail);
        txtEventPoiName.text = eventPoi.Name;
        txtEventPoiSubDesc.text = eventPoi.short_desc;

        await MapController.instance.LoadMapScean(onlineMapsRawImageTouchForwarder, new Vector2(float.Parse(eventPoi.longitude), float.Parse(eventPoi.latitude)), Zoom_offset);
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
        MapController.instance.OpenMapViaLink(eventPoi.longitude, eventPoi.latitude);
    }


}
