using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Master.UIKit;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ScreenPoiDetail : UIScreenView
{

    Poi poi;
    [SerializeField] ImageLoader bg;
    [SerializeField] Text txtname;
    [SerializeField] TextMeshProUGUI txtDescription;

    [SerializeField] Transform otherPoisParent;
    [SerializeField] GameObject otherPoi;

    [SerializeField] ScrollRect scrollRect;

    [SerializeField] GameObject ObjCarousel;
    [SerializeField] GameObject imgPrefab;
    [SerializeField] GameObject dotTrackPrefab;
    [SerializeField] GameObject carouselImgParent, dotParent;
    [SerializeField] ScrollControl scrollControl;
    SwipeControl swipeControl;

    [SerializeField] Button listenHereBtn;
    [SerializeField] GameObject AudioPlayerParent;

    //[SerializeField] UnityVideoController unityVideoController;

    public ContentSizeFitter[] contentSizeFitters;

    [SerializeField] PhysicalTrailAudioPlayer ourAudioPlayer;

    string details = "";

    List<Transform> imgs;
    List<Toggle> dots;

    private void Start()
    {
        imgs = new List<Transform>();
        dots = new List<Toggle>();
        contentSizeFitters = transform.GetComponentsInChildren<ContentSizeFitter>();
        Array.Reverse(contentSizeFitters);
        swipeControl = ObjCarousel.GetComponent<SwipeControl>();
    }
    public override void OnScreenShowCalled()
    {
        base.OnScreenShowCalled();
        if (otherPoisParent.childCount == 0 && ApiHandler.instance.data.trailPois.Count != 0)
        {
            foreach (var item in ApiHandler.instance.data.trailPois)
            {
                if (item.intro_id != TrailsHandler.instance.CurrentTrail.num) continue;
                GameObject obj = Instantiate(otherPoi, otherPoisParent);
                obj.GetComponent<PoiItemPreview>().SetData(item.num, item.Name, item.thumbnail);
            }
        }
        SetPoiDetails();
        swipeControl.canSwipe = true;
        ourAudioPlayer.AssignAudioClip();
        ResetNextLocation();
        isVisited();
    }

    public override void OnScreenShowAnimationCompleted()
    {
        base.OnScreenShowAnimationCompleted();

    }

    public override void OnScreenHideCalled()
    {
        base.OnScreenHideCalled();
        swipeControl.canSwipe = false;
        ListenHereDefaultState();
    }
    public override void OnScreenHideAnimationCompleted()
    {
        for (int i = otherPoisParent.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(otherPoisParent.GetChild(i).gameObject);
        }
        base.OnScreenHideAnimationCompleted();
        StopCoroutine("AutoScrollImages");
        Clear();
        //unityVideoController.RemovePlayer();
        scrollRect.verticalNormalizedPosition = 1f;
    }
    public override void OnBack()
    {
        base.OnBack();
        BackToPhysicalPoiList();
        if (Screen.orientation != ScreenOrientation.Portrait)
        {
            UIController.instance.ChangeOrientation(ScreenOrientation.Portrait);
        }
    }
    public void BackToPhysicalPoiList()
    {
        UIController.instance.ShowNextScreen(ScreenType.Poi);
        TrailsHandler.instance.CurrentTrailPoi = null;
    }

    public void SetPoiDetails()
    {
        poi = TrailsHandler.instance.CurrentTrailPoi;
        bg.Downloading(poi.num, poi.thumbnail);
        txtname.text = poi.Name;
        txtDescription.text = details = poi.description;
        //bookingUrl = poi.booking_url;
        if (poi.poi_images.Count > 1)
        {
            bg.gameObject.SetActive(false);
            ObjCarousel.SetActive(true);
            LoadImages();

        }
        else
        {
            bg.gameObject.SetActive(true);
            bg.Downloading(poi.num, poi.thumbnail);
            ObjCarousel.SetActive(false);
        }
        Refresh();
    }

    public void OpenQuiz()
    {
        UIController.instance.ShowNextScreen(ScreenType.Quiz);
    }

    void isVisited()
    {
        //foreach (var item in SavedDataHandler.instance._saveData.mySculptures)
        //{
        if (SavedDataHandler.instance._saveData.mySculptures.Exists(x => x.Num == TrailsHandler.instance.CurrentTrailPoi.num))
        {
            Debug.Log("in ");
            if (!SavedDataHandler.instance._saveData.mySculptures.Find(x => x.Num == TrailsHandler.instance.CurrentTrailPoi.num).IsVisited)
            {
                //notificationPopup.Show("You are approaching the " + sculp.title + " sculpture");
                SavedDataHandler.instance._saveData.mySculptures.Find(x => x.Num == TrailsHandler.instance.CurrentTrailPoi.num).IsVisited = true;
            }

        }
        //}

    }

    public void OnListenHereClick()
    {
        AudioPlayerParent.SetActive(true);
        listenHereBtn.interactable = false;
    }

    public void ListenHereDefaultState()
    {
        ourAudioPlayer.StopAudio();
        AudioPlayerParent.SetActive(false);
        listenHereBtn.interactable = true;
    }
    //public void OnClickMoreInfo()
    //{
    //    if (!string.IsNullOrEmpty(webSite) && bookingUrl.ToLower() != "na")
    //        Application.OpenURL(webSite);
    //    else
    //    {
    //        UIController.instance.ShowPopupMsg("Oops!", "More info URL is Empty.");
    //    }
    //}
    //public void OpenMobilenumber()
    //{
    //    Application.OpenURL("tel:" + mobileNumber);

    //}
    //public void OpneMail()
    //{
    //    Application.OpenURL("mailto:" + email);
    //}
    //public void OpneBookingUrl()
    //{
    //    Application.OpenURL(bookingUrl);
    //}
    public void OpenMapScreen()
    {
        if (string.IsNullOrEmpty(TrailsHandler.instance.CurrentTrailPoi.latitude) || string.IsNullOrEmpty(TrailsHandler.instance.CurrentTrailPoi.longitude))
            UIController.instance.ShowPopupMsg("Oops!", "Unable to determine trail location.");
        else
            UIController.instance.ShowNextScreen(ScreenType.PoiMap);
    }
    public void Play360Video()
    {
        if (!string.IsNullOrEmpty(TrailsHandler.instance.CurrentTrailPoi.video_360_file))
        {
            UIController.instance.ShowNextScreen(ScreenType.Video360);
            UIController.instance.getScreen(ScreenType.Video360).GetComponent<Screen360VideoUI>().DownloadAndLoadVideo(TrailsHandler.instance.CurrentTrailPoi.num, TrailsHandler.instance.CurrentTrailPoi.video_360_file, TrailsHandler.instance.CurrentTrailPoi.audio_file);
        }
        else if (!string.IsNullOrEmpty(TrailsHandler.instance.CurrentTrailPoi.video_360_url))
        {
            Application.OpenURL(TrailsHandler.instance.CurrentTrailPoi.video_360_url);
        }
        else
            UIController.instance.ShowPopupMsg("Oops!", "360 video not available.");
    }

    public void Play360Image()
    {
        Debug.Log("PlayImage called 1");
        if (!string.IsNullOrEmpty(TrailsHandler.instance.CurrentTrailPoi.image_360_file))
        {
            Debug.Log("PlayImage called 1:1");
            UIController.instance.ShowNextScreen(ScreenType.Image360);
        }
        else if (!string.IsNullOrEmpty(TrailsHandler.instance.CurrentTrailPoi.image_360_url))
        {
            Debug.Log("PlayImage called 1:2");
            Application.OpenURL(TrailsHandler.instance.CurrentTrailPoi.image_360_url);
        }
        else
        {
            Debug.Log("PlayImage called 1:3");
            UIController.instance.ShowPopupMsg("Oops!", "360 image not available.");

        }
    }



    //public void PlayGuidence()
    //{
    //    if (string.IsNullOrEmpty(TrailsHandler.instance.CurrentTrailPoi.transparent_video))
    //        UIController.instance.ShowPopupMsg("Oops!", "Guidance not available.");
    //    else
    //        UIController.instance.ShowNextScreen(ScreenType.VideoPlayerCamera);
    //}
    [SerializeField] ScrollRect nextLocationScrollRect;
    void ResetNextLocation()
    {
        nextLocationScrollRect.horizontalNormalizedPosition = 1f;
    }
    async void Refresh()
    {
        contentSizeFitters = transform.GetComponentsInChildren<ContentSizeFitter>();
        Array.Reverse(contentSizeFitters);
        await UIController.instance.RefreshContent(contentSizeFitters);
        scrollRect.verticalNormalizedPosition = 1f;
    }

    //public void ReadMoreToggle()
    //{
    //    if (txtReadMore.text == "ReadMore")
    //    {
    //        txtReadMore.text = "ReadLess";

    //    }
    //    else
    //    {
    //        txtReadMore.text = "ReadMore";
    //    }
    //}

    public async void LoadImages()
    {
        StopCoroutine("AutoScrollImages");
        Clear();

        Toggle toggle = null;
        foreach (var item in poi.poi_images)
        {
            GameObject go = Instantiate(imgPrefab, carouselImgParent.transform);
            go.GetComponent<CarouselPage>().LoadImage(item);

            GameObject dot = Instantiate(dotTrackPrefab, dotParent.transform);
            toggle = dot.GetComponent<Toggle>();
            toggle.group = dotParent.GetComponent<ToggleGroup>();

            EventTrigger trigger = dot.GetComponent<EventTrigger>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerUp;
            entry.callback.AddListener((eventData) => { scrollControl.OnOpenScreen(trigger.transform.GetSiblingIndex()); });

            trigger.triggers.Add(entry);

            imgs.Add(go.transform);
            dots.Add(toggle);
        }
        await Task.Delay(TimeSpan.FromSeconds(0.2f));
        scrollControl.isNextWelcomeScreen = false;
        scrollControl.Init(imgs, dots);
        StartCoroutine("AutoScrollImages");
    }
    [SerializeField] float time;
    IEnumerator AutoScrollImages()
    {
        if (poi.poi_images.Count <= 1) yield break;
        while (true)
        {
            yield return new WaitForSeconds(time);
            scrollControl.IsImageTouchManual();
            scrollControl.BtnRight();
        }
    }
    public void StopAutoScrollImage()
    {
        StopCoroutine("AutoScrollImages");
    }

    void Clear()
    {
        if (imgs.Count > 0)
        {
            foreach (var img in imgs)
            {
                DestroyImmediate(img.gameObject);
            }
        }
        if (dots.Count > 0)
        {
            foreach (var dot in dots)
            {
                DestroyImmediate(dot.gameObject);
            }
        }
        imgs.Clear();
        dots.Clear();
    }
}

