// dnSpy decompiler from Assembly-CSharp.dll class: FilePathName
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

public class FilePathName
{
	public string GetSaveDirectory()
	{
		return Application.persistentDataPath;
	}

	public string GeFileNameWithoutExt(bool millisecond = false)
	{
		if (millisecond)
		{
			return this._GetComparedFileName(DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fff"), FilePathName._lastGeneratedFileNameWithoutExt_fff, FilePathName._lastSameFileNameCounter_fff, out FilePathName._lastGeneratedFileNameWithoutExt_fff, out FilePathName._lastSameFileNameCounter_fff);
		}
		return this._GetComparedFileName(DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"), FilePathName._lastGeneratedFileNameWithoutExt, FilePathName._lastSameFileNameCounter, out FilePathName._lastGeneratedFileNameWithoutExt, out FilePathName._lastSameFileNameCounter);
	}

	private string _GetComparedFileName(string newFileName, string lastGeneratedFileName, int sameFileNameCounter, out string outLastGeneratedFileName, out int outSameFileNameCounter)
	{
		if (lastGeneratedFileName == newFileName)
		{
			sameFileNameCounter++;
		}
		else
		{
			sameFileNameCounter = 1;
		}
		outLastGeneratedFileName = newFileName;
		outSameFileNameCounter = sameFileNameCounter;
		if (sameFileNameCounter > 1)
		{
			newFileName = newFileName + " " + sameFileNameCounter;
		}
		return newFileName;
	}

	public string EnsureLocalPath(string path)
	{
		if (!path.ToLower().StartsWith("file:///"))
		{
			while (path.StartsWith("/"))
			{
				path = path.Remove(0, 1);
			}
			path = "file:///" + path;
		}
		return path;
	}

	public string GetGifFileName()
	{
		string str = this.GeFileNameWithoutExt(false);
		return "GIF_" + str;
	}

	public string GetGifFullPath()
	{
		return this.GetSaveDirectory() + "/" + this.GetGifFileName() + ".gif";
	}

	public string GetDownloadedGifSaveFullPath()
	{
		return this.GetSaveDirectory() + "/" + this.GetGifFileName() + ".gif";
	}

	public string GetJpgFileName()
	{
		string str = this.GeFileNameWithoutExt(true);
		return "Photo_" + str;
	}

	public string GetJpgFullPath()
	{
		return this.GetSaveDirectory() + "/" + this.GetJpgFileName() + ".jpg";
	}

	public string GetPngFileName()
	{
		string str = this.GeFileNameWithoutExt(true);
		return "Photo_" + str;
	}

	public string GetPngFullPath()
	{
		return this.GetSaveDirectory() + "/" + this.GetPngFileName() + ".png";
	}

	public byte[] ReadFileToBytes(string fromFullPath)
	{
		return File.ReadAllBytes(fromFullPath);
	}

	public void WriteBytesToFile(string toFullpath, byte[] byteArray)
	{
		File.WriteAllBytes(toFullpath, byteArray);
	}

	public void CopyFile(string fromFullPath, string toFullPath)
	{
		File.Copy(fromFullPath, toFullPath);
	}

	public bool FileStreamTo(string fullpath, byte[] byteArray)
	{
		bool result;
		try
		{
			using (FileStream fileStream = new FileStream(fullpath, FileMode.Create, FileAccess.Write))
			{
				fileStream.Write(byteArray, 0, byteArray.Length);
				result = true;
			}
		}
		catch (Exception arg)
		{
			Console.WriteLine("Exception caught in process: {0}", arg);
			result = false;
		}
		return result;
	}

	public void WriteBytesToText(byte[] bytes, string toFullPath, string separator = "", bool toChar = true)
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (string.IsNullOrEmpty(separator))
		{
			if (toChar)
			{
				for (int i = 0; i < bytes.Length; i++)
				{
					stringBuilder.Append((char)bytes[i]);
				}
			}
			else
			{
				for (int j = 0; j < bytes.Length; j++)
				{
					stringBuilder.Append(bytes[j]);
				}
			}
		}
		else if (toChar)
		{
			for (int k = 0; k < bytes.Length; k++)
			{
				stringBuilder.Append((char)bytes[k]);
				stringBuilder.Append(separator);
			}
		}
		else
		{
			for (int l = 0; l < bytes.Length; l++)
			{
				stringBuilder.Append(bytes[l]);
				stringBuilder.Append(separator);
			}
		}
		File.WriteAllText(toFullPath, stringBuilder.ToString());
	}

	public string SaveTextureAs(Texture2D texture2D, FilePathName.SaveFormat format = FilePathName.SaveFormat.JPG)
	{
		string text = string.Empty;
		if (format != FilePathName.SaveFormat.JPG)
		{
			if (format != FilePathName.SaveFormat.PNG)
			{
				if (format != FilePathName.SaveFormat.GIF)
				{
				}
			}
			else
			{
				text = this.GetPngFullPath();
				this.WriteBytesToFile(text, texture2D.EncodeToPNG());
			}
		}
		else
		{
			text = this.GetJpgFullPath();
			this.WriteBytesToFile(text, texture2D.EncodeToJPG(90));
		}
		return text;
	}

	public Texture2D LoadImage(string fullFilePath)
	{
		if (!File.Exists(fullFilePath))
		{
			return null;
		}
		Texture2D texture2D = new Texture2D(1, 1);
		texture2D.LoadImage(this.ReadFileToBytes(fullFilePath));
		return texture2D;
	}

	public List<Texture2D> LoadImages(string directory, List<string> fileExtensions = null)
	{
		if (fileExtensions == null || fileExtensions.Count <= 0)
		{
			fileExtensions = new List<string>
			{
				".jpg",
				".png",
				".gif"
			};
		}
		List<Texture2D> list = new List<Texture2D>();
		foreach (string text in this.GetFilePaths(directory, fileExtensions))
		{
			if (fileExtensions.Contains(Path.GetExtension(text).ToLower()))
			{
				list.Add(this.LoadImage(text));
			}
		}
		return list;
	}

	public List<byte[]> LoadFiles(string directory, List<string> fileExtensions = null)
	{
		List<byte[]> list = new List<byte[]>();
		foreach (string fromFullPath in this.GetFilePaths(directory, fileExtensions))
		{
			list.Add(this.ReadFileToBytes(fromFullPath));
		}
		return list;
	}

	public List<string> GetFilePaths(string directory, List<string> fileExtensions = null)
	{
		if (!Directory.Exists(directory))
		{
			throw new DirectoryNotFoundException("Directory not found at " + directory);
		}
		string[] files = Directory.GetFiles(directory);
		bool flag = fileExtensions == null || fileExtensions.Count <= 0;
		if (flag)
		{
			return files.ToList<string>();
		}
		if (fileExtensions == null)
		{
			fileExtensions = new List<string>();
		}
		else
		{
			for (int i = 0; i < fileExtensions.Count; i++)
			{
				fileExtensions[i] = fileExtensions[i].ToLower();
			}
		}
		List<string> list = new List<string>();
		foreach (string text in files)
		{
			if (fileExtensions.Contains(Path.GetExtension(text).ToLower()))
			{
				list.Add(text);
			}
		}
		return list;
	}

	public IEnumerator LoadFileWWW(string url, Action<byte[]> onLoadCompleted)
	{
		string path = url;
		if (!path.StartsWith("http"))
		{
			path = this.EnsureLocalPath(path);
		}
		using (WWW www = new WWW(path))
		{
			yield return www;
			if (!string.IsNullOrEmpty(www.error))
			{
				UnityEngine.Debug.LogError("File load error.\n" + www.error);
				onLoadCompleted(null);
				yield break;
			}
			onLoadCompleted(www.bytes);
		}
		yield break;
	}

	public Sprite Texture2DToSprite(Texture2D texture2D)
	{
		if (texture2D == null)
		{
			return null;
		}
		Vector2 pivot = new Vector2(0.5f, 0.5f);
		float pixelsPerUnit = 100f;
		return Sprite.Create(texture2D, new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), pivot, pixelsPerUnit);
	}

	private static string _lastGeneratedFileNameWithoutExt_fff = string.Empty;

	private static int _lastSameFileNameCounter_fff = 1;

	private static string _lastGeneratedFileNameWithoutExt = string.Empty;

	private static int _lastSameFileNameCounter = 1;

	public enum SaveFormat
	{
		NONE = -1,
		GIF,
		JPG,
		PNG
	}
}
