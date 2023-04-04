using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Master;
using UnityEngine;

public class TrailsHandler : Singleton<TrailsHandler>
{

    public TrailCat currentTrailCat = null;
    public Trail CurrentTrail = null;
    public Poi CurrentTrailPoi = null;
    public SculptureEvent sculptureEvent = null;
    public VillageDiscount villageDiscount = null;
    int sculptureFindUnderMeters = 20;
    bool isInvokeNearestSculp;
    [SerializeField] NotificationPopup notificationPopup ;
    private void OnEnable()
    {
        isInvokeNearestSculp = true;
    }

    private void OnDisable()
    {
        isInvokeNearestSculp = false;
    }

    private void Start()
    {
        foreach (var item in ApiHandler.instance.data.trailPois)
        {
            if (SavedDataHandler.instance._saveData.mySculptures.Exists(x => x.Num == item.num)) continue;
            SavedDataHandler.instance.AddSculp(item.num,item.Name,item.intro_id);
        }
        foreach (var item in ApiHandler.instance.data.trails)
        {
            if (SavedDataHandler.instance._saveData.myTrails.Exists(x => x.Num == item.num)) continue;
            SavedDataHandler.instance.AddTrail(item.num, item.title, item.category_id);
        }
        //CheckSculpNearestMe();
    }

    public void isAllPoiVisited()
    {
        foreach (var item in ApiHandler.instance.data.trailPois)
        {
            if (SavedDataHandler.instance._saveData.mySculptures.Exists(x => x.Num == item.num))
            {
                if (SavedDataHandler.instance._saveData.mySculptures.Find(x => x.Num == item.num).IsVisited != true) return;
                Debug.Log("123You are now approaching "+item.Name);
            }
        }
        Debug.Log("All Visited");
    }



    public async void CheckSculpNearestMe()
    {
        Vector2 currentLocation = await MapController.instance.GetCurrentLocation();
        foreach (var sculp in ApiHandler.instance.data.trailPois)
        {
            Debug.Log("123 current location "+ currentLocation);
            //Vector2 v = new Vector2(float.Parse(sculp.longitude), float.Parse(sculp.latitude));
            await Task.Delay(TimeSpan.FromSeconds(2));
            Debug.Log("longitude = " + sculp.longitude +" latitude = "+ sculp.latitude);
            //Debug.Log("123 poi location = " + v);
            if (string.IsNullOrEmpty(sculp.longitude) || string.IsNullOrEmpty(sculp.latitude)) continue;
            if (GetDistance(new Vector2(float.Parse(sculp.longitude), float.Parse(sculp.latitude)), currentLocation) < sculptureFindUnderMeters)// sculpture find in under 20 meters.
            {

                Debug.LogError("123"+sculp.Name + " Cheack is neareas me.");
                // Sculpture is nearest 30 meters.
                //SculptureVisited();
                if (SavedDataHandler.instance._saveData.mySculptures.Exists(x => x.Num == sculp.num))
                {
                    if (!SavedDataHandler.instance._saveData.mySculptures.Find(x => x.Num == sculp.num).popUpShow)
                    {
                        notificationPopup.Show("You are approaching the " + sculp.Name + " sculpture");
                        SavedDataHandler.instance._saveData.mySculptures.Find(x => x.Num == sculp.num).popUpShow = true;
                    }
                }
                Events.NearestSculpture(sculp.num, true);
            }
            else
                Events.NearestSculpture(sculp.num, false);
        }
        await Task.Delay(TimeSpan.FromSeconds(1));
        //CheckSculpNearestMe();
        if (isInvokeNearestSculp) CheckSculpNearestMe();
    }

    float GetDistance(Vector2 lat1, Vector2 lat2)
    {
        return (float)OnlineMapsUtils.DistanceBetweenPointsD(lat1, lat2) * 1000;
    }
}
