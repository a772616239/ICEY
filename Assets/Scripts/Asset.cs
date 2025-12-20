using System;
using System.IO;
using LitJson;
using Tools;
using UnityEngine;

public class Asset
{
	public static bool SerializeToFile(string path, string name, object obj)
	{
		string str = JsonMapper.ToJson(obj);
		return FBFile.SaveToFile(path, name, str);
	}

	public static T DeserializeFromFile<T>(string path, string name)
	{
		string json = FBFile.LoadFromFile(path, name);
		return JsonMapper.ToObject<T>(json);
	}

	public static T LoadFromResources<T>(string path, string name) where T : UnityEngine.Object
	{
		return Resources.Load<T>(System.IO.Path.Combine(path, name));
	}

	public struct Path
	{
		public const string Config = "Conf/";
	}
}
