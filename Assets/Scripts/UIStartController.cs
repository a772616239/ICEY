using System;
using System.Collections;
using Core;
using DG.Tweening;
using ExtensionMethods;
using I2.Loc;
using UnityEngine;

public class UIStartController : MonoBehaviour
{
	private void Awake()
	{
		this._startLocalize = this._startLabel.GetComponent<Localize>();
	}

	private void Start()
	{
		this._widget.SetAnchor(R.Ui.RootWidget.gameObject, 0, 0, 0, 0);
		this._black.color = Color.black;
		R.Camera.Controller.IsFollowPivot = false;
		R.Camera.Controller.MovableCamera.position = Vector3.zero.SetZ(-10f);
		R.Mode.Reset();
		R.Mode.EnterMode(Mode.AllMode.UI);
		R.Ui.Pause.Enabled = false;
		R.Ui.HideUI(true);
		UIKeyInput.SaveAndSetHoveredObject(this.circle.gameObject);
		base.StartCoroutine(this.FadeInCoroutine());
		for (int i = 0; i < this._useOnStandalone.Length; i++)
		{
			this._useOnStandalone[i].SetActive(false);
		}
		if (!SaveManager.IsAutoSaveDataExists)
		{
			this._mainContainerGameObject.SetActive(false);
			this._userNameGameObject.SetActive(true);
			this._sensitiveWordsFilter = new SensitiveWordsFilter();
		}
	}

	private IEnumerator FadeInCoroutine()
	{
		yield return DOTween.To(() => this._black.color, delegate(Color c)
		{
			this._black.color = c;
		}, Color.clear, 1f).WaitForCompletion();
		this._startBtn.SetActive(true);
		this._optionsBtn.SetActive(true);
		yield break;
	}

	private void OnEnable()
	{
		this._startLocalize.Term = ((!SaveManager.IsAutoSaveDataExists) ? "ui/start/start" : "ui/start/_continue");
	}

	private void Update()
	{
		if (Core.Input.UI.Cancel.OnClick && this._mainContainerGameObject.activeSelf)
		{
			this.OnExitClick();
		}
	}

	public void OnStartClick()
	{
		if (!Core.Input.JoystickIsOpen)
		{
			return;
		}
		if (UIStartController.IsEnterWithVoice)
		{
			base.StartCoroutine(this.OnStartWithVoiceOverClick());
		}
		else
		{
			base.StartCoroutine(this.OnStartClickCoroutine());
		}
	}

	private IEnumerator ProgressAnimCoroutine()
	{
		float nowprocess = 0f;
		float currentVelocity = 0f;
		float smoothTime = 0.1f;
		while (nowprocess < 99.5f)
		{
			float toProcess;
			if (R.SceneGate.Progress < 0.9f)
			{
				toProcess = R.SceneGate.Progress * 100f;
			}
			else
			{
				toProcess = 100f;
			}
			if (nowprocess < toProcess)
			{
				nowprocess = Mathf.SmoothDamp(nowprocess, toProcess, ref currentVelocity, smoothTime);
			}
			this._loading.value = nowprocess / 100f;
			yield return null;
		}
		DOTween.To(() => this._black.color, delegate(Color c)
		{
			this._black.color = c;
		}, Color.black, 2f).WaitForCompletion();
		InputSetting.Resume(false);
		R.Mode.ExitMode(Mode.AllMode.UI);
		UIKeyInput.LoadHoveredObject();
		R.SceneGate.AllowSceneActivation = true;
		R.Ui.ShowUI(false);
		yield return new WaitForSeconds(3f);
		yield break;
	}

	private IEnumerator OnStartClickCoroutine()
	{
		this._mainLayer.Disappear();
		InputSetting.Stop(false);
		UIKeyInput.LoadHoveredObject();
		this._loading.alpha = 1f;
		R.SceneGate.AllowSceneActivation = false;
		R.Ui.Pause.Enabled = true;
		if (SaveManager.IsAutoSaveDataExists)
		{
			yield return R.GameData.Load();
			SingletonMono<MobileInputPlayer>.Instance.VisiableBladeStorm();
			LevelManager.LoadLevelByPosition(R.GameData.SceneName, R.GameData.PlayerPosition, true);
			yield return base.StartCoroutine(this.ProgressAnimCoroutine());
			R.Player.Transform.position = R.GameData.PlayerPosition;
			R.Player.Action.TurnRound(R.GameData.PlayerAttributeGameData.faceDir);
			R.Camera.Controller.CameraResetPostionAfterSwitchScene();
		}
		else
		{
			this._tutorialGate.Enter(true);
			SingletonMono<MobileInputPlayer>.Instance.VisiableBladeStorm();
			base.StartCoroutine(this.ProgressAnimCoroutine());
		}
		yield break;
	}

	private IEnumerator OnStartWithVoiceOverClick()
	{
		yield return base.StartCoroutine(this.MakeNoise(0.1f, null));
		this._pressCount++;
		if (this._pressCount > 4)
		{
			if (this._pressCount == 5)
			{
				this._startLocalize.Term = "ui/start/start_with_voiceover";
				this._startLabel.color = Color.red;
			}
			else
			{
				R.Audio.PlayEffect(376, null);
				InputSetting.Stop(false);
				base.StartCoroutine(this.MakeNoise(2f, null));
				R.Trophy.AwardTrophy(23);
				yield return R.Ui.BlackScene.FadeBlack(2f, false);
				InputSetting.Resume(false);
				R.Mode.ExitMode(Mode.AllMode.UI);
				UIKeyInput.LoadHoveredObject();
				UIStartController.IsEnterWithVoice = false;
				this._tutorialGate.data.ToLevelId = "ui_story_start";
				this._tutorialGate.data.ToId = 1;
				R.Ui.Pause.Enabled = true;
				this._tutorialGate.Enter(false);
				SingletonMono<MobileInputPlayer>.Instance.VisiableBladeStorm();
			}
		}
		yield break;
	}

	private IEnumerator MakeNoise(float seconds, Action callback = null)
	{
		InputSetting.Stop(false);
		CameraFilterUtils.Create<CameraFilterPack_TV_Old>(R.Ui.CameraGO);
		yield return new WaitForSeconds(seconds);
		InputSetting.Resume(false);
		CameraFilterUtils.Remove<CameraFilterPack_TV_Old>(R.Ui.CameraGO);
		if (callback != null)
		{
			callback();
		}
		yield break;
	}

	public void OnExitClick()
	{
		Application.Quit();
	}

	public void ChangeUserName()
	{
		if (string.IsNullOrEmpty(this._userNameInput.value))
		{
			return;
		}
		string value;
		if (this._sensitiveWordsFilter.Filter(this._userNameInput.value, out value))
		{
			this._userNameInput.value = value;
		}
	}

	public void ConfirmUserName()
	{
		if (string.IsNullOrEmpty(this._userNameInput.value))
		{
			return;
		}
		R.GameData.UserName = this._userNameInput.value;
		this._userNameGameObject.SetActive(false);
		this._mainContainerGameObject.SetActive(true);
		Analytics.SetUser(SystemInfo.deviceUniqueIdentifier, TGTUserType.TGTTypeAnonymous, TGTUserSex.TGTSexUnknown, 0, R.GameData.UserName);
	}

	public static bool IsEnterWithVoice;

	private int _pressCount;

	[SerializeField]
	private UIWidget _widget;

	[SerializeField]
	private SceneGate _tutorialGate;

	[SerializeField]
	private UISprite _black;

	[SerializeField]
	private UIProgressBar _loading;

	[SerializeField]
	private UITexture circle;

	[SerializeField]
	private UILayerController _mainLayer;

	[SerializeField]
	private UILabel _startLabel;

	private Localize _startLocalize;

	[SerializeField]
	private GameObject[] _useOnStandalone;

	[SerializeField]
	private UIInput _userNameInput;

	[SerializeField]
	private GameObject _mainContainerGameObject;

	[SerializeField]
	private GameObject _userNameGameObject;

	[SerializeField]
	private GameObject _startBtn;

	[SerializeField]
	private GameObject _optionsBtn;

	private SensitiveWordsFilter _sensitiveWordsFilter;
}
