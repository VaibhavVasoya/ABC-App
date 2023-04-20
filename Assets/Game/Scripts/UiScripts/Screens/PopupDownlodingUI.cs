using System.Collections;
using System.Collections.Generic;
using Master.UIKit;
using UnityEngine;
using UnityEngine.UI;

namespace Master.UI
{
    public class PopupDownlodingUI : UIScreenView
    {
        [SerializeField] Text txtDownalodName;
        [SerializeField] Slider downlodProgress;
        public string DownlodAssetName = "";
        private void OnEnable()
        {
            Events.OnDownlodProgress += Downloading;
        }
        private void OnDisable()
        {
            Events.OnDownlodProgress -= Downloading;
        }

        void Downloading(float progress)
        {
            txtDownalodName.text = DownlodAssetName + " (" + (int)Map(0, 1, 0, 100, progress) + "%)";
            downlodProgress.value = progress;
        }

        public void ClosePopup()
        {
            UIController.instance.HidePopup(ScreenType.PopupDownloding);
        }

        public static float Map(float current1, float current2, float target1, float target2, float val)
        {
            //third parameter is the interpolant between the current range which, in turn, is used in the linear interpolation of the target range. 
            return Mathf.Lerp(target1, target2, Mathf.InverseLerp(current1, current2, val));
        }
    }
}