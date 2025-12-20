using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DatabaseModel;

public static class DB
{
	public static IDictionary<int, AudioClipData> AudioClipData
	{
		get
		{
			return DB._audioClipData;
		}
	}

	public static IDictionary<int, CameraEffectProxyPrefabData> CameraEffectProxyPrefabData
	{
		get
		{
			return DB._cameraEffectProxyPrefabData;
		}
	}

	public static IList<EnemyAttrData> EnemyAttrData
	{
		get
		{
			return DB._enemyAttrData;
		}
	}

	public static IDictionary<string, Enhancement> Enhancements
	{
		get
		{
			return DB._enhancements;
		}
	}

	public static IDictionary<string, VoiceOver> VoiceOvers
	{
		get
		{
			return DB._voiceOvers;
		}
	}

	public static void Preload()
	{
		if (DB._isPreloaded)
		{
			return;
		}
		string fileName = "AudioClipData";
		
		Func<string, int> setKey = new Func<string, int>(int.Parse);
		
		DB._audioClipData = CSVHelper.Csv2Dictionary<int, AudioClipData>(fileName, setKey, new Func<string[], AudioClipData>(DatabaseModel.AudioClipData.SetValue));
		string fileName2 = "CameraEffectProxyPrefabData";
		
		Func<string, int> setKey2 = new Func<string, int>(int.Parse);
		
		DB._cameraEffectProxyPrefabData = CSVHelper.Csv2Dictionary<int, CameraEffectProxyPrefabData>(fileName2, setKey2, new Func<string[], CameraEffectProxyPrefabData>(DatabaseModel.CameraEffectProxyPrefabData.SetValue));
		string fileName3 = "EnemyAttrData";
		
		DB._enemyAttrData = CSVHelper.Csv2List<EnemyAttrData>(fileName3, new Func<string[], EnemyAttrData>(DatabaseModel.EnemyAttrData.SetValue));
		string fileName4 = "Enhancement";
		Func<string, string> setKey3 = (string a) => a;
		
		DB._enhancements = CSVHelper.Csv2Dictionary<string, Enhancement>(fileName4, setKey3, new Func<string[], Enhancement>(Enhancement.SetValue));
		string fileName5 = "VoiceOver";
		Func<string, string> setKey4 = (string a) => a;
		
		DB._voiceOvers = CSVHelper.Csv2Dictionary<string, VoiceOver>(fileName5, setKey4, new Func<string[], VoiceOver>(VoiceOver.SetValue));
		DB._isPreloaded = true;
	}

	private static IList<EnemyAttrData> _enemyAttrData;

	private static IDictionary<int, AudioClipData> _audioClipData;

	private static IDictionary<int, CameraEffectProxyPrefabData> _cameraEffectProxyPrefabData;

	private static IDictionary<string, Enhancement> _enhancements;

	private static IDictionary<string, VoiceOver> _voiceOvers;

	private static bool _isPreloaded;

	
}
