using UnityEngine;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using UnityEngine.UI;

namespace Unity.VideoHelper.Animation
{
    /// <summary>
    /// Animates <see cref="CanvasGroup.alpha"/>.
    /// </summary>
    public class CanvasGroupAnimator : MonoBehaviour
    {
        public CanvasGroup Group;
        [SerializeField] CanvasGroup groupPauseBtn;
        public Canvas ControllCanvas;
        IEnumerator ShowCouroutine;
        private void Start()
        {
            ControllCanvas.enabled = false;
            Group.alpha = groupPauseBtn.alpha = 0;
            groupPauseBtn.gameObject.SetActive(false);
        }
        float timer = 0;
        float estimate = .4f;
        public bool show;
        public void OnClick()
        {
            if(!show)
            {
                if (ShowCouroutine != null)
                {
                    StopCoroutine(ShowCouroutine);
                }
                
                StartCoroutine(ShowCouroutine = CanvasGroupShow());
            }
            else
            {
                if(ShowCouroutine != null)
                {
                    StopCoroutine(ShowCouroutine);
                    ShowCouroutine = null;
                }
                StartCoroutine(CanvasGroupHide());
            }
        }
        IEnumerator Updates()
        {
            while(true)
            {
                if (Input.GetMouseButtonDown(0) && show)
                {
                    if (ShowCouroutine != null)
                    {
                        StopCoroutine(ShowCouroutine);
                    }
                    CancelInvoke("CanvasGroupHideWait");
                }
                else if (Input.GetMouseButtonUp(0) && show)
                {
                    if (ShowCouroutine != null)
                    {
                        StopCoroutine(ShowCouroutine);
                        ShowCouroutine = null;
                    }
                    Invoke("CanvasGroupHideWait",5);
                }
                yield return null;
            }
        }

        public IEnumerator CanvasGroupShow()
        {
            StartCoroutine("Updates");
            ControllCanvas.enabled = true;
            groupPauseBtn.gameObject.SetActive(true);
            timer = 0;
            while (timer < estimate)
            {
                timer += Time.deltaTime;
                Group.alpha = groupPauseBtn.alpha = Mathf.Lerp(0, 1, timer/estimate);
                yield return null;
            }
            show = true;
            yield return new WaitForSeconds(5);
            StartCoroutine(CanvasGroupHide());
        }
        float timer1;
        float estimate1 = 0.4f;
        public IEnumerator CanvasGroupHide()
        {
            Debug.Log("Hide");
            timer1 = 0;
            while (timer1 < estimate1)
            {
                timer1 += Time.deltaTime;
                Group.alpha = groupPauseBtn.alpha = Mathf.Lerp(1, 0, timer1 / estimate1 );
                yield return null;
            }
            show = false;
            ControllCanvas.enabled = false;
            groupPauseBtn.gameObject.SetActive(false);
            StopCoroutine("Updates");
        }
        void  CanvasGroupHideWait()
        {
            Debug.Log("HideWait");
            StartCoroutine(CanvasGroupHide());
        }
    }

}
