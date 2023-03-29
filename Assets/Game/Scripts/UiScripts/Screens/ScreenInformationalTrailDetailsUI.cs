using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Master.UIKit;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

namespace Master.UI
{
    public class ScreenInformationalTrailDetailsUI : UIScreenView
    {
        //    Poi poi;
        //    [SerializeField] ImageLoader bg;
        //    [SerializeField] Text txtname;
        //    [SerializeField] Text txtShortDesc;
        //    [SerializeField] TextMeshProUGUI txtDescription;

        //    [SerializeField] Transform otherPoisParent;
        //    [SerializeField] GameObject otherPoi;

        //    [SerializeField] ScrollRect scrollRect;

        //    [SerializeField] GameObject ObjCarousel;
        //    [SerializeField] GameObject imgPrefab;
        //    [SerializeField] GameObject dotTrackPrefab;
        //    [SerializeField] GameObject carouselImgParent, dotParent;
        //    [SerializeField] ScrollControl scrollControl;
        //    SwipeControl swipeControl;

        //    [SerializeField] UnityVideoController unityVideoController;

        //    ContentSizeFitter[] contentSizeFitters;

        //    List<Transform> imgs;
        //    List<Toggle> dots;
        //    private void Start()
        //    {
        //        imgs = new List<Transform>();
        //        dots = new List<Toggle>();
        //        contentSizeFitters = transform.GetComponentsInChildren<ContentSizeFitter>();
        //        Array.Reverse(contentSizeFitters);
        //        swipeControl = ObjCarousel.GetComponent<SwipeControl>();
        //    }
        //    public override void OnScreenShowCalled()
        //    {
        //        if (otherPoisParent.childCount == 0 && ApiHandler.instance.data.trailPois.Count != 0)
        //        {
        //            foreach (var item in ApiHandler.instance.data.trailPois)
        //            {
        //                if (item.intro_id != TrailsHandler.instance.CurrentTrail.num) continue;
        //                GameObject obj = Instantiate(otherPoi, otherPoisParent);
        //                obj.GetComponent<PoiItemPreview>().SetData(item.num, item.Name, item.thumbnail);
        //            }
        //        }

        //        SetPoiDetails();
        //        base.OnScreenShowCalled();
        //        swipeControl.canSwipe = true;
        //    }

        //    public override void OnScreenHideCalled()
        //    {
        //        swipeControl.canSwipe = false;
        //        base.OnScreenHideCalled();
        //    }
        //    public override void OnScreenHideAnimationCompleted()
        //    {
        //        for (int i = otherPoisParent.childCount - 1; i >= 0; i--)
        //        {
        //            DestroyImmediate(otherPoisParent.GetChild(i).gameObject);
        //        }
        //        base.OnScreenHideAnimationCompleted();
        //        StopCoroutine("AutoScrollImages");
        //        Clear();
        //        unityVideoController.RemovePlayer();
        //        scrollRect.verticalNormalizedPosition = 1f;
        //    }
        //    public override void OnBack()
        //    {
        //        if (ScreenOrientation.Portrait != Screen.orientation)
        //        {
        //            ChangeOrientation();
        //            return;
        //        }
        //        base.OnBack();
        //        BackToInformationalPoi();
        //        if (Screen.orientation != ScreenOrientation.Portrait)
        //        {
        //            UIController.instance.ChangeOrientation(ScreenOrientation.Portrait);
        //        }
        //    }
        //    void ChangeOrientation()
        //    {
        //        Debug.LogError(Screen.orientation);

        //        Screen.autorotateToPortrait = true;
        //        Screen.autorotateToPortraitUpsideDown = true;
        //        Screen.autorotateToLandscapeLeft = false;
        //        Screen.autorotateToLandscapeRight = false;
        //        Screen.orientation = ScreenOrientation.Portrait;
        //        Screen.orientation = ScreenOrientation.AutoRotation;
        //    }
        //    public void BackToInformationalPoi()
        //    {
        //        UIController.instance.ShowNextScreen(ScreenType.Poi);
        //        TrailsHandler.instance.CurrentTrailPoi = null;
        //    }

        //    public void SetPoiDetails()
        //    {
        //        unityVideoController.RemovePlayer();
        //        poi = TrailsHandler.instance.CurrentTrailPoi;

        //        //video player
        //        if (!string.IsNullOrEmpty(poi.standard_video_file_url))
        //            unityVideoController.LoadPlayer(poi.num, false, poi.standard_video_file_url);
        //        else if (!string.IsNullOrEmpty(poi.standard_video_stream_url))
        //            unityVideoController.LoadPlayer(poi.num, true, poi.standard_video_stream_url);
        //        else
        //        {
        //            unityVideoController.ToggleVideoParent(false);
        //            Debug.LogError("=======>>>>>>> Standard Video url is Null.");
        //        }

        //        txtname.text = poi.Name;
        //        txtShortDesc.text = UIController.instance.HtmlToStringParse(poi.short_desc.ToString());
        //        txtDescription.text = UIController.instance.HtmlToStringParse(poi.description);
        //        webSite = poi.video_360_url;
        //        imgWeb360 = poi.image_360_url;

        //        if (poi.poi_images.Count > 1)
        //        {
        //            bg.gameObject.SetActive(false);
        //            ObjCarousel.SetActive(true);
        //            LoadImages();

        //        }
        //        else
        //        {
        //            bg.gameObject.SetActive(true);
        //            bg.Downloading(poi.num, poi.thumbnail);
        //            ObjCarousel.SetActive(false);
        //        }


        //        Refresh();
        //    }
        //    public void Play360Video()
        //    {
        //        if (!string.IsNullOrEmpty(TrailsHandler.instance.CurrentTrailPoi.video_360_file))
        //        {
        //            UIController.instance.ShowNextScreen(ScreenType.Video360);
        //            UIController.instance.getScreen(ScreenType.Video360).GetComponent<Screen360VideoUI>().DownloadAndLoadVideo(TrailsHandler.instance.CurrentTrailPoi.num, TrailsHandler.instance.CurrentTrailPoi.video_360_file, TrailsHandler.instance.CurrentTrailPoi.audio_file);
        //        }
        //        else if (!string.IsNullOrEmpty(TrailsHandler.instance.CurrentTrailPoi.video_360_url))
        //        {
        //            Application.OpenURL(TrailsHandler.instance.CurrentTrailPoi.video_360_url);
        //        }
        //        else
        //            UIController.instance.ShowPopupMsg("Oops!", "360 video not available.");
        //    }

        //    public void Play360Image()
        //    {
        //        if (!string.IsNullOrEmpty(TrailsHandler.instance.CurrentTrailPoi.image_360_file))
        //        {
        //            UIController.instance.ShowNextScreen(ScreenType.Image360);

        //        }
        //        else if (!string.IsNullOrEmpty(TrailsHandler.instance.CurrentTrailPoi.image_360_url))
        //        {
        //            Application.OpenURL(TrailsHandler.instance.CurrentTrailPoi.image_360_url);
        //        }
        //        else
        //            UIController.instance.ShowPopupMsg("Oops!", "360 image not available.");
        //    }

        //    //public void PlayGuidence()
        //    //{
        //    //    if (string.IsNullOrEmpty(TrailsHandler.instance.CurrentTrailPoi.transparent_video))
        //    //        UIController.instance.ShowPopupMsg("Oops!", "Guidance not available.");
        //    //    else
        //    //        UIController.instance.ShowNextScreen(ScreenType.VideoPlayerCamera);
        //    //}

        //    async void Refresh()
        //    {
        //        await UIController.instance.RefreshContent(contentSizeFitters);
        //        scrollRect.verticalNormalizedPosition = 1f;
        //    }
        //    string webSite = null;
        //    public void Open360Web()
        //    {
        //        if (!string.IsNullOrEmpty(webSite))
        //            Application.OpenURL(webSite);
        //        else
        //        {
        //            UIController.instance.ShowPopupMsg("Oops!", "URL is Empty.");
        //        }
        //    }
        //    string imgWeb360 = null;
        //    public void OpenImage360Web()
        //    {
        //        if (!string.IsNullOrEmpty(imgWeb360))
        //            Application.OpenURL(imgWeb360);
        //        else
        //        {
        //            UIController.instance.ShowPopupMsg("Oops!", "URL is Empty.");
        //        }
        //    }
        //    public void OpenQuizScreen()
        //    {
        //        if (poi.questions.Count > 0)
        //            UIController.instance.ShowNextScreen(ScreenType.Quiz);
        //        else
        //        {
        //            UIController.instance.ShowPopupMsg("Oops!", "Quiz not available.");
        //        }
        //    }
        //    public async void LoadImages()
        //    {
        //        StopCoroutine("AutoScrollImages");
        //        Clear();

        //        Toggle toggle = null;
        //        foreach (var item in poi.poi_images)
        //        {
        //            GameObject go = Instantiate(imgPrefab, carouselImgParent.transform);
        //            go.GetComponent<CarouselPage>().LoadImage(item);

        //            GameObject dot = Instantiate(dotTrackPrefab, dotParent.transform);
        //            toggle = dot.GetComponent<Toggle>();
        //            toggle.group = dotParent.GetComponent<ToggleGroup>();

        //            EventTrigger trigger = dot.GetComponent<EventTrigger>();
        //            EventTrigger.Entry entry = new EventTrigger.Entry();
        //            entry.eventID = EventTriggerType.PointerUp;
        //            entry.callback.AddListener((eventData) => { scrollControl.OnOpenScreen(trigger.transform.GetSiblingIndex()); });

        //            trigger.triggers.Add(entry);

        //            imgs.Add(go.transform);
        //            dots.Add(toggle);
        //        }
        //        await Task.Delay(TimeSpan.FromSeconds(0.2f));
        //        scrollControl.isNextWelcomeScreen = false;
        //        scrollControl.Init(imgs, dots);
        //        StartCoroutine("AutoScrollImages");
        //    }
        //    [SerializeField] float time;
        //    IEnumerator AutoScrollImages()
        //    {
        //        if (poi.poi_images.Count <= 1) yield break;
        //        while (true)
        //        {
        //            yield return new WaitForSeconds(time);
        //            scrollControl.IsImageTouchManual();
        //            scrollControl.BtnRight();
        //        }
        //    }
        //    public void StopAutoScrollImage()
        //    {
        //        StopCoroutine("AutoScrollImages");
        //    }

        //    void Clear()
        //    {
        //        if (imgs.Count > 0)
        //        {
        //            foreach (var img in imgs)
        //            {
        //                DestroyImmediate(img.gameObject);
        //            }
        //        }
        //        if (dots.Count > 0)
        //        {
        //            foreach (var dot in dots)
        //            {
        //                DestroyImmediate(dot.gameObject);
        //            }
        //        }
        //        imgs.Clear();
        //        dots.Clear();
        //    }
    }
}