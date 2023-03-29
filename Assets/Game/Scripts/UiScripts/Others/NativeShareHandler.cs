using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

public class NativeShareHandler : MonoBehaviour
{
	private void OnEnable()
	{
		Events.WebRequestCompleted += ShareWithOtherCallBack;
	}
	private void OnDisable()
	{
		Events.WebRequestCompleted -= ShareWithOtherCallBack;
	}


	private IEnumerator TakeScreenshotAndShare()
	{
		yield return new WaitForEndOfFrame();

		Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
		ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
		ss.Apply();

		string filePath = Path.Combine(Application.temporaryCachePath, "shared img.png");
		File.WriteAllBytes(filePath, ss.EncodeToPNG());

		// To avoid memory leaks
		Destroy(ss);

		new NativeShare().AddFile(filePath)
			.SetSubject("Subject goes here").SetText("Hello world!").SetUrl("https://github.com/yasirkula/UnityNativeShare")
			.SetCallback((result, shareTarget) => Debug.Log("Share result: " + result + ", selected app: " + shareTarget))
			.Share();

		// Share on WhatsApp only, if installed (Android only)
		//if( NativeShare.TargetExists( "com.whatsapp" ) )
		//	new NativeShare().AddFile( filePath ).AddTarget( "com.whatsapp" ).Share();
	}
	bool isShareInit = false;
	public void ShareText()
    {
		if (ApiHandler.instance.data.shareWithOther == null)
		{
			ApiHandler.instance.GetShareWithOthers();
			isShareInit = true;
		}
		else
		{
			new NativeShare().SetSubject(Application.productName).SetText(ApiHandler.instance.data.shareWithOther.share_text).SetUrl(ApiHandler.instance.data.shareWithOther.share_link)
			.SetCallback((result, shareTarget) => Debug.Log("Share result: " + result + ", selected app: " + shareTarget))
			.Share();
		}
	}

	async void ShareWithOtherCallBack(API_TYPE aPI_TYPE, string obj)
	{
		if (aPI_TYPE != API_TYPE.API_SHARE_WITH_OTHER && !isShareInit) return;
		await Task.Delay(TimeSpan.FromSeconds(0.2f));
		isShareInit = false;
		new NativeShare().SetSubject(Application.productName).SetText(ApiHandler.instance.data.shareWithOther.share_text).SetUrl(ApiHandler.instance.data.shareWithOther.share_link)
			.SetCallback((result, shareTarget) => Debug.Log("Share result: " + result + ", selected app: " + shareTarget))
			.Share();
	}



}
