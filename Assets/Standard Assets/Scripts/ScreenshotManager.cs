// dnSpy decompiler from Assembly-CSharp.dll class: ScreenshotManager
using System;
using System.Collections;
using System.IO;
using UnityEngine;

public class ScreenshotManager : MonoBehaviour
{
	public static IEnumerator Save(string fileName, Texture2D val)
	{
		GameObject gameObject = new GameObject();
		gameObject.name = "Screenshot";
		gameObject.AddComponent<ScreenshotManager>();
		ScreenshotManager.ScreenShotNumber++;
		if (Application.platform == RuntimePlatform.Android)
		{
			string text = "/storage/emulated/0/DCIM/" + Application.productName;
			Directory.CreateDirectory(text);
			File.WriteAllBytes(text + "/" + fileName, val.EncodeToPNG());
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
			AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("android.net.Uri");
			AndroidJavaObject androidJavaObject = new AndroidJavaObject("android.content.Intent", new object[]
			{
				"android.intent.action.MEDIA_SCANNER_SCAN_FILE",
				androidJavaClass2.CallStatic<AndroidJavaObject>("parse", new object[]
				{
					"file://" + text + "/" + fileName
				})
			});
			@static.Call("sendBroadcast", new object[]
			{
				androidJavaObject
			});
		}
		UnityEngine.Object.Destroy(gameObject);
		yield break;
	}

	public static IEnumerator Wait(float delay)
	{
		float pauseTarget = Time.realtimeSinceStartup + delay;
		while (Time.realtimeSinceStartup < pauseTarget)
		{
			yield return null;
		}
		yield break;
	}

	public static int ScreenShotNumber
	{
		get
		{
			return PlayerPrefs.GetInt("screenShotNumber");
		}
		set
		{
			PlayerPrefs.SetInt("screenShotNumber", value);
		}
	}
}
