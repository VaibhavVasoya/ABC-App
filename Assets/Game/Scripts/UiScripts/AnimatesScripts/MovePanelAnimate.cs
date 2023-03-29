using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Master.UIKit;
using UnityEngine;

public class MovePanelAnimate : MonoBehaviour
{
    public AnimationDirection animationDirection;
    public AnimationTransition AnimationTransition = AnimationTransition.Reverse;
    public Ease InAnimationEase = Ease.OutExpo;
    public Ease OutAnimationEase = Ease.InExpo;
    public float InTime = 0.8f;
    public float OutTime = 0.5f;
    public float delay = 0f;

   [SerializeField] RectTransform _Parent;
    RectTransform _TargetTransform;
    Vector3 _initialPosition;

    float multiplier = 2f;
    public void SetInitPos()
    {
        _initialPosition = _TargetTransform.anchoredPosition3D;
    }
    public void Start()
    {
        _TargetTransform = GetComponent<RectTransform>();
        _initialPosition = _TargetTransform.anchoredPosition3D;

        float offset = _Parent.rect.width > _Parent.rect.height ? _Parent.rect.width : _Parent.rect.height;
        //float offset = Screen.width > Screen.height ? Screen.width : Screen.height;
        offset *= multiplier;


        switch (animationDirection)
        {
            case AnimationDirection.Top:
                _TargetTransform.anchoredPosition = new Vector2(_initialPosition.x, _initialPosition.y + offset);
                break;

            case AnimationDirection.Bottom:
                _TargetTransform.anchoredPosition = new Vector2(_initialPosition.x, _initialPosition.y - offset);
                break;

            case AnimationDirection.Left:
                _TargetTransform.anchoredPosition = new Vector2(_initialPosition.x - offset, _initialPosition.y);
                break;

            case AnimationDirection.Right:
                _TargetTransform.anchoredPosition = new Vector2(_initialPosition.x + offset, _initialPosition.y);
                break;
        }

        gameObject.SetActive(false);

    }

    public void ResetAnimator()
    {
        float offset = _Parent.rect.width > _Parent.rect.height ? _Parent.rect.width : _Parent.rect.height;
        //float offset = Screen.width > Screen.height ? Screen.width : Screen.height;
        offset *= multiplier;

        switch (animationDirection)
        {
            case AnimationDirection.Top:
                _TargetTransform.anchoredPosition = new Vector2(_initialPosition.x, _initialPosition.y + offset);
                break;

            case AnimationDirection.Bottom:
                _TargetTransform.anchoredPosition = new Vector2(_initialPosition.x, _initialPosition.y - offset);
                break;

            case AnimationDirection.Left:
                _TargetTransform.anchoredPosition = new Vector2(_initialPosition.x - offset, _initialPosition.y);
                break;

            case AnimationDirection.Right:
                _TargetTransform.anchoredPosition = new Vector2(_initialPosition.x + offset, _initialPosition.y);
                break;
        }

        gameObject.SetActive(true);

    }

    public void ShowAnimation()
    {
        _TargetTransform.DOAnchorPos(_initialPosition, InTime).SetEase(InAnimationEase).SetDelay(delay).OnStart(ResetAnimator).onComplete = () =>
        {
        };
    }

    public void HideAnimation(Action HideCallback = null)
    {
        float offset = _Parent.rect.width > _Parent.rect.height ? _Parent.rect.width : _Parent.rect.height;
        //float offset = Screen.width > Screen.height ? Screen.width : Screen.height;
        offset *= multiplier;

        Vector2 _endPosition = Vector2.zero;

        switch (animationDirection)
        {

            case AnimationDirection.Top:

                if (AnimationTransition == AnimationTransition.Forward)
                {
                    _endPosition = new Vector2(_initialPosition.x, _initialPosition.y + offset);
                }
                else
                {
                    _endPosition = new Vector2(_initialPosition.x, _initialPosition.y - offset);
                }

                break;

            case AnimationDirection.Bottom:
                if (AnimationTransition == AnimationTransition.Forward)
                {
                    _endPosition = new Vector2(_initialPosition.x, _initialPosition.y - offset);
                }
                else
                {
                    _endPosition = new Vector2(_initialPosition.x, _initialPosition.y + offset);
                }
                break;

            case AnimationDirection.Left:

                if (AnimationTransition == AnimationTransition.Forward)
                {
                    _endPosition = new Vector2(_initialPosition.x - offset, _initialPosition.y);
                }
                else
                {
                    _endPosition = new Vector2(_initialPosition.x + offset, _initialPosition.y);
                }

                break;

            case AnimationDirection.Right:

                if (AnimationTransition == AnimationTransition.Forward)
                {
                    _endPosition = new Vector2(_initialPosition.x + offset, _initialPosition.y);
                }
                else
                {
                    _endPosition = new Vector2(_initialPosition.x - offset, _initialPosition.y);
                }
                break;
        }

        _TargetTransform.DOAnchorPos(_endPosition, OutTime).SetEase(OutAnimationEase).onComplete = () =>
        {
            gameObject.SetActive(false);
            HideCallback();
        };
    }
}
