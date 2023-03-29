using System.Collections;
using System.Collections.Generic;
using Master.UIKit;
using UnityEngine;
using UnityEngine.UI;

public class ScrollControl : MonoBehaviour
{
	bool canScroll = true;
	public bool canSnap = false;
	[SerializeField] RectTransform _fullRect;
	[SerializeField] ScrollRect rectPos;
	[SerializeField] List<float> Product;

	[SerializeField] List<Toggle> dots;
	bool isTouchOnImg = false;
	//void Start()
	//{
	//	Init();
	//	StartCoroutine(SmoothScrollTo(rectPos.horizontalNormalizedPosition, 0, 0.2f));
	//}
	public async void Init(List<Transform> pages,List<Toggle> dotToggles)
	{
		dots = dotToggles;
		foreach (var item in pages)
        {
			item.SetParent(rectPos.transform.GetChild(0).GetChild(0));
		}
		Product.Clear();
		// scrollrect.horizontalNormalizedPosition = 0f / 9f;
		canScroll = true;

		for (int i = 0; i < rectPos.transform.GetChild(0).GetChild(0).childCount; i++)
		{
			Product.Add(i / (((float)rectPos.transform.GetChild(0).GetChild(0).childCount - 1)));
		}
		await System.Threading.Tasks.Task.Delay(System.TimeSpan.FromSeconds(0.2f));
		StartCoroutine(SmoothScrollTo(rectPos.horizontalNormalizedPosition, 0, 0.2f));
    }
	#region  check scroll product display.
	[SerializeField] int snap = 300;
	public bool isNextWelcomeScreen = true;
	public void OnEndDrag()
	{
		if (Product.Count <= 1) return;
		if ((rectPos.horizontalNormalizedPosition) > 0.95f && isNextWelcomeScreen)
        {
			UIController.instance.ShowNextScreen(ScreenType.TrailCat);
		}
		canSnap = true;
		isTouchOnImg = true;
	}
	public void IsImageTouchManual()
	{
		isTouchOnImg = true;
	}
	public float Closest(IEnumerable<float> coll, float targetVal)
	{
		float lastDiff = Mathf.Infinity;
		float closestVal = 0;
		
		foreach (var element in coll)
		{
			float currentDiff = Mathf.Abs(element - targetVal);
			if (currentDiff < lastDiff)
			{
				closestVal = element;
				lastDiff = currentDiff;
			}
		}
		SetDotsTrack(closestVal);
		return closestVal;
	}
	#endregion

	void SetDotsTrack(float closestVal)
    {
		dots[Product.IndexOf(closestVal)].isOn = true;
    }

	public void OnOpenScreen(int screenIndex)
	{
		StartCoroutine(SmoothScrollTo(rectPos.horizontalNormalizedPosition, Product[screenIndex], 0.4f));
	}
	[SerializeField] float scrollSpeed = 0.4f;
	public void ShopBtnLeft()
	{
		if (!isTouchOnImg) return;
		if (Product.Count <= 1) return;
		if (!canScroll)
			return;

		if ((rectPos.horizontalNormalizedPosition) > 0.05f)
		{
			//StartCoroutine(SmoothScrollTo(rectPos.horizontalNormalizedPosition, rectPos.horizontalNormalizedPosition - (1f / (rectPos.transform.GetChild(0).GetChild(0).childCount - 1)), 0.2f));
			float target=0;
			foreach (var item in Product)
			{
				if (item < rectPos.horizontalNormalizedPosition)
					target = item;
			}
			//Debug.LogError("Left : " +target);
			StartCoroutine(SmoothScrollTo(rectPos.horizontalNormalizedPosition, target, scrollSpeed));
			//SetDotsTrack(target);

		}
	}

	public void BtnRight()
	{
		if (!isTouchOnImg) return;
		if (Product.Count <= 1) return;
		if (!canScroll)
			return;

		if ((rectPos.horizontalNormalizedPosition) < 0.95f)
		{
			count++;
			float target = 0;
           
            foreach (var item in Product)
            {
                if (item > rectPos.horizontalNormalizedPosition)
                {
                    target = item;
                    if (target.ToString("0.00000") != rectPos.horizontalNormalizedPosition.ToString("0.00000"))
                        break;
                }
            }
            //SetDotsTrack(target);
			StartCoroutine(SmoothScrollTo(rectPos.horizontalNormalizedPosition, target, scrollSpeed));
			//StartCoroutine(SmoothScrollTo(rectPos.horizontalNormalizedPosition, Closest(Product, rectPos.horizontalNormalizedPosition), 0.2f));
			//StartCoroutine(SmoothScrollTo(rectPos.horizontalNormalizedPosition, rectPos.horizontalNormalizedPosition + (1f / (rectPos.transform.GetChild(0).GetChild(0).childCount - 1)), 0.2f));

		}
        else
        {
			MoveToFirstPage();
        }
	}
	public void MoveToFirstPage()
	{
		if (!canScroll)	return;

		float target = 0;
		StartCoroutine(SmoothScrollTo(rectPos.horizontalNormalizedPosition, target, scrollSpeed));
		//SetDotsTrack(target);
	}
	int count = -1;
	IEnumerator SmoothScrollTo(float fromVal, float toVal, float duration)
	{
		SetDotsTrack(toVal);
		//Debug.LogError("Swipe..."+ fromVal + " - "+toVal);
		canScroll = false;
		float elapsed = 0;

		// print(rectPos.horizontalNormalizedPosition+(1f/14f));
		while (elapsed <= duration)
		{
			rectPos.horizontalNormalizedPosition = Mathf.Lerp(fromVal, toVal, elapsed / duration);
			elapsed += Time.deltaTime;
			yield return null;
		}
		yield return new WaitForSeconds(Time.deltaTime);

		rectPos.horizontalNormalizedPosition = toVal;
		isTouchOnImg = false;
		canScroll = true;
		yield return null;
	}
}
