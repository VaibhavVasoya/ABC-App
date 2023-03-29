using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Master.UIKit;
using UnityEngine;
using UnityEngine.UI;
using Master.UI;

public class DiscountCeil : MonoBehaviour
{
    [SerializeField] ImageLoader bg;
    [SerializeField] Text txtTitle;
    [SerializeField] Text txtShortDesc;

    VillageDiscount villageDiscount;
    ContentSizeFitter[] contentSizeFitters;

    private void Start()
    {
        contentSizeFitters = transform.GetComponentsInChildren<ContentSizeFitter>();
        Array.Reverse(contentSizeFitters);
    }

    public void SetData(VillageDiscount _villageDiscount)
    {
        contentSizeFitters = transform.GetComponentsInChildren<ContentSizeFitter>();
        villageDiscount = _villageDiscount;
        bg.Downloading(_villageDiscount.num , _villageDiscount.image);
        txtTitle.text = _villageDiscount.business_name;
        txtShortDesc.text = _villageDiscount.short_description;
        Refresh();
    }

    public void OpenDiscountDetails()
    {
        TrailsHandler.instance.villageDiscount = villageDiscount;
        //UIController.instance.getScreen(ScreenType.VillageDiscDetails).GetComponent<ScreenVillageDiscountDetailsUI>().SetDiscountDetails(villageDiscount);
        UIController.instance.ShowNextScreen(ScreenType.VillageDiscDetails);
    }

    async void Refresh()
    {
        await Task.Delay(TimeSpan.FromSeconds(0.2f));
        await UIController.instance.RefreshContent(contentSizeFitters);
    }
}
