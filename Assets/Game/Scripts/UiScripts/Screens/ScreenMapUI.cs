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

    Poi trailPoi;
    
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
        trailPoi = TrailsHandler.instance.CurrentTrailPoi;
        if (trailPoi == null)
        {
            Debug.Log("123456789 onscreen show popup");
            UIController.instance.ShowPopupMsg("Oops!!", "Unable to determine trail location.", "Ok");
            return;
        }
        PoiImg.Downloading(trailPoi.num, trailPoi.thumbnail);
        txtEventPoiName.text = trailPoi.Name;
        txtEventPoiSubDesc.text = trailPoi.short_desc;

        await MapController.instance.LoadMapScean(onlineMapsRawImageTouchForwarder, new Vector2(float.Parse(trailPoi.longitude), float.Parse(trailPoi.latitude)), Zoom_offset);
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
        MapController.instance.OpenMapViaLink(trailPoi.longitude, trailPoi.latitude);
    }


}
