// dnSpy decompiler from Assembly-CSharp.dll class: DImageDisplayHandler
using System;
using UnityEngine;
using UnityEngine.UI;

public class DImageDisplayHandler : MonoBehaviour
{
	public void SetImage(Image displayImage, Sprite sprite)
	{
		this.Clear(displayImage);
		displayImage.sprite = sprite;
		this._SetSize(displayImage);
	}

	public void SetImage(Image displayImage, Texture2D texture2D)
	{
		this.Clear(displayImage);
		displayImage.sprite = this._TextureToSprite(texture2D);
		this._SetSize(displayImage);
	}

	public void SetRawImage(RawImage displayImage, Sprite sprite)
	{
		this.Clear(displayImage);
		displayImage.texture = sprite.texture;
		this._SetSize(displayImage);
	}

	public void SetRawImage(RawImage displayImage, Texture2D texture2D)
	{
		this.Clear(displayImage);
		displayImage.texture = texture2D;
		this._SetSize(displayImage);
	}

	private void _SetSize(Image displayImage)
	{
		if (this.m_BoundingType == DImageDisplayHandler.BoundingType.SetNativeSize)
		{
			displayImage.SetNativeSize();
		}
		else
		{
			displayImage.rectTransform.sizeDelta = this._CalculateSize(new Vector2((float)displayImage.sprite.texture.width, (float)displayImage.sprite.texture.height));
		}
		this._ApplyScaleFactor(displayImage.transform);
	}

	private void _SetSize(RawImage displayImage)
	{
		if (this.m_BoundingType == DImageDisplayHandler.BoundingType.SetNativeSize)
		{
			displayImage.SetNativeSize();
		}
		else
		{
			displayImage.rectTransform.sizeDelta = this._CalculateSize(new Vector2((float)displayImage.texture.width, (float)displayImage.texture.height));
		}
		this._ApplyScaleFactor(displayImage.transform);
	}

	private void _ApplyScaleFactor(Transform displayImageT)
	{
		displayImageT.localScale = new Vector3(this.m_ScaleFactor, this.m_ScaleFactor, 1f);
	}

	private Vector2 _CalculateSize(Vector2 textureSize)
	{
		Vector2 vector = Vector2.zero;
		DImageDisplayHandler.BoundingTarget boundingTarget = this.m_BoundingTarget;
		if (boundingTarget != DImageDisplayHandler.BoundingTarget.Size)
		{
			if (boundingTarget != DImageDisplayHandler.BoundingTarget.RectTransform)
			{
				if (boundingTarget == DImageDisplayHandler.BoundingTarget.Screen)
				{
					vector = new Vector2((float)Screen.width, (float)Screen.height);
				}
			}
			else
			{
				vector = this.m_RectTransform.GetComponent<RectTransform>().rect.size;
			}
		}
		else
		{
			vector = this.m_Size;
		}
		float num = textureSize.x;
		float num2 = textureSize.y;
		float num3 = num / num2;
		switch (this.m_BoundingType)
		{
		case DImageDisplayHandler.BoundingType.WidthAndHeight:
			num = vector.x;
			num2 = num / num3;
			if (num2 > vector.y)
			{
				num2 = vector.y;
				num = num2 * num3;
			}
			break;
		case DImageDisplayHandler.BoundingType.Width:
			num = vector.x;
			num2 = num / num3;
			break;
		case DImageDisplayHandler.BoundingType.Height:
			num2 = vector.y;
			num = num2 * num3;
			break;
		default:
			num = textureSize.x;
			num2 = textureSize.y;
			break;
		}
		return new Vector2(num, num2);
	}

	private Sprite _TextureToSprite(Texture2D texture)
	{
		if (texture == null)
		{
			return null;
		}
		Vector2 pivot = new Vector2(0.5f, 0.5f);
		float pixelsPerUnit = 100f;
		return Sprite.Create(texture, new Rect(0f, 0f, (float)texture.width, (float)texture.height), pivot, pixelsPerUnit);
	}

	public void Clear(Image displayImage)
	{
		if (displayImage != null && displayImage.sprite != null && displayImage.sprite.texture != null)
		{
			UnityEngine.Object.Destroy(displayImage.sprite.texture);
			displayImage.sprite = null;
		}
	}

	public void Clear(RawImage displayImage)
	{
		if (displayImage != null && displayImage.texture != null)
		{
			UnityEngine.Object.Destroy(displayImage.texture);
			displayImage.texture = null;
		}
	}

	[Header("[ Image Display Handler ]")]
	public DImageDisplayHandler.BoundingTarget m_BoundingTarget;

	public RectTransform m_RectTransform;

	public Vector2 m_Size = new Vector2(512f, 512f);

	[Space]
	public DImageDisplayHandler.BoundingType m_BoundingType;

	[Space]
	public float m_ScaleFactor = 1f;

	public enum BoundingTarget
	{
		Size,
		RectTransform,
		Screen
	}

	public enum BoundingType
	{
		SetNativeSize,
		WidthAndHeight,
		Width,
		Height
	}
}
