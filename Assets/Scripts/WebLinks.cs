// dnSpy decompiler from Assembly-CSharp.dll class: WebLinks
using System;
using UnityEngine;

public static class WebLinks
{
	public static void RateGame()
	{
		string url = string.Empty;
		if (Application.platform == RuntimePlatform.Android)
		{
			url = "market://details?id=com.gamesforgirlsfree.animeprincess";
		}
		else if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			url = "https://itunes.apple.com/app/anime-princess-dress-up-games-for-girls/id1239336991";
		}
		else
		{
			url = "https://play.google.com/store/apps/details?id=com.gamesforgirlsfree.animeprincess";
		}
		Application.OpenURL(url);
	}

	public static void OpenMoreGames()
	{
		string url = string.Empty;
		if (Application.platform == RuntimePlatform.Android)
		{
			url = WebLinks.ANDROID_MORE_GAMES;
		}
		else if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			url = WebLinks.IOS_MORE_GAMES;
		}
		else
		{
			url = WebLinks.ANDROID_MORE_GAMES_WEB;
		}
		Application.OpenURL(url);
	}

	public const string ANDROID_LISTING = "market://details?id=com.gamesforgirlsfree.animeprincess";

	public const string IOS_LISTING = "https://itunes.apple.com/app/anime-princess-dress-up-games-for-girls/id1239336991";

	public const string ANDROID_LISTING_WEB = "https://play.google.com/store/apps/details?id=com.gamesforgirlsfree.animeprincess";

	public static string ANDROID_MORE_GAMES = "market://search?q=pub:Best%20Dress%20Up%20Games%20For%20Girls";

	public static string IOS_MORE_GAMES = "itms://itunes.apple.com/artist/irina-marina/id1131525291";

	public static string ANDROID_MORE_GAMES_WEB = "https://play.google.com/store/apps/developer?id=Best%20Dress%20Up%20Games%20For%20Girls";
}
