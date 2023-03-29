using DG.Tweening;
using UnityEngine;

public class FadePanelAnimate : MonoBehaviour
{
    public Ease AnimationEase = Ease.OutExpo;
    public float time = 0.4f;
    public float delay = 0f;

    CanvasGroup _canvasGroup;

    public void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        if (_canvasGroup == null)
        {
            _canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    public void ResetAnimator()
    {
        _canvasGroup.alpha = 0;
    }

    public void ShowAnimation()
    {
        _canvasGroup.alpha = 0;
        _canvasGroup.DOFade(1, time).SetEase(AnimationEase).SetDelay(delay).onComplete = () =>{ };
    }
    public void HideAnimation()
    {
        _canvasGroup.alpha = 1;
        _canvasGroup.DOFade(0, time).SetEase(AnimationEase).SetDelay(delay).onComplete = () => { };
    }
}
