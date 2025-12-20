using System;

namespace I2.Loc
{
	[Serializable]
	public struct LocalizedString
	{
		public static implicit operator string(LocalizedString s)
		{
			return s.ToString();
		}

		public static implicit operator LocalizedString(string term)
		{
			return new LocalizedString
			{
				mTerm = term
			};
		}

		public override string ToString()
		{
			string termTranslation = LocalizationManager.GetTermTranslation(this.mTerm, !this.mRTL_IgnoreArabicFix, this.mRTL_MaxLineLength, !this.mRTL_ConvertNumbers);
			LocalizationManager.ApplyLocalizationParams(ref termTranslation, null);
			return termTranslation;
		}

		public string mTerm;

		public bool mRTL_IgnoreArabicFix;

		public int mRTL_MaxLineLength;

		public bool mRTL_ConvertNumbers;
	}
}
