// dnSpy decompiler from Assembly-CSharp.dll class: ScreenshotHelper
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ScreenshotHelper : MonoBehaviour
{
	public static ScreenshotHelper Instance
	{
		get
		{
			if (ScreenshotHelper._instance == null)
			{
				ScreenshotHelper._instance = new GameObject("[ScreenshotHelper]").AddComponent<ScreenshotHelper>();
			}
			return ScreenshotHelper._instance;
		}
	}

	public void Clear()
	{
		if (this._texture2D != null)
		{
			UnityEngine.Object.Destroy(this._texture2D);
		}
		if (this._renderTexture != null)
		{
			UnityEngine.Object.Destroy(this._renderTexture);
		}
		if (this.m_MainOnCaptured != null)
		{
			this.m_MainOnCaptured.RemoveAllListeners();
			this.m_MainOnCaptured = null;
		}
		this.UnRegisterAllRenderCameras();
	}

	private void Awake()
	{
		if (ScreenshotHelper._instance == null)
		{
			ScreenshotHelper._instance = this;
		}
	}

	private void _InitMainOnCaptured()
	{
		if (this.m_MainOnCaptured == null)
		{
			this.m_MainOnCaptured = new UnityEvent();
		}
		this.m_MainOnCaptured.RemoveAllListeners();
	}

	public void SetMainOnCapturedCallback(Action mainOnCaptured)
	{
		this._InitMainOnCaptured();
		this.m_MainOnCaptured.AddListener(delegate()
		{
			mainOnCaptured();
		});
	}

	public void SetMainOnCapturedCallback(Action<Texture2D> mainOnCaptured)
	{
		this._InitMainOnCaptured();
		this.m_MainOnCaptured.AddListener(delegate()
		{
			mainOnCaptured(this._texture2D);
		});
	}

	public void SetMainOnCapturedCallback(Action<Sprite> mainOnCaptured)
	{
		this._InitMainOnCaptured();
		this.m_MainOnCaptured.AddListener(delegate()
		{
			mainOnCaptured(this.GetCurrentSprite());
		});
	}

	public void SetMainOnCapturedCallback(Action<RenderTexture> mainOnCaptured)
	{
		this._InitMainOnCaptured();
		this.m_MainOnCaptured.AddListener(delegate()
		{
			mainOnCaptured(this._renderTexture);
		});
	}

	public void CaptureScreen(Action<Texture2D> onCaptured = null)
	{
		base.StartCoroutine(this._CaptureFullscreen(onCaptured));
	}

	public void Capture(Vector2 screenPosition, Vector2 imageSize, Action<Texture2D> onCaptured = null)
	{
		if (this._isOnCapture)
		{
			UnityEngine.Debug.LogWarning("Screenshot being captured, please wait for at least 1 frame for starting another capture!");
			return;
		}
		this._isOnCapture = true;
		if (imageSize.x > (float)Screen.width)
		{
			imageSize = new Vector2((float)Screen.width, imageSize.y);
		}
		if (imageSize.y > (float)Screen.height)
		{
			imageSize = new Vector2(imageSize.x, (float)Screen.height);
		}
		if (screenPosition.x + imageSize.x / 2f > (float)Screen.width)
		{
			screenPosition = new Vector2(screenPosition.x - (screenPosition.x + imageSize.x / 2f - (float)Screen.width), screenPosition.y);
		}
		if (screenPosition.x - imageSize.x / 2f < 0f)
		{
			screenPosition = new Vector2(screenPosition.x + (imageSize.x / 2f - screenPosition.x), screenPosition.y);
		}
		if (screenPosition.y + imageSize.y / 2f > (float)Screen.height)
		{
			screenPosition = new Vector2(screenPosition.x, screenPosition.y - (screenPosition.y + imageSize.y / 2f - (float)Screen.height));
		}
		if (screenPosition.y - imageSize.y / 2f < 0f)
		{
			screenPosition = new Vector2(screenPosition.x, screenPosition.y + (imageSize.y / 2f - screenPosition.y));
		}
		this.UpdateDebugText(string.Concat(new object[]
		{
			"Capture screenPosition: ",
			screenPosition,
			" | imageSize: ",
			imageSize
		}));
		Rect targetRect = new Rect(screenPosition, imageSize);
		base.StartCoroutine(this._ReadPixelWithRect(targetRect, onCaptured));
	}

	public void CaptureWithCamera(Camera camera, Action<Texture2D> onCaptured = null)
	{
		this.UpdateDebugText(string.Concat(new object[]
		{
			camera.name,
			" rect: ",
			camera.pixelWidth,
			" x ",
			camera.pixelHeight
		}));
		this.RegisterRenderCamera(camera);
		CameraOnRender component = camera.gameObject.GetComponent<CameraOnRender>();
		if (component != null)
		{
			component.SetOnCaptureCallback(delegate(Texture2D tex)
			{
				this._texture2D = tex;
				if (this.m_MainOnCaptured != null)
				{
					this.m_MainOnCaptured.Invoke();
				}
				if (onCaptured != null)
				{
					onCaptured(this._texture2D);
				}
			});
		}
		else
		{
			UnityEngine.Debug.LogWarning("Require this camera to be registered with method RegisterCaptureCamera!");
		}
	}

	public void CaptureRenderTextureWithCamera(Camera camera, Action<RenderTexture> onCaptured = null)
	{
		this.UpdateDebugText(string.Concat(new object[]
		{
			camera.name,
			" rect: ",
			camera.pixelWidth,
			" x ",
			camera.pixelHeight
		}));
		this.RegisterRenderCamera(camera);
		CameraOnRender component = camera.gameObject.GetComponent<CameraOnRender>();
		if (component != null)
		{
			component.SetOnCaptureCallback(delegate(RenderTexture rTex)
			{
				this._renderTexture = rTex;
				if (this.m_MainOnCaptured != null)
				{
					this.m_MainOnCaptured.Invoke();
				}
				if (onCaptured != null)
				{
					onCaptured(this._renderTexture);
				}
			});
		}
		else
		{
			UnityEngine.Debug.LogWarning("Require this camera to be registered with method RegisterCaptureCamera!");
		}
	}

	public Texture2D GetCurrentTexture()
	{
		return this._texture2D;
	}

	public Sprite GetCurrentSprite()
	{
		return ScreenshotHelper.ToSprite(this.GetCurrentTexture());
	}

	public RenderTexture GetCurrentRenderTexture()
	{
		return this._renderTexture;
	}

	private void _ProceedReadPixels(Rect targetRect, Action<Texture2D> onCaptured)
	{
		if (targetRect.width > (float)Screen.width)
		{
			targetRect.width = (float)Screen.width;
		}
		if (targetRect.height > (float)Screen.height)
		{
			targetRect.height = (float)Screen.height;
		}
		this._texture2D = new Texture2D((int)targetRect.width, (int)targetRect.height, TextureFormat.RGB24, false);
		Rect source = new Rect(targetRect.position.x - targetRect.width / 2f, targetRect.position.y - targetRect.height / 2f, targetRect.width, targetRect.height);
		this._texture2D.ReadPixels(source, 0, 0);
		this._texture2D.Apply();
		if (this.m_MainOnCaptured != null)
		{
			this.m_MainOnCaptured.Invoke();
		}
		if (onCaptured != null)
		{
			onCaptured(this._texture2D);
		}
		this._isOnCapture = false;
		this.UpdateDebugText(string.Concat(new object[]
		{
			"Capture screenPosition: (",
			targetRect.position.x,
			", ",
			targetRect.position.y,
			") | imageSize: (",
			targetRect.width,
			", ",
			targetRect.height,
			")"
		}));
	}

	private IEnumerator _CaptureFullscreen(Action<Texture2D> onCaptured)
	{
		yield return new WaitForEndOfFrame();
		Rect targetRect = new Rect((float)(Screen.width / 2), (float)(Screen.height / 2), (float)Screen.width, (float)Screen.height);
		this._ProceedReadPixels(targetRect, onCaptured);
		yield break;
	}

	private IEnumerator _ReadPixelWithRect(Rect targetRect, Action<Texture2D> onCaptured)
	{
		yield return new WaitForEndOfFrame();
		this._ProceedReadPixels(targetRect, onCaptured);
		yield break;
	}

	public void RegisterRenderCamera(Camera camera)
	{
		if (camera != null && camera.gameObject.GetComponent<CameraOnRender>() == null)
		{
			camera.gameObject.AddComponent<CameraOnRender>();
		}
	}

	public void UnRegisterRenderCamera(Camera camera)
	{
		if (camera != null && camera.gameObject.GetComponent<CameraOnRender>() != null)
		{
			camera.gameObject.GetComponent<CameraOnRender>().Clear();
		}
	}

	public void UnRegisterAllRenderCameras()
	{
		Camera[] allCameras = Camera.allCameras;
		if (allCameras != null)
		{
			foreach (Camera camera in allCameras)
			{
				this.UnRegisterRenderCamera(camera);
			}
		}
	}

	public static Texture2D CurrentTexture
	{
		get
		{
			return ScreenshotHelper.Instance.GetCurrentTexture();
		}
	}

	public static Sprite CurrentSprite
	{
		get
		{
			return ScreenshotHelper.Instance.GetCurrentSprite();
		}
	}

	public static RenderTexture CurrentRenderTexture
	{
		get
		{
			return ScreenshotHelper.Instance.GetCurrentRenderTexture();
		}
	}

	public static Vector2 CurrentCaptureSize
	{
		get
		{
			return ScreenshotHelper.Instance.m_CaptureSize;
		}
	}

	public static void iSetCaptureSize(Vector2 captureSize)
	{
		ScreenshotHelper.Instance.m_CaptureSize = captureSize;
	}

	public static void iSetMainOnCapturedCallback(Action mainOnCaptured)
	{
		ScreenshotHelper.Instance.SetMainOnCapturedCallback(mainOnCaptured);
	}

	public static void iSetMainOnCapturedCallback(Action<Texture2D> mainOnCaptured)
	{
		ScreenshotHelper.Instance.SetMainOnCapturedCallback(mainOnCaptured);
	}

	public static void iSetMainOnCapturedCallback(Action<Sprite> mainOnCaptured)
	{
		ScreenshotHelper.Instance.SetMainOnCapturedCallback(mainOnCaptured);
	}

	public static void iSetMainOnCapturedCallback(Action<RenderTexture> mainOnCaptured)
	{
		ScreenshotHelper.Instance.SetMainOnCapturedCallback(mainOnCaptured);
	}

	public static void iCaptureScreen(Action<Texture2D> onCaptured = null)
	{
		ScreenshotHelper.Instance.CaptureScreen(onCaptured);
	}

	public static void iCapture(Vector2 screenPosition, Vector2 imageSize, Action<Texture2D> onCaptured = null)
	{
		ScreenshotHelper.Instance.Capture(screenPosition, imageSize, onCaptured);
	}

	public static void iCaptureWithCamera(Camera camera, Action<Texture2D> onCaptured = null)
	{
		ScreenshotHelper.Instance.CaptureWithCamera(camera, onCaptured);
	}

	public static void iCaptureRenderTextureWithCamera(Camera camera, Action<RenderTexture> onCaptured = null)
	{
		ScreenshotHelper.Instance.CaptureRenderTextureWithCamera(camera, onCaptured);
	}

	public static void iRegisterRenderCamera(Camera camera)
	{
		ScreenshotHelper.Instance.RegisterRenderCamera(camera);
	}

	public static void iUnRegisterRenderCamera(Camera camera)
	{
		ScreenshotHelper.Instance.UnRegisterRenderCamera(camera);
	}

	public static void iUnRegisterAllRenderCameras()
	{
		ScreenshotHelper.Instance.UnRegisterAllRenderCameras();
	}

	public static void iClear()
	{
		ScreenshotHelper.Instance.Clear();
	}

	public void UpdateDebugText(string text)
	{
		if (this.m_DebugText != null)
		{
			this.m_DebugText.text = text;
		}
	}

	public static Sprite ToSprite(Texture2D texture)
	{
		if (texture == null)
		{
			return null;
		}
		Vector2 pivot = new Vector2(0.5f, 0.5f);
		float pixelsPerUnit = 100f;
		return Sprite.Create(texture, new Rect(0f, 0f, (float)texture.width, (float)texture.height), pivot, pixelsPerUnit);
	}

	public UnityEvent m_MainOnCaptured;

	public Vector2 m_CaptureSize = new Vector2(512f, 512f);

	private bool _isOnCapture;

	private Texture2D _texture2D;

	private RenderTexture _renderTexture;

	public Text m_DebugText;

	private static ScreenshotHelper _instance;
}
