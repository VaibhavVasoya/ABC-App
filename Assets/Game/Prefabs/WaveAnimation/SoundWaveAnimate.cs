using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundWaveAnimate : MonoBehaviour
{
    RectTransform rect;
    [SerializeField] int height_min, height_max;
    [SerializeField] float minTime, MaxTime;

    float time, duration;
    float initPos, targetPos;

    public void AnimateStart()
    {
        StartCoroutine("Animate");
    }
    public void AnimateStop()
    {
        StopCoroutine("Animate");
    }
    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }
    IEnumerator Animate()
    {
        time = 0;
        duration = Random.Range(minTime, MaxTime);
        initPos = rect.rect.height;
        targetPos = Random.Range(height_min, height_max);

        while (time < duration)
        {
            time += Time.deltaTime;
            rect.sizeDelta = new Vector2(rect.rect.width, Mathf.Lerp(initPos, targetPos, time / duration));
            yield return null;
        }
        StartCoroutine("Animate");
    }
}
