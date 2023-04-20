using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Master;
using Master.UIKit;
using UnityEngine;

public class TrailsHandler : Singleton<TrailsHandler>
{

    public TrailCat currentTrailCat = null;
    public Trail CurrentTrail = null;
    public Poi CurrentTrailPoi = null;
    public SculptureEvent sculptureEvent = null;
    public VillageDiscount villageDiscount = null;
    int sculptureFindUnderMeters = 5;
    public bool isInvokeNearestSculp = true;
    [SerializeField] NotificationPopup notificationPopup;

    private void Start()
    {
        foreach (var item in ApiHandler.instance.data.trailPois)
        {
            Debug.Log("add");
            Debug.Log("is exist "+ SavedDataHandler.instance._saveData.mySculptures.Exists(x => x.Num == item.num));
            if (SavedDataHandler.instance._saveData.mySculptures.Exists(x => x.Num == item.num)) continue;
            SavedDataHandler.instance.AddSculp(item.num, item.Name, item.intro_id);
        }
        foreach (var item in ApiHandler.instance.data.trails)
        {
            if (SavedDataHandler.instance._saveData.myTrails.Exists(x => x.Num == item.num)) continue;
            SavedDataHandler.instance.AddTrail(item.num, item.title, item.category_id);
        }
    }
    public void isAllPoiVisited()
    {
        foreach (var item in ApiHandler.instance.data.trailPois)
        {
            if (SavedDataHandler.instance._saveData.mySculptures.Exists(x => x.Num == item.num))
            {
                if (SavedDataHandler.instance._saveData.mySculptures.Find(x => x.Num == item.num).IsVisited != true) return;
                Debug.Log("123You are now approaching " + item.Name);
            }
        }
        Debug.Log("All Visited");
    }

    public void MethodInvoke()
    {
        CancelInvoke("CheckSculpNearestMe");
        Invoke("CheckSculpNearestMe", 1);
    }

    public void MethodCancleInvoke()
    {
        CancelInvoke("CheckSculpNearestMe");
    }
    public async void CheckSculpNearestMe()
    {
        Vector2 currentLocation = await MapController.instance.GetCurrentLocation();
        foreach (var sculp in ApiHandler.instance.data.trailPois)
        {
            //Debug.Log("123 foreeach");
            if (sculp.intro_id != CurrentTrail.num) continue;
            if (string.IsNullOrEmpty(sculp.longitude) || string.IsNullOrEmpty(sculp.latitude)) continue;
            if (GetDistance(new Vector2(float.Parse(sculp.longitude), float.Parse(sculp.latitude)), currentLocation) < sculptureFindUnderMeters)// sculpture find in under 20 meters.
            {
                if (SavedDataHandler.instance._saveData.mySculptures.Exists(x => x.Num == sculp.num))
                {
                    if (!SavedDataHandler.instance._saveData.mySculptures.Find(x => x.Num == sculp.num).popUpShow)
                    {
                        if (UIController.instance.getCurrentScreen() == ScreenType.PoiDetails && (CurrentTrailPoi.num == ApiHandler.instance.data.trailPois.Find(x => x.num == sculp.num).num))
                        {
                            Debug.Log("already poi detail open");
                        }
                        else if (UIController.instance.getCurrentScreen() == ScreenType.PoiMap && (CurrentTrailPoi.num == ApiHandler.instance.data.trailPois.Find(x => x.num == sculp.num).num))
                        {
                            Debug.Log("already poi map open");
                        }
                        else if (UIController.instance.getCurrentScreen() == ScreenType.Quiz && (CurrentTrailPoi.num == ApiHandler.instance.data.trailPois.Find(x => x.num == sculp.num).num))
                        {
                            Debug.Log("already poi map open");
                        }
                        else if (UIController.instance.getCurrentScreen() == ScreenType.UpcomingEvents || UIController.instance.getCurrentScreen() == ScreenType.EventPoiMap || UIController.instance.getCurrentScreen() == ScreenType.EventsDetails
                            || UIController.instance.getCurrentScreen() == ScreenType.AboutThisApp || UIController.instance.getCurrentScreen() == ScreenType.AboutUs)
                        {
                            Debug.Log("not open pop up in this screen");
                        }
                        else
                        {
                            UIController.instance.ShowNotificationPopUp("You are now approaching " + sculp.Name);
                            //notificationPopup.Show("You are now approaching " + sculp.Name);
                            SavedDataHandler.instance._saveData.mySculptures.Find(x => x.Num == sculp.num).popUpShow = true;
                            CurrentTrailPoi = ApiHandler.instance.data.trailPois.Find(x => x.num == sculp.num);
                        }
                    }
                }
            }
            else
            {
                if (SavedDataHandler.instance._saveData.mySculptures.Exists(x => x.Num == sculp.num))
                {
                    if (SavedDataHandler.instance._saveData.mySculptures.Find(x => x.Num == sculp.num).popUpShow)
                    {
                        Debug.Log("again add");
                        SavedDataHandler.instance._saveData.mySculptures.Find(x => x.Num == sculp.num).popUpShow = false;
                    }
                }
            }
            //Debug.Log("distance = " + GetDistance(new Vector2(float.Parse(sculp.longitude), float.Parse(sculp.latitude)), currentLocation));
        }
        if (isInvokeNearestSculp) Invoke("CheckSculpNearestMe", 1);
    }

    float GetDistance(Vector2 lat1, Vector2 lat2)
    {
        return (float)OnlineMapsUtils.DistanceBetweenPointsD(lat1, lat2) * 1000;
    }
}
