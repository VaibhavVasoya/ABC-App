using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Master.UIKit;
using UnityEngine;
using UnityEngine.UI;
using Master.UI;

public class EventCeil : MonoBehaviour
{
    [SerializeField] ImageLoader bg;
    [SerializeField] Text txtTitle;
    [SerializeField] Text txtStreetCount;
    [SerializeField] Text txtStartDate;
    [SerializeField] Text txtSubDetail;

    SculptureEvent sculpEvent;

    ContentSizeFitter[] contentSizeFitters;

    DateTime _startDate;


    public void SetData(SculptureEvent _sculpEvent)
    {
        contentSizeFitters = transform.GetComponentsInChildren<ContentSizeFitter>();
        sculpEvent = _sculpEvent;
        bg.Downloading(_sculpEvent.num, _sculpEvent.main_image);
        txtTitle.text = _sculpEvent.name;
       // txtStreetCount.text = _sculpEvent.address;
        if (!string.IsNullOrEmpty(_sculpEvent.start_date) && _sculpEvent.start_date.Length > 4)
        {
            txtStartDate.text = Helper.DateConvert(_sculpEvent.start_date).ToString("dd MMM");
        }
        
        txtSubDetail.text = _sculpEvent.short_desc.ToString();
        Refresh();
    }

    public void OpenEventDetails()
    {
        TrailsHandler.instance.sculptureEvent = sculpEvent;
        UIController.instance.ShowNextScreen(ScreenType.EventsDetails);
    }

    void Refresh()
    {
        UIController.instance.RefreshContent(contentSizeFitters);
    }
    
}
