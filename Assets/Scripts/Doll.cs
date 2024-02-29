// dnSpy decompiler from Assembly-CSharp.dll class: Doll
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Doll : CheckedMonoBehaviour
{
	public bool HasItemEquipped(EquippableItem item)
	{
		return -1 != Array.IndexOf<EquippableItem>(this.itemPrefabsForSlots, item);
	}

	public void EquipItem(EquippableItem newItem)
	{
		this.UnequipAllItemsSharingSlotWith(newItem);
		foreach (Slot slot in newItem.neededSlots)
		{
			this.itemPrefabsForSlots[(int)slot] = newItem;
		}
		EquippableItem component = UnityEngine.Object.Instantiate<GameObject>(newItem.gameObject).GetComponent<EquippableItem>();
		this.displayedItems[newItem] = component;
		component.transform.parent = base.transform;
	}

	private void UnequipAllItemsSharingSlotWith(EquippableItem item)
	{
		foreach (Slot slot in item.neededSlots)
		{
			EquippableItem equippableItem = this.itemPrefabsForSlots[(int)slot];
			if (null != equippableItem)
			{
				this.UnequipItem(equippableItem);
			}
		}
	}

	public void UnequipItem(EquippableItem item)
	{
		foreach (Slot slot in item.neededSlots)
		{
			this.itemPrefabsForSlots[(int)slot] = null;
		}
		if (this.displayedItems.ContainsKey(item) && null != this.displayedItems[item])
		{
			UnityEngine.Object.Destroy(this.displayedItems[item].gameObject);
			this.displayedItems[item] = null;
		}
	}

	public void UnequipDisplayedItem(EquippableItem displayedItem)
	{
		EquippableItem equippableItem = (from k in this.displayedItems
		where k.Value == displayedItem
		select k.Key).FirstOrDefault<EquippableItem>();
		if (null != equippableItem)
		{
			this.UnequipItem(equippableItem);
		}
	}

	public bool HasDisplayedItemEquipped(EquippableItem displayedItem)
	{
		return this.displayedItems.ContainsValue(displayedItem);
	}

	public void EquipDefaultDollSpecificItems()
	{
		foreach (EquippableItem newItem in this.defaultDollSpecificItems)
		{
			this.EquipItem(newItem);
		}
	}

	[CanBeEmpty]
	public EquippableItem[] defaultDollSpecificItems;

	[CanHaveNullElements]
	private EquippableItem[] itemPrefabsForSlots = new EquippableItem[36];

	private Dictionary<EquippableItem, EquippableItem> displayedItems = new Dictionary<EquippableItem, EquippableItem>();
}
