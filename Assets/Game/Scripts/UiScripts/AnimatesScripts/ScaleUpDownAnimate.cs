using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleUpDownAnimate : MonoBehaviour
{
    [SerializeField] AnimationCurve curver;
    float animationTime = 0.4f;
    float elapsed = 0;
    float perc;
    private void Start()
    {
        transform.localScale = Vector3.zero;
    }

    [EasyButtons.Button]
    public void ShowAnimation()
    {
        if (animate != null)
            StopCoroutine(animate);
        StartCoroutine(animate = Animate(1));
    }
    [EasyButtons.Button]
    public void HideAnimation()
    {
        if (animate != null)
            StopCoroutine(animate);
        StartCoroutine(animate = Animate(0));
    }
    IEnumerator animate;
    IEnumerator Animate(int scale)
    {
        elapsed = 0;
        perc = 0;
        Vector3 oldSclae = (scale == 1) ? Vector3.zero : Vector3.one;
        Vector3 newScale = (scale == 1) ? Vector3.one : Vector3.zero;
        while (elapsed < animationTime)
        {
            perc = elapsed / animationTime;
            transform.localScale = Vector3.Lerp(oldSclae, newScale, curver.Evaluate(perc));
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localScale = newScale;
    }
}
