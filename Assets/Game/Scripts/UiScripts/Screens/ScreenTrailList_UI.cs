using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Master.UIKit;
using UnityEngine;
using UnityEngine.UI;

namespace Master.UI
{
    public class ScreenTrailList_UI : UIScreenView
    {
        [SerializeField] GameObject trail_Prefab;
        [SerializeField] Transform contentParent;
        [SerializeField] ScrollRect scrollRect;

        ContentSizeFitter[] contentSizeFitters;
        private void OnEnable()
        {
            Events.WebRequestCompleted += SculptureTrailsCallBack;
        }
        private void OnDisable()
        {
            Events.WebRequestCompleted -= SculptureTrailsCallBack;
        }
        public override void OnScreenShowCalled()
        {
            base.OnScreenShowCalled();
            //TrailsHandler.instance.CurrentTrail = null;
            SculptureTrailsCallBack(API_TYPE.API_TRAILS, "");
            Refresh();
        }

        public override void OnBack()
        {
            base.OnBack();
            UIController.instance.ShowNextScreen(ScreenType.TrailCat);
        }

        void SculptureTrailsCallBack(API_TYPE aPI_TYPE, string obj)
        {
            if (aPI_TYPE != API_TYPE.API_TRAILS) return;
            for (int i = contentParent.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(contentParent.GetChild(i).gameObject);
            }

            foreach (var trail in ApiHandler.instance.data.trails)
            {
                if(TrailsHandler.instance.currentTrailCat.num == trail.category_id)
                {
                    GameObject go = Instantiate(trail_Prefab, contentParent);
                    SculptureTrail sculpTrail = go.GetComponent<SculptureTrail>();
                    sculpTrail.SetTrail(trail);
                }
            }
            contentSizeFitters = transform.GetComponentsInChildren<ContentSizeFitter>();
            Array.Reverse(contentSizeFitters);
            Refresh();
        }

        async void Refresh()
        {
            await UIController.instance.RefreshContent(contentSizeFitters);
            scrollRect.verticalNormalizedPosition = 1f;
        }
    }
}