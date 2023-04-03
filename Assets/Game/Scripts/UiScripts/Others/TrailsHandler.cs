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

    private void Start()
    {
        foreach (var item in ApiHandler.instance.data.trailPois)
        {
            if (SavedDataHandler.instance._saveData.mySculptures.Exists(x => x.Num == item.num)) continue;
            SavedDataHandler.instance.AddSculp(item.num,item.Name,item.intro_id);
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
            }
        }
        Debug.Log("All Visited");
    }



    public async void CheckSculpNearestMe()
    {
        Vector2 currentLocation = await MapController.instance.GetCurrentLocation();
        foreach (var sculp in ApiHandler.instance.data.trailPois)
        {
            if (string.IsNullOrEmpty(sculp.longitude) || string.IsNullOrEmpty(sculp.latitude)) continue;
            if (GetDistance(new Vector2(float.Parse(sculp.longitude), float.Parse(sculp.latitude)), currentLocation) < sculptureFindUnderMeters)// sculpture find in under 20 meters.
            {
                //Debug.LogError(sculp.title + " Cheack is neareas me.");
                // Sculpture is nearest 30 meters.
                //SculptureVisited();
                if (SavedDataHandler.instance._saveData.mySculptures.Exists(x => x.Num == sculp.num))
                {
                    if (!SavedDataHandler.instance._saveData.mySculptures.Find(x => x.Num == sculp.num).IsVisited)
                    {
                        //notificationPopup.Show("You are approaching the " + sculp.title + " sculpture");
                        SavedDataHandler.instance._saveData.mySculptures.Find(x => x.Num == sculp.num).IsVisited = true;
                    }
                }
                Events.NearestSculpture(sculp.num, true);
            }
            else
                Events.NearestSculpture(sculp.num, false);
        }
        await Task.Delay(TimeSpan.FromSeconds(1));
        CheckSculpNearestMe();
        //if (isInvokeNearestSculp) CheckSculpNearestMe();
    }

    float GetDistance(Vector2 lat1, Vector2 lat2)
    {
        return (float)OnlineMapsUtils.DistanceBetweenPointsD(lat1, lat2) * 1000;
    }
}
