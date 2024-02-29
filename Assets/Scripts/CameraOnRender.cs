// dnSpy decompiler from Assembly-CSharp.dll class: CameraOnRender
using System;
using UnityEngine;

public class CameraOnRender : MonoBehaviour
{
	private void Start()
	{
		this.m_RenderTexture = new RenderTexture(4, 4, 24);
	}

	public void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		Graphics.Blit(source, destination);
		if (!this.m_ToCapture)
		{
			return;
		}
		this.m_ToCapture = false;
		if (this.m_RenderTexture.width != source.width || this.m_RenderTexture.height != source.height)
		{
			this.m_RenderTexture = new RenderTexture(source.width, source.height, 24);
		}
		this.m_RenderTexture.DiscardContents();
		Graphics.Blit(source, this.m_RenderTexture);
		if (this._onCaptureCallback != null)
		{
			this._onCaptureCallback(this.GetLastTexture2D());
			this._onCaptureCallback = null;
		}
		if (this._onCaptureCallbackRTex != null)
		{
			RenderTexture renderTexture = new RenderTexture(this.m_RenderTexture.width, this.m_RenderTexture.height, 24);
			Graphics.Blit(this.m_RenderTexture, renderTexture);
			this._onCaptureCallbackRTex(renderTexture);
			this._onCaptureCallbackRTex = null;
		}
	}

	public void SetOnCaptureCallback(Action<Texture2D> onCaptured)
	{
		this._onCaptureCallback = onCaptured;
		this.m_ToCapture = true;
	}

	public void SetOnCaptureCallback(Action<RenderTexture> onCaptured)
	{
		this._onCaptureCallbackRTex = onCaptured;
		this.m_ToCapture = true;
	}

	public Texture2D GetLastTexture2D()
	{
		return this._RenderTextureToTexture2D(this.m_RenderTexture);
	}

	public RenderTexture GetLastRenderTexture()
	{
		return this.m_RenderTexture;
	}

	private Texture2D _RenderTextureToTexture2D(RenderTexture source)
	{
		RenderTexture.active = source;
		Texture2D texture2D = new Texture2D(source.width, source.height, TextureFormat.RGB24, false);
		texture2D.ReadPixels(new Rect(0f, 0f, (float)source.width, (float)source.height), 0, 0);
		texture2D.Apply();
		RenderTexture.active = null;
		return texture2D;
	}

	public void Clear()
	{
		this._onCaptureCallback = null;
		if (this.m_RenderTexture != null)
		{
			UnityEngine.Object.Destroy(this.m_RenderTexture);
		}
		UnityEngine.Object.Destroy(this);
	}

	[HideInInspector]
	public RenderTexture m_RenderTexture;

	[HideInInspector]
	public bool m_ToCapture = true;

	private Action<Texture2D> _onCaptureCallback;

	private Action<RenderTexture> _onCaptureCallbackRTex;
}
