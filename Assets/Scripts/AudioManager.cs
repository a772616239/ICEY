using System;
using System.Collections;
using System.Collections.Generic;
using DatabaseModel;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : SingletonMono<AudioManager>
{
	public float EffectsVolume
	{
		get
		{
			return R.Settings.EffectsVolume;
		}
		set
		{
			R.Settings.EffectsVolume = value;
		}
	}

	public float BGMVolume
	{
		get
		{
			return R.Settings.BGMVolume;
		}
		set
		{
			R.Settings.BGMVolume = value;
		}
	}

	public bool IsEffectsMute
	{
		get
		{
			return R.Settings.IsEffectsMute;
		}
		set
		{
			R.Settings.IsEffectsMute = value;
		}
	}

	public bool IsBGMMute
	{
		get
		{
			return R.Settings.IsBGMMute;
		}
		set
		{
			R.Settings.IsBGMMute = value;
		}
	}

	private void Awake()
	{
		this._bgmSource = this._bgmSource1;
		this.CreatePool();
		AudioManager.Preload();
		AudioListener.volume = 0f;
		DOTween.To(delegate(float v)
		{
			AudioListener.volume = v;
		}, 0f, 1f, 1f).SetDelay(1f);
	}

	private void Update()
	{
		int num = 100;
		int num2 = -20;
		this._mainAudioMixer.SetFloat("Effects", (!this.IsEffectsMute) ? (this.EffectsVolume * (float)(-(float)num2) / (float)num + (float)num2) : -80f);
		this._mainAudioMixer.SetFloat("BGM", (!this.IsBGMMute) ? (this.BGMVolume * (float)(-(float)num2) / (float)num + (float)num2) : -80f);
	}

	private void OnEnable()
	{
		this.EffectsVolume = R.Settings.EffectsVolume;
		this.BGMVolume = R.Settings.BGMVolume;
	}

	private void OnDisable()
	{
		R.Settings.EffectsVolume = this.EffectsVolume;
		R.Settings.BGMVolume = this.BGMVolume;
		R.Settings.Save();
	}

	private static void Preload()
	{
		SingletonMono<AudioManager>.Instance._audioClipCache = new Dictionary<int, AudioClip>();
		SingletonMono<AudioManager>.Instance._bgmCache = new Dictionary<string, AudioClip>();
		foreach (AudioClipData audioClipData in DB.AudioClipData.Values)
		{
			if (audioClipData.type != AudioClipData.AudioClipDataType.Group)
			{
				AudioClip audioClip = Asset.LoadFromResources<AudioClip>(audioClipData.path, audioClipData.name);
				if (audioClip == null)
				{
					string text = string.Format("位于 {0} 的{1}号音效{2}({3})无法被加载", new object[]
					{
						audioClipData.path,
						audioClipData.id,
						audioClipData.name,
						audioClipData.desc
					});
					if (audioClipData.name.Contains(" "))
					{
						Log.Alert(text + "，音效名包含空格");
					}
					else
					{
						Log.Alert(text);
					}
				}
				else
				{
					if (audioClipData.pitchMax < audioClipData.pitchMin)
					{
						Log.Warning(string.Format("音效ID:{0}音调最大值 小于 音调最小值", audioClipData.id));
					}
					if (audioClipData.volumeMax < audioClipData.volumeMin)
					{
						Log.Warning(string.Format("音效ID:{0}音量最大值 小于 音量最小值", audioClipData.id));
					}
					if (audioClipData.type == AudioClipData.AudioClipDataType.BGM)
					{
						SingletonMono<AudioManager>.Instance._bgmCache.Add(audioClipData.path + audioClipData.name, audioClip);
					}
					else
					{
						SingletonMono<AudioManager>.Instance._audioClipCache.Add(audioClipData.id, audioClip);
					}
				}
			}
		}
	}

	private void CreatePool()
	{
		for (int i = 0; i < this._audioList.Count; i++)
		{
			AudioManager.AudioMap audioMap = this._audioList[i];
			ObjectPool objectPool = new ObjectPool();
			base.StartCoroutine(objectPool.Init(audioMap.Prefab, this.PoolParent.gameObject, 5, 10));
			this._audioPool.Add(audioMap.Name, objectPool);
		}
	}

	public AudioSource PlayEffect(int id, Vector3? position = null)
	{
		Vector3 vector = (position == null) ? R.Camera.Transform.position : position.Value;
		AudioClipData audioClipData = AudioClipData.FindById(id);
		switch (audioClipData.type)
		{
		case AudioClipData.AudioClipDataType.EnemyBoss:
			return this.PlayAudioClip(audioClipData, vector, "EnemyBoss", this.enemyBossSourcePrefab);
		case AudioClipData.AudioClipDataType.EnemyElite:
			return this.PlayAudioClip(audioClipData, vector, "EnemyElite", this.enemyEliteSourcePrefab);
		case AudioClipData.AudioClipDataType.EnemyNormal:
			return this.PlayAudioClip(audioClipData, vector, "EnemyNormal", this.enemyNormalSourcePrefab);
		case AudioClipData.AudioClipDataType.PlayerMove:
			return this.PlayAudioClip(audioClipData, vector, "PlayerMove", this.playerMoveSourcePrefab);
		case AudioClipData.AudioClipDataType.PlayerAtk:
			return this.PlayAudioClip(audioClipData, vector, "PlayerAtk", this.playerAtkSourcePrefab);
		case AudioClipData.AudioClipDataType.PlayerMaterial:
			return this.PlayAudioClip(audioClipData, vector, "PlayerMaterial", this.playerMaterialSourcePrefab);
		case AudioClipData.AudioClipDataType.UI:
			return this.PlayAudioClip2D(audioClipData, this._uiSource);
		case AudioClipData.AudioClipDataType.Group:
		{
			int?[] array = StringTools.ParseIntArray(audioClipData.path, ';');
			int num = UnityEngine.Random.Range(0, array.Length);
			int? num2 = array[num];
			if (num2 != null && num2.Value != id)
			{
				return this.PlayEffect(num2.Value, new Vector3?(vector));
			}
			return null;
		}
		case AudioClipData.AudioClipDataType.Scene:
			return this.PlayAudioClip(audioClipData, vector, "Scene", this.sceneSourcePrefab);
		case AudioClipData.AudioClipDataType.Special:
			return this.PlayAudioClip(audioClipData, vector, "Special", this.specialSourcePrefab);
		case AudioClipData.AudioClipDataType.Video:
			return this.PlayAudioClip(audioClipData, vector, "Video", this.videoSourcePrefab);
		default:
			Log.Alert(string.Format("有个未被分类的音效ID是{0}，是怎么回事呢", id));
			return this.PlayAudioClip(audioClipData, vector, "UnSorted", this.unSortedSourcePrefab);
		}
	}

	private AudioSource PlayAudioClip2D(AudioClipData data, [NotNull] AudioSource source)
	{
		AudioClip clip = this._audioClipCache[data.id];
		source.clip = clip;
		source.Play();
		return source;
	}

	public AudioSource PlayAudioClip2D(AudioClip clip, AudioClipData.AudioClipDataType type)
	{
		AudioSource audioSource;
		if (type != AudioClipData.AudioClipDataType.UI)
		{
			if (type != AudioClipData.AudioClipDataType.Video)
			{
				throw new ArgumentNullException("type", "非2D类型");
			}
			audioSource = UnityEngine.Object.Instantiate<GameObject>(this.videoSourcePrefab).GetComponent<AudioSource>();
		}
		else
		{
			audioSource = this._uiSource;
		}
		audioSource.clip = clip;
		audioSource.Play();
		return audioSource;
	}

	private AudioSource PlayAudioClip(AudioClipData data, Vector3 pos, string type, [NotNull] GameObject sourcePrefab)
	{
		AudioClip clip = this._audioClipCache[data.id];
		GameObject gameObject = this.LoadFromPool(type);
		GameObject gameObject2 = (!(gameObject != null)) ? UnityEngine.Object.Instantiate<GameObject>(sourcePrefab) : gameObject;
		AudioSource component = gameObject2.GetComponent<AudioSource>();
		if (!gameObject2.activeSelf)
		{
			gameObject2.SetActive(true);
		}
		component.clip = clip;
		component.pitch = UnityEngine.Random.Range(data.pitchMin, data.pitchMax);
		component.volume = UnityEngine.Random.Range(data.volumeMin, data.volumeMax);
		component.Play();
		component.transform.position = pos;
		return component;
	}

	private GameObject LoadFromPool(string type)
	{
		ObjectPool objectPool;
		return (!this._audioPool.TryGetValue(type, out objectPool)) ? null : objectPool.GetObject();
	}

	public YieldInstruction PlayBGM(int id, bool loop = true)
	{
		AudioClipData audioClipData = AudioClipData.FindById(id);
		if (audioClipData.type != AudioClipData.AudioClipDataType.BGM)
		{
			Log.Alert(string.Format("ID{0}的 Clip，其类型不是BGM", id));
		}
		AudioClip audioClip = null;
		AudioClip audioClip2;
		if (audioClipData.name.Contains("Intro"))
		{
			audioClip = SingletonMono<AudioManager>.Instance._bgmCache[audioClipData.path + audioClipData.name];
			audioClip2 = SingletonMono<AudioManager>.Instance._bgmCache[audioClipData.path + audioClipData.name.Replace("Intro", "Loop")];
		}
		else if (audioClipData.name.Contains("Loop"))
		{
			audioClip = SingletonMono<AudioManager>.Instance._bgmCache[audioClipData.path + audioClipData.name.Replace("Loop", "Intro")];
			audioClip2 = SingletonMono<AudioManager>.Instance._bgmCache[audioClipData.path + audioClipData.name];
		}
		else
		{
			audioClip2 = SingletonMono<AudioManager>.Instance._bgmCache[audioClipData.path + audioClipData.name];
		}
		if (audioClip == null)
		{
			return this.PlayBGM(audioClip2, loop);
		}
		base.StopCoroutine(this.PlayBgmWithIntroCoroutine(null, null));
		return this.PlayBgmWithIntro(audioClip, audioClip2);
	}

	public void PauseBGM()
	{
		this._bgmSource.Pause();
	}

	public void UnPauseBGM()
	{
		this._bgmSource.UnPause();
	}

	public YieldInstruction StopBGM(bool fade = true)
	{
		base.StopCoroutine(this.PlayBgmWithIntroCoroutine(null, null));
		if (fade)
		{
			return this.AudioSourceFadeOut(this._bgmSource);
		}
		this._bgmSource.Stop();
		this._bgmSource.clip = null;
		return null;
	}

	private YieldInstruction PlayBGM(AudioClip clip, bool loop = true)
	{
		if (!this._bgmSource.isPlaying)
		{
			this._bgmSource.Stop();
			this._bgmSource.volume = 1f;
			this._bgmSource.loop = loop;
			this._bgmSource.clip = clip;
			this._bgmSource.Play();
			return this.WaitForEndOfPlaying(this._bgmSource);
		}
		if (this._bgmSource.clip == clip)
		{
			this._bgmSource.loop = loop;
			return null;
		}
		return this.PlayBgmWithCrossFade(clip, loop);
	}

	private YieldInstruction PlayBgmWithIntro(AudioClip introClip, AudioClip loopClip)
	{
		return base.StartCoroutine(this.PlayBgmWithIntroCoroutine(introClip, loopClip));
	}

	private IEnumerator PlayBgmWithIntroCoroutine(AudioClip introClip, AudioClip loopClip)
	{
		if (this._bgmSource.clip == introClip || this._bgmSource.clip == loopClip)
		{
			yield break;
		}
		yield return this.PlayBGM(introClip, false);
		if (this._bgmSource.clip == introClip)
		{
			yield return this.PlayBGM(loopClip, true);
		}
		yield break;
	}

	private Coroutine PlayBgmWithCrossFade(AudioClip nextClip, bool isNextClipLoop = true)
	{
		AudioSource bgmSource = this._bgmSource;
		this._bgmSource = ((!(this._bgmSource == this._bgmSource1)) ? this._bgmSource1 : this._bgmSource2);
		this._bgmSource.loop = isNextClipLoop;
		this._bgmSource.clip = nextClip;
		this._bgmSource.Play();
		this.AudioSourceFadeOut(bgmSource);
		this._bgmSource.volume = 0f;
		this.AudioSourceFadeIn(this._bgmSource);
		return this.WaitForEndOfPlaying(this._bgmSource);
	}

	private Coroutine WaitForEndOfPlaying(AudioSource source)
	{
		return base.StartCoroutine(this.WaitForEndOfPlayingCoroutine(source));
	}

	private IEnumerator WaitForEndOfPlayingCoroutine(AudioSource source)
	{
		while (source.isPlaying)
		{
			yield return null;
		}
		yield break;
	}

	private YieldInstruction AudioSourceFadeOut(AudioSource source)
	{
		if (source.isPlaying)
		{
			return source.DOFade(0f, 0.5f).OnComplete(delegate
			{
				source.clip = null;
			}).WaitForCompletion();
		}
		return null;
	}

	private YieldInstruction AudioSourceFadeIn(AudioSource source)
	{
		if (source.isPlaying)
		{
			return source.DOFade(1f, 0.5f).WaitForCompletion();
		}
		return null;
	}

	public YieldInstruction PlayVoiceOver(string key, Action callback = null, bool hideSubtitle = false)
	{
		this.StopVoiceOver();
		YieldInstruction result;
		try
		{
			VoiceOver[] voiceOvers = VoiceOver.FindByKey(key);
			this._playVoiceOverReturn = base.StartCoroutine(this.PlayVoiceOverCoroutine(voiceOvers, callback, hideSubtitle));
			result = this._playVoiceOverReturn;
		}
		catch (KeyNotFoundException)
		{
			if (Log.CurrentLevel <= Log.LogLevel.Debug)
			{
				throw;
			}
			result = null;
		}
		return result;
	}

	public void PauseVoiceOver()
	{
		if (this._playVoiceOverReturn != null)
		{
			this._voiceOverSource.Pause();
		}
	}

	public void ResumeVoiceOver()
	{
		if (this._playVoiceOverReturn != null)
		{
			this._voiceOverSource.UnPause();
		}
	}

	public void StopVoiceOver()
	{
		if (this._playVoiceOverReturn != null)
		{
			base.StopCoroutine(this._playVoiceOverReturn);
			this._voiceOverSource.Stop();
			R.Ui.UISubtitle.Hide();
		}
	}

	private IEnumerator PlayVoiceOverCoroutine(VoiceOver[] voiceOvers, Action callback, bool hideSubtitle)
	{
		string currentSceneName = LevelManager.SceneName;
		for (int i = 0; i < voiceOvers.Length; i++)
		{
			if (currentSceneName == LevelManager.SceneName)
			{
				Log.Info(string.Format("Play {0}:{1}", voiceOvers[i].Key, voiceOvers[i].LocalizedSubtitle));
				yield return base.StartCoroutine(this.PlayVoiceOverSingleCoroutine(voiceOvers[i], hideSubtitle));
			}
		}
		if (callback != null)
		{
			callback();
		}
		yield break;
	}

	private IEnumerator PlayVoiceOverSingleCoroutine(VoiceOver voiceOver, bool hideSubtitle)
	{
		string path = string.Format("Audio/VoiceOver/{0}/{1}", UIAudioLanguage.CurrentAudioLanguage.Name, voiceOver.FilePath);
		string clipName = voiceOver.FileName;
		if (string.IsNullOrEmpty(voiceOver.FileName))
		{
			clipName = voiceOver.Key;
		}
		AudioClip clip = Asset.LoadFromResources<AudioClip>(path, clipName);
		this._voiceOverSource.clip = clip;
		this._voiceOverSource.Play();
		if (R.Settings.SubtitleVisiable && !hideSubtitle)
		{
			R.Ui.UISubtitle.FadeIn(voiceOver.LocalizedSubtitle);
		}
		bool isComplate;
		bool isSkip;
		do
		{
			yield return null;
			isComplate = (this._voiceOverSource.timeSamples == 0 && !this._voiceOverSource.isPlaying);
			isSkip = false;
			if (Log.CurrentLevel == Log.LogLevel.Debug)
			{
				isSkip = UnityEngine.Input.GetKey(KeyCode.N);
			}
		}
		while (!isComplate && !isSkip);
		if (isSkip)
		{
			this._voiceOverSource.Stop();
		}
		if (R.Settings.SubtitleVisiable && !hideSubtitle)
		{
			R.Ui.UISubtitle.FadeOut();
		}
		yield break;
		yield break;
	}

	public Coroutine PlayVoiceOverArray(string[] array, float interval, Action callback = null, float delay = 0f)
	{
		this.StopVoiceOver();
		return base.StartCoroutine(this.PlayVoiceOverArrayCoroutine(array, interval, callback, delay));
	}

	private IEnumerator PlayVoiceOverArrayCoroutine(string[] array, float interval, Action callback, float delay = 0f)
	{
		string currentSceneName = LevelManager.SceneName;
		yield return new WaitForSeconds(delay);
		for (int i = 0; i < array.Length; i++)
		{
			if (currentSceneName == LevelManager.SceneName)
			{
				yield return this.PlayVoiceOver(array[i], null, false);
				yield return null;
				yield return new WaitForSeconds(interval);
			}
		}
		if (callback != null)
		{
			callback();
		}
		yield break;
	}

	[SerializeField]
	private AudioMixer _mainAudioMixer;

	[SerializeField]
	private AudioSource _bgmSource;

	[SerializeField]
	private AudioSource _bgmSource1;

	[SerializeField]
	private AudioSource _bgmSource2;

	[SerializeField]
	private AudioSource _voiceOverSource;

	[SerializeField]
	private AudioSource _uiSource;

	[SerializeField]
	private GameObject enemyBossSourcePrefab;

	[SerializeField]
	private GameObject enemyEliteSourcePrefab;

	[SerializeField]
	private GameObject enemyNormalSourcePrefab;

	[SerializeField]
	private GameObject playerMoveSourcePrefab;

	[SerializeField]
	private GameObject playerAtkSourcePrefab;

	[SerializeField]
	private GameObject playerMaterialSourcePrefab;

	[SerializeField]
	private GameObject unSortedSourcePrefab;

	[SerializeField]
	private GameObject sceneSourcePrefab;

	[SerializeField]
	private GameObject specialSourcePrefab;

	[SerializeField]
	private GameObject videoSourcePrefab;

	public Transform PoolParent;

	private Dictionary<int, AudioClip> _audioClipCache;

	private Dictionary<string, AudioClip> _bgmCache;

	private Dictionary<string, ObjectPool> _audioPool = new Dictionary<string, ObjectPool>();

	[SerializeField]
	private List<AudioManager.AudioMap> _audioList = new List<AudioManager.AudioMap>();

	private const float FadeDuration = 0.5f;

	private Coroutine _playVoiceOverReturn;

	[Serializable]
	private struct AudioMap
	{
		public string Name;

		public GameObject Prefab;
	}
}
