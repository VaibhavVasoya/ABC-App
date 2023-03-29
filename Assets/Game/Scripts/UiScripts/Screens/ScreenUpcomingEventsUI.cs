using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Master.UIKit;
using UnityEngine;
using UnityEngine.UI;

namespace Master.UI
{
    public class ScreenUpcomingEventsUI : UIScreenView
    {
        [SerializeField] Text txtTitle;
        [SerializeField] GameObject eventCeilPrefab;
        [SerializeField] Transform contentParent;

        ContentSizeFitter[] contentSizeFitters;

        private void Start()
        {
            for (int i = contentParent.childCount - 1; i >= 0; i--)
            {
                Destroy(contentParent.GetChild(i).gameObject);
            }
        }
        private void OnEnable()
        {
            Events.WebRequestCompleted += SculptureEventsCallBack;
        }
        private void OnDisable()
        {
            Events.WebRequestCompleted -= SculptureEventsCallBack;
        }

        public override void OnScreenShowCalled()
        {

            base.OnScreenShowCalled();
            txtTitle.text = ApiHandler.instance.data.menuList[1].title;
        }
        public override void OnScreenShowAnimationCompleted()
        {
            base.OnScreenShowAnimationCompleted();
            Refresh();
        }
        public override void OnBack()
        {
            base.OnBack();
            BackToSculpture();
        }
        public void BackToSculpture()
        {
            UIController.instance.ShowNextScreen(ScreenType.TrailList);
        }

        void SculptureEventsCallBack(API_TYPE aPI_TYPE, string obj)
        {
            if (aPI_TYPE != API_TYPE.API_EVENTS) return;

            for (int i = contentParent.childCount - 1; i >= 0; i--)
            {
                Destroy(contentParent.GetChild(i).gameObject);
            }

            foreach (var sculpEvent in ApiHandler.instance.data.sculptureEvents)
            {
                GameObject go = Instantiate(eventCeilPrefab, contentParent);
                EventCeil eventCeil = go.GetComponent<EventCeil>();
                eventCeil.SetData(sculpEvent);
            }
            Refresh();
        }
        async void Refresh()
        {
            await Task.Delay(TimeSpan.FromSeconds(0.2f));
            contentSizeFitters = transform.GetComponentsInChildren<ContentSizeFitter>();
            Array.Reverse(contentSizeFitters);
            await UIController.instance.RefreshContent(contentSizeFitters);
        }
    }
}