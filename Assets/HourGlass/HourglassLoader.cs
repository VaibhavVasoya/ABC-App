using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HourglassLoader : MonoBehaviour
{
    [SerializeField] Image img1, img2;
    [SerializeField] private RawImage sandImage;
    [SerializeField] float sandSpeed = 1;
    [SerializeField] AnimationCurve animationCurve;

    float t1;
    [SerializeField] float FillTime = 2;
    float t2;
    [SerializeField] float RotateTime = 2;
    private void OnEnable()
    {
        StartCoroutine("StartLoading");
    }
    private void OnDisable()
    {
        StopCoroutine("StartLoading");
    }
    IEnumerator StartLoading()
    {
        t1 = 0;
        sandImage.gameObject.SetActive(true);
        transform.eulerAngles = Vector3.zero;
        img1.fillAmount = 1;
        img2.fillAmount = 0;
        while (t1 < FillTime)
        {
            t1 += Time.deltaTime;
            img1.fillAmount = Map(0, 1, 1, 0, t1 / FillTime);
            img2.fillAmount = t1 / FillTime;
            sandImage.uvRect = new Rect(sandImage.uvRect.x, sandImage.uvRect.y + sandSpeed * Time.deltaTime  , sandImage.uvRect.width, sandImage.uvRect.height);
            yield return null;
        }
        img1.fillAmount = 0;
        img2.fillAmount = 1;
        sandImage.gameObject.SetActive(false);
        t2 = 0;
        while (t2 < RotateTime)
        {
            t2 += Time.deltaTime;
            transform.eulerAngles = new Vector3(0, 0, Mathf.Lerp(0, -180, animationCurve.Evaluate(t2 / RotateTime)));
            yield return null;
        }
        transform.eulerAngles = new Vector3(0, 0, -180);
        StartCoroutine("StartLoading");
    }

    public static float Map(float current1, float current2, float target1, float target2, float val)
    {
        //third parameter is the interpolant between the current range which, in turn, is used in the linear interpolation of the target range. 
        return Mathf.Lerp(target1, target2, Mathf.InverseLerp(current1, current2, val));
    }
}
