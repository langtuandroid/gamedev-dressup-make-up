// dnSpy decompiler from Assembly-CSharp.dll class: MyAudioSettings
using System;
using UnityEngine;

public static class MyAudioSettings
{
	public static bool IsMusicOn
	{
		get
		{
			MyAudioSettings.Load();
			return MyAudioSettings.isMusicOn;
		}
		set
		{
			MyAudioSettings.isMusicOn = value;
			PlayerPrefs.SetInt("music", (!MyAudioSettings.isMusicOn) ? 0 : 1);
		}
	}

	public static bool IsSoundOn
	{
		get
		{
			MyAudioSettings.Load();
			return MyAudioSettings.isSoundOn;
		}
		set
		{
			MyAudioSettings.isSoundOn = value;
			PlayerPrefs.SetInt("sound", (!MyAudioSettings.isSoundOn) ? 0 : 1);
		}
	}

	private static void Load()
	{
		if (MyAudioSettings.areSettingsLoaded)
		{
			return;
		}
		MyAudioSettings.isMusicOn = (PlayerPrefs.GetInt("music", 1) != 0);
		MyAudioSettings.isSoundOn = (PlayerPrefs.GetInt("sound", 1) != 0);
		MyAudioSettings.areSettingsLoaded = true;
	}

	private static bool areSettingsLoaded;

	private static bool isMusicOn;

	private static bool isSoundOn;
}
