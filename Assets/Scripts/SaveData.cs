using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using LitJson;
using UnityEngine;

public class SaveData : BaseBehaviour
{
	//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public static event EventHandler<SaveLoadedEventArgs> OnGameLoaded;

	private static string SaveDataPath
	{
		get
		{
			return Application.persistentDataPath + "/SaveData/";
		}
	}

	private static string SaveDataFilePath
	{
		get
		{
			return SaveData.SaveDataPath + "save_data.bin";
		}
	}

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
	}

	private void Update()
	{
	}

	public static bool IsBusy
	{
		get
		{
			return false;
		}
	}

	public static bool Save(GameData data)
	{
		byte[] buffer = SaveData.GetBuffer(data);
		bool result;
		try
		{
			if (!Directory.Exists(SaveData.SaveDataPath))
			{
				Directory.CreateDirectory(SaveData.SaveDataPath);
			}
			FileStream fileStream = File.OpenWrite(SaveData.SaveDataFilePath);
			BinaryWriter binaryWriter = new BinaryWriter(fileStream, Encoding.UTF8);
			binaryWriter.Write(buffer);
			binaryWriter.Close();
			fileStream.Close();
			result = true;
		}
		catch (FileNotFoundException)
		{
			Log.Warning("SaveData file is not exist");
			result = false;
		}
		return result;
	}

	public static bool IsAutoSaveDataExists()
	{
		return File.Exists(SaveData.SaveDataFilePath);
	}

	public static bool Load()
	{
		bool result;
		try
		{
			FileStream fileStream = File.OpenRead(SaveData.SaveDataFilePath);
			byte[] array = new byte[fileStream.Length];
			fileStream.Read(array, 0, array.Length);
			if (SaveData.OnGameLoaded != null)
			{
				SaveData.OnGameLoaded(null, new SaveLoadedEventArgs(SaveData.GetObject(array)));
			}
			fileStream.Close();
			result = true;
		}
		catch (FileNotFoundException)
		{
			Log.Warning("SaveData file is not exist");
			result = false;
		}
		return result;
	}

	public static void Delete()
	{
		if (File.Exists(SaveData.SaveDataFilePath))
		{
			File.Delete(SaveData.SaveDataFilePath);
		}
	}

	private static byte[] GetBuffer(GameData obj)
	{
		MemoryStream memoryStream = new MemoryStream();
		BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
		binaryWriter.Write(JsonMapper.ToJson(obj));
		binaryWriter.Close();
		return memoryStream.GetBuffer();
	}

	private static GameData GetObject(byte[] buffer)
	{
		MemoryStream input = new MemoryStream(buffer);
		BinaryReader binaryReader = new BinaryReader(input);
		GameData result = JsonMapper.ToObject<GameData>(binaryReader.ReadString());
		binaryReader.Close();
		return result;
	}
}
