using System;
using System.Collections.Generic;
using I2.Loc;

public static class UILanguage
{
	public static List<string> Languages
	{
		get
		{
			List<string> result;
			if ((result = UILanguage._languages) == null)
			{
				result = (UILanguage._languages = LocalizationManager.GetAllLanguages(true));
			}
			return result;
		}
	}

	public static UILanguage.LanguageInfo CurrentLanguage
	{
		get
		{
			return UILanguage.LanguageInfoDic[R.Settings.Language];
		}
	}

	public static bool IsChinese
	{
		get
		{
			return UILanguage.Is(new string[]
			{
				"Chinese (Simplified)",
				"Chinese (Traditional)"
			});
		}
	}

	public static bool IsTraditionalChinese
	{
		get
		{
			return UILanguage.Is(new string[]
			{
				"Chinese (Traditional)"
			});
		}
	}

	public static bool IsSimplifiedChinese
	{
		get
		{
			return UILanguage.Is(new string[]
			{
				"Chinese (Simplified)"
			});
		}
	}

	public static bool IsEnglish
	{
		get
		{
			return UILanguage.Is(new string[]
			{
				"English (United States)"
			});
		}
	}

	public static bool IsJapanese
	{
		get
		{
			return UILanguage.Is(new string[]
			{
				"Japanese"
			});
		}
	}

	private static bool Is(params string[] languages)
	{
		string currentLanguage = LocalizationManager.CurrentLanguage;
		for (int i = 0; i < languages.Length; i++)
		{
			if (currentLanguage == languages[i])
			{
				return true;
			}
		}
		return false;
	}

	public const string Default = "English (United States)";

	public const string English = "English (United States)";

	public const string SimplifiedChinese = "Chinese (Simplified)";

	public const string TraditionalChinese = "Chinese (Traditional)";

	public const string Japanese = "Japanese";

	public const string French = "French";

	public const string Spanish = "Spanish";

	private static readonly Dictionary<string, UILanguage.LanguageInfo> LanguageInfoDic = new Dictionary<string, UILanguage.LanguageInfo>
	{
		{
			"English (United States)",
			new UILanguage.LanguageInfo("English", 2f, "English")
		},
		{
			"Chinese (Simplified)",
			new UILanguage.LanguageInfo("中文（简体）", 1f, "Chinese (Mandarin)")
		},
		{
			"Chinese (Traditional)",
			new UILanguage.LanguageInfo("中文（繁體）", 1f, "Chinese (Mandarin)")
		},
		{
			"Japanese",
			new UILanguage.LanguageInfo("日本語", 2f, "Japanese")
		},
		{
			"French",
			new UILanguage.LanguageInfo("français", 2f, "English")
		},
		{
			"Spanish",
			new UILanguage.LanguageInfo("Español", 2f, "English")
		}
	};

	private static List<string> _languages;

	public class LanguageInfo
	{
		public LanguageInfo(string localizedName, float typeSpeed, string defaultAudioLanguage)
		{
			this.LocalizedName = localizedName;
			this.TypeSpeed = typeSpeed;
			this.DefaultAudioLanguage = defaultAudioLanguage;
		}

		public readonly string DefaultAudioLanguage;

		public readonly string LocalizedName;

		public readonly float TypeSpeed;
	}
}
