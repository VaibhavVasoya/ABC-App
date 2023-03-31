using System;
using TMPro;
using System.Collections.Generic;
using System.Threading.Tasks;
using Master.UIKit;
using UnityEngine;
//using UnityEngine.Localization.Settings;
using UnityEngine.UI;

namespace Master.UI
{
    public class ScreenVillageDiscountDetailsUI : UIScreenView
    {
        //VillageDiscount _villageDiscount;

        //[SerializeField] ImageLoader bg;
        //[SerializeField] Text txtTitle;
        //[SerializeField] Text txtShorDesc;
        //[SerializeField] TextMeshProUGUI txtDescription;
        //[SerializeField] Text txtDescriptionLess;
        //[SerializeField] Text txtMobile;
        //[SerializeField] Text txtEmail;
        //[SerializeField] Text txtAddress;
        //[SerializeField] Text txtStartDate;
        //[SerializeField] Text txtReadMoreBtn;
        //[SerializeField] Transform errow;
        //ContentSizeFitter[] contentSizeFitters;

        //[SerializeField] ScrollRect scrollRect;

        //[SerializeField] int DescriptionLength = 100;

        //[SerializeField] RectTransform detailMaskRect;
        //[SerializeField] RectTransform detailRect;

        //float initHeight;
        //float targetHeight;

        //string description;
        //bool isShareInit = false;
        //string shareStr;

      

        //public override void OnAwake()
        //{
        //    contentSizeFitters = transform.GetComponentsInChildren<ContentSizeFitter>();
        //    Array.Reverse(contentSizeFitters);
        //    base.OnAwake();
        //    initHeight = detailMaskRect.sizeDelta.y;
        //}

        //public override void OnScreenShowCalled()
        //{
        //    Events.WebRequestCompleted += ShareWithOtherCallBack;
        //    SetDiscountDetails();
        //    base.OnScreenShowCalled();
        //}
        //public override void OnScreenShowAnimationCompleted()
        //{
        //    Events.WebRequestCompleted -= ShareWithOtherCallBack;
        //    base.OnScreenShowAnimationCompleted();
        //    //Refresh();
        //}
        //public override void OnBack()
        //{
        //    base.OnBack();
        //    BackToVillageDiscount();
        //}
        //public void BackToVillageDiscount()
        //{
        //    TrailsHandler.instance.villageDiscount = null;
        //    UIController.instance.ShowNextScreen(ScreenType.VillageDiscount);
            
        //}

        //public async void SetDiscountDetails()
        //{
        //    _villageDiscount = TrailsHandler.instance.villageDiscount;
        //    bg.Downloading(_villageDiscount.num, _villageDiscount.image);

        //    txtTitle.text = _villageDiscount.business_name;
        //    txtShorDesc.text = _villageDiscount.short_description;
        //    description = UIController.instance.HtmlToStringParse(_villageDiscount.full_description);
        //    txtDescription.text = description;
        //    //if (description.Length > DescriptionLength)
        //    //    txtDescriptionLess.text = description.Substring(0, DescriptionLength) + "...";
        //    //txtReadMoreBtn.transform.parent.gameObject.SetActive(description.Length > DescriptionLength);
        //    isSamllContent = false;
        //    //detailMaskRect.sizeDelta = new Vector2(detailMaskRect.sizeDelta.x, initHeight);
        //    //targetHeight = detailRect.rect.height;
        //    //d = 0.1f;
        //    //OnClickReadMoreAndLess();
        //    txtMobile.text = _villageDiscount.tel;
        //    txtEmail.text = _villageDiscount.email;
        //    txtAddress.text = _villageDiscount.address;

            
        //    if (!string.IsNullOrEmpty(_villageDiscount.end_date) && _villageDiscount.end_date.Length > 4)
        //    {
        //        txtStartDate.transform.parent.gameObject.SetActive(true);
        //        Debug.LogError("Start Date : " + _villageDiscount.end_date);
        //        txtStartDate.text = Helper.DateConvert(_villageDiscount.end_date).ToString("dd MMM");
        //    }
        //    else
        //        txtStartDate.transform.parent.gameObject.SetActive(false);

        //    Refresh();
        //    //await Task.Delay(TimeSpan.FromSeconds(0.4f));
            
        //    //targetHeight = detailRect.rect.height;
        //    //if (description.Length <= DescriptionLength) {
        //    //    Debug.LogError("========>>>> detail obj set");
        //    //    txtDescriptionLess.gameObject.SetActive(false);
        //    //    txtDescription.gameObject.SetActive(true);
        //    //}
        //    //d = 0.15f;
        //}
        
        //public void OnClickOpenMap()
        //{
        //    if (!string.IsNullOrEmpty(_villageDiscount.longitude) && !string.IsNullOrEmpty(_villageDiscount.latitude))
        //    {
        //        Debug.Log("Show Direction on map.");
        //        UIController.instance.ShowNextScreen(ScreenType.DiscountPoiMap);
        //    }
        //    else
        //    {
        //        UIController.instance.ShowPopupMsg("Oops!!", "Unable to determine village discount location.");
        //    }
        //}
        //bool isSamllContent = false;
        //float t = 0, d = 0.15f;
        //public async void OnClickReadMoreAndLess()
        //{
        //    t = 0;
        //    if (isSamllContent)
        //    {
        //        txtDescription.gameObject.SetActive(true);
        //        txtDescriptionLess.gameObject.SetActive(false);
        //    }
        //    //UnityEngine.Localization.Settings.
        //    txtReadMoreBtn.text = (isSamllContent) ? LocalizationSettings.StringDatabase.GetLocalizedString("UI_Text", "Read_Less") : LocalizationSettings.StringDatabase.GetLocalizedString("UI_Text", "Read_More");// "Read Less" : "Read More";
        //    errow.localScale = new Vector3(1, (isSamllContent) ? 1 : -1, 1);
        //    //Debug.LogError("init : " + initHeight + " || target : " + targetHeight);
        //    while (t < d)
        //    {
        //        t += Time.deltaTime;
        //        if (isSamllContent)
        //            detailMaskRect.sizeDelta = new Vector2(detailMaskRect.sizeDelta.x, Mathf.Lerp(initHeight, targetHeight, t / d));
        //        else
        //            detailMaskRect.sizeDelta = new Vector2(detailMaskRect.sizeDelta.x, Mathf.Lerp(targetHeight, initHeight, t / d));
        //        await Task.Delay(TimeSpan.FromSeconds(Time.deltaTime));
        //    }
            
        //    if (!isSamllContent)
        //    {
        //        //Debug.LogError("Animate completes.");
        //        txtDescriptionLess.gameObject.SetActive(true);
        //        txtDescription.gameObject.SetActive(false);
        //    }
        //    isSamllContent = !isSamllContent;
        //}
        
        
        //public void OnClickShare()
        //{
        //    shareStr = "I thought you might be interested in this village discount:\n";
        //    shareStr += $"Name : {_villageDiscount.business_name}\n";
        //    shareStr += $"Start Date : {_villageDiscount.end_date}\n";
        //    shareStr += $"Description : {_villageDiscount.short_description}\n";
        //    shareStr += $"Address : {_villageDiscount.address}\n";
        //    shareStr += $"Telephone : {_villageDiscount.tel}\n";
        //    shareStr += $"Email : {_villageDiscount.email}\n";
        //    shareStr += $"This discount has been shared from the {Application.platform} mobile app, available for download from The ";

        //    if (ApiHandler.instance.data.shareWithOther == null)
        //    {
        //        ApiHandler.instance.GetShareWithOthers();
        //        isShareInit = true;
        //    }
        //    else
        //    {
        //        new NativeShare().SetSubject(Application.productName).SetTitle(_villageDiscount.business_name).SetText(shareStr).SetUrl(ApiHandler.instance.data.shareWithOther.share_link)
        //        .SetCallback((result, shareTarget) => Debug.Log("Share result: " + result + ", selected app: " + shareTarget))
        //        .Share();
        //    }
        //}

        //public void OpenMobilenumber()
        //{
        //    Application.OpenURL("tel:" + _villageDiscount.tel);
        //}
        //public void OpneMail()
        //{
        //    Application.OpenURL("mailto:" + _villageDiscount.email);
        //}

        //async void Refresh()
        //{
        //    await Task.Delay(TimeSpan.FromSeconds(0.2f));
        //    await UIController.instance.RefreshContent(contentSizeFitters);
        //    scrollRect.verticalNormalizedPosition = 1f;
        //}

        //async void ShareWithOtherCallBack(API_TYPE aPI_TYPE, string obj)
        //{
        //    if (aPI_TYPE != API_TYPE.API_SHARE_WITH_OTHER && !isShareInit) return;
        //    Debug.LogError("========>>>>>>>    "+aPI_TYPE);
        //    await Task.Delay(TimeSpan.FromSeconds(0.2f));
        //    isShareInit = false;
        //    new NativeShare().SetSubject(Application.productName).SetTitle(_villageDiscount.business_name).SetText(shareStr).SetUrl(ApiHandler.instance.data.shareWithOther.share_link)
        //    .SetCallback((result, shareTarget) => Debug.Log("Share result: " + result + ", selected app: " + shareTarget))
        //    .Share();
        //}
    }
}