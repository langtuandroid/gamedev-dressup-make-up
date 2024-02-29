// dnSpy decompiler from Assembly-CSharp.dll class: TouchScreen
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TouchScreen : CheckedMonoBehaviour
{
	//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action<Vector3> OnTouchDownEvent;

	//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action<Vector3, Vector3> OnDragEvent;

	//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action<Vector3> OnTouchUpEvent;

	//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action<Vector3> OnTapEvent;

	//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnCanceledTapEvent;

	private void Awake()
	{
		this.tapTolerancePixels *= this.rootUICanvas.scaleFactor;
		foreach (EventSystem eventSystem in UnityEngine.Object.FindObjectsOfType<EventSystem>())
		{
			eventSystem.pixelDragThreshold = Mathf.RoundToInt(this.tapTolerancePixels);
		}
	}

	private void Update()
	{
		if (Input.touches.Length > 0)
		{
			Touch touch = Input.touches[0];
			switch (touch.phase)
			{
			case TouchPhase.Began:
				this.vTouchStart = touch.position;
				this.vTouchLastPos = touch.position;
				this.bIsDragging = false;
				if (!this.IsPointOverUIObject(touch.position) && this.OnTouchDownEvent != null)
				{
					this.OnTouchDownEvent(touch.position);
				}
				break;
			case TouchPhase.Moved:
				if (!this.bIsDragging)
				{
					float sqrMagnitude = (touch.position - (Vector2)this.vTouchStart).sqrMagnitude;
					if (sqrMagnitude > this.tapTolerancePixels * this.tapTolerancePixels)
					{
						this.bIsDragging = true;
					}
				}
				if (this.bIsDragging && this.OnDragEvent != null)
				{
					this.OnDragEvent(touch.position - (Vector2)this.vTouchLastPos, touch.position);
				}
				this.vTouchLastPos = touch.position;
				break;
			case TouchPhase.Ended:
				if (!this.IsPointOverUIObject(touch.position) && this.OnTouchUpEvent != null)
				{
					this.OnTouchUpEvent(touch.position);
				}
				if (this.bIsDragging)
				{
					this.bIsDragging = false;
					if (this.OnDragEvent != null)
					{
						this.OnDragEvent(touch.position - (Vector2)this.vTouchLastPos, touch.position);
					}
				}
				else if (!this.IsPointOverUIObject(touch.position) && this.OnTapEvent != null)
				{
					this.OnTapEvent(touch.position);
				}
				break;
			case TouchPhase.Canceled:
				this.bIsDragging = false;
				if (!this.IsPointOverUIObject(touch.position) && this.OnCanceledTapEvent != null)
				{
					this.OnCanceledTapEvent();
				}
				break;
			}
			return;
		}
		if (Input.GetMouseButtonDown(0))
		{
			this.vTouchStart = UnityEngine.Input.mousePosition;
			this.vTouchLastPos = UnityEngine.Input.mousePosition;
			this.bIsDragging = false;
			if (!this.IsPointerOverUIObject() && this.OnTouchDownEvent != null)
			{
				this.OnTouchDownEvent(UnityEngine.Input.mousePosition);
			}
		}
		else if (Input.GetMouseButton(0))
		{
			if (!this.bIsDragging)
			{
				float sqrMagnitude2 = (UnityEngine.Input.mousePosition - this.vTouchStart).sqrMagnitude;
				if (sqrMagnitude2 > this.tapTolerancePixels * this.tapTolerancePixels)
				{
					this.bIsDragging = true;
				}
			}
			if (this.bIsDragging)
			{
				if (this.OnDragEvent != null)
				{
					this.OnDragEvent(UnityEngine.Input.mousePosition - this.vTouchLastPos, UnityEngine.Input.mousePosition);
				}
				this.vTouchLastPos = UnityEngine.Input.mousePosition;
			}
		}
		else if (Input.GetMouseButtonUp(0))
		{
			if (!this.IsPointerOverUIObject() && this.OnTouchUpEvent != null)
			{
				this.OnTouchUpEvent(UnityEngine.Input.mousePosition);
			}
			if (this.bIsDragging)
			{
				if (!this.IsPointerOverUIObject() && this.OnDragEvent != null)
				{
					this.OnDragEvent(UnityEngine.Input.mousePosition - this.vTouchLastPos, UnityEngine.Input.mousePosition);
				}
			}
			else if (!this.IsPointerOverUIObject() && this.OnTapEvent != null)
			{
				this.OnTapEvent(UnityEngine.Input.mousePosition);
			}
			this.bIsDragging = false;
		}
	}

	private bool IsPointOverUIObject(Vector2 screenPosition)
	{
		PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
		pointerEventData.position = screenPosition;
		GraphicRaycaster component = this.rootUICanvas.gameObject.GetComponent<GraphicRaycaster>();
		List<RaycastResult> list = new List<RaycastResult>();
		component.Raycast(pointerEventData, list);
		return list.Count > 0;
	}

	private bool IsPointerOverUIObject()
	{
		PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
		pointerEventData.position = new Vector2(UnityEngine.Input.mousePosition.x, UnityEngine.Input.mousePosition.y);
		List<RaycastResult> list = new List<RaycastResult>();
		EventSystem.current.RaycastAll(pointerEventData, list);
		return list.Count > 0;
	}

	private Vector3 vTouchStart;

	private Vector3 vTouchLastPos;

	public float tapTolerancePixels = 10f;

	[NonSerialized]
	public bool bIsDragging;

	public Canvas rootUICanvas;
}
