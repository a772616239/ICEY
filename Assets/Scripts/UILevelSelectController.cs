using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Colorful;
using Core;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using I2.Loc;
using UnityEngine;

public class UILevelSelectController : MonoBehaviour
{
	//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event EventHandler StartButtonClickEvent;

	private bool TrueEndEnable
	{
		get
		{
			string[] source = new string[]
			{
				"E3",
				"E7",
				"E10",
				"E13",
				"E14",
				"E15",
				"E16"
			};
			bool result;
			if (!SaveStorage.Get("E21_Finish", false))
			{
				result = source.All((string eventId) => SaveStorage.Get(eventId + "_Finish", false));
			}
			else
			{
				result = false;
			}
			return result;
		}
	}

	private void Awake()
	{
		EventDelegate.Add(this._levelButtons.Last<Transform>().GetComponent<UIButton>().onClick, delegate()
		{
			UIKeyInput.SaveHoveredObject();
			this._layers[0].SetActive(false);
			this._layers.Last<GameObject>().SetActive(true);
		});
		for (int i = 0; i < this._menuButtons.Count; i++)
		{
			EventDelegate.Add(this._menuButtons[i].GetComponent<UIToggle>().onChange, new EventDelegate.Callback(this.OnMenuClick));
		}
		for (int j = 0; j < this.LevelBeginningSceneNames.Length; j++)
		{
			EventDelegate.Add(this._levelButtons[j].GetComponent<UIToggle>().onChange, new EventDelegate.Callback(this.OnLevelButtonClick));
		}
	}

	private void OnMenuClick()
	{
		if (UIToggle.current.value)
		{
			int num = this._menuButtons.IndexOf(UIToggle.current.gameObject);
			if (num != -1)
			{
				for (int i = 0; i < this._layers.Count; i++)
				{
					this._layers[i].SetActive(i == num);
				}
			}
		}
	}

	private void OnLevelButtonClick()
	{
		if (UIToggle.current.value)
		{
			this._currentSelectedLevel = this.LevelBeginningSceneNames[this._levelButtons.IndexOf(UIToggle.current.transform)];
		}
	}

	public void OnConfirmClick()
	{
		if (!Core.Input.JoystickIsOpen)
		{
			return;
		}
		if (this.StartButtonClickEvent != null)
		{
			this.StartButtonClickEvent(this._menuButtons[0], EventArgs.Empty);
		}
		if (!string.IsNullOrEmpty(this._currentSelectedLevel))
		{
			this.LoadLevel(this._currentSelectedLevel);
		}
	}

	public Coroutine LoadLevel(string sceneName)
	{
		return base.StartCoroutine(this.LoadLevelCoroutine(sceneName));
	}

	private IEnumerator LoadLevelCoroutine(string sceneName)
	{
		InputSetting.Stop(false);
		R.Ui.ShowUI(false);
		R.Ui.BlackScene.FadeBlack(0.3f, false);
		yield return R.Ui.LevelSelect.CloseWithAnim();
		R.RoundReset();
		LevelManager.LoadLevelByGateId(sceneName, SceneGate.OpenType.None);
		InputSetting.Resume(false);
		yield break;
	}

	private void Open()
	{
		base.enabled = true;
		this._menuButtons[this._menuButtons.Count - 1].transform.parent.gameObject.SetActive(this.TrueEndEnable);
		this._levelButtons[this._levelButtons.Count - 1].parent.gameObject.SetActive(this.TrueEndEnable);
		for (int i = 0; i < this.LevelBeginningSceneNames.Length; i++)
		{
			this._levelButtons[i].transform.parent.gameObject.SetActive(SaveStorage.Get(this.LevelBeginningSceneNames[i] + "HasConquer", false));
		}
		R.Mode.EnterMode(Mode.AllMode.UI);
		R.Ui.Pause.Enabled = false;
		this._layers[0].SetActive(true);
		for (int j = 1; j < this._layers.Count; j++)
		{
			this._layers[j].SetActive(false);
		}
		this._panel.gameObject.SetActive(true);
		CameraFilterUtils.Create<CameraFilterPack_TV_80>(R.Ui.CameraGO);
		AnalogTV analogTV = R.Ui.CameraGO.AddComponent<AnalogTV>();
		analogTV.Shader = Shader.Find("Hidden/Colorful/Analog TV");
		analogTV.NoiseIntensity = 1f;
		analogTV.ScanlinesIntensity = 0f;
		analogTV.ScanlinesCount = 696;
		analogTV.Distortion = 0.18f;
		analogTV.CubicDistortion = 0f;
		analogTV.Scale = 1.02f;
		this._levelGrid.Reposition();
	}

	public YieldInstruction OpenWithAnim(bool enterEmptyScene = true, bool withProgressBar = true)
	{
		return base.StartCoroutine(this.OpenWithAnimCoroutine(enterEmptyScene, withProgressBar));
	}

	private IEnumerator OpenWithAnimCoroutine(bool enterEmptyScene, bool withProgressBar)
	{
		if (withProgressBar)
		{
			R.SceneData.CanAIRun = false;
			yield return R.Ui.Terminal.OpenWithAnim(null);
			yield return R.Ui.Terminal.ShowProgressBar(0f);
			yield return R.Ui.Terminal.SetProgressBarValueWithAnim(1f, 2f);
			yield return R.Ui.Terminal.HideProgressBar();
			yield return R.Ui.Terminal.CloseWithAnim();
		}
		R.Audio.PlayBGM(433, true);
		if (enterEmptyScene)
		{
			yield return LevelManager.LoadLevelByGateId("empty", SceneGate.OpenType.None);
		}
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
		this._panel.gameObject.SetActive(false);
		R.Ui.Pause.Enabled = true;
		R.Mode.ExitMode(Mode.AllMode.UI);
		base.enabled = false;
	}

	public YieldInstruction CloseWithAnim()
	{
		AnalogTV analogTV = R.Ui.CameraGO.GetComponent<AnalogTV>();
		return DOTween.To(() => analogTV.Scale, delegate(float scale)
		{
			analogTV.Scale = scale;
		}, 0f, 0.5f).SetUpdate(true).OnComplete(delegate
		{
			this.Close();
			R.Audio.StopBGM(true);
		}).WaitForCompletion();
	}

	[ContextMenu("CreateToggles")]
	private void CreateToggles()
	{
		for (int i = 0; i < this.LevelBeginningSceneNames.Length; i++)
		{
			Transform transform;
			if (i != 0)
			{
				transform = UnityEngine.Object.Instantiate<Transform>(this._levelGrid.GetChild(0));
				transform.parent = this._levelGrid.transform;
				transform.localScale = Vector3.one;
			}
			else
			{
				transform = this._levelGrid.GetChild(0);
			}
			transform.name = this.LevelBeginningSceneNames[i];
			transform.GetComponent<Localize>().Term = "ui/levelName/" + this.LevelBeginningSceneNames[i];
			this._levelButtons[i] = this._levelGrid.GetChild(i).GetChild(0);
		}
		this._levelGrid.Reposition();
	}

	[SerializeField]
	private UIPanel _panel;

	[SerializeField]
	private List<GameObject> _menuButtons;

	[SerializeField]
	private List<GameObject> _layers;

	[SerializeField]
	private List<Transform> _levelButtons;

	[SerializeField]
	private UIGrid _levelGrid;

	private string _currentSelectedLevel;

	public readonly string[] LevelBeginningSceneNames = new string[]
	{
		"C1L1S1",
		"C1L2S1",
		"C1L3S1",
		"C2L1S2",
		"C2L2S1",
		"C3L1S1",
		"C3L1S4",
		"C4L1S4",
		"C4L2S1",
		"C5L1S1"
	};
}
