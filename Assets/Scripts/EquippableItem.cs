// dnSpy decompiler from Assembly-CSharp.dll class: EquippableItem
using System;
using UnityEngine;

public class EquippableItem : CheckedMonoBehaviour
{
	public void GetIcon(ref Sprite iconSprite, ref Color iconColor)
	{
		if (null != this.iconSprite)
		{
			iconSprite = this.iconSprite;
			iconColor = this.iconColor;
		}
		else
		{
			SpriteRenderer componentInChildren = base.gameObject.GetComponentInChildren<SpriteRenderer>();
			if (null == componentInChildren)
			{
				return;
			}
			iconSprite = componentInChildren.sprite;
			iconColor = componentInChildren.color;
		}
	}

	public static EquippableItem GetEquippableItemToWhichSpriteBelongs(SpriteRenderer spr)
	{
		if (null == spr)
		{
			return null;
		}
		EquippableItem component = spr.GetComponent<EquippableItem>();
		Transform transform = spr.transform;
		while (null == component && null != transform.parent)
		{
			transform = transform.parent;
			component = transform.GetComponent<EquippableItem>();
		}
		return component;
	}

	public Slot[] neededSlots;

	[CanBeNull]
	public Sprite iconSprite;

	public Color iconColor = Color.white;
}
