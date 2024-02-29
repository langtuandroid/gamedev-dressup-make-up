// dnSpy decompiler from Assembly-CSharp.dll class: ScreenshotDemo
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScreenshotDemo : DImageDisplayHandler
{
	private void Start()
	{
		ScreenshotHelper.iSetMainOnCapturedCallback(delegate(Sprite sprite)
		{
			this.SetImage(sprite);
			this.cubeMesh.material.mainTexture = sprite.texture;
			FilePathName.SaveFormat saveFormat = this.saveFormat;
			if (saveFormat != FilePathName.SaveFormat.JPG)
			{
				if (saveFormat != FilePathName.SaveFormat.PNG)
				{
					if (saveFormat != FilePathName.SaveFormat.GIF)
					{
					}
				}
				else
				{
					this.SaveAsPNG(sprite.texture);
				}
			}
			else
			{
				this.SaveAsJPG(sprite.texture);
			}
		});
		ScreenshotHelper.Instance.m_DebugText = this.debugText;
		this.OnInputChanges();
		if (Screen.width > Screen.height)
		{
			this.canvasScaler.referenceResolution = new Vector2(1920f, 1080f);
		}
		else
		{
			this.canvasScaler.referenceResolution = new Vector2(1080f, 1920f);
		}
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			this.uiPointerEventData.position = UnityEngine.Input.mousePosition;
			EventSystem.current.RaycastAll(this.uiPointerEventData, this.uiRaycastResuls);
			this._isPointedOnUI = (this.uiRaycastResuls.Count > 0);
		}
		if (Input.GetMouseButtonUp(0) && !this._isPointedOnUI)
		{
			ScreenshotHelper.Instance.UpdateDebugText("Touch Position: " + UnityEngine.Input.mousePosition);
			ScreenshotHelper.iCapture(UnityEngine.Input.mousePosition, ScreenshotHelper.CurrentCaptureSize, delegate(Texture2D texture2D)
			{
			});
		}
	}

	public void OnInputChanges()
	{
		int num = 512;
		int.TryParse(this.widthInputField.text, out num);
		int num2 = 512;
		int.TryParse(this.heightInputField.text, out num2);
		ScreenshotHelper.iSetCaptureSize(new Vector2((float)num, (float)num2));
	}

	public void CaptureScreen()
	{
		ScreenshotHelper.iCaptureScreen(delegate(Texture2D texture2D)
		{
		});
	}

	public void CaptureWithCamera(Camera camera)
	{
		ScreenshotHelper.iCaptureWithCamera(camera, delegate(Texture2D texture2D)
		{
		});
	}

	private void SetImage(Sprite sprite)
	{
		base.SetImage(this.displayImage, sprite);
	}

	public void Clear()
	{
		base.Clear(this.displayImage);
		this.displayImage.rectTransform.sizeDelta = Vector2.zero;
	}

	private void SaveAsJPG(Texture2D tex2D)
	{
		string text = "Saved_as_JPG_to:_" + new FilePathName().SaveTextureAs(tex2D, FilePathName.SaveFormat.JPG);
		ScreenshotHelper.Instance.UpdateDebugText(text);
	}

	private void SaveAsPNG(Texture2D tex2D)
	{
		string text = "Saved_as_PNG_to:_" + new FilePathName().SaveTextureAs(tex2D, FilePathName.SaveFormat.PNG);
		ScreenshotHelper.Instance.UpdateDebugText(text);
	}

	public void ClearScreenshotHelper()
	{
		ScreenshotHelper.iClear();
	}

	public void UnRegRenderCameras()
	{
		ScreenshotHelper.iUnRegisterAllRenderCameras();
	}

	public void MoreAssets()
	{
		Application.OpenURL("https://www.swanob2.com/assets");
	}

	[Header("[ Save Settings ]")]
	public FilePathName.SaveFormat saveFormat = FilePathName.SaveFormat.JPG;

	[Header("[ Object References ]")]
	public CanvasScaler canvasScaler;

	public Image displayImage;

	public Text debugText;

	public InputField widthInputField;

	public InputField heightInputField;

	public MeshRenderer cubeMesh;

	public Camera camera1;

	public Camera camera2;

	public Camera camera3;

	private PointerEventData uiPointerEventData = new PointerEventData(EventSystem.current);

	private List<RaycastResult> uiRaycastResuls = new List<RaycastResult>();

	private bool _isPointedOnUI;
}
