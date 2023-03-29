using System;
using System.Collections;
using System.Collections.Generic;
using Master.UIKit;
using UnityEngine;
using UnityEngine.UI;

namespace Master.UI
{
    public class ScreenVillageDiscountUI : UIScreenView
    {
        //[SerializeField] Text txtTitle;
        //[SerializeField] GameObject discountCeilPrefab;
        //[SerializeField] Transform contentParent;

        //ContentSizeFitter[] contentSizeFitters;

        //private void Start()
        //{
        //    for (int i = 0; i < contentParent.childCount; i++)
        //    {
        //        Destroy(contentParent.GetChild(i).gameObject);
        //    }
        //}

        //private void OnEnable()
        //{
        //    Events.WebRequestCompleted += VillageDiscountCallBack;
        //}
        //private void OnDisable()
        //{
        //    Events.WebRequestCompleted -= VillageDiscountCallBack;
        //}

        //public override void OnScreenShowCalled()
        //{
        //    base.OnScreenShowCalled();
        //    txtTitle.text = ApiHandler.instance.data.menuList[2].title;
        //}
        //public override void OnScreenShowAnimationCompleted()
        //{
        //    base.OnScreenShowAnimationCompleted();
        //    Refresh();
        //}
        //public override void OnBack()
        //{
        //    base.OnBack();
        //    BackToSculpture();
        //}
        //public void BackToSculpture()
        //{
        //    UIController.instance.ShowNextScreen(ScreenType.TrailList);
        //}

        //void VillageDiscountCallBack(API_TYPE aPI_TYPE, string obj)
        //{
        //    if (aPI_TYPE != API_TYPE.API_DISCOUNTS) return;

        //    for (int i = 0; i < contentParent.childCount; i++)
        //    {
        //        Destroy(contentParent.GetChild(i).gameObject);
        //    }

        //    foreach (var discount in ApiHandler.instance.data.villageDiscounts)
        //    {
        //        GameObject go = Instantiate(discountCeilPrefab, contentParent);
        //        DiscountCeil discountCeil = go.GetComponent<DiscountCeil>();
        //        discountCeil.SetData(discount);
        //    }

        //    contentSizeFitters = transform.GetComponentsInChildren<ContentSizeFitter>();
        //    Array.Reverse(contentSizeFitters);
        //    Refresh();
        //}
        //async void Refresh()
        //{
        //    await UIController.instance.RefreshContent(contentSizeFitters);
        //}


    }

}