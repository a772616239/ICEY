using System;
using System.IO;
using UnityEngine;

namespace Tools
{
	public class FBFile
	{
		public static bool SaveToFile(string path, string name, string str)
		{
			UnityEngine.Debug.LogError(path + name + " 不能在非编辑器下保存");
			return false;
		}

		public static string LoadFromFile(string path, string name)
		{
			string result = string.Empty;
			TextAsset textAsset = Resources.Load<TextAsset>(Path.Combine(path, name));
			if (textAsset != null)
			{
				result = textAsset.text;
			}
			else
			{
				Log.Error(Path.Combine(path, name) + " 文件不存在");
			}
			return result;
		}

		public static bool IsFileExist(string path, string name)
		{
			UnityEngine.Object x = Resources.Load(Path.Combine(path, name));
			return x != null;
		}
	}
}
