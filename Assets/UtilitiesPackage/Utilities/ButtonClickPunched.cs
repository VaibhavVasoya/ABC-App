using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class ButtonClickPunched : MonoBehaviour , IPointerDownHandler, IPointerUpHandler {

    RectTransform rectTransform;
    public bool canAnimate = true;
    float duration = 0.1f;
    public AnimationCurve buttonCurve;
    public Image overlayDark;
    Button btn;
    Toggle tgl;

    IEnumerator animate;
    void Awake()
    {
        
        if (overlayDark)
            overlayDark.color = Color.black.WithAlpha(0);

        rectTransform = GetComponent<RectTransform>();

        //get button
        if( GetComponent<Button>()!=null)
            btn = GetComponent<Button>();
        else if( GetComponent<Toggle>()!=null)
            tgl = GetComponent<Toggle>();
    }

    enum PointerState
    {
        None,
        Down,
        Up
    }
    PointerState pState = PointerState.None;

    public virtual void OnPointerUp(PointerEventData data)
    {
        if (canAnimate && ((btn!=null) ?btn.IsInteractable():false))
        {
            pState = PointerState.Up;
            if (animate != null) StopCoroutine(animate);
            StartCoroutine(animate = Animate(new Vector2(0.9f, 0.9f), Vector2.one, Color.black.WithAlpha(0)));
        }
        else if (canAnimate && ((tgl!=null) ?tgl.IsInteractable():false))
        {
            pState = PointerState.Up;
            if (animate != null) StopCoroutine(animate);
            StartCoroutine(animate = Animate(new Vector2(0.9f, 0.9f), Vector2.one, Color.black.WithAlpha(0)));
        }
    }

    public virtual void OnPointerDown(PointerEventData data)
    {
        if (canAnimate && ((btn!=null) ?btn.IsInteractable():false))
        {
            pState = PointerState.Down;
            if (animate != null) StopCoroutine(animate);
            StartCoroutine(animate = Animate(Vector2.one, new Vector2(0.9f, 0.9f), Color.black.WithAlpha(.25f)));
        }
        else if (canAnimate && ((tgl!=null) ?tgl.IsInteractable():false))
        {
            pState = PointerState.Down;
            if (animate != null) StopCoroutine(animate);
            StartCoroutine(animate = Animate(Vector2.one, new Vector2(0.9f, 0.9f), Color.black.WithAlpha(.25f)));
        }
    }

    IEnumerator Animate(Vector2 fromScale, Vector2 toScale, Color overlayCol)
    {
        float elapsed = 0;
        while (elapsed < duration)
        {

            float time = buttonCurve.Evaluate(elapsed / duration);
            rectTransform.localScale = Vector2.LerpUnclamped(fromScale, toScale, time);
            if (overlayDark)
                overlayDark.color = Color.Lerp(overlayDark.color, overlayCol, time);
            elapsed += Time.unscaledDeltaTime;
            yield return null;

        }
        rectTransform.localScale = toScale;
        yield return null;
    }

    //[SerializeField] Vector3 amount = new Vector3(0.5f,0.5f,0);
    //public void OnPointerDown(PointerEventData eventData)
    //{
    //    iTween.PunchScale(this.gameObject, amount, 0.8f);
    //    //throw new System.NotImplementedException();
    //}

    
}
