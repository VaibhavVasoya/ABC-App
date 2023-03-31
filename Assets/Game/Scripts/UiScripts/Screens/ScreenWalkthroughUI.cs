using System.Collections;
using System.Collections.Generic;
using Master.UIKit;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Master.UI
{
    public class ScreenWalkthroughUI : UIScreenView
    {
        [SerializeField] SwipeControl swipeControl;
        [SerializeField] GameObject pagePrefab;
        [SerializeField] GameObject dotPrefab;
        [SerializeField] Transform pageParent;
        [SerializeField] Transform dotParent;
        [SerializeField] ScrollControl scrollControl;
        [SerializeField] List<Transform> pages;
        [SerializeField] List<Toggle> dotToggles;

        public override void OnScreenShowCalled()
        {
            base.OnScreenShowCalled();
            UIController.instance.IsBottomImageEnable(false);
            SpawnScreen();
        }

        public override void OnScreenHideCalled()
        {
            base.OnScreenHideCalled();
            swipeControl.canSwipe = false;
            UIController.instance.IsBottomImageEnable(true);
        }

        //[EasyButtons.Button]
        //void ShowPopup()
        //{
        //    UIController.instance.ShowPopupMsg("Unknown Error", "We couldn't connect you to the server.\nTry again later.");
        //}

        void SpawnScreen()
        {
            int index = 0;
            Toggle toggle = null;
          
            foreach (var item in ApiHandler.instance.data.walkThroughs)
            {
                GameObject page = Instantiate(pagePrefab, pageParent);
                page.GetComponent<WalkthroughPage>().SetData(item);

                GameObject dot = Instantiate(dotPrefab, dotParent);
                toggle = dot.GetComponent<Toggle>();
                toggle.group = dotParent.GetComponent<ToggleGroup>();

                EventTrigger trigger = dot.GetComponent<EventTrigger>();
                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerUp;
                entry.callback.AddListener((eventData) => { scrollControl.OnOpenScreen(trigger.transform.GetSiblingIndex()); });

                trigger.triggers.Add(entry);

                pages.Add(page.transform);
                dotToggles.Add(toggle);
                index++;
            }

            swipeControl.canSwipe = true;
            scrollControl.Init(pages, dotToggles);

        }
        public void SkipAndOpenTrailsScreen()
        {
            UIController.instance.ShowNextScreen(ScreenType.TrailCat);
        }

    }
}