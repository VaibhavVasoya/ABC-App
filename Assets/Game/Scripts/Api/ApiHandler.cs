using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Master;
using SimpleJSON;
using Master.UIKit;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using System.IO;
using System;
using Master.UI;
//using UnityEngine.Localization.Settings;

public class ApiHandler : Singleton<ApiHandler>
{
    public ApiData data;

    public MultiLanguage currentLanguage;

    bool isWalkthroughLoaded, isTrailsLoaded, isPoisLoaded, isEventsLoaded, isDiscountLoded, isTrailCatLoaded;
    public bool isReadyToStartSplash = false;
    public override void OnAwake()
    {
        base.OnAwake();
        DontDestroyOnLoad(this);
        //Application.targetFrameRate = 60;
    }

    IEnumerator Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        ClearData();
        //Debug.Log("start");
        //yield return new WaitForSeconds(2);
        while (Application.internetReachability == NetworkReachability.NotReachable)
        {
            //Debug.Log("in while");
            ShowInternetMessage();
            //UIController.instance.ShowPopupMsg("Internet Connection", "Please check your connection and try again!!", "Ok");
            yield return new WaitForSeconds(2);
        }
        isReadyToStartSplash = true;
        //Debug.Log("fff");
        //UIController.instance.HideScreen(ScreenType.PopupMSG);
        //GetAppIcon();
        GetLanguageList();
    }

    //async void Start()
    //{
    //    ClearData();

    //    await Task.Delay(TimeSpan.FromSeconds(Time.deltaTime));
    //    GetAppIcon();
    //    GetLanguageList();
    //    //LoadInitData();
    //}

    /// <summary>
    /// static data
    /// </summary>
    public string GetIconUrl(IconKey iconKey, string defaultVal)
    {
        AppIcon appIcon = data.appIcons.Find(x => x.icon_meta == iconKey.ToString());
        if (appIcon != null)
        {
            return appIcon.icon_value;
        }
        else
        {
            Debug.Log("appIcon was null");
            return "";
        }
    }


    public void ClearData()
    {
        DownloadFiles.Clear();
        data.walkThroughs.Clear();
        data.menuList.Clear();
        data.trailCats.Clear();
        data.trails.Clear();
        data.trailPois.Clear();
        data.sculptureEvents.Clear();
        data.villageDiscounts.Clear();
        data.aboutUs = null;
        data.aboutTheApp = null;
        data.shareWithOther = null;
        isWalkthroughLoaded = isTrailsLoaded = isPoisLoaded = isEventsLoaded = isDiscountLoded = isTrailCatLoaded = false;
    }
    public List<ItemType> DownloadFiles;
    public bool AllDataFetched = false;
    public async void LoadInitData()
    {
        ClearData();
        AllDataFetched = false;
        GetMenuList();
        GetWalkthroughScreen();
        GetTrailCat(false);
        GetTrailList(false);
        GetTrailPois(false);
        GetSculptureEvetns(false);
        GetVillageDiscount(false);
        GetAboutUsDetails(false);
        GetAboutThisApp(false);
        GetFeedback();
        Debug.Log("Load init data..");
        PrepareForDownloding();
    }
    //this is portadown
    void AddFileForDownlod(ItemType itemType)
    {
        if (itemType.fileType == FileType.IMAGE)
        {
            if (!ImageExist(itemType.prefix, itemType.url))
            {
                DownloadFiles.Add(itemType);
            }
        }
        else if (itemType.fileType != FileType.NONE)
        {
            if (!VideoAudioExist(itemType.prefix, itemType.url, (itemType.fileType != FileType.AUDIO)))
            {
                DownloadFiles.Add(itemType);
            }
        }
        else
        {
            Debug.Log("File Not Match");
        }
    }

    bool ImageExist(string prefix, string url)
    {
        return Services.ImageFileExist(prefix, url);
    }

    bool VideoAudioExist(string prefix, string url, bool isVideoFile)
    {
        if (isVideoFile && string.IsNullOrEmpty(Path.GetExtension(url)))
            return true;

        string fileName = isVideoFile ? Path.GetFileName(url) : Path.GetFileNameWithoutExtension(url);
        return Services.FileExist(prefix, url, isVideoFile);
    }


    async void PrepareForDownloding()
    {
        while (!isWalkthroughLoaded || !isTrailsLoaded || !isPoisLoaded || !isEventsLoaded || !isDiscountLoded || !isTrailCatLoaded)
        {
            await Task.Delay(TimeSpan.FromSeconds(1));
        }
        if (DownloadFiles == null) DownloadFiles = new List<ItemType>();
        DownloadFiles.Clear();
        foreach (var item in data.walkThroughs)
        {
            AddFileForDownlod(new ItemType(FileType.IMAGE, item.num, item.image));
        }
        foreach (var item in data.trailCats)
        {
            AddFileForDownlod(new ItemType(FileType.IMAGE, item.num, item.image));
        }
        foreach (var item in data.trails)
        {
            AddFileForDownlod(new ItemType(FileType.IMAGE, item.num, item.cover_image));
        }
        foreach (var item in data.trailPois)
        {
            AddFileForDownlod(new ItemType(FileType.IMAGE, item.num, item.thumbnail));
            AddFileForDownlod(new ItemType(FileType.VIDEO, item.num, item.transparent_video));
            AddFileForDownlod(new ItemType(FileType.VIDEO, item.num, item.standard_video_file_url));
            AddFileForDownlod(new ItemType(FileType.VIDEO, item.num, item.video_360_file));
            AddFileForDownlod(new ItemType(FileType.AUDIO, item.num, item.audio_file));
            foreach (var img in item.poi_images)
            {
                AddFileForDownlod(new ItemType(FileType.IMAGE, img.num, img.images));
            }
        }

        foreach (var item in data.sculptureEvents)
        {
            AddFileForDownlod(new ItemType(FileType.IMAGE, item.num, item.main_image));
            foreach (var img in item.multiple_images)
            {
                AddFileForDownlod(new ItemType(FileType.IMAGE, img.num, img.image));
            }
        }
        foreach (var item in data.villageDiscounts)
        {
            AddFileForDownlod(new ItemType(FileType.IMAGE, item.num, item.image));
        }
        AddFileForDownlod(new ItemType(FileType.IMAGE, "1", data.aboutUs.about_image));
        AddFileForDownlod(new ItemType(FileType.IMAGE, "11", data.aboutUs.below_content_image));
        AddFileForDownlod(new ItemType(FileType.IMAGE, "2", data.aboutTheApp.about_app_image));
        AddFileForDownlod(new ItemType(FileType.IMAGE, "22", data.aboutTheApp.below_content_image));
        Debug.Log("<color=green>Start Downloding data..</color> " + DownloadFiles.Count + "</color>");

        UIController.instance.getScreen(ScreenType.PrepareForLaunch).GetComponent<ScreenPrepareForLaunch>().StartDownloading(DownloadFiles.Count);
        Debug.Log("Start download ");
        if (DownloadFiles.Count > 0)
        {
            Debug.Log("load bundle ");
            //Services.LoadBundlesParellel(DownloadFiles);
            await Services.GetDataSize(DownloadFiles);
            UIController.instance.ShowPopUpDownloadSize("", "We need to download further content (" + Services.totalDataSize.ToString("n2") + "MB) to complete your journey.");
        }
        //AllDataFetched = true;
        //await Task.Delay(TimeSpan.FromSeconds(3));
        Debug.Log("All Data Loaded");
    }
    // Create 

    bool isEnableMsg = false;
    bool isEnter = false;
    public async void ShowInternetMessage()
    {
        if (isEnter) return;
        isEnter = true;
        Canvas _canvas = UIController.instance.getScreen(ScreenType.PopupMSG).GetComponent<Canvas>();
        while (isEnter)
        {
            if (!isEnableMsg)
            {
                isEnableMsg = true;
                Services.CheckInternetConnection();
            }
            else
                isEnter = isEnableMsg = (_canvas == null) ? false : _canvas.enabled;
            await Task.Delay(TimeSpan.FromSeconds(1));
        }
        isEnter = isEnableMsg = false;
    }

    /// <summary>
    /// GET Walktheough Screens
    /// </summary>
    public async void GetLanguageList()
    {
        //while (Application.internetReachability == NetworkReachability.NotReachable)
        //{
        //    ShowInternetMessage();
        //    await Task.Delay(TimeSpan.FromSeconds(1));
        //}
        if (data.multiLanguages.Count > 0)
        {
            foreach (var item in data.multiLanguages)
            {
                if (item.texture2D == null)
                    await item.DonwloadAssets();
            }
            Events.OnWebRequestComplete(API_TYPE.API_MULTI_LANGUAGE, null);
            return;
        }
        Services.Get(GameData.API_MULTI_LANGUAGE, MultiLanguageCallBack, false, true, true);
    }
    async void MultiLanguageCallBack(string obj)
    {
        var res = ParseResponse(obj);
        if (res == null) return;

        if (data.multiLanguages == null)
            data.multiLanguages = new List<MultiLanguage>();

        data.multiLanguages.Clear();
        for (int i = 0; i < res["data"].Count; i++)
        {
            MultiLanguage ml = new MultiLanguage();
            ml = JsonUtility.FromJson<MultiLanguage>(res["data"][i].ToString());
            data.multiLanguages.Add(ml);
        }
        foreach (var item in data.multiLanguages)
        {
            await item.DonwloadAssets();
        }
        Events.OnWebRequestComplete(API_TYPE.API_MULTI_LANGUAGE, obj);
        LoadingUI.instance.OnScreenHide();
    }

    /// <summary>
    /// GET Walktheough Screens
    /// </summary>
    public void GetMenuList()
    {
        Debug.Log("Fetch menu Data...");
        if (data.menuList.Count > 0)
        {
            Events.OnWebRequestComplete(API_TYPE.API_MENU, null);
            return;
        }
        Services.Get(GameData.API_MENULIST, MenuListCallBack, false, true, true);
    }
    void MenuListCallBack(string obj)
    {
        var res = ParseResponse(obj);
        if (res == null) return;

        if (data.menuList == null)
            data.menuList = new List<Menu>();

        data.menuList.Clear();

        for (int i = 0; i < res["data"].Count; i++)
        {
            Menu menu = new Menu();
            menu = JsonUtility.FromJson<Menu>(res["data"][i].ToString());
            data.menuList.Add(menu);
        }

        Events.OnWebRequestComplete(API_TYPE.API_MENU, obj);
        LoadingUI.instance.OnScreenHide();
    }

    JSONNode ParseResponse(string obj)
    {
        if (obj != null)
        {
            var res = JSON.Parse(obj);

            if (res["status"] != "success")
            {
                Debug.LogError("Invalid API Response.");
                UIController.instance.ShowPopupMsg("Error", res["message"].ToString(), "Ok");
                LoadingUI.instance.OnScreenHide();
                return null;
            }

            return res;
        }
        else
        {
            UIController.instance.ShowPopupMsg("Unknown Error", "We couldn't connect you to the server.\nTry again later.", "Ok");
        }
        LoadingUI.instance.OnScreenHide();
        return null;
    }

    /// <summary>
    /// GET Walktheough Screens
    /// </summary>
    public void GetWalkthroughScreen()
    {
        if (data.walkThroughs.Count > 0)
        {
            Events.OnWebRequestComplete(API_TYPE.API_LOADINGSCREEN, null);
            return;
        }   
        Services.Get(GameData.API_LOADINGSCREEN, FetchWalkthroughScreenCallBack, false, true, true);
    }
    void FetchWalkthroughScreenCallBack(string obj)
    {
        if (obj != null)
        {
            var res = JSON.Parse(obj);

            if (res["status"] != "success")
            {
                Debug.LogError("Invalid API Response.");
                UIController.instance.ShowPopupMsg("Error", res["message"].ToString(), "Ok");
            }
            else
            {
                //Data.loginData = JsonUtility.FromJson<LoginData>(res.ToString());
                if (data.walkThroughs == null)
                    data.walkThroughs = new List<WalkThrough>();
                data.walkThroughs.Clear();
                for (int i = 0; i < res["data"].Count; i++)
                {
                    WalkThrough wt = new WalkThrough();
                    wt = JsonUtility.FromJson<WalkThrough>(res["data"][i].ToString());
                    data.walkThroughs.Add(wt);
                }
                //foreach (var item in data.walkThroughs)
                //{
                //    item.DonwloadAssets();
                //}
                Events.OnWebRequestComplete(API_TYPE.API_LOADINGSCREEN, obj);
            }
        }
        else
        {
            UIController.instance.ShowPopupMsg("Unknown Error", "We couldn't connect you to the server.\nTry again later.", "Ok");
        }
        isWalkthroughLoaded = true;
        LoadingUI.instance.OnScreenHide();
    }

    /// <summary>
    /// GET Sculpture Trail "trails.php"
    /// Scupture trial details like: name, short description, banner, etc..
    /// </summary>
    public void GetTrailList(bool showLoading = true)
    {
        if (data.trails.Count > 0)
        {
            Events.OnWebRequestComplete(API_TYPE.API_TRAILS, null);
            //foreach (var item in data.trails)
            //{
            //    if (item.texture2D == null)
            //        item.DonwloadAssets();
            //}
            return;
        }
        if (showLoading) LoadingUI.instance.OnScreenShow();
        Services.Get(GameData.API_TRAILS, TrailsListCallBack, false, false);
    }
    void TrailsListCallBack(string obj)
    {
        var res = ParseResponse(obj);
        if (res != null)
        {
            if (data.trails == null)
                data.trails = new List<Trail>();
            data.trails.Clear();
            for (int i = 0; i < res["data"].Count; i++)
            {
                Trail trails = new Trail();
                //Debug.LogError(res["data"][i].ToString());
                trails = JsonUtility.FromJson<Trail>(res["data"][i].ToString());
                data.trails.Add(trails);
            }
            foreach (var item in data.trails)
            {
                item.Parse();
                //item.DonwloadAssets();
            }
        }
        Events.OnWebRequestComplete(API_TYPE.API_TRAILS, obj);
        LoadingUI.instance.OnScreenHide();
        isTrailsLoaded = true;
    }

    /// <summary>
    /// GET Sculpture Trail "trails.php"
    /// Scupture trial poi details like: FindMe, AboutMe, Quiz, EnhanceMe, etc..
    /// </summary>
    public void GetTrailCat(bool showLoading = true)
    {
        if (data.trailCats.Count > 0)
        {
            Events.OnWebRequestComplete(API_TYPE.API_TRAIL_CAT, null);
            return;
        }
        if (showLoading) LoadingUI.instance.OnScreenShow();
        Services.Get(GameData.API_TRAIL_CAT, TrailsCatCallBack, false, false);
    }
    void TrailsCatCallBack(string obj)
    {
        var res = ParseResponse(obj);
        if (res != null)
        {
            if (data.trailCats == null)
                data.trailCats = new List<TrailCat>();
            data.trailCats.Clear();
            for (int i = 0; i < res["data"].Count; i++)
            {
                TrailCat trailCat = new TrailCat();
                trailCat = JsonUtility.FromJson<TrailCat>(res["data"][i].ToString());
                data.trailCats.Add(trailCat);
            }

        }
        Events.OnWebRequestComplete(API_TYPE.API_TRAIL_CAT, obj);
        LoadingUI.instance.OnScreenHide();
        isTrailCatLoaded = true;
    }



    public async void GetTrailPois(bool showLoading = true)
    {
        if (data.trailPois.Count > 0)
        {
            Events.OnWebRequestComplete(API_TYPE.API_TRAIL_POIS, null);
            //foreach (var item in data.trailPois)
            //{
            //    if (item.texture2D == null)
            //        await item.DonwloadAssets();
            //}
            return;
        }
        if (showLoading) LoadingUI.instance.OnScreenShow();
        Services.Get(GameData.API_POIS, TrailPoisCallBack, false, false);
    }
    async void TrailPoisCallBack(string obj)
    {
        var res = ParseResponse(obj);
        if (res != null)
        {

            if (data.trailPois == null)
                data.trailPois = new List<Poi>();
            data.trailPois.Clear();
            for (int i = 0; i < res["data"].Count; i++)
            {
                Poi poi = new Poi();
                poi = JsonUtility.FromJson<Poi>(res["data"][i].ToString());
                data.trailPois.Add(poi);
            }
            //foreach (var item in data.trailPois)
            //{
            //    await item.DonwloadAssets();
            //}
        }
        Events.OnWebRequestComplete(API_TYPE.API_TRAIL_POIS, obj);

        LoadingUI.instance.OnScreenHide();
        isPoisLoaded = true;
    }


    /// <summary>
    /// GET Sculpture Events "events_list.php"
    /// Scupture upcoming event list.
    /// </summary>
    public void GetSculptureEvetns(bool showLoading = true)
    {
        if (data.sculptureEvents.Count > 0)
        {
            //foreach (var item in data.sculptureEvents)
            //{
            //    if(item.texture2D == null)
            //        item.DonwloadAssets();
            //}
            Events.OnWebRequestComplete(API_TYPE.API_EVENTS, null);
            return;
        }
        if (showLoading) LoadingUI.instance.OnScreenShow();
        Services.Get(GameData.API_EVENTS, SculptureEventsCallBack, false, true, true);
    }
    void SculptureEventsCallBack(string obj)
    {
        if (obj != null)
        {
            var res = JSON.Parse(obj);

            if (res["status"] != "success")
            {
                Debug.LogError("Invalid API Response.");
                UIController.instance.ShowPopupMsg("Error", res["message"].ToString(), "Ok");
            }
            else
            {
                Debug.LogError("Event : " + res.ToString());
                //Data.loginData = JsonUtility.FromJson<LoginData>(res.ToString());
                if (data.sculptureEvents == null)
                    data.sculptureEvents = new List<SculptureEvent>();
                data.sculptureEvents.Clear();
                for (int i = 0; i < res["data"].Count; i++)
                {
                    SculptureEvent sculpEvent = new SculptureEvent();
                    //Debug.LogError(res["data"][i].ToString());
                    sculpEvent = JsonUtility.FromJson<SculptureEvent>(res["data"][i].ToString());
                    data.sculptureEvents.Add(sculpEvent);
                }
                //foreach (var item in data.sculptureEvents)
                //{
                //    item.DonwloadAssets();
                //}
                Debug.Log("<color=green>Fetched Events</color>");
                Events.OnWebRequestComplete(API_TYPE.API_EVENTS, obj);
            }
        }
        else
        {
            UIController.instance.ShowPopupMsg("Unknown Error", "We couldn't connect you to the server.\nTry again later.", "Ok");
        }
        LoadingUI.instance.OnScreenHide();
        isEventsLoaded = true;
    }

    /// <summary>
    /// GET Sculpture Events "events_list.php"
    /// Scupture upcoming event list.
    /// </summary>
    public void GetVillageDiscount(bool showLoading = true)
    {
        if (data.villageDiscounts.Count > 0)
        {
            //foreach (var item in data.villageDiscounts)
            //{
            //    if (item.texture2D == null)
            //        item.DonwloadAssets();
            //}
            Events.OnWebRequestComplete(API_TYPE.API_DISCOUNTS, null);
            return;
        }
        if (showLoading) LoadingUI.instance.OnScreenShow();
        Services.Get(GameData.API_DISCOUNTS, VillageDiscountCallBack, false, true, true);
    }
    void VillageDiscountCallBack(string obj)
    {
        if (obj != null)
        {
            var res = JSON.Parse(obj);

            if (res["status"] != "success")
            {
                Debug.LogError("Invalid API Response.");
                UIController.instance.ShowPopupMsg("Error", res["message"].ToString(), "Ok");
            }
            else
            {
                //Debug.LogError(res.ToString());
                if (data.villageDiscounts == null)
                    data.villageDiscounts = new List<VillageDiscount>();
                data.villageDiscounts.Clear();
                for (int i = 0; i < res["data"].Count; i++)
                {
                    VillageDiscount villageDiscount = new VillageDiscount();
                    villageDiscount = JsonUtility.FromJson<VillageDiscount>(res["data"][i].ToString());
                    data.villageDiscounts.Add(villageDiscount);
                }
                //foreach (var item in data.villageDiscounts)
                //{
                //    item.DonwloadAssets();
                //}
                Events.OnWebRequestComplete(API_TYPE.API_DISCOUNTS, obj);
            }
        }
        else
        {
            UIController.instance.ShowPopupMsg("Unknown Error", "We couldn't connect you to the server.\nTry again later.", "Ok");
        }
        LoadingUI.instance.OnScreenHide();
        isDiscountLoded = true;
    }

    /// <summary>
    /// GET About Us "https://portadown.touristwise.co.uk/webservices/master.php?api=about_us"
    /// Scupture trial poi details like: FindMe, AboutMe, Quiz, EnhanceMe, etc..
    /// </summary>
    public void GetAboutUsDetails(bool showLoading = true)
    {
        if (showLoading) LoadingUI.instance.OnScreenShow();
        Services.Get(GameData.API_ABOUT_US, AboutUsCallBack, false, false);
    }
    void AboutUsCallBack(string obj)
    {
        if (obj != null)
        {
            var res = JSON.Parse(obj);

            if (res["status"] != "success")
            {
                Debug.LogError("Invalid API Response.");
                UIController.instance.ShowPopupMsg("Error", res["message"].ToString(), "Ok");
                LoadingUI.instance.OnScreenHide();
                return;
            }
            if (string.IsNullOrEmpty(res["data"].ToString()) || res["data"].ToString() == "null")
                UIController.instance.ShowPopupMsg("Opps!", "Something went wrong, Unable to fetch app link.", "Ok");
            else
            {
                data.aboutUs = new AboutUs();
                data.aboutUs = JsonUtility.FromJson<AboutUs>(res["data"].ToString());

                Events.OnWebRequestComplete(API_TYPE.API_ABOUT_US, obj);
            }
        }
        else
        {
            UIController.instance.ShowPopupMsg("Unknown Error", "We couldn't connect you to the server.\nTry again later.", "Ok");
        }
        LoadingUI.instance.OnScreenHide();
    }

    /// <summary>
    /// GET About This App "https://portadown.touristwise.co.uk/webservices/master.php?api=about_app"
    /// Scupture trial poi details like: FindMe, AboutMe, Quiz, EnhanceMe, etc..
    /// </summary>
    public void GetAboutThisApp(bool showLoading = true)
    {
        if (showLoading) LoadingUI.instance.OnScreenShow();

        Services.Get(GameData.API_ABOUT_THIS_APP, AboutThisAppCallBack, false, false);
    }
    void AboutThisAppCallBack(string obj)
    {

        if (obj != null)
        {
            var res = JSON.Parse(obj);

            if (res["status"] != "success")
            {
                Debug.LogError("Invalid API Response.");
                UIController.instance.ShowPopupMsg("Error", res["message"].ToString(), "Ok");
                LoadingUI.instance.OnScreenHide();
                return;
            }
            if (string.IsNullOrEmpty(res["data"].ToString()) || res["data"].ToString() == "null")
                UIController.instance.ShowPopupMsg("Opps!", "Something went wrong, Unable to fetch app link.", "Ok");
            else
            {
                data.aboutTheApp = new AboutTheApp();
                data.aboutTheApp = JsonUtility.FromJson<AboutTheApp>(res["data"].ToString());

                Events.OnWebRequestComplete(API_TYPE.API_ABOUT_THIS_APP, obj);
            }
        }
        else
        {
            UIController.instance.ShowPopupMsg("Unknown Error", "We couldn't connect you to the server.\nTry again later.", "Ok");
        }
        LoadingUI.instance.OnScreenHide();
    }

    /// <summary>
    /// GET Share with othres "https://portadown.touristwise.co.uk/webservices/master.php?api=share_text"
    /// Share with other share text and share link.
    /// </summary>
    public void GetShareWithOthers(bool showLoading = true)
    {
        if (showLoading) LoadingUI.instance.OnScreenShow();

        Services.Get(GameData.API_SHARE_WITH_OTHER, ShareWithOthersCallBack, false, false, isOnlyEnglish: true);
    }
    void ShareWithOthersCallBack(string obj)
    {
        if (obj != null)
        {
            var res = JSON.Parse(obj);

            if (res["status"] != "success")
            {
                Debug.LogError("Invalid API Response.");
                UIController.instance.ShowPopupMsg("Error", res["message"].ToString(), "Ok");
                LoadingUI.instance.OnScreenHide();
                return;
            }
            if (string.IsNullOrEmpty(res["data"].ToString()) || res["data"].ToString() == "null")
                UIController.instance.ShowPopupMsg("Opps!", "Something went wrong, Unable to fetch app link.", "Ok");
            else
            {
                data.shareWithOther = new ShareWithOther();
                data.shareWithOther = JsonUtility.FromJson<ShareWithOther>(res["data"].ToString());

                Events.OnWebRequestComplete(API_TYPE.API_SHARE_WITH_OTHER, obj);
            }
        }
        else
        {
            UIController.instance.ShowPopupMsg("Unknown Error", "We couldn't connect you to the server.\nTry again later.", "Ok");
        }
        LoadingUI.instance.OnScreenHide();
    }


    public void GetFeedback()
    {
        Debug.Log("call");
        Services.Get(GameData.API_Feedback, FeedbackOptionsCallback, false, false);
    }


    void FeedbackOptionsCallback(string obj)
    {
        if (obj != null)
        {
            var res = JSON.Parse(obj);

            if (res["status"] != "success")
            {
                Debug.LogError("Invalid API Response.");
                UIController.instance.ShowPopupMsg("Error", res["message"].ToString(), "Ok");
            }
            else
            {
                Debug.LogError("Event : " + res.ToString());
                //Data.loginData = JsonUtility.FromJson<LoginData>(res.ToString());
                if (data.feedBackOptions == null)
                    data.feedBackOptions = new List<Feedback>();
                data.feedBackOptions.Clear();
                for (int i = 0; i < res["data"].Count; i++)
                {
                    Feedback feedback = new Feedback();
                    //Debug.LogError(res["data"][i].ToString());
                    feedback = JsonUtility.FromJson<Feedback>(res["data"][i].ToString());
                    data.feedBackOptions.Add(feedback);
                }
                //foreach (var item in data.sculptureEvents)
                //{
                //    item.DonwloadAssets();
                //}
                Debug.Log("<color=green>Fetched Events</color>");
                Events.OnWebRequestComplete(API_TYPE.API_Feedback, obj);
            }
        }
    }
    /// <summary>
    /// Feedback post
    /// Scupture trial details like: name, short description, banner, etc..
    /// </summary>
    public void PostFeedback(string lang_id, string trail_id, string comment_id)
    {
        KVPList<string, string> list = new KVPList<string, string>();
        list.Add("lang_id", lang_id);
        list.Add("trail_id", trail_id);
        list.Add("comment_id", comment_id);
        Services.Post("https://abc.touristwise.co.uk/webservices/comments.php?lang_id=1", list, (var) => { Debug.Log("Feedback submit : " + var); }, false, false);
    }
    //void TrailsListCallBack(string obj)
    //{
    //    var res = ParseResponse(obj);
    //    if (res != null)
    //    {
    //        if (data.trails == null)
    //            data.trails = new List<Trail>();
    //        data.trails.Clear();
    //        for (int i = 0; i < res["data"].Count; i++)
    //        {
    //            Trail trails = new Trail();
    //            //Debug.LogError(res["data"][i].ToString());
    //            trails = JsonUtility.FromJson<Trail>(res["data"][i].ToString());
    //            data.trails.Add(trails);
    //        }
    //        foreach (var item in data.trails)
    //        {
    //            item.Parse();
    //            //item.DonwloadAssets();
    //        }
    //    }
    //    Events.OnWebRequestComplete(API_TYPE.API_TRAILS, obj);
    //    LoadingUI.instance.OnScreenHide();
    //    isTrailsLoaded = true;
    //}

    /// <summary>
    /// App icon
    /// </summary>
    public void GetAppIcon()
    {
        Services.Get(GameData.API_APP_ICON, AppIconCallBack, false, false);
    }
    void AppIconCallBack(string obj)
    {
        var res = ParseResponse(obj);
        if (res != null)
        {
            if (data.appIcons == null)
                data.appIcons = new List<AppIcon>();
            data.appIcons.Clear();
            for (int i = 0; i < res["data"].Count; i++)
            {
                AppIcon appIcon = new AppIcon();
                //Debug.LogError(res["data"][i].ToString());
                appIcon = JsonUtility.FromJson<AppIcon>(res["data"][i].ToString());
                data.appIcons.Add(appIcon);
            }

        }
        Events.OnWebRequestComplete(API_TYPE.API_APP_ICON, obj);
    }

}

public enum VariableName
{
}

public enum IconKey
{
    icon_quiz_incorrect = 0,
    icon_quiz_correct = 1,
    icon_menu_home = 2,
    icon_menu_aboutus = 3,
    icon_menu_aboutapp = 4,
    icon_menu_events = 5,
    map_pin = 6,
    icon_menu_discounts = 7,
    icon_menu_preferences = 8,
    share_icon = 9,
    quiz_icon = 10,
    map_icon = 11,
    listen_icon = 12,
    list_icon = 13,
    icon_happy = 14,
    icon_menu_trails = 15,
    ar_icon = 16,
    icon_360 = 17

}