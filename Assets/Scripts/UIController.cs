using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class UIController : SingletonMono<UIController>
{
	private void Awake()
	{
		this.Camera = this.CameraGO.GetComponent<Camera>();
	}

	public void Reset()
	{
		if (this.bossHpBar.Visible)
		{
			this.bossHpBar.Disappear();
		}
		R.Ui.HitsGrade.HideHitNumAndBar();
	}

	public YieldInstruction ShowUI(bool immediately = false)
	{
		if (immediately)
		{
			for (int i = 0; i < this._uiNeedHide.Length; i++)
			{
				this._uiNeedHide[i].alpha = 1f;
			}
			this.IsHide = false;
			return null;
		}
		return base.StartCoroutine(this.IFadeInAnim());
	}

	public YieldInstruction HideUI(bool immediately = false)
	{
		if (immediately)
		{
			for (int i = 0; i < this._uiNeedHide.Length; i++)
			{
				this._uiNeedHide[i].alpha = 0f;
			}
			this.IsHide = true;
			return null;
		}
		return base.StartCoroutine(this.IFadeOutAnim());
	}

	public GameObject CreateEnemyPoint(EnemyAttribute enemy)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.enemyPoint);
		if (gameObject != null)
		{
			UIEnemyPointController component = gameObject.GetComponent<UIEnemyPointController>();
			component.enemy = enemy;
		}
		return this.enemyPoint;
	}

	public void EnterMovieMode()
	{
		UIWidget component = this.movieModePlayAnimation.GetComponent<UIWidget>();
		if (component.alpha < 1f)
		{
			component.alpha = 1f;
			this.movieModePlayAnimation.Play(true);
			return;
		}
	}

	public void ExitMovieMode()
	{
		UIWidget widget = this.movieModePlayAnimation.GetComponent<UIWidget>();
		if (widget.alpha < 1f)
		{
			return;
		}
		EventDelegate.Add(this.movieModePlayAnimation.onFinished, delegate()
		{
			widget.alpha = 0f;
		}, true);
		this.movieModePlayAnimation.Play(false);
	}

	private IEnumerator IFadeInAnim()
	{
		for (int i = 0; i < this._uiNeedHide.Length; i++)
		{
			this.FadeTo(this._uiNeedHide[i], 1f, 0.5f);
		}
		yield return WorldTime.WaitForSecondsIgnoreTimeScale(0.5f);
		this.IsHide = false;
		yield break;
	}

	private IEnumerator IFadeOutAnim()
	{
		this.IsHide = true;
		for (int i = 0; i < this._uiNeedHide.Length; i++)
		{
			this.FadeTo(this._uiNeedHide[i], 0f, 0.5f);
		}
		yield return WorldTime.WaitForSecondsIgnoreTimeScale(0.5f);
		yield break;
	}

	private void FadeTo(UIRect widget, float endValue, float duration)
	{
		DOTween.To(() => widget.alpha, delegate(float alpha)
		{
			widget.alpha = alpha;
		}, endValue, duration);
	}

	public bool IsHide;

	[SerializeField]
	public UIWidget RootWidget;

	[SerializeField]
	public GameObject CameraGO;

	[NonSerialized]
	public Camera Camera;

	[SerializeField]
	private UIRect[] _uiNeedHide;

	[HideInInspector]
	public UISubtitleController UISubtitle;

	[SerializeField]
	public UIBlackSceneController BlackScene;

	[SerializeField]
	public UIHitsGradeController HitsGrade;

	[SerializeField]
	public UIEnhancementController Enhancement;

	[SerializeField]
	public UINotifacationController uiNotifacation;

	[SerializeField]
	public UIBossHpBarController bossHpBar;

	[SerializeField]
	public UIToast Toast;

	[SerializeField]
	public UISaveProgressCircleController SaveProgressCircle;

	[SerializeField]
	public UITerminalController Terminal;

	[SerializeField]
	public UILevelSelectController LevelSelect;

	[SerializeField]
	public UITutorialController Tutorial;

	[SerializeField]
	public UIPauseController Pause;

	[SerializeField]
	public UILevelNameController LevelName;

	[SerializeField]
	public UIVolumeController Volume;

	[SerializeField]
	public UIEndTitleController EndTitle;

	[SerializeField]
	public UIFlashController Flash;

	[SerializeField]
	public UIBloodPalaceController BloodPalace;

	[SerializeField]
	public UITrophyNotificationController TrophyNotification;

	[SerializeField]
	private GameObject enemyPoint;

	[SerializeField]
	private UIPlayAnimation movieModePlayAnimation;
}
