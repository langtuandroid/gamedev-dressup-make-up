// dnSpy decompiler from Assembly-CSharp.dll class: GameDressUp
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random=UnityEngine.Random;

[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(TouchScreen))]
public class GameDressUp : Game
{
	protected override void OnAwake()
	{
		base.OnAwake();
		this.panelItemGroups = GameObject.Find("PanelItemGroups");
		this.btnItemGroup0 = GameObject.Find("BtnItemGroup0");
		this.btnItemGroup1 = GameObject.Find("BtnItemGroup1");
		this.panelItems = GameObject.Find("PanelItems");
		this.btnItem0 = GameObject.Find("BtnItem0");
		this.btnItem1 = GameObject.Find("BtnItem1");
		this.scrollRectItems = GameObject.Find("ScrollRectItems");
		this.PopulateItemGroupsButtons();
		this.btnItems.Add(this.btnItem0);
		this.btnItems.Add(this.btnItem1);
		this.ResetDressUp();
	}

	private void PopulateItemGroupsButtons()
	{
		this.btnItemGroups = new GameObject[this.itemGroups.Length];
		Vector3 a = this.btnItemGroup1.transform.localPosition - this.btnItemGroup0.transform.localPosition;
		if (this.itemGroups.Length <= 1)
		{
			this.btnItemGroup1.SetActive(false);
		}
		else
		{
			this.btnItemGroups[1] = this.btnItemGroup1;
		}
		if (this.itemGroups.Length == 0)
		{
			this.btnItemGroup0.SetActive(false);
		}
		else
		{
			this.btnItemGroups[0] = this.btnItemGroup0;
		}
		for (int i = 2; i < this.itemGroups.Length; i++)
		{
			Vector3 localPosition = this.btnItemGroup0.transform.localPosition + a * (float)i;
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.btnItemGroup0, this.panelItemGroups.transform);
			gameObject.name = "BtnItemGroup" + i;
			gameObject.transform.localPosition = localPosition;
			this.btnItemGroups[i] = gameObject;
		}
		this.AdjustItemGroupsPanelSize();
		this.PopulateItemsGroupsIcons();
	}

	private void AdjustItemGroupsPanelSize()
	{
		Vector3 vector = this.btnItemGroup1.transform.localPosition - this.btnItemGroup0.transform.localPosition;
		float num = Mathf.Abs(vector.x * (float)this.itemGroups.Length);
		float num2 = Mathf.Abs(vector.y * (float)this.itemGroups.Length);
		if (num2 > num)
		{
			float num3 = 50f;
			num2 += num3;
		}
		this.panelItemGroups.GetComponent<RectTransform>().sizeDelta = new Vector2(num, num2);
	}

	private void PopulateItemsGroupsIcons()
	{
		for (int i = 0; i < this.itemGroups.Length; i++)
		{
			Image component = this.btnItemGroups[i].transform.GetChild(0).GetComponent<Image>();
			component.sprite = this.itemGroups[i].icon;
		}
	}

	private void ResetDressUp()
	{
		this.popupScreenshot.SetActive(false);
		this.popupUnequipQ.SetActive(false);
		if (null == this.dollInsteadOfSelector)
		{
			this.panelDollSelection.SetActive(true);
		}
		else
		{
			this.StartGameWithDoll(this.dollInsteadOfSelector);
		}
	}

	public void StartGameWithDoll(UnityEngine.Object prefabDoll)
	{
		this.panelDollSelection.SetActive(false);
		this.doll = UnityEngine.Object.Instantiate<GameObject>(prefabDoll as GameObject).GetComponent<Doll>();
		if (this.itemGroups.Length > 0)
		{
			this.SelectItemGroup(0);
		}
		this.EquipDefaultItems();
	}

	public void OnItemGroupBtnPressed(GameObject btnItemGroup)
	{
		int num = Array.IndexOf<GameObject>(this.btnItemGroups, btnItemGroup);
		if (num != -1)
		{
			this.SelectItemGroup(num);
		}
	}

	private void SelectItemGroup(int groupIndex)
	{
		this.selectedItemGroupIndex = groupIndex;
		this.itemsInSelectedGroup = this.itemGroups[groupIndex].items;
		if (this.itemsInSelectedGroup == null)
		{
			this.itemsInSelectedGroup = new EquippableItem[0];
		}
		this.RefreshSelectedStateOfItemGroupsPanel();
		this.PopulateItemsButtons();
	}

	private void RefreshSelectedStateOfItemGroupsPanel()
	{
		for (int i = 0; i < this.itemGroups.Length; i++)
		{
			this.btnItemGroups[i].GetComponent<Image>().sprite = ((this.selectedItemGroupIndex != i) ? this.btn_item_group : this.btn_item_group_selected);
		}
	}

	private void PopulateItemsButtons()
	{
		if (this.itemsInSelectedGroup.Length > this.btnItems.Count)
		{
			this.CreateNewItemsButtons();
		}
		for (int i = 0; i < this.btnItems.Count; i++)
		{
			if (i < this.itemsInSelectedGroup.Length)
			{
				this.btnItems[i].SetActive(true);
				this.UpdateItemBtnImage(i);
			}
			else
			{
				this.btnItems[i].SetActive(false);
			}
		}
		this.RefreshEquipStateOfItemsPanel();
		this.AdjustItemsPanelSize();
		this.ResetItemsScrollRect();
	}

	private void CreateNewItemsButtons()
	{
		for (int i = this.btnItems.Count; i < this.itemsInSelectedGroup.Length; i++)
		{
			this.CreateNewItemButton(i);
		}
	}

	private void CreateNewItemButton(int itemIndex)
	{
		Vector3 a = this.btnItem1.transform.localPosition - this.btnItem0.transform.localPosition;
		Vector3 localPosition = this.btnItem0.transform.localPosition + a * (float)itemIndex;
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.btnItem0, this.panelItems.transform);
		gameObject.name = "BtnItem" + itemIndex;
		gameObject.transform.localPosition = localPosition;
		this.btnItems.Add(gameObject);
	}

	private void UpdateItemBtnImage(int itemIndex)
	{
		Image component = this.btnItems[itemIndex].transform.GetChild(0).GetComponent<Image>();
		Sprite sprite = null;
		Color white = Color.white;
		this.itemsInSelectedGroup[itemIndex].GetIcon(ref sprite, ref white);
		component.sprite = sprite;
		component.color = white;
	}

	private void AdjustItemsPanelSize()
	{
		Vector3 vector = this.btnItem1.transform.localPosition - this.btnItem0.transform.localPosition;
		float num = Mathf.Abs(vector.x * (float)this.itemsInSelectedGroup.Length);
		float num2 = Mathf.Abs(vector.y * (float)this.itemsInSelectedGroup.Length);
		if (num2 > num)
		{
			float num3 = 50f;
			num2 += num3;
		}
		this.panelItems.GetComponent<RectTransform>().sizeDelta = new Vector2(num, num2);
	}

	private void ResetItemsScrollRect()
	{
		this.scrollRectItems.GetComponent<ScrollRect>().horizontalNormalizedPosition = 0f;
		this.scrollRectItems.GetComponent<ScrollRect>().verticalNormalizedPosition = 1f;
	}

	public void OnItemBtnPressed(GameObject btnItem)
	{
		int num = this.btnItems.IndexOf(btnItem);
		if (num != -1)
		{
			EquippableItem item = this.itemsInSelectedGroup[num];
			this.ToggleItemEquippedState(item);
		}
	}

	public void ToggleItemEquippedState(EquippableItem item)
	{
		if (this.doll.HasItemEquipped(item))
		{
			if (this.CanUnequipItemWithoutReplacement(item))
			{
				this.doll.UnequipItem(item);
			}
		}
		else
		{
			this.doll.EquipItem(item);
		}
		this.RefreshEquipStateOfItemsPanel();
	}

	private void RefreshEquipStateOfItemsPanel()
	{
		for (int i = 0; i < this.itemsInSelectedGroup.Length; i++)
		{
			this.btnItems[i].GetComponent<Image>().sprite = ((!this.doll.HasItemEquipped(this.itemsInSelectedGroup[i])) ? this.btn_item : this.btn_item_equipped);
		}
	}

	private void EquipDefaultItems()
	{
		foreach (EquippableItem newItem in this.itemsEquippedByDefault)
		{
			this.doll.EquipItem(newItem);
		}
		this.doll.EquipDefaultDollSpecificItems();
		this.RefreshEquipStateOfItemsPanel();
	}

	public void MakeScreenshotToGallery()
	{
	
			base.StartCoroutine(this.MakeScreenshotToGalleryCoroutine());
		
	}

	public void ShowScreenshotStatusPopup(string msg)
	{
		this.popupScreenshot.SetActive(true);
		Text componentInChildren = this.popupScreenshot.GetComponentInChildren<Text>();
		componentInChildren.text = msg;
		//this.getAds.ShowHideAds(true);
	}

	public void CloseScreenshotStatusPopup()
	{
		this.popupScreenshot.SetActive(false);
//		this.SetCameraParamsForItemGroup(this.selectedItemGroupIndex);
	}

	private IEnumerator MakeScreenshotToGalleryCoroutine()
	{
		this.canvas.gameObject.SetActive(false);
		ScreenshotHelper.iCaptureWithCamera(this.GetComponent<Camera>(), delegate(Texture2D texture2D)
		{
			base.StartCoroutine(this.SaveImage(texture2D));
			this.ShowScreenshotStatusPopup("Screenshot was successfully saved in album '" + Application.productName + "'");
			AudioUtils.PlaySound(base.GetComponent<AudioSource>(), this.audioMakeScreenshot);
		});
		yield return new WaitForSeconds(1.5f);
		this.canvas.gameObject.SetActive(true);
		yield break;
	}

	private IEnumerator SaveImage(Texture2D val)
	{
		yield return new WaitForEndOfFrame();
		string sTimestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
		string sFileName = sTimestamp + ".png";
		base.StartCoroutine(ScreenshotManager.Save(sFileName, val));
		yield break;
	}
	private void OnEnable()
	{
		this.touchScreen.OnTouchDownEvent += this.HandleOnTouchDownEvent;
	}

	private void OnDisable()
	{
		this.touchScreen.OnTouchDownEvent -= this.HandleOnTouchDownEvent;
	}

	private void HandleOnTouchDownEvent(Vector3 tapPos)
	{
		SpriteRenderer frontmostSpriteAtTapPos = this.GetFrontmostSpriteAtTapPos(tapPos);
		this.displayedItemToUnequip = EquippableItem.GetEquippableItemToWhichSpriteBelongs(frontmostSpriteAtTapPos);
		if (null != this.displayedItemToUnequip && this.doll.HasDisplayedItemEquipped(this.displayedItemToUnequip) && this.CanUnequipItemWithoutReplacement(this.displayedItemToUnequip))
		{
			this.popupUnequipQ.SetActive(true);
			Sprite sprite = null;
			Color white = Color.white;
			this.displayedItemToUnequip.GetIcon(ref sprite, ref white);
			this.imgItemToUnequip.sprite = sprite;
			this.imgItemToUnequip.color = white;
		}
	}

	private SpriteRenderer GetFrontmostSpriteAtTapPos(Vector3 tapPos)
	{
		Vector3 vector = base.GetComponent<Camera>().ScreenToWorldPoint(tapPos);
		Collider2D[] colliders = Physics2D.OverlapPointAll(new Vector2(vector.x, vector.y));
		return this.GetFrontmostSpriteFromColliders(colliders);
	}

	private SpriteRenderer GetFrontmostSpriteFromColliders(Collider2D[] colliders)
	{
		SpriteRenderer spriteRenderer = null;
		foreach (Collider2D collider2D in colliders)
		{
			SpriteRenderer component = collider2D.GetComponent<SpriteRenderer>();
			if (null != component && (null == spriteRenderer || component.sortingOrder > spriteRenderer.sortingOrder))
			{
				spriteRenderer = component;
			}
		}
		return spriteRenderer;
	}

	public void OnUnequipItemA(bool wantsToUnequip)
	{
		this.popupUnequipQ.SetActive(false);
		if (wantsToUnequip)
		{
			this.doll.UnequipDisplayedItem(this.displayedItemToUnequip);
			this.RefreshEquipStateOfItemsPanel();
		}
		this.displayedItemToUnequip = null;
	}

	public void OnBtnMoreGamesPressed()
	{
		WebLinks.OpenMoreGames();
	}

	public void OnBtnGiveRatingPressed()
	{
		WebLinks.RateGame();
	}

	public void OnBtnResetPressed()
	{
		if (null != this.doll)
		{
			UnityEngine.Object.Destroy(this.doll.gameObject);
		}
		this.doll = null;
		this.ResetDressUp();
	}

	private bool CanUnequipItemWithoutReplacement(EquippableItem item)
	{
		foreach (Slot value in item.neededSlots)
		{
			if (Array.IndexOf<Slot>(this.slotsThatCantBeEmpty, value) != -1)
			{
				return false;
			}
		}
		return true;
	}

	public string screenshotsAlbumName = "Dress Up";

	[AutoLink]
	private TouchScreen touchScreen;

	public GameObject panelDollSelection;

	private Doll doll;

	[CanBeNull]
	public GameObject dollInsteadOfSelector;

	public EquippableItemGroup[] itemGroups;

	private int selectedItemGroupIndex = -1;

	private GameObject panelItemGroups;

	private GameObject btnItemGroup0;

	private GameObject btnItemGroup1;

	private GameObject[] btnItemGroups;

	private GameObject panelItems;

	private GameObject btnItem0;

	private GameObject btnItem1;

	private GameObject scrollRectItems;

	private EquippableItem[] itemsInSelectedGroup = new EquippableItem[0];

	private List<GameObject> btnItems = new List<GameObject>();

	public Sprite btn_item;

	public Sprite btn_item_equipped;

	public Sprite btn_item_group;

	public Sprite btn_item_group_selected;

	[CanBeEmpty]
	public EquippableItem[] itemsEquippedByDefault;

	[CanBeEmpty]
	public Slot[] slotsThatCantBeEmpty;

	public GameObject popupScreenshot;

	public GameObject canvas;

	public AudioClip audioMakeScreenshot;

	public GameObject popupUnequipQ;

	private EquippableItem displayedItemToUnequip;

	public Image imgItemToUnequip;
}
