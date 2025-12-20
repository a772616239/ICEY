using System;
using System.Collections;
using Colorful;
using DG.Tweening;
using ExtensionMethods;
using GameWorld;
using I2.Loc;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class StoryE21 : BaseBehaviour
{
	private bool e21Finish
	{
		set
		{
			SaveStorage.Set("E21_Finish", value);
		}
	}

	private void Start()
	{
		this.spwanEnemy = true;
		base.StartCoroutine(this.P1Coroutine());
	}

	private void Update()
	{
		R.Player.Attribute.currentHP = R.Player.Attribute.maxHP;
	}

	private void OnEnable()
	{
		EventManager.RegisterEvent<EnemyHurtAtkEventArgs>("EnemyHurtAtk", new EventManager.FBEventHandler<EnemyHurtAtkEventArgs>(this.EnemyHurt), EventManager.ListenerQueue.Game);
		EventManager.RegisterEvent<EventArgs>("EnemyKilled", new EventManager.FBEventHandler<EventArgs>(this.EnemyDie), EventManager.ListenerQueue.Game);
	}

	private bool EnemyDie(string eventDefine, object sender, EventArgs msg)
	{
		EnemyAttribute e = sender as EnemyAttribute;
		base.StartCoroutine(this.SpwanEnemy(e));
		return true;
	}

	private void OnDisable()
	{
		EventManager.UnregisterEvent<EnemyHurtAtkEventArgs>("EnemyHurtAtk", new EventManager.FBEventHandler<EnemyHurtAtkEventArgs>(this.EnemyHurt), EventManager.ListenerQueue.Game);
		EventManager.UnregisterEvent<EventArgs>("EnemyKilled", new EventManager.FBEventHandler<EventArgs>(this.EnemyDie), EventManager.ListenerQueue.Game);
	}

	private bool EnemyHurt(string eventDefine, object sender, EnemyHurtAtkEventArgs msg)
	{
		this.hitEnemy = true;
		return true;
	}

	private YieldInstruction ShowTextWithContent(TextMesh text, string content, float time)
	{
		if (text.text != content)
		{
			text.text = content;
		}
		return DOTween.To(delegate(float a)
		{
			text.color = text.color.SetAlpha(a);
		}, 0f, 1f, time).WaitForCompletion();
	}

	private YieldInstruction HideText(TextMesh text, float time = 1f)
	{
		return DOTween.To(delegate(float a)
		{
			text.color = text.color.SetAlpha(a);
		}, 1f, 0f, 1f).WaitForCompletion();
	}

	private void TextShine(bool start, string content = "")
	{
		this.text.text = content;
		this._startShine = start;
		if (start)
		{
			base.StartCoroutine(this.StartShineCoroutine());
		}
	}

	private IEnumerator StartShineCoroutine()
	{
		while (this._startShine)
		{
			yield return DOTween.To(delegate(float a)
			{
				this.text.color = this.text.color.SetAlpha(a);
			}, 1f, 0f, 1f).WaitForCompletion();
		}
		yield break;
	}

	private IEnumerator P1Coroutine()
	{
		yield return R.Ui.HideUI(false);
		yield return this.ShowTextWithContent(this.text, ScriptLocalization.Story.e21s1, 1f);
		yield return R.Audio.PlayVoiceOver("e21t1", null, false);
		CameraFilterUtils.Create<CameraFilterPack_TV_Chromatical>(null);
		R.Camera.Controller.CameraShake(1f, 0.2f, CameraController.ShakeTypeEnum.Rect, false);
		yield return this.HideText(this.text, 1f);
		CameraFilterUtils.Remove<CameraFilterPack_TV_Chromatical>(null);
		YieldInstruction t3 = R.Audio.PlayVoiceOver("e21t3", null, false);
		yield return DOTween.To(() => this.black.color, delegate(Color color)
		{
			this.black.color = color;
		}, Color.black, 1f).WaitForCompletion();
		yield return t3;
		CameraFilterUtils.Create<CameraFilterPack_Color_GrayScale>(null);
		R.Camera.Controller.CameraShake(1f, 0.2f, CameraController.ShakeTypeEnum.Rect, false);
		yield return this.ShowTextWithContent(this.text, ScriptLocalization.Story.e21s29, 1f);
		yield return R.Audio.PlayVoiceOver("e21t4", null, false);
		yield return this.ShowTextWithContent(this.text, ScriptLocalization.Story.e21s2, 1f);
		yield return R.Audio.PlayVoiceOver("e21t5", null, false);
		YieldInstruction t4 = R.Audio.PlayVoiceOver("e21t6", null, false);
		yield return this.ShowTextWithContent(this.text, ScriptLocalization.Story.e21s3, 1f);
		yield return t4;
		YieldInstruction t5 = R.Audio.PlayVoiceOver("e21t7", null, false);
		yield return this.HideText(this.text, 1f);
		yield return t5;
		string userName = R.Player.UserName;
		if (string.IsNullOrEmpty(userName))
		{
			userName = ScriptLocalization.ui.anonymous;
		}
		yield return R.Ui.Terminal.OpenWithAnim(new Color?(Color.white));
		yield return R.Ui.Terminal.Println(ScriptLocalization.Story.e21s4, 0.1f);
		yield return R.Ui.Terminal.ShowProgressBar(0f);
		yield return R.Ui.Terminal.SetProgressBarValueWithAnim(0.99f, 3f);
		yield return R.Audio.PlayVoiceOver("e21t8", null, false);
		R.Ui.CameraGO.transform.DOShakePosition(1f, 0.1f, 10, 90f, false, true);
		yield return R.Ui.Terminal.SetProgressBarValueWithAnim(0f, 2f);
		yield return R.Ui.Terminal.HideProgressBar();
		if (UILanguage.IsSimplifiedChinese)
		{
			yield return R.Ui.Terminal.ShowTexture(this.invaded_cn, 1f, 1f);
		}
		else if (UILanguage.IsTraditionalChinese)
		{
			yield return R.Ui.Terminal.ShowTexture(this.invaded_cn_tw, 1f, 1f);
		}
		else
		{
			yield return R.Ui.Terminal.ShowTexture(this.invaded, 1f, 1f);
		}
		YieldInstruction t6 = R.Audio.PlayVoiceOver("e21t9", null, false);
		yield return R.Ui.Terminal.Println(string.Format("{0}{1}[-]", Color.red.ToBBCode(), ScriptLocalization.Story.e21s6), 0.1f);
		yield return R.Ui.Terminal.ShowProgressBar(0f);
		yield return R.Ui.Terminal.SetProgressBarValueWithAnim(1f, 3f);
		yield return t6;
		AnalogTV analoyTv = R.Ui.CameraGO.GetComponent<AnalogTV>();
		float originScale = analoyTv.Scale;
		for (int i = 0; i < 30; i++)
		{
			analoyTv.Scale += ((float)(i % 2) - 0.5f) * 2f * 0.1f;
			yield return null;
			yield return null;
		}
		analoyTv.Scale = originScale;
		R.Ui.Terminal.transform.DOShakePosition(1f, 0.1f, 10, 90f, false, true);
		yield return R.Audio.PlayVoiceOver("e21t10", null, false);
		yield return R.Ui.Terminal.Println(string.Format("{0}{1}[-]", Color.red.ToBBCode(), ScriptLocalization.Story.e21s7), 0.1f);
		yield return new WaitForSeconds(1f);
		this.HideText(this.text, 1f);
		yield return R.Ui.Terminal.HideTexture();
		yield return R.Ui.Terminal.HideProgressBar();
		yield return R.Ui.Terminal.CloseWithAnim();
		YieldInstruction t7 = R.Audio.PlayVoiceOver("e21t11", null, false);
		yield return new WaitForSeconds(3f);
		yield return this.ShowTextWithContent(this.text, userName, 1f);
		yield return new WaitForSeconds(1f);
		yield return this.HideText(this.text, 1f);
		yield return t7;
		YieldInstruction t8 = R.Audio.PlayVoiceOver("e21t12", null, false);
		this.text.transform.localScale = new Vector3(100f, 100f, 1f);
		this.text.text = ScriptLocalization.Story.e21s8;
		this.text.color = Color.white;
		R.Audio.PlayEffect(360, new Vector3?(this.text.transform.position));
		yield return DOTween.To(() => this.text.transform.localScale, delegate(Vector3 scale)
		{
			this.text.transform.localScale = scale;
		}, Vector3.one, 0.2f).WaitForCompletion();
		yield return new WaitForSeconds(2f);
		yield return this.ShowTextWithContent(this.text, ScriptLocalization.Story.e21s9, 1f);
		yield return new WaitForSeconds(2f);
		yield return t8;
		yield return R.Audio.PlayVoiceOver("e21t13", null, false);
		yield return this.ShowTextWithContent(this.text, ScriptLocalization.Story.e21s10, 1f);
		yield return new WaitForSeconds(2f);
		this.text.text = string.Empty;
		this.animator.gameObject.SetActive(true);
		this.animator.Play("ControllerIdle");
		this.animation.gameObject.SetActive(true);
		R.Audio.PlayEffect(145, new Vector3?(this.text.transform.position));
		yield return R.Audio.PlayVoiceOver("e21t14", null, false);
		yield return R.Audio.PlayVoiceOver("e21t15", null, false);
		this.animator.gameObject.SetActive(false);
		this.text.text = ScriptLocalization.Story.e21s30;
		R.Audio.PlayEffect(145, new Vector3?(this.text.transform.position));
		yield return R.Audio.PlayVoiceOver("e21t16", null, false);
		this.lgate.Appear();
		this.rgate.Appear();
		GameArea.EnemyRange.xMin = this.lgate.transform.Find("PlayerLimitPosLX").transform.position.x;
		GameArea.EnemyRange.xMax = this.rgate.transform.Find("PlayerLimitPosRX").transform.position.x;
		GameArea.CameraRange.xMin = this.lgate.transform.Find("CameraLimitPosLX").transform.position.x;
		GameArea.CameraRange.xMax = this.rgate.transform.Find("CameraLimitPosRX").transform.position.x;
		R.SceneData.CanAIRun = true;
		for (int j = 0; j < 5; j++)
		{
			float xPos = Mathf.Clamp(R.Player.Transform.position.x + UnityEngine.Random.Range(-6f, 6f), GameArea.EnemyRange.xMin, GameArea.EnemyRange.xMax);
			Singleton<EnemyGenerator>.Instance.GenerateEnemy(this.enemy[j % this.enemy.Length], new Vector2?(new Vector2(xPos, R.Player.Transform.position.y)), true, true);
			yield return null;
		}
		float t9 = 0f;
		bool time20 = false;
		bool time21 = false;
		bool time22 = false;
		while (!this.hitEnemy)
		{
			t9 += Time.deltaTime;
			if (t9 > 60f && !time22)
			{
				time22 = true;
				R.Audio.PlayVoiceOver("e21t39", null, false);
			}
			if (t9 > 40f && !time21)
			{
				time21 = true;
				R.Audio.PlayVoiceOver("e21t38", null, false);
			}
			if (t9 > 20f && !time20)
			{
				time20 = true;
				R.Audio.PlayVoiceOver("e21t37", null, false);
			}
			yield return null;
		}
		base.StartCoroutine(this.P2Coroutine());
		yield break;
	}

	private IEnumerator P2Coroutine()
	{
		R.Audio.PlayBGM(262, true);
		yield return R.Audio.PlayVoiceOver("e21t17", null, false);
		yield return new WaitForSeconds(15f);
		this.animation.gameObject.SetActive(false);
		this.text.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 180f));
		this.text.text = ScriptLocalization.Story.e21s11;
		Camera.main.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 180f));
		yield return R.Audio.PlayVoiceOver("e21t18", null, false);
		yield return new WaitForSeconds(15f);
		this.text.text = string.Empty;
		this.text.transform.rotation = Quaternion.identity;
		Camera.main.transform.rotation = Quaternion.identity;
		CameraFilterUtils.Create<CameraFilterPack_TV_Videoflip>(null);
		yield return R.Audio.PlayVoiceOver("e21t19", null, false);
		yield return new WaitForSeconds(15f);
		CameraFilterUtils.Remove<CameraFilterPack_TV_Videoflip>(null);
		YieldInstruction t20 = R.Audio.PlayVoiceOver("e21t20", null, false);
		this.animator.gameObject.SetActive(true);
		this.animator.Play("ControllerRSpark");
		for (int i = 0; i < 5; i++)
		{
			yield return new WaitForSeconds(2f);
			InputSetting.Stop(false);
			yield return new WaitForSeconds(1f);
			InputSetting.Resume(false);
		}
		yield return R.Audio.PlayVoiceOver("e21t21", null, false);
		InputSetting.Stop(false);
		this.animator.Play("ControllerNoUse");
		R.Ui.uiNotifacation.FadeIn(ScriptLocalization.Story.e21s12);
		yield return R.Audio.PlayVoiceOver("e21t22", null, false);
		if (!R.Player.Action.stateMachine.currentState.IsInArray(PlayerAction.NormalSta))
		{
			R.Player.ActionController.ChangeState(PlayerAction.StateEnum.Idle);
		}
		this.spwanEnemy = false;
		foreach (EnemyAttribute enemyAttribute in R.Enemy.EnemyAttributes)
		{
			enemyAttribute.currentHp = 0;
		}
		R.Audio.StopBGM(true);
		yield return R.Audio.PlayVoiceOver("e21t23", null, false);
		CameraFilterUtils.Remove<CameraFilterPack_Color_GrayScale>(null);
		yield return new WaitForSeconds(2f);
		yield return R.Audio.PlayVoiceOver("e21t24", null, false);
		int dir = InputSetting.JudgeDir(R.Player.Transform.position, this.stopPos.position);
		R.Player.ActionController.TurnRound(dir);
		R.Player.ActionController.StartMove();
		while (Vector3.Distance(R.Player.Transform.position, this.stopPos.position) > 0.5f)
		{
			yield return null;
		}
		R.Player.ActionController.StopMove();
		R.SceneData.CanAIRun = false;
		CameraFilterPack_Pixel_Pixelisation pixelisation = CameraFilterUtils.Create<CameraFilterPack_Pixel_Pixelisation>(null);
		pixelisation._Pixelisation = 2f;
		pixelisation._SizeX = 2.25f;
		pixelisation._SizeY = 4f;
		yield return R.Audio.PlayVoiceOver("e21t25", null, false);
		for (int j = 0; j < 5; j++)
		{
			float xPos = Mathf.Clamp(R.Player.Transform.position.x + UnityEngine.Random.Range(-6f, 6f), GameArea.EnemyRange.xMin, GameArea.EnemyRange.xMax);
			Singleton<EnemyGenerator>.Instance.GenerateEnemy(this.enemy[j % this.enemy.Length], new Vector2?(new Vector2(xPos, R.Player.Transform.position.y)), true, true);
			yield return null;
		}
		yield return R.Audio.PlayVoiceOver("e21t26", null, false);
		CameraFilterUtils.Remove<CameraFilterPack_Pixel_Pixelisation>(null);
		YieldInstruction t21 = R.Audio.PlayVoiceOver("e21t27", null, false);
		yield return R.Player.Transform.DOShakePosition(1f, Vector3.right * 0.1f, 10, 90f, false, true).WaitForCompletion();
		yield return t21;
		YieldInstruction t22 = R.Audio.PlayVoiceOver("e21t28", null, false);
		yield return R.Player.Transform.DOShakePosition(1f, Vector3.right * 0.2f, 10, 90f, false, true).WaitForCompletion();
		yield return t22;
		YieldInstruction t23 = R.Audio.PlayVoiceOver("e21t29", null, false);
		yield return R.Player.Transform.DOShakePosition(1f, Vector3.right * 0.3f, 10, 90f, false, true).WaitForCompletion();
		yield return t23;
		foreach (EnemyAttribute enemyAttribute2 in R.Enemy.EnemyAttributes)
		{
			enemyAttribute2.currentHp = 1;
		}
		R.Player.Transform.GetComponent<Claymore>().StartCharge(false);
		yield return new WaitForSeconds(4.2f);
		R.Player.Transform.GetComponent<Claymore>().ReleaseCharge(false);
		yield return new WaitForSeconds(0.2f);
		this.tv_Main.sprite = this.tv_M;
		this.tv_Left.sprite = this.tv_L;
		this.tv_Right.sprite = this.tv_R;
		R.Audio.PlayEffect(368, new Vector3?(this.text.transform.position));
		yield return R.Audio.PlayVoiceOver("e21t30", null, false);
		UnityEngine.Object.Destroy(this.animator.gameObject);
		this.lgate.DisAppear();
		this.rgate.DisAppear();
		yield return new WaitForSeconds(3f);
		R.Audio.StopBGM(true);
		R.Audio.PlayBGM(479, false);
		yield return new WaitForSeconds(2f);
		yield return R.Audio.PlayVoiceOver("e21t31", null, false);
		yield return R.Audio.PlayVoiceOver("e21t32", null, false);
		yield return R.Ui.Volume.Show(1f);
		yield return new WaitForSeconds(0.5f);
		R.Player.Transform.GetComponent<Claymore>().StartCharge(false);
		yield return new WaitForSeconds(1f);
		YieldInstruction t24 = R.Audio.PlayVoiceOver("e21t33", null, false);
		R.Player.Transform.GetComponent<Claymore>().ReleaseCharge(false);
		yield return R.Ui.Volume.BarAnim(0f, 0.2f);
		yield return R.Ui.Volume.Hide();
		yield return t24;
		CameraFilterUtils.Create<CameraFilterPack_TV_ARCADE_2>(null);
		yield return R.Ui.uiNotifacation.FadeOut(0f, 1f);
		SunShafts sun = Camera.main.gameObject.AddComponent<SunShafts>();
		sun.sunThreshold = Color.black;
		sun.sunShaftBlurRadius = 10f;
		sun.sunShaftsShader = Shader.Find("Hidden/SunShaftsComposite");
		sun.simpleClearShader = Shader.Find("Hidden/SimpleClear");
		R.Mode.EnterMode(Mode.AllMode.Story);
		DOTween.To(() => sun.sunShaftIntensity, delegate(float intensity)
		{
			sun.sunShaftIntensity = intensity;
		}, 2f, 5f);
		YieldInstruction t25 = R.Audio.PlayVoiceOver("e21t34", null, false);
		R.Camera.Controller.EnableGlobalBloom();
		R.Camera.Controller.CameraZoom(R.Player.Transform.position + Vector3.up, 5f, 4f);
		R.Ui.Toast.Show(ScriptLocalization.Story.e21s13, 1f, true);
		yield return new WaitForSeconds(1.2f);
		R.Ui.Toast.Show(ScriptLocalization.Story.e21s14, 1f, true);
		yield return new WaitForSeconds(1.2f);
		yield return t25;
		YieldInstruction t26 = R.Audio.PlayVoiceOver("e21t35", null, false);
		R.Player.RoleName = R.Player.UserName;
		if (string.IsNullOrEmpty(R.Player.RoleName))
		{
			R.Player.RoleName = "anonymous";
		}
		yield return DOTween.To(() => sun.sunShaftIntensity, delegate(float intensity)
		{
			sun.sunShaftIntensity = intensity;
		}, 10f, 5f).WaitForCompletion();
		yield return t26;
		YieldInstruction t27 = R.Audio.PlayVoiceOver("e21t36", null, false);
		CameraFilterUtils.Remove<CameraFilterPack_TV_ARCADE_2>(null);
		R.Trophy.AwardTrophy(22);
		R.Ui.Toast.Show(ScriptLocalization.Story.e21s15, 5f, true);
		yield return new WaitForSeconds(5f);
		yield return t27;
		SingletonMono<MobileInputPlayer>.Instance.MainControllerVisiable = false;
		yield return R.Ui.EndTitle.Show(true);
		yield return DOTween.To(() => this.windyBlack.color, delegate(Color color)
		{
			this.windyBlack.color = color;
		}, Color.white, 3f).WaitForCompletion();
		yield return DOTween.To(() => this.windy.color, delegate(Color color)
		{
			this.windy.color = color;
		}, Color.white, 5f).WaitForCompletion();
		yield return new WaitForSeconds(1f);
		this.windy.gameObject.SetActive(false);
		yield return R.Ui.Terminal.OpenWithAnim(new Color?(Color.white));
		yield return R.Ui.Terminal.Println(ScriptLocalization.Story.e21s16, 0.1f);
		yield return R.Ui.Terminal.ShowProgressBar(0f);
		yield return R.Ui.Terminal.SetProgressBarValueWithAnim(1f, 3f);
		yield return new WaitForSeconds(3f);
		yield return R.Ui.Terminal.HideProgressBar();
		string icey = Color.green.ToBBCode();
		yield return R.Ui.Terminal.Println(ScriptLocalization.Story.e21s17, 0.1f);
		yield return new WaitForSeconds(2f);
		yield return R.Ui.Terminal.Println(ScriptLocalization.Story.e21s18, 0.1f);
		yield return new WaitForSeconds(2f);
		yield return R.Ui.Terminal.Println(ScriptLocalization.Story.e21s19, 0.1f);
		yield return new WaitForSeconds(2f);
		yield return R.Ui.Terminal.Println(ScriptLocalization.Story.e21s20, 0.1f);
		yield return new WaitForSeconds(2f);
		yield return R.Ui.Terminal.Println(ScriptLocalization.Story.e21s21, 0.1f);
		yield return new WaitForSeconds(1f);
		yield return R.Ui.Terminal.PrintlnInstantly(ScriptLocalization.Story.e21s22);
		yield return new WaitForSeconds(1f);
		yield return R.Ui.Terminal.Println(ScriptLocalization.Story.e21s23, 0.1f);
		yield return new WaitForSeconds(1f);
		yield return R.Ui.Terminal.PrintlnInstantly(ScriptLocalization.Story.e21s24);
		yield return new WaitForSeconds(5f);
		yield return R.Ui.Terminal.PrintlnInstantly(ScriptLocalization.Story.e21s25);
		yield return R.Ui.Terminal.Println(string.Format("\n{0}{1}[-]", icey, ScriptLocalization.Story.e21s26), 0.1f);
		yield return new WaitForSeconds(1f);
		yield return R.Ui.Terminal.PrintlnInstantly(ScriptLocalization.Story.e21s27);
		yield return new WaitForSeconds(2f);
		R.Ui.Terminal.Clear();
		yield return R.Ui.Terminal.Println(ScriptLocalization.Story.e21s28, 0.2f);
		yield return new WaitForSeconds(3f);
		yield return R.Ui.Terminal.CloseWithAnim();
		R.Ui.BlackScene.Alpha = 1f;
		UnityEngine.Object.Destroy(Camera.main.GetComponent<SunShafts>());
		this.e21Finish = true;
		yield return new WaitForSeconds(1f);
		InputSetting.Resume(false);
		R.Ui.BlackScene.Alpha = 0f;
		R.Mode.EnterMode(Mode.AllMode.Story);
		yield return R.Ui.LevelSelect.OpenWithAnim(true, true);
		yield break;
	}

	private IEnumerator SpwanEnemy(EnemyAttribute e)
	{
		yield return new WaitForSeconds(UnityEngine.Random.Range(2f, 3f));
		if (this.spwanEnemy)
		{
			Singleton<EnemyGenerator>.Instance.GenerateEnemy(e.baseData.enemyType, new Vector2?(R.Player.Transform.position + new Vector3(UnityEngine.Random.Range(-3f, 3f), 0f, 0f)), true, true);
		}
		yield break;
	}

	private void OnDestroy()
	{
		if (!SingletonMono<CameraController>.ApplicationIsQuitting)
		{
			Camera.main.transform.rotation = Quaternion.identity;
			CameraFilterUtils.Remove<CameraFilterPack_TV_Chromatical>(null);
			CameraFilterUtils.Remove<CameraFilterPack_Color_GrayScale>(null);
			CameraFilterUtils.Remove<CameraFilterPack_TV_Videoflip>(null);
			CameraFilterUtils.Remove<CameraFilterPack_Pixel_Pixelisation>(null);
			CameraFilterUtils.Remove<CameraFilterPack_TV_ARCADE_2>(null);
			UnityEngine.Object.Destroy(Camera.main.GetComponent<SunShafts>());
			SingletonMono<MobileInputPlayer>.Instance.MainControllerVisiable = true;
		}
	}

	[SerializeField]
	private TextMesh text;

	[SerializeField]
	private SpriteRenderer black;

	[SerializeField]
	private EnemyType[] enemy;

	[SerializeField]
	private Transform stopPos;

	private bool _startShine;

	private bool spwanEnemy;

	private bool hitEnemy;

	[SerializeField]
	private BattleZoneGate lgate;

	[SerializeField]
	private BattleZoneGate rgate;

	[SerializeField]
	private SpriteRenderer windy;

	[SerializeField]
	private SpriteRenderer windyBlack;

	[SerializeField]
	private Texture invaded;

	[SerializeField]
	private Texture invaded_cn;

	[SerializeField]
	private Texture invaded_cn_tw;

	[SerializeField]
	private Animator animator;

	[SerializeField]
	private Animation animation;

	[SerializeField]
	private SpriteRenderer tv_Main;

	[SerializeField]
	private SpriteRenderer tv_Left;

	[SerializeField]
	private SpriteRenderer tv_Right;

	[SerializeField]
	private Sprite tv_M;

	[SerializeField]
	private Sprite tv_L;

	[SerializeField]
	private Sprite tv_R;
}
