using Master.UIKit;
using Master.UI;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public class PoiItemPreview : MonoBehaviour
{
    [SerializeField] ImageLoader bg;
    [SerializeField] Text txtTitle;
    
    string poiId = "";
    public void SetData(string poiid,string title, string imgUrl = null)
    {
        poiId = poiid;
        txtTitle.text = title;
        bg.Downloading(poiid,imgUrl);
    }

    public void ShowPoiDetail()
    {
        int flag = ApiHandler.instance.data.trailPois.FindIndex(0, ApiHandler.instance.data.trailPois.Count, x => (x.num == poiId));
        TrailsHandler.instance.CurrentTrailPoi = ApiHandler.instance.data.trailPois.Find(x => (x.num == poiId));// ApiHandler.instance.data.trailPois[flag];
                                                                                                                //if (TrailsHandler.instance.CurrentTrail.type == TRAIL_TYPE.PHYSICAL)
        xyz();
        //else
        //    UIController.instance.getScreen(ScreenType.InformationalPoiDetails).GetComponent<ScreenInformationalTrailDetailsUI>().SetPoiDetails();
    }

    public async void xyz()
    {
        //await Task.Delay(100);
        UIController.instance.getScreen(ScreenType.PoiDetails).GetComponent<ScreenPoiDetail>().SetPoiDetails();
        //await Task.Delay(100);
        //UIController.instance.getScreen(ScreenType.PoiDetails).GetComponent<ScreenPoiDetail>().CheckPoiVisited();
    }
}
