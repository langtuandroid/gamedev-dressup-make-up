// dnSpy decompiler from Assembly-CSharp.dll class: Game
using System;
using UnityEngine;

public abstract class Game : CheckedMonoBehaviour
{
	protected override void OnAwake()
	{
		this.timestampAtGameCreated = Time.time;
		base.OnAwake();
	}

	private void OnDestroy()
	{
		long num = (long)(Time.time - this.timestampAtGameCreated);
	}

	protected virtual void Start()
	{
	}

	protected virtual void Update()
	{
		if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
		{
			this.OnClosePressed();
			return;
		}
	}

	public void OnClosePressed()
	{
		Application.Quit();
	}

	private AsyncOperation loadingLevelProgress;

	private float timestampAtGameCreated;
}
