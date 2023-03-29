using System.Collections;
using System.Collections.Generic;
using Master.UIKit;
using UnityEngine;
using UnityEngine.UI;

public class ScreenPhysicalMapUI : UIScreenView
{
    //[SerializeField] Text txtTrailTitle;

    //[SerializeField] OnlineMapsRawImageTouchForwarder onlineMapsRawImageTouchForwarder;
    //[SerializeField] float Zoom_offset = 0;

    //private void Start()
    //{
    //    Zoom_offset = (1 - UIController.AspectRatio) * -2.5f;
    //}
    //public override void OnScreenShowCalled()
    //{
    //    LoadMapScean();
    //    base.OnScreenShowCalled();
    //}
    //public override void OnScreenHideCalled()
    //{
    //    base.OnScreenHideCalled();
    //    RemoveMapScean();
    //}
    ////public override void OnBack()
    ////{
    ////    base.OnBack();
    ////    BackToPhysicalTrailDetails();
    ////}
    ////public void BackToPhysicalTrailDetails()
    ////{
    ////    UIController.instance.ShowNextScreen(ScreenType.PhysicalPoiDetails);
    ////}

    //public async void LoadMapScean()
    //{
    //    if (TrailsHandler.instance.CurrentTrailPoi == null)// (string.IsNullOrEmpty(TrailsHandler.instance.CurrentTrailPoi.num))
    //    {
    //        UIController.instance.ShowPopupMsg("Oops!!", "Unable to determine trail location.");
    //        return;
    //    }
    //    txtTrailTitle.text = TrailsHandler.instance.CurrentTrailPoi.Name;
    //    if (string.IsNullOrEmpty(TrailsHandler.instance.CurrentTrailPoi.longitude) || string.IsNullOrEmpty(TrailsHandler.instance.CurrentTrailPoi.latitude))
    //    {
    //        UIController.instance.ShowPopupMsg("Oops!!", "Unable to determine trail location.");
    //        return;
    //    }
    //    await MapController.instance.LoadMapScean(onlineMapsRawImageTouchForwarder, new Vector2(float.Parse(TrailsHandler.instance.CurrentTrailPoi.longitude), float.Parse(TrailsHandler.instance.CurrentTrailPoi.latitude)), Zoom_offset);
    //}
    //void RemoveMapScean()
    //{
    //    MapController.instance.RemoveMapScean();
    //}
    //public void ResetZoom()
    //{
    //    MapController.instance.ResetZoom(Zoom_offset);
    //}

    //public void OpenMapViaLink()
    //{
    //    MapController.instance.OpenMapViaLink(TrailsHandler.instance.CurrentTrailPoi.longitude, TrailsHandler.instance.CurrentTrailPoi.latitude);
    //}


}
