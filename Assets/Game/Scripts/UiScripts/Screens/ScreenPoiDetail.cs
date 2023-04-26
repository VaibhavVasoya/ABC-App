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
    [SerializeField] GameObject quiz;
    [SerializeField] GameObject map;
    
    //[SerializeField] UnityVideoController unityVideoController;

    public ContentSizeFitter[] contentSizeFitters;
    bool isCheckFeedBack;
    [SerializeField] PhysicalTrailAudioPlayer ourAudioPlayer;
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

    void ButtonDisable()
    {
        AudioPlayerParent.transform.parent.gameObject.SetActive(!string.IsNullOrEmpty(poi.audio_file));
        quiz.SetActive(poi.questions.Count !=0);
        map.SetActive(!(string.IsNullOrEmpty(poi.latitude) || string.IsNullOrEmpty(poi.longitude)));

    }


    public async void CheckPoiVisited()
    {
        Debug.Log("poi visit check "+ SavedDataHandler.instance._saveData.myTrails.Find(x => x.Num == TrailsHandler.instance.CurrentTrail.num).IsVisited);
        if (SavedDataHandler.instance._saveData.myTrails.Find(x => x.Num == TrailsHandler.instance.CurrentTrail.num).IsVisited) return;
        foreach (var item in ApiHandler.instance.data.trailPois)
        {
            if (SavedDataHandler.instance._saveData.mySculptures.Exists(x => x.IntroId == TrailsHandler.instance.CurrentTrail.num))
            {
                SavedDataHandler.instance._saveData.mySculptures.Find(x => x.Num == TrailsHandler.instance.CurrentTrailPoi.num).IsVisited = true;
            }
        }

        foreach (var item in ApiHandler.instance.data.trailPois)
        {
            if (item.intro_id != TrailsHandler.instance.CurrentTrail.num)
            {
                continue;
            }
            else
            {
            }

            if (SavedDataHandler.instance._saveData.mySculptures.Find(x => x.Num == item.num).IsVisited == false)
            {
                return;
            }
        }

        delayw();

    }


    async void delayw()
    {
        await Task.Delay(4000);
        UIController.instance.ShowNextScreen(ScreenType.Feedback);
        SavedDataHandler.instance._saveData.myTrails.Find(x => x.Num == TrailsHandler.instance.CurrentTrail.num).IsVisited = true;
    }


    public override void OnScreenShowCalled()
    {
        base.OnScreenShowCalled();
        SetPoiDetails();
        isCheckFeedBack = true;
        swipeControl.canSwipe = true;
    }

    public override void OnScreenShowAnimationCompleted()
    {
        base.OnScreenShowAnimationCompleted();
        if (otherPoisParent.childCount == 0 && ApiHandler.instance.data.trailPois.Count != 0)
        {
            foreach (var item in ApiHandler.instance.data.trailPois)
            {
                if (item.intro_id != TrailsHandler.instance.CurrentTrail.num) continue;
                GameObject obj = Instantiate(otherPoi, otherPoisParent);
                obj.GetComponent<PoiItemPreview>().SetData(item.num, item.Name, item.thumbnail);
            }
        }
        ResetNextLocation();
        ourAudioPlayer.AssignAudioClip();
    }

    public override void OnScreenHideCalled()
    {
        base.OnScreenHideCalled();
        swipeControl.canSwipe = false;
        ListenHereDefaultState();
        isCheckFeedBack = false;
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
        txtDescription.text = poi.description;
        if (poi.poi_images.Count > 1)
        {
            bg.gameObject.SetActive(false);
            ObjCarousel.SetActive(true);
            LoadImages();

        }
        else if (poi.poi_images.Count == 1)
        {
            bg.gameObject.SetActive(true);
            bg.Downloading(poi.num, poi.poi_images[0].images);
            ObjCarousel.SetActive(false);
        }
        else
        {
            bg.gameObject.SetActive(true);
            bg.Downloading(poi.num, poi.thumbnail);
            ObjCarousel.SetActive(false);
        }
        ButtonDisable();
        CheckPoiVisited();
        Refresh();
    }

    public void OpenQuiz()
    {
        UIController.instance.ShowNextScreen(ScreenType.Quiz);
    }

    void isVisited()
    {
        if (SavedDataHandler.instance._saveData.mySculptures.Exists(x => x.Num == TrailsHandler.instance.CurrentTrailPoi.num))
        {
            Debug.Log("in ");
            if (!SavedDataHandler.instance._saveData.mySculptures.Find(x => x.Num == TrailsHandler.instance.CurrentTrailPoi.num).IsVisited)
            {
                SavedDataHandler.instance._saveData.mySculptures.Find(x => x.Num == TrailsHandler.instance.CurrentTrailPoi.num).IsVisited = true;
            }

        }
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
   
    public void OpenMapScreen()
    {
        if (!Services.CheckInternetConnection()) return;
        if (string.IsNullOrEmpty(TrailsHandler.instance.CurrentTrailPoi.latitude) || string.IsNullOrEmpty(TrailsHandler.instance.CurrentTrailPoi.longitude))
        {
            Debug.Log("123456897 poidetail open map popup");
            UIController.instance.ShowPopupMsg("Oops!", "Unable to determine trail location.", "Ok");
        }
        else
            UIController.instance.ShowNextScreen(ScreenType.PoiMap);
    }
    
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

