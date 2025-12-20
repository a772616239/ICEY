using System;
using I2.Loc;
using UnityEngine;

public class UIStartOptionController : MonoBehaviour
{
	private void Start()
	{
		this._languageLabel.text = UILanguage.CurrentLanguage.LocalizedName;
		this._audioLanguageLocalize.Term = UIAudioLanguage.CurrentAudioLanguage.LocalizeTerm;
	}

	private void Update()
	{
	}

	private void OnDisable()
	{
		R.Settings.Save();
	}

	private void OnLanguageClick(int step)
	{
		R.Settings.Language = UILanguage.Languages[(UILanguage.Languages.IndexOf(R.Settings.Language) + step + UILanguage.Languages.Count) % UILanguage.Languages.Count];
		R.Settings.AudioLanguage = UILanguage.CurrentLanguage.DefaultAudioLanguage;
		R.Settings.Save();
		LocalizationManager.CurrentLanguage = R.Settings.Language;
		this._languageLabel.text = UILanguage.CurrentLanguage.LocalizedName;
		this._audioLanguageLocalize.Term = UIAudioLanguage.CurrentAudioLanguage.LocalizeTerm;
	}

	private void OnAudioLanguageClick(int step)
	{
		R.Settings.AudioLanguage = UIAudioLanguage.AudioLanguages[(UIAudioLanguage.AudioLanguages.IndexOf(R.Settings.AudioLanguage) + step + UIAudioLanguage.AudioLanguages.Count) % UIAudioLanguage.AudioLanguages.Count];
		R.Settings.Save();
		this._audioLanguageLocalize.Term = UIAudioLanguage.CurrentAudioLanguage.LocalizeTerm;
	}

	public void OnLanguageClickPrevious()
	{
		this.OnLanguageClick(-1);
	}

	public void OnLanguageClickNext()
	{
		this.OnLanguageClick(1);
	}

	public void OnAudioLanguageClickPrevious()
	{
		this.OnAudioLanguageClick(-1);
	}

	public void OnAudioLanguageClickNext()
	{
		this.OnAudioLanguageClick(1);
	}

	public void OnResetConfirmClick()
	{
		R.SaveReset();
		R.Mode.EnterMode(Mode.AllMode.UI);
		R.Ui.Pause.Enabled = false;
		LevelManager.LoadLevelByGateId("ui_start", SceneGate.OpenType.None);
	}

	[SerializeField]
	private GameObject _languageButton;

	[SerializeField]
	private UILabel _languageLabel;

	[SerializeField]
	private GameObject _audioLanguageButton;

	[SerializeField]
	private Localize _audioLanguageLocalize;
}
