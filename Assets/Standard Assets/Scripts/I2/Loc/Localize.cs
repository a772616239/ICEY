using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

namespace I2.Loc
{
	[AddComponentMenu("I2/Localization/Localize")]
	public class Localize : MonoBehaviour
	{
		public string Term
		{
			get
			{
				return this.mTerm;
			}
			set
			{
				this.SetTerm(value);
			}
		}

		public string SecondaryTerm
		{
			get
			{
				return this.mTermSecondary;
			}
			set
			{
				this.SetTerm(null, value);
			}
		}

		//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action EventFindTarget;

		private void Awake()
		{
			this.RegisterTargets();
			if (this.HasTargetCache())
			{
				this.EventFindTarget();
			}
			if (this.LocalizeOnAwake)
			{
				this.OnLocalize(false);
			}
		}

		private void RegisterTargets()
		{
			if (this.EventFindTarget != null)
			{
				return;
			}
			this.RegisterEvents_NGUI();
			Localize.RegisterEvents_DFGUI();
			this.RegisterEvents_UGUI();
			Localize.RegisterEvents_2DToolKit();
			Localize.RegisterEvents_TextMeshPro();
			this.RegisterEvents_UnityStandard();
			Localize.RegisterEvents_SVG();
		}

		private void OnEnable()
		{
			this.OnLocalize(false);
		}

		public void OnLocalize(bool Force = false)
		{
			if (!Force && (!base.enabled || base.gameObject == null || !base.gameObject.activeInHierarchy))
			{
				return;
			}
			if (string.IsNullOrEmpty(LocalizationManager.CurrentLanguage))
			{
				return;
			}
			if (!this.AlwaysForceLocalize && !Force && !this.LocalizeCallBack.HasCallback() && this.LastLocalizedLanguage == LocalizationManager.CurrentLanguage)
			{
				return;
			}
			this.LastLocalizedLanguage = LocalizationManager.CurrentLanguage;
			if (!this.HasTargetCache())
			{
				this.FindTarget();
			}
			if (!this.HasTargetCache())
			{
				return;
			}
			if (string.IsNullOrEmpty(this.FinalTerm) || string.IsNullOrEmpty(this.FinalSecondaryTerm))
			{
				this.GetFinalTerms(out this.FinalTerm, out this.FinalSecondaryTerm);
			}
			bool flag = Application.isPlaying && this.LocalizeCallBack.HasCallback();
			if (!flag && string.IsNullOrEmpty(this.FinalTerm) && string.IsNullOrEmpty(this.FinalSecondaryTerm))
			{
				return;
			}
			Localize.CallBackTerm = this.FinalTerm;
			Localize.CallBackSecondaryTerm = this.FinalSecondaryTerm;
			Localize.MainTranslation = LocalizationManager.GetTermTranslation(this.FinalTerm, false);
			Localize.SecondaryTranslation = LocalizationManager.GetTermTranslation(this.FinalSecondaryTerm, false);
			if (!flag && string.IsNullOrEmpty(Localize.MainTranslation) && string.IsNullOrEmpty(Localize.SecondaryTranslation))
			{
				return;
			}
			Localize.CurrentLocalizeComponent = this;
			if (Application.isPlaying)
			{
				this.LocalizeCallBack.Execute(this);
				LocalizationManager.ApplyLocalizationParams(ref Localize.MainTranslation, base.gameObject);
			}
			if (LocalizationManager.IsRight2Left && !this.IgnoreRTL)
			{
				if (this.AllowMainTermToBeRTL && !string.IsNullOrEmpty(Localize.MainTranslation))
				{
					Localize.MainTranslation = LocalizationManager.ApplyRTLfix(Localize.MainTranslation, this.MaxCharactersInRTL, this.IgnoreNumbersInRTL);
				}
				if (this.AllowSecondTermToBeRTL && !string.IsNullOrEmpty(Localize.SecondaryTranslation))
				{
					Localize.SecondaryTranslation = LocalizationManager.ApplyRTLfix(Localize.SecondaryTranslation);
				}
			}
			switch (this.PrimaryTermModifier)
			{
			case Localize.TermModification.ToUpper:
				Localize.MainTranslation = Localize.MainTranslation.ToUpper();
				break;
			case Localize.TermModification.ToLower:
				Localize.MainTranslation = Localize.MainTranslation.ToLower();
				break;
			case Localize.TermModification.ToUpperFirst:
				Localize.MainTranslation = GoogleTranslation.UppercaseFirst(Localize.MainTranslation);
				break;
			case Localize.TermModification.ToTitle:
				Localize.MainTranslation = GoogleTranslation.TitleCase(Localize.MainTranslation);
				break;
			}
			switch (this.SecondaryTermModifier)
			{
			case Localize.TermModification.ToUpper:
				Localize.SecondaryTranslation = Localize.SecondaryTranslation.ToUpper();
				break;
			case Localize.TermModification.ToLower:
				Localize.SecondaryTranslation = Localize.SecondaryTranslation.ToLower();
				break;
			case Localize.TermModification.ToUpperFirst:
				Localize.SecondaryTranslation = GoogleTranslation.UppercaseFirst(Localize.SecondaryTranslation);
				break;
			case Localize.TermModification.ToTitle:
				Localize.SecondaryTranslation = GoogleTranslation.TitleCase(Localize.SecondaryTranslation);
				break;
			}
			this.EventDoLocalize(Localize.MainTranslation, Localize.SecondaryTranslation);
			Localize.CurrentLocalizeComponent = null;
		}

		public bool FindTarget()
		{
			if (this.HasTargetCache())
			{
				return true;
			}
			if (this.EventFindTarget == null)
			{
				this.RegisterTargets();
			}
			this.EventFindTarget();
			return this.HasTargetCache();
		}

		public void FindAndCacheTarget<T>(ref T targetCache, Localize.DelegateSetFinalTerms setFinalTerms, Localize.DelegateDoLocalize doLocalize, bool UseSecondaryTerm, bool MainRTL, bool SecondRTL) where T : Component
		{
			if (this.mTarget != null)
			{
				targetCache = (this.mTarget as T);
			}
			else
			{
				this.mTarget = (targetCache = base.GetComponent<T>());
			}
			if (targetCache != null)
			{
				this.EventSetFinalTerms = setFinalTerms;
				this.EventDoLocalize = doLocalize;
				this.CanUseSecondaryTerm = UseSecondaryTerm;
				this.AllowMainTermToBeRTL = MainRTL;
				this.AllowSecondTermToBeRTL = SecondRTL;
			}
		}

		private void FindAndCacheTarget(ref GameObject targetCache, Localize.DelegateSetFinalTerms setFinalTerms, Localize.DelegateDoLocalize doLocalize, bool UseSecondaryTerm, bool MainRTL, bool SecondRTL)
		{
			if (this.mTarget != targetCache && targetCache)
			{
				UnityEngine.Object.Destroy(targetCache);
			}
			if (this.mTarget != null)
			{
				targetCache = (this.mTarget as GameObject);
			}
			else
			{
				Transform transform = base.transform;
				GameObject gameObject;
				targetCache = (gameObject = ((transform.childCount >= 1) ? transform.GetChild(0).gameObject : null));
				this.mTarget = gameObject;
			}
			if (targetCache != null)
			{
				this.EventSetFinalTerms = setFinalTerms;
				this.EventDoLocalize = doLocalize;
				this.CanUseSecondaryTerm = UseSecondaryTerm;
				this.AllowMainTermToBeRTL = MainRTL;
				this.AllowSecondTermToBeRTL = SecondRTL;
			}
		}

		private bool HasTargetCache()
		{
			return this.EventDoLocalize != null;
		}

		public void GetFinalTerms(out string PrimaryTerm, out string SecondaryTerm)
		{
			if (this.EventSetFinalTerms == null || (!this.mTarget && !this.HasTargetCache()))
			{
				this.FindTarget();
			}
			PrimaryTerm = string.Empty;
			SecondaryTerm = string.Empty;
			if (this.mTarget != null && (string.IsNullOrEmpty(this.mTerm) || string.IsNullOrEmpty(this.mTermSecondary)) && this.EventSetFinalTerms != null)
			{
				this.EventSetFinalTerms(this.mTerm, this.mTermSecondary, out PrimaryTerm, out SecondaryTerm);
			}
			if (!string.IsNullOrEmpty(this.mTerm))
			{
				PrimaryTerm = this.mTerm;
			}
			if (!string.IsNullOrEmpty(this.mTermSecondary))
			{
				SecondaryTerm = this.mTermSecondary;
			}
			if (PrimaryTerm != null)
			{
				PrimaryTerm = PrimaryTerm.Trim();
			}
			if (SecondaryTerm != null)
			{
				SecondaryTerm = SecondaryTerm.Trim();
			}
		}

		public string GetMainTargetsText()
		{
			string text = null;
			string text2 = null;
			if (this.EventSetFinalTerms != null)
			{
				this.EventSetFinalTerms(null, null, out text, out text2);
			}
			return (!string.IsNullOrEmpty(text)) ? text : this.mTerm;
		}

		private void SetFinalTerms(string Main, string Secondary, out string PrimaryTerm, out string SecondaryTerm, bool RemoveNonASCII)
		{
			PrimaryTerm = ((!RemoveNonASCII || string.IsNullOrEmpty(Main)) ? Main : Regex.Replace(Main, "[^a-zA-Z0-9_ ]+", " "));
			SecondaryTerm = Secondary;
		}

		public void SetTerm(string primary)
		{
			if (!string.IsNullOrEmpty(primary))
			{
				this.mTerm = primary;
				this.FinalTerm = primary;
			}
			this.OnLocalize(true);
		}

		public void SetTerm(string primary, string secondary)
		{
			if (!string.IsNullOrEmpty(primary))
			{
				this.mTerm = primary;
				this.FinalTerm = primary;
			}
			this.mTermSecondary = secondary;
			this.FinalSecondaryTerm = secondary;
			this.OnLocalize(true);
		}

		private T GetSecondaryTranslatedObj<T>(ref string MainTranslation, ref string SecondaryTranslation) where T : UnityEngine.Object
		{
			string text;
			string text2;
			this.DeserializeTranslation(MainTranslation, out text, out text2);
			T t = (T)((object)null);
			if (!string.IsNullOrEmpty(text2))
			{
				t = this.GetObject<T>(text2);
				if (t != null)
				{
					MainTranslation = text;
					SecondaryTranslation = text2;
				}
			}
			if (t == null)
			{
				t = this.GetObject<T>(SecondaryTranslation);
			}
			return t;
		}

		private T GetObject<T>(string Translation) where T : UnityEngine.Object
		{
			if (string.IsNullOrEmpty(Translation))
			{
				return (T)((object)null);
			}
			T translatedObject = this.GetTranslatedObject<T>(Translation);
			if (translatedObject == null)
			{
				int num = Translation.LastIndexOfAny("/\\".ToCharArray());
				if (num >= 0)
				{
					Translation = Translation.Substring(num + 1);
					translatedObject = this.GetTranslatedObject<T>(Translation);
				}
			}
			return translatedObject;
		}

		private T GetTranslatedObject<T>(string Translation) where T : UnityEngine.Object
		{
			return this.FindTranslatedObject<T>(Translation);
		}

		private void DeserializeTranslation(string translation, out string value, out string secondary)
		{
			if (!string.IsNullOrEmpty(translation) && translation.Length > 1 && translation[0] == '[')
			{
				int num = translation.IndexOf(']');
				if (num > 0)
				{
					secondary = translation.Substring(1, num - 1);
					value = translation.Substring(num + 1);
					return;
				}
			}
			value = translation;
			secondary = string.Empty;
		}

		public T FindTranslatedObject<T>(string value) where T : UnityEngine.Object
		{
			if (string.IsNullOrEmpty(value))
			{
				return (T)((object)null);
			}
			if (this.TranslatedObjects != null)
			{
				int i = 0;
				int num = this.TranslatedObjects.Length;
				while (i < num)
				{
					if (this.TranslatedObjects[i] as T != null && value == this.TranslatedObjects[i].name)
					{
						return this.TranslatedObjects[i] as T;
					}
					i++;
				}
			}
			T t = LocalizationManager.FindAsset(value) as T;
			if (t)
			{
				return t;
			}
			return ResourceManager.pInstance.GetAsset<T>(value);
		}

		public bool HasTranslatedObject(UnityEngine.Object Obj)
		{
			return Array.IndexOf<UnityEngine.Object>(this.TranslatedObjects, Obj) >= 0 || ResourceManager.pInstance.HasAsset(Obj);
		}

		public void AddTranslatedObject(UnityEngine.Object Obj)
		{
			Array.Resize<UnityEngine.Object>(ref this.TranslatedObjects, this.TranslatedObjects.Length + 1);
			this.TranslatedObjects[this.TranslatedObjects.Length - 1] = Obj;
		}

		public void SetGlobalLanguage(string Language)
		{
			LocalizationManager.CurrentLanguage = Language;
		}

		public static void RegisterEvents_2DToolKit()
		{
		}

		public static void RegisterEvents_DFGUI()
		{
		}

		public void RegisterEvents_NGUI()
		{
			this.EventFindTarget += this.FindTarget_UILabel;
			this.EventFindTarget += this.FindTarget_UISprite;
			this.EventFindTarget += this.FindTarget_UITexture;
		}

		private void FindTarget_UILabel()
		{
			this.FindAndCacheTarget<UILabel>(ref this.mTarget_UILabel, new Localize.DelegateSetFinalTerms(this.SetFinalTerms_UIlabel), new Localize.DelegateDoLocalize(this.DoLocalize_UILabel), true, true, false);
		}

		private void FindTarget_UISprite()
		{
			this.FindAndCacheTarget<UISprite>(ref this.mTarget_UISprite, new Localize.DelegateSetFinalTerms(this.SetFinalTerms_UISprite), new Localize.DelegateDoLocalize(this.DoLocalize_UISprite), true, false, false);
		}

		private void FindTarget_UITexture()
		{
			this.FindAndCacheTarget<UITexture>(ref this.mTarget_UITexture, new Localize.DelegateSetFinalTerms(this.SetFinalTerms_UITexture), new Localize.DelegateDoLocalize(this.DoLocalize_UITexture), false, false, false);
		}

		private void SetFinalTerms_UIlabel(string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			string secondary = (!(this.mTarget_UILabel.ambigiousFont != null)) ? string.Empty : this.mTarget_UILabel.ambigiousFont.name;
			this.SetFinalTerms(this.mTarget_UILabel.text, secondary, out primaryTerm, out secondaryTerm, true);
		}

		public void SetFinalTerms_UISprite(string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			string secondary = (!(this.mTarget_UISprite.atlas != null)) ? string.Empty : this.mTarget_UISprite.atlas.name;
			this.SetFinalTerms(this.mTarget_UISprite.spriteName, secondary, out primaryTerm, out secondaryTerm, false);
		}

		public void SetFinalTerms_UITexture(string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			this.SetFinalTerms(this.mTarget_UITexture.mainTexture.name, null, out primaryTerm, out secondaryTerm, false);
		}

		public void DoLocalize_UILabel(string MainTranslation, string SecondaryTranslation)
		{
			Font secondaryTranslatedObj = this.GetSecondaryTranslatedObj<Font>(ref MainTranslation, ref SecondaryTranslation);
			if (secondaryTranslatedObj != null)
			{
				if (secondaryTranslatedObj != this.mTarget_UILabel.ambigiousFont)
				{
					this.mTarget_UILabel.ambigiousFont = secondaryTranslatedObj;
				}
			}
			else
			{
				UIFont secondaryTranslatedObj2 = this.GetSecondaryTranslatedObj<UIFont>(ref MainTranslation, ref SecondaryTranslation);
				if (secondaryTranslatedObj2 != null && this.mTarget_UILabel.ambigiousFont != secondaryTranslatedObj2)
				{
					this.mTarget_UILabel.ambigiousFont = secondaryTranslatedObj2;
				}
			}
			if (this.mInitializeAlignment)
			{
				this.mInitializeAlignment = false;
				this.mAlignmentNGUI_LTR = (this.mAlignmentNGUI_RTL = this.mTarget_UILabel.alignment);
				if (LocalizationManager.IsRight2Left && this.mAlignmentNGUI_RTL == NGUIText.Alignment.Right)
				{
					this.mAlignmentNGUI_LTR = NGUIText.Alignment.Left;
				}
				if (!LocalizationManager.IsRight2Left && this.mAlignmentNGUI_LTR == NGUIText.Alignment.Left)
				{
					this.mAlignmentNGUI_RTL = NGUIText.Alignment.Right;
				}
			}
			UIInput uiinput = NGUITools.FindInParents<UIInput>(this.mTarget_UILabel.gameObject);
			if (uiinput != null && uiinput.label == this.mTarget_UILabel)
			{
				if (!string.IsNullOrEmpty(MainTranslation) && uiinput.defaultText != MainTranslation)
				{
					if (Localize.CurrentLocalizeComponent.CorrectAlignmentForRTL && (uiinput.label.alignment == NGUIText.Alignment.Left || uiinput.label.alignment == NGUIText.Alignment.Right))
					{
						uiinput.label.alignment = ((!LocalizationManager.IsRight2Left) ? this.mAlignmentNGUI_LTR : this.mAlignmentNGUI_RTL);
					}
					uiinput.defaultText = MainTranslation;
				}
			}
			else if (!string.IsNullOrEmpty(MainTranslation) && this.mTarget_UILabel.text != MainTranslation)
			{
				if (Localize.CurrentLocalizeComponent.CorrectAlignmentForRTL && (this.mTarget_UILabel.alignment == NGUIText.Alignment.Left || this.mTarget_UILabel.alignment == NGUIText.Alignment.Right))
				{
					this.mTarget_UILabel.alignment = ((!LocalizationManager.IsRight2Left) ? this.mAlignmentNGUI_LTR : this.mAlignmentNGUI_RTL);
				}
				this.mTarget_UILabel.text = MainTranslation;
			}
		}

		public void DoLocalize_UISprite(string MainTranslation, string SecondaryTranslation)
		{
			if (this.mTarget_UISprite.spriteName == MainTranslation)
			{
				return;
			}
			UIAtlas secondaryTranslatedObj = this.GetSecondaryTranslatedObj<UIAtlas>(ref MainTranslation, ref SecondaryTranslation);
			bool flag = false;
			if (secondaryTranslatedObj != null && this.mTarget_UISprite.atlas != secondaryTranslatedObj)
			{
				this.mTarget_UISprite.atlas = secondaryTranslatedObj;
				flag = true;
			}
			if (this.mTarget_UISprite.spriteName != MainTranslation && this.mTarget_UISprite.atlas.GetSprite(MainTranslation) != null)
			{
				this.mTarget_UISprite.spriteName = MainTranslation;
				flag = true;
			}
			if (flag)
			{
				this.mTarget_UISprite.MakePixelPerfect();
			}
		}

		public void DoLocalize_UITexture(string MainTranslation, string SecondaryTranslation)
		{
			Texture mainTexture = this.mTarget_UITexture.mainTexture;
			if (mainTexture != null && mainTexture.name != MainTranslation)
			{
				this.mTarget_UITexture.mainTexture = this.FindTranslatedObject<Texture>(MainTranslation);
			}
		}

		public static void RegisterEvents_SVG()
		{
		}

		public static void RegisterEvents_TextMeshPro()
		{
		}

		public void RegisterEvents_UGUI()
		{
			this.EventFindTarget += this.FindTarget_uGUI_Text;
			this.EventFindTarget += this.FindTarget_uGUI_Image;
			this.EventFindTarget += this.FindTarget_uGUI_RawImage;
		}

		private void FindTarget_uGUI_Text()
		{
			this.FindAndCacheTarget<Text>(ref this.mTarget_uGUI_Text, new Localize.DelegateSetFinalTerms(this.SetFinalTerms_uGUI_Text), new Localize.DelegateDoLocalize(this.DoLocalize_uGUI_Text), true, true, false);
		}

		private void FindTarget_uGUI_Image()
		{
			this.FindAndCacheTarget<Image>(ref this.mTarget_uGUI_Image, new Localize.DelegateSetFinalTerms(this.SetFinalTerms_uGUI_Image), new Localize.DelegateDoLocalize(this.DoLocalize_uGUI_Image), false, false, false);
		}

		private void FindTarget_uGUI_RawImage()
		{
			this.FindAndCacheTarget<RawImage>(ref this.mTarget_uGUI_RawImage, new Localize.DelegateSetFinalTerms(this.SetFinalTerms_uGUI_RawImage), new Localize.DelegateDoLocalize(this.DoLocalize_uGUI_RawImage), false, false, false);
		}

		private void SetFinalTerms_uGUI_Text(string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			string secondary = (!(this.mTarget_uGUI_Text.font != null)) ? string.Empty : this.mTarget_uGUI_Text.font.name;
			this.SetFinalTerms(this.mTarget_uGUI_Text.text, secondary, out primaryTerm, out secondaryTerm, true);
		}

		public void SetFinalTerms_uGUI_Image(string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			this.SetFinalTerms(this.mTarget_uGUI_Image.mainTexture.name, null, out primaryTerm, out secondaryTerm, false);
		}

		public void SetFinalTerms_uGUI_RawImage(string Main, string Secondary, out string primaryTerm, out string secondaryTerm)
		{
			this.SetFinalTerms(this.mTarget_uGUI_RawImage.texture.name, null, out primaryTerm, out secondaryTerm, false);
		}

		public static T FindInParents<T>(Transform tr) where T : Component
		{
			if (!tr)
			{
				return (T)((object)null);
			}
			T component = tr.GetComponent<T>();
			while (!component && tr)
			{
				component = tr.GetComponent<T>();
				tr = tr.parent;
			}
			return component;
		}

		public void DoLocalize_uGUI_Text(string MainTranslation, string SecondaryTranslation)
		{
			Font secondaryTranslatedObj = this.GetSecondaryTranslatedObj<Font>(ref MainTranslation, ref SecondaryTranslation);
			if (secondaryTranslatedObj != null && secondaryTranslatedObj != this.mTarget_uGUI_Text.font)
			{
				this.mTarget_uGUI_Text.font = secondaryTranslatedObj;
			}
			if (this.mInitializeAlignment)
			{
				this.mInitializeAlignment = false;
				this.InitAlignment_UGUI(this.mTarget_uGUI_Text.alignment, out this.mAlignmentUGUI_LTR, out this.mAlignmentUGUI_RTL);
			}
			if (!string.IsNullOrEmpty(MainTranslation) && this.mTarget_uGUI_Text.text != MainTranslation)
			{
				if (Localize.CurrentLocalizeComponent.CorrectAlignmentForRTL)
				{
					this.mTarget_uGUI_Text.alignment = ((!LocalizationManager.IsRight2Left) ? this.mAlignmentUGUI_LTR : this.mAlignmentUGUI_RTL);
				}
				this.mTarget_uGUI_Text.text = MainTranslation;
				this.mTarget_uGUI_Text.SetVerticesDirty();
			}
		}

		private void InitAlignment_UGUI(TextAnchor alignment, out TextAnchor alignLTR, out TextAnchor alignRTL)
		{
			alignRTL = alignment;
			alignLTR = alignment;
			if (LocalizationManager.IsRight2Left)
			{
				if (alignment != TextAnchor.UpperRight)
				{
					if (alignment != TextAnchor.MiddleRight)
					{
						if (alignment == TextAnchor.LowerRight)
						{
							alignLTR = TextAnchor.LowerLeft;
						}
					}
					else
					{
						alignLTR = TextAnchor.MiddleLeft;
					}
				}
				else
				{
					alignLTR = TextAnchor.UpperLeft;
				}
			}
			else if (alignment != TextAnchor.UpperLeft)
			{
				if (alignment != TextAnchor.MiddleLeft)
				{
					if (alignment == TextAnchor.LowerLeft)
					{
						alignRTL = TextAnchor.LowerRight;
					}
				}
				else
				{
					alignRTL = TextAnchor.MiddleRight;
				}
			}
			else
			{
				alignRTL = TextAnchor.UpperRight;
			}
		}

		public void DoLocalize_uGUI_Image(string MainTranslation, string SecondaryTranslation)
		{
			Sprite sprite = this.mTarget_uGUI_Image.sprite;
			if (sprite == null || sprite.name != MainTranslation)
			{
				this.mTarget_uGUI_Image.sprite = this.FindTranslatedObject<Sprite>(MainTranslation);
			}
		}

		public void DoLocalize_uGUI_RawImage(string MainTranslation, string SecondaryTranslation)
		{
			Texture texture = this.mTarget_uGUI_RawImage.texture;
			if (texture == null || texture.name != MainTranslation)
			{
				this.mTarget_uGUI_RawImage.texture = this.FindTranslatedObject<Texture>(MainTranslation);
			}
		}

		public void RegisterEvents_UnityStandard()
		{
			this.EventFindTarget += this.FindTarget_GUIText;
			this.EventFindTarget += this.FindTarget_TextMesh;
			this.EventFindTarget += this.FindTarget_AudioSource;
			this.EventFindTarget += this.FindTarget_GUITexture;
			this.EventFindTarget += this.FindTarget_Child;
			this.EventFindTarget += this.FindTarget_SpriteRenderer;
		}

		private void FindTarget_GUIText()
		{
			this.FindAndCacheTarget<GUIText>(ref this.mTarget_GUIText, new Localize.DelegateSetFinalTerms(this.SetFinalTerms_GUIText), new Localize.DelegateDoLocalize(this.DoLocalize_GUIText), true, true, false);
		}

		private void FindTarget_TextMesh()
		{
			this.FindAndCacheTarget<TextMesh>(ref this.mTarget_TextMesh, new Localize.DelegateSetFinalTerms(this.SetFinalTerms_TextMesh), new Localize.DelegateDoLocalize(this.DoLocalize_TextMesh), true, true, false);
		}

		private void FindTarget_AudioSource()
		{
			this.FindAndCacheTarget<AudioSource>(ref this.mTarget_AudioSource, new Localize.DelegateSetFinalTerms(this.SetFinalTerms_AudioSource), new Localize.DelegateDoLocalize(this.DoLocalize_AudioSource), false, false, false);
		}

		private void FindTarget_GUITexture()
		{
			this.FindAndCacheTarget<GUITexture>(ref this.mTarget_GUITexture, new Localize.DelegateSetFinalTerms(this.SetFinalTerms_GUITexture), new Localize.DelegateDoLocalize(this.DoLocalize_GUITexture), false, false, false);
		}

		private void FindTarget_Child()
		{
			this.FindAndCacheTarget(ref this.mTarget_Child, new Localize.DelegateSetFinalTerms(this.SetFinalTerms_Child), new Localize.DelegateDoLocalize(this.DoLocalize_Child), false, false, false);
		}

		private void FindTarget_SpriteRenderer()
		{
			this.FindAndCacheTarget<SpriteRenderer>(ref this.mTarget_SpriteRenderer, new Localize.DelegateSetFinalTerms(this.SetFinalTerms_SpriteRenderer), new Localize.DelegateDoLocalize(this.DoLocalize_SpriteRenderer), false, false, false);
		}

		public void SetFinalTerms_GUIText(string Main, string Secondary, out string PrimaryTerm, out string SecondaryTerm)
		{
			if (string.IsNullOrEmpty(Secondary) && this.mTarget_GUIText.font != null)
			{
				Secondary = this.mTarget_GUIText.font.name;
			}
			this.SetFinalTerms(this.mTarget_GUIText.text, Secondary, out PrimaryTerm, out SecondaryTerm, true);
		}

		public void SetFinalTerms_TextMesh(string Main, string Secondary, out string PrimaryTerm, out string SecondaryTerm)
		{
			string secondary = (!(this.mTarget_TextMesh.font != null)) ? string.Empty : this.mTarget_TextMesh.font.name;
			this.SetFinalTerms(this.mTarget_TextMesh.text, secondary, out PrimaryTerm, out SecondaryTerm, true);
		}

		public void SetFinalTerms_GUITexture(string Main, string Secondary, out string PrimaryTerm, out string SecondaryTerm)
		{
			if (!this.mTarget_GUITexture || !this.mTarget_GUITexture.texture)
			{
				this.SetFinalTerms(string.Empty, string.Empty, out PrimaryTerm, out SecondaryTerm, false);
			}
			else
			{
				this.SetFinalTerms(this.mTarget_GUITexture.texture.name, string.Empty, out PrimaryTerm, out SecondaryTerm, false);
			}
		}

		public void SetFinalTerms_AudioSource(string Main, string Secondary, out string PrimaryTerm, out string SecondaryTerm)
		{
			if (!this.mTarget_AudioSource || !this.mTarget_AudioSource.clip)
			{
				this.SetFinalTerms(string.Empty, string.Empty, out PrimaryTerm, out SecondaryTerm, false);
			}
			else
			{
				this.SetFinalTerms(this.mTarget_AudioSource.clip.name, string.Empty, out PrimaryTerm, out SecondaryTerm, false);
			}
		}

		public void SetFinalTerms_Child(string Main, string Secondary, out string PrimaryTerm, out string SecondaryTerm)
		{
			this.SetFinalTerms(this.mTarget_Child.name, string.Empty, out PrimaryTerm, out SecondaryTerm, false);
		}

		public void SetFinalTerms_SpriteRenderer(string Main, string Secondary, out string PrimaryTerm, out string SecondaryTerm)
		{
			this.SetFinalTerms((!(this.mTarget_SpriteRenderer.sprite != null)) ? string.Empty : this.mTarget_SpriteRenderer.sprite.name, string.Empty, out PrimaryTerm, out SecondaryTerm, false);
		}

		private void DoLocalize_GUIText(string MainTranslation, string SecondaryTranslation)
		{
			Font secondaryTranslatedObj = this.GetSecondaryTranslatedObj<Font>(ref MainTranslation, ref SecondaryTranslation);
			if (secondaryTranslatedObj != null && this.mTarget_GUIText.font != secondaryTranslatedObj)
			{
				this.mTarget_GUIText.font = secondaryTranslatedObj;
			}
			if (this.mInitializeAlignment)
			{
				this.mInitializeAlignment = false;
				this.mAlignmentStd_LTR = (this.mAlignmentStd_RTL = this.mTarget_GUIText.alignment);
				if (LocalizationManager.IsRight2Left && this.mAlignmentStd_RTL == TextAlignment.Right)
				{
					this.mAlignmentStd_LTR = TextAlignment.Left;
				}
				if (!LocalizationManager.IsRight2Left && this.mAlignmentStd_LTR == TextAlignment.Left)
				{
					this.mAlignmentStd_RTL = TextAlignment.Right;
				}
			}
			if (!string.IsNullOrEmpty(MainTranslation) && this.mTarget_GUIText.text != MainTranslation)
			{
				if (Localize.CurrentLocalizeComponent.CorrectAlignmentForRTL && this.mTarget_GUIText.alignment != TextAlignment.Center)
				{
					this.mTarget_GUIText.alignment = ((!LocalizationManager.IsRight2Left) ? this.mAlignmentStd_LTR : this.mAlignmentStd_RTL);
				}
				this.mTarget_GUIText.text = MainTranslation;
			}
		}

		private void DoLocalize_TextMesh(string MainTranslation, string SecondaryTranslation)
		{
			Font secondaryTranslatedObj = this.GetSecondaryTranslatedObj<Font>(ref MainTranslation, ref SecondaryTranslation);
			if (secondaryTranslatedObj != null && this.mTarget_TextMesh.font != secondaryTranslatedObj)
			{
				this.mTarget_TextMesh.font = secondaryTranslatedObj;
				base.GetComponent<Renderer>().sharedMaterial = secondaryTranslatedObj.material;
			}
			if (this.mInitializeAlignment)
			{
				this.mInitializeAlignment = false;
				this.mAlignmentStd_LTR = (this.mAlignmentStd_RTL = this.mTarget_TextMesh.alignment);
				if (LocalizationManager.IsRight2Left && this.mAlignmentStd_RTL == TextAlignment.Right)
				{
					this.mAlignmentStd_LTR = TextAlignment.Left;
				}
				if (!LocalizationManager.IsRight2Left && this.mAlignmentStd_LTR == TextAlignment.Left)
				{
					this.mAlignmentStd_RTL = TextAlignment.Right;
				}
			}
			if (!string.IsNullOrEmpty(MainTranslation) && this.mTarget_TextMesh.text != MainTranslation)
			{
				if (Localize.CurrentLocalizeComponent.CorrectAlignmentForRTL && this.mTarget_TextMesh.alignment != TextAlignment.Center)
				{
					this.mTarget_TextMesh.alignment = ((!LocalizationManager.IsRight2Left) ? this.mAlignmentStd_LTR : this.mAlignmentStd_RTL);
				}
				this.mTarget_TextMesh.text = MainTranslation;
			}
		}

		private void DoLocalize_AudioSource(string MainTranslation, string SecondaryTranslation)
		{
			bool isPlaying = this.mTarget_AudioSource.isPlaying;
			AudioClip clip = this.mTarget_AudioSource.clip;
			AudioClip audioClip = this.FindTranslatedObject<AudioClip>(MainTranslation);
			if (clip != audioClip)
			{
				this.mTarget_AudioSource.clip = audioClip;
			}
			if (isPlaying && this.mTarget_AudioSource.clip)
			{
				this.mTarget_AudioSource.Play();
			}
		}

		private void DoLocalize_GUITexture(string MainTranslation, string SecondaryTranslation)
		{
			Texture texture = this.mTarget_GUITexture.texture;
			if (texture != null && texture.name != MainTranslation)
			{
				this.mTarget_GUITexture.texture = this.FindTranslatedObject<Texture>(MainTranslation);
			}
		}

		private void DoLocalize_Child(string MainTranslation, string SecondaryTranslation)
		{
			if (this.mTarget_Child && this.mTarget_Child.name == MainTranslation)
			{
				return;
			}
			GameObject gameObject = this.mTarget_Child;
			GameObject gameObject2 = this.FindTranslatedObject<GameObject>(MainTranslation);
			if (gameObject2)
			{
				this.mTarget_Child = UnityEngine.Object.Instantiate<GameObject>(gameObject2);
				Transform transform = this.mTarget_Child.transform;
				Transform transform2 = (!gameObject) ? gameObject2.transform : gameObject.transform;
				transform.SetParent(base.transform);
				transform.localScale = transform2.localScale;
				transform.localRotation = transform2.localRotation;
				transform.localPosition = transform2.localPosition;
			}
			if (gameObject)
			{
				UnityEngine.Object.Destroy(gameObject);
			}
		}

		private void DoLocalize_SpriteRenderer(string MainTranslation, string SecondaryTranslation)
		{
			Sprite sprite = this.mTarget_SpriteRenderer.sprite;
			if (sprite == null || sprite.name != MainTranslation)
			{
				this.mTarget_SpriteRenderer.sprite = this.FindTranslatedObject<Sprite>(MainTranslation);
			}
		}

		public string mTerm = string.Empty;

		public string mTermSecondary = string.Empty;

		[NonSerialized]
		public string FinalTerm;

		[NonSerialized]
		public string FinalSecondaryTerm;

		public Localize.TermModification PrimaryTermModifier;

		public Localize.TermModification SecondaryTermModifier;

		public bool LocalizeOnAwake = true;

		private string LastLocalizedLanguage;

		public UnityEngine.Object mTarget;

		public Localize.DelegateSetFinalTerms EventSetFinalTerms;

		public Localize.DelegateDoLocalize EventDoLocalize;

		public bool CanUseSecondaryTerm;

		public bool AllowMainTermToBeRTL;

		public bool AllowSecondTermToBeRTL;

		public bool IgnoreRTL;

		public int MaxCharactersInRTL;

		public bool IgnoreNumbersInRTL;

		public bool CorrectAlignmentForRTL = true;

		public UnityEngine.Object[] TranslatedObjects;

		public EventCallback LocalizeCallBack = new EventCallback();

		public static string MainTranslation;

		public static string SecondaryTranslation;

		public static string CallBackTerm;

		public static string CallBackSecondaryTerm;

		public static Localize CurrentLocalizeComponent;

		public bool AlwaysForceLocalize;

		public bool mGUI_ShowReferences;

		public bool mGUI_ShowTems = true;

		public bool mGUI_ShowCallback;

		private UILabel mTarget_UILabel;

		private UISprite mTarget_UISprite;

		private UITexture mTarget_UITexture;

		private NGUIText.Alignment mAlignmentNGUI_LTR = NGUIText.Alignment.Left;

		private NGUIText.Alignment mAlignmentNGUI_RTL = NGUIText.Alignment.Right;

		private Text mTarget_uGUI_Text;

		private Image mTarget_uGUI_Image;

		private RawImage mTarget_uGUI_RawImage;

		private TextAnchor mAlignmentUGUI_RTL = TextAnchor.UpperRight;

		private TextAnchor mAlignmentUGUI_LTR;

		private GUIText mTarget_GUIText;

		private TextMesh mTarget_TextMesh;

		private AudioSource mTarget_AudioSource;

		private GUITexture mTarget_GUITexture;

		private GameObject mTarget_Child;

		private SpriteRenderer mTarget_SpriteRenderer;

		private bool mInitializeAlignment = true;

		private TextAlignment mAlignmentStd_LTR;

		private TextAlignment mAlignmentStd_RTL = TextAlignment.Right;

		public enum TermModification
		{
			DontModify,
			ToUpper,
			ToLower,
			ToUpperFirst,
			ToTitle
		}

		public delegate void DelegateSetFinalTerms(string Main, string Secondary, out string primaryTerm, out string secondaryTerm);

		public delegate void DelegateDoLocalize(string primaryTerm, string secondaryTerm);
	}
}
