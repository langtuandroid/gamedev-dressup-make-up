// dnSpy decompiler from Assembly-CSharp.dll class: ScreenshotsMaker
using System;
using UnityEngine;

public class ScreenshotsMaker : MonoBehaviour
{
	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	private void Update()
	{
		if (UnityEngine.Input.GetKeyDown(KeyCode.P))
		{
			string text = DateTime.Now.ToString("yyyyMMddHHmmss");
			string text2 = string.Concat(new object[]
			{
				Application.persistentDataPath,
				"/",
				Screen.width,
				"_",
				Screen.height,
				"_",
				text,
				".png"
			});
			ScreenCapture.CaptureScreenshot(text2, this.superSize);
			UnityEngine.Debug.Log("Screenshot saved to " + text2);
			AudioUtils.PlaySound(UnityEngine.Object.FindObjectOfType<AudioSource>(), this.audioMakeScreenshot);
		}
	}

	public int superSize = 1;

	public AudioClip audioMakeScreenshot;
}
