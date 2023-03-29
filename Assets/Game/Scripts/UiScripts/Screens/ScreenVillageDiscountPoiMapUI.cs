using System.Collections;
using System.Collections.Generic;
using Master.UIKit;
using UnityEngine;
using UnityEngine.UI;

public class ScreenVillageDiscountPoiMapUI : UIScreenView
{
    //[SerializeField] ImageLoader discountPoiImg;
    //[SerializeField] Text txtDiscountPoiName;
    //[SerializeField] Text txtDiscountPoiSubDesc;

    //[SerializeField] OnlineMapsRawImageTouchForwarder onlineMapsRawImageTouchForwarder;
    //[SerializeField] float Zoom_offset = 0;

    //VillageDiscount discountPoi;
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
    //public override void OnBack()
    //{
    //    base.OnBack();
    //    BackToDiscountDetailsScreen();
    //}
    //public void BackToDiscountDetailsScreen()
    //{
    //    UIController.instance.ShowNextScreen(ScreenType.VillageDiscDetails);
    //}

    //public async void LoadMapScean()
    //{
    //    discountPoi = TrailsHandler.instance.villageDiscount;
    //    if (discountPoi == null)
    //    {
    //        UIController.instance.ShowPopupMsg("Oops!!", "Unable to determine trail location.");
    //        return;
    //    }
    //    discountPoiImg.Downloading(discountPoi.num, discountPoi.image);
    //    txtDiscountPoiName.text = discountPoi.business_name;
    //    txtDiscountPoiSubDesc.text = discountPoi.short_description;
        
    //    await MapController.instance.LoadMapScean(onlineMapsRawImageTouchForwarder, new Vector2(float.Parse(discountPoi.longitude), float.Parse(discountPoi.latitude)), Zoom_offset);
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
    //    MapController.instance.OpenMapViaLink(discountPoi.longitude, discountPoi.latitude);
    //}


}
