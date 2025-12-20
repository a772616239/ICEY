using System;
using System.Collections.Generic;

public static class UIAudioLanguage
{
	public static UIAudioLanguage.AudioLanguageInfo CurrentAudioLanguage
	{
		get
		{
			return UIAudioLanguage.AudioLanguageInfoDic[R.Settings.AudioLanguage];
		}
	}

	public static bool IsChinese
	{
		get
		{
			return UIAudioLanguage.Is(new string[]
			{
				"Chinese (Mandarin)"
			});
		}
	}

	public static bool IsEnglish
	{
		get
		{
			return UIAudioLanguage.Is(new string[]
			{
				"English"
			});
		}
	}

	public static bool IsJapanese
	{
		get
		{
			return UIAudioLanguage.Is(new string[]
			{
				"Japanese"
			});
		}
	}

	private static bool Is(params string[] languages)
	{
		string name = UIAudioLanguage.CurrentAudioLanguage.Name;
		for (int i = 0; i < languages.Length; i++)
		{
			if (name == languages[i])
			{
				return true;
			}
		}
		return false;
	}

	public const string English = "English";

	public const string MandarinChinese = "Chinese (Mandarin)";

	public const string Japanese = "Japanese";

	public static readonly List<string> AudioLanguages = new List<string>
	{
		"English",
		"Chinese (Mandarin)"
	};

	private static readonly Dictionary<string, UIAudioLanguage.AudioLanguageInfo> AudioLanguageInfoDic = new Dictionary<string, UIAudioLanguage.AudioLanguageInfo>
	{
		{
			"English",
			new UIAudioLanguage.AudioLanguageInfo("English", "ui/language/english")
		},
		{
			"Chinese (Mandarin)",
			new UIAudioLanguage.AudioLanguageInfo("Chinese (Mandarin)", "ui/language/mandarin_chinese")
		},
		{
			"Japanese",
			new UIAudioLanguage.AudioLanguageInfo("Japanese", "ui/language/japanese")
		}
	};

	public class AudioLanguageInfo
	{
		public AudioLanguageInfo(string name, string localizeTerm)
		{
			this.Name = name;
			this.LocalizeTerm = localizeTerm;
		}

		public readonly string LocalizeTerm;

		public readonly string Name;
	}
}
