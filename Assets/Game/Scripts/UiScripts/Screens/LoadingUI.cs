using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Master;
using System.Threading.Tasks;
using System;
using Master.UIKit;
using DG.Tweening;

public class LoadingUI : Singleton<LoadingUI>
{
    Canvas _canvas;
    [SerializeField] CanvasGroup _canvasGroup;
    [SerializeField] GameObject loader;
    public override void OnAwake()
    {
        base.OnAwake();
        _canvas = GetComponent<Canvas>();
    }
    public void OnScreenShow()
    {
        if (_canvas.enabled) return;
        loader.SetActive(true);
        ShowAnimation(0.4f,Ease.OutExpo,0);
    }

    public void OnScreenHide()
    {
        if (!_canvas.enabled) return;
        HideAnimation(0.4f, Ease.OutExpo, 0);
    }

    
    void ShowAnimation(float time, Ease ease, float delay)
    {
        _canvasGroup.alpha = 0;
        _canvas.enabled = true;
        _canvasGroup.DOFade(1, time).SetEase(ease).SetDelay(delay).onComplete = () =>{ };
    }

    void HideAnimation(float time, Ease ease, float delay, AnimationTransition animationTransition = AnimationTransition.Reverse)
    {
        _canvasGroup.alpha = 1;
        _canvasGroup.DOFade(0, time).SetEase(ease).SetDelay(delay).onComplete = () =>
        {
            _canvas.enabled = false;
            loader.SetActive(false);
        };
    }
}
