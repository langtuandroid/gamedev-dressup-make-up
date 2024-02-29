// dnSpy decompiler from Assembly-CSharp.dll class: AudioUtils
using System;
using UnityEngine;
using Random=UnityEngine.Random;
public static class AudioUtils
{
	public static void PlaySound(AudioSource audioSource, AudioClip audioClip)
	{
		AudioUtils.PlaySound(audioSource, audioClip, 1f);
	}

	public static void PlaySound(AudioSource audioSource, AudioClip audioClip, float volumeScale)
	{
		if (MyAudioSettings.IsSoundOn)
		{
			audioSource.PlayOneShot(audioClip, volumeScale);
		}
	}

	public static void PlayRandomSound(AudioSource audioSource, AudioClip[] audioClips)
	{
		AudioUtils.PlayRandomSound(audioSource, audioClips, 1f);
	}

	public static void PlayRandomSound(AudioSource audioSource, AudioClip[] audioClips, float volumeScale)
	{
		if (MyAudioSettings.IsSoundOn)
		{
			audioSource.PlayOneShot(audioClips[Random.Range(0, audioClips.Length)], volumeScale);
		}
	}

	public static void StartLoopingSound(AudioSource audioSource, AudioClip audioClip)
	{
		if (MyAudioSettings.IsSoundOn)
		{
			audioSource.loop = true;
			audioSource.clip = audioClip;
			audioSource.Play();
		}
	}

	public static void StopLooping(AudioSource audioSource)
	{
		audioSource.loop = false;
		audioSource.Stop();
	}
}
