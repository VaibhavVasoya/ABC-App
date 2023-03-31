using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Master.UIKit;
using UnityEngine;
using UnityEngine.UI;

public class ScreenTrailPois : UIScreenView
{
    [SerializeField] Text txtpoiTitle;
    [SerializeField] GameObject trail_poi_Prefab;
    [SerializeField] Transform contentParent;
    [SerializeField] ScrollRect scrollRect;

    [SerializeField] Toggle poiListToggle;
    [SerializeField] Toggle poiMapToggle;

    [SerializeField] GameObject poiListContentObj;
    [SerializeField] GameObject poiMapContentObj;

    ContentSizeFitter[] contentSizeFitters;

    [SerializeField] OnlineMapsRawImageTouchForwarder onlineMapsRawImageTouchForwarder;
    [SerializeField] float Zoom_offset = -1;

    [SerializeField] ImageLoader poiImg;
    [SerializeField] Text txtpoiName;
    [SerializeField] Text txtpoiSubDesc;
    [SerializeField] MovePanelAnimate poiPanelAnimate;

    int currentSelectedPoiPin = 0;
    [SerializeField] List<Poi> pois;
    private void OnEnable()
    {
        Events.WebRequestCompleted += SculptureTrailPoisCallBack;
    }
    private void OnDisable()
    {
        Events.WebRequestCompleted -= SculptureTrailPoisCallBack;
    }
    public override void OnScreenShowCalled()
    {
        base.OnScreenShowCalled();
        TrailsHandler.instance.CurrentTrailPoi = null;
        poiListToggle.isOn = true;
        OpenTab(poiMapToggle.isOn);
        SculptureTrailPoisCallBack(API_TYPE.API_TRAIL_POIS, "");
    }
    public override void OnScreenHideCalled()
    {
        base.OnScreenHideCalled();
        poiPanelAnimate.HideAnimation();
        RemoveMapScean();
    }
    public override void OnBack()
    {
        base.OnBack();
        BackToTrailList();
    }
    public void BackToTrailList()
    {
        UIController.instance.ShowNextScreen(ScreenType.TrailList);
        Helper.Execute(this, () => OpenTab(false), 0.4f);
        currentSelectedPoiPin = 0;
    }
    // Start is called before the first frame update
    void Start()
    {
        //Zoom_offset = UIController.AspectRatio * -1.8f;
        OpenTab(false);
        for (int i = contentParent.childCount - 1; i >= 0; i--)
        {
            Destroy(contentParent.GetChild(i).gameObject);
        }

        CheckPoiVisited();
    }
    async void CheckPoiVisited()
    {
        return;
        if (!string.IsNullOrEmpty(TrailsHandler.instance.CurrentTrail.num))
        {
            Debug.Log("123 if");
            bool allVisited = true;
            //TrailsHandler.instance.isAllPoiVisited();
            foreach (var item in SavedDataHandler.instance._saveData.mySculptures)
            {
                if (item.IntroId == TrailsHandler.instance.CurrentTrail.num)
                {
                    if (!item.IsVisited)
                    {
                        Debug.Log("123 item name " + item.Title);
                        allVisited = false;
                        break;
                    }
                }
            }
            await Task.Delay(TimeSpan.FromSeconds(2));
            if (allVisited)
            {
                Debug.Log("123 all poi visited");
            }
            else
            {
                Debug.Log("123");
                CheckPoiVisited();
            }
        }
        else
        {
            Debug.Log("123 else");
            await Task.Delay(TimeSpan.FromSeconds(2));
            CheckPoiVisited();
        }
    }
    async void SculptureTrailPoisCallBack(API_TYPE aPI_TYPE, string obj)
    {
        if (aPI_TYPE != API_TYPE.API_TRAIL_POIS) return;
        if (TrailsHandler.instance.CurrentTrail == null) return;
        for (int i = contentParent.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(contentParent.GetChild(i).gameObject);
        }

        await Task.Delay(TimeSpan.FromSeconds(Time.deltaTime));
        foreach (var poi in ApiHandler.instance.data.trailPois)
        {
            if (poi.intro_id != TrailsHandler.instance.CurrentTrail.num) continue;
            GameObject go = Instantiate(trail_poi_Prefab, contentParent);
            PoiCeil Poi = go.GetComponent<PoiCeil>();
            Poi.SetTrail(poi);
        }
        await Task.Delay(TimeSpan.FromSeconds(0.4f));
        Refresh();
    }
    public void OpenTab(bool IsMap)
    {
        poiListContentObj.SetActive(!IsMap);
        poiMapContentObj.SetActive(IsMap);
        poiListToggle.isOn = !IsMap;
        poiMapToggle.isOn = IsMap;
        MapController.instance.MapInteractable(IsMap);
        if (IsMap) LoadMapScean();
        if(IsMap)
        {
            txtpoiTitle.text = "Point of Interest";
        }
        else
        {
            txtpoiTitle.text = "Trails";
        }
    }

    async void Refresh()
    {
        contentSizeFitters = transform.GetComponentsInChildren<ContentSizeFitter>();
        Array.Reverse(contentSizeFitters);
        await UIController.instance.RefreshContent(contentSizeFitters);
        scrollRect.verticalNormalizedPosition = 1f;
    }

    async void LoadMapScean()
    {
        if (TrailsHandler.instance.CurrentTrail == null)// (string.IsNullOrEmpty(TrailsHandler.instance.CurrentTrailPoi.num))
        {
            UIController.instance.ShowPopupMsg("Oops!!", "Unable to determine trail location.");
            return;
        }
        List<Vector2> markers = new List<Vector2>();
        pois = ApiHandler.instance.data.trailPois.FindAll(x => (x.intro_id == TrailsHandler.instance.CurrentTrail.num));
        foreach (var poi in pois)
        {
            Debug.LogError("Add Marker : " + poi.Name);
            if (string.IsNullOrEmpty(poi.longitude) || string.IsNullOrEmpty(poi.latitude)) continue;
            markers.Add(new Vector2(float.Parse(poi.longitude), float.Parse(poi.latitude)));
        }
        await MapController.instance.LoadMapScean(onlineMapsRawImageTouchForwarder, markers.ToArray(), Zoom_offset, currentSelectedPoiPin);
        OnClickMarkerSetPoiDetails(currentSelectedPoiPin);
        poiPanelAnimate.ShowAnimation();
    }
    void RemoveMapScean()
    {
        MapController.instance.RemoveMapScean();
    }
    public void ResetZoom()
    {
        MapController.instance.ResetZoom(Zoom_offset);
    }
    public void OnClickMarkerSetPoiDetails(int index)
    {
        Debug.Log("marker click");
        currentSelectedPoiPin = index;

        poiImg.Downloading(pois[index].num, pois[index].thumbnail);
        txtpoiName.text = pois[index].Name;
        txtpoiSubDesc.text = pois[index].short_desc;
    }

    /*public void OnClickShowPoiDetailsScreen()
    //{
    //    TrailsHandler.instance.CurrentTrailPoi = pois[currentSelectedPoiPin];
    //    UIController.instance.ShowNextScreen(ScreenType.PhysicalPoiDetails);
    //}*/
}
