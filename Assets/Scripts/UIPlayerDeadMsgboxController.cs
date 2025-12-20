using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Colorful;
using Core;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class UIPlayerDeadMsgboxController : MonoBehaviour
{
	private int DeathCount
	{
		get
		{
			return RoundStorage.Get("DeathCount", 0);
		}
		set
		{
			RoundStorage.Set("DeathCount", value);
		}
	}

	private bool HasEnteredLevelSelectSystem
	{
		get
		{
			return SaveStorage.Get("HasEnteredLevelSelectSystem", false);
		}
		set
		{
			SaveStorage.Set("HasEnteredLevelSelectSystem", value);
		}
	}

	private void Start()
	{
		this.listener = R.Player.GetComponent<PlayerAnimEventListener>();
		this.listener.OnPlayerDead += this.OnPlayerDead;
	}

	private void OnDestroy()
	{
		if (this.listener)
		{
			this.listener.OnPlayerDead -= this.OnPlayerDead;
		}
	}

	private void OnPlayerDead(object sender, EventArgs e)
	{
		if (R.SceneData.BloodPalaceMode)
		{
			return;
		}
		if (UILanguage.IsChinese)
		{
			List<List<int>> list = new List<List<int>>
			{
				new List<int>
				{
					1,
					5,
					8,
					10
				},
				new List<int>
				{
					2,
					6,
					9,
					10
				},
				new List<int>
				{
					3,
					7,
					8,
					9,
					10
				},
				new List<int>
				{
					4,
					5,
					6,
					7,
					8,
					9,
					10
				}
			};
			string text = "又㕛叒叕";
			if (this.DeathCount <= 10)
			{
				StringBuilder stringBuilder = new StringBuilder("你");
				for (int i = 0; i < 4; i++)
				{
					if (list[i].Contains(this.DeathCount))
					{
						stringBuilder.Append(text[i]);
					}
				}
				stringBuilder.Append("死了");
				this._youAreDie.text = stringBuilder.ToString();
			}
			else
			{
				this._youAreDie.text = "我已经记不清了";
			}
		}
		else if (this.DeathCount == 0)
		{
			this._youAreDie.text = "YOU DIED";
		}
		else if (this.DeathCount <= 10)
		{
			StringBuilder stringBuilder2 = new StringBuilder("YOU DIE AGAIN");
			for (int j = 0; j < this.DeathCount - 1; j++)
			{
				stringBuilder2.Append('N');
			}
			this._youAreDie.text = stringBuilder2.ToString();
		}
		else
		{
			this._youAreDie.text = "I CAN NEVER REMEMBER";
		}
		this.OpenWithAnim();
	}

	private void Open()
	{
		this._backBtn.transform.parent.gameObject.SetActive(this.HasEnteredLevelSelectSystem);
		R.Mode.EnterMode(Mode.AllMode.UI);
		R.Ui.Pause.Enabled = false;
		UIKeyInput.SaveAndSetHoveredObject(this._resumeBtn);
		this.playerDeadWindow.gameObject.SetActive(true);
		CameraFilterUtils.Create<CameraFilterPack_TV_80>(R.Ui.CameraGO);
		AnalogTV analogTV = R.Ui.CameraGO.AddComponent<AnalogTV>();
		analogTV.Shader = Shader.Find("Hidden/Colorful/Analog TV");
		analogTV.NoiseIntensity = 1f;
		analogTV.ScanlinesIntensity = 0f;
		analogTV.ScanlinesCount = 696;
		analogTV.Distortion = 0.18f;
		analogTV.CubicDistortion = 0f;
		analogTV.Scale = 1.02f;
	}

	private YieldInstruction OpenWithAnim()
	{
		return base.StartCoroutine(this.OpenWithAnimCoroutine());
	}

	private IEnumerator OpenWithAnimCoroutine()
	{
		R.Audio.StopBGM(true);
		yield return LevelManager.LoadLevelByGateId("empty", SceneGate.OpenType.None);
		this.Open();
		AnalogTV analogTV = R.Ui.CameraGO.GetComponent<AnalogTV>();
		analogTV.Scale = 0f;
		yield return DOTween.To(() => analogTV.Scale, delegate(float scale)
		{
			analogTV.Scale = scale;
		}, 1.02f, 0.5f).SetUpdate(true).WaitForCompletion();
		yield break;
	}

	private void Close()
	{
		CameraFilterUtils.Remove<CameraFilterPack_TV_80>(R.Ui.CameraGO);
		UnityEngine.Object.Destroy(R.Ui.CameraGO.GetComponent<AnalogTV>());
		this.playerDeadWindow.gameObject.SetActive(false);
		UIKeyInput.LoadHoveredObject();
		R.Ui.Pause.Enabled = true;
		R.Mode.ExitMode(Mode.AllMode.UI);
	}

	private YieldInstruction CloseWithAnim()
	{
		AnalogTV analogTV = R.Ui.CameraGO.GetComponent<AnalogTV>();
		return DOTween.To(() => analogTV.Scale, delegate(float scale)
		{
			analogTV.Scale = scale;
		}, 0f, 0.5f).SetUpdate(true).OnComplete(delegate
		{
			this.Close();
		}).WaitForCompletion();
	}

	public void Go2StartScene()
	{
		if (!Core.Input.JoystickIsOpen)
		{
			return;
		}
		base.StartCoroutine(this.OnRoundOverCoroutine());
	}

	private IEnumerator OnRoundOverCoroutine()
	{
		InputSetting.Stop(false);
		R.Ui.BlackScene.FadeBlack(0.3f, false);
		yield return this.CloseWithAnim();
		R.Ui.BlackScene.FadeTransparent(0.3f, false);
		yield return LevelManager.OnRoundOver();
		InputSetting.Resume(false);
		SingletonMono<MobileInputPlayer>.Instance.Visible = false;
		yield break;
	}

	public void Resurrect()
	{
		if (!Core.Input.JoystickIsOpen)
		{
			return;
		}
		base.StartCoroutine(this.OnPlayerDieCoroutine());
	}

	private IEnumerator OnPlayerDieCoroutine()
	{
		InputSetting.Stop(false);
		R.Ui.BlackScene.FadeBlack(0.3f, false);
		yield return this.CloseWithAnim();
		yield return LevelManager.OnPlayerDie(true);
		R.Ui.BlackScene.FadeTransparent(0.3f, false);
		InputSetting.Resume(false);
		yield break;
	}

	[SerializeField]
	private GameObject playerDeadWindow;

	[SerializeField]
	private GameObject _resumeBtn;

	[SerializeField]
	private GameObject _backBtn;

	[SerializeField]
	private UILabel _youAreDie;

	private PlayerAnimEventListener listener;
}
