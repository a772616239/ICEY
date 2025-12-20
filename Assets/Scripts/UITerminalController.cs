using System;
using System.Collections;
using System.Text.RegularExpressions;
using Colorful;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using ExtensionMethods;
using UnityEngine;

public class UITerminalController : MonoBehaviour
{
	private YieldInstruction Open(Color? color)
	{
		R.Mode.EnterMode(Mode.AllMode.UI);
		R.Ui.Pause.Enabled = false;
		this.Clear();
		if (color != null)
		{
			UILabel console = this._console;
			console.text += color.Value.ToBBCode();
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
		return null;
	}

	public YieldInstruction OpenWithoutAnim(Color? color = null)
	{
		this.Open(color);
		R.Camera.Camera.enabled = false;
		return null;
	}

	public YieldInstruction OpenWithAnim(Color? color = null)
	{
		this.Open(color);
		AnalogTV analogTV = R.Ui.CameraGO.GetComponent<AnalogTV>();
		analogTV.Scale = 0f;
		R.Audio.PlayEffect(358, null);
		return DOTween.To(() => analogTV.Scale, delegate(float scale)
		{
			analogTV.Scale = scale;
		}, 1.02f, 0.5f).OnComplete(delegate
		{
			R.Camera.Camera.enabled = false;
		}).SetUpdate(true).WaitForCompletion();
	}

	private YieldInstruction Close()
	{
		CameraFilterUtils.Remove<CameraFilterPack_TV_80>(R.Ui.CameraGO);
		UnityEngine.Object.Destroy(R.Ui.CameraGO.GetComponent<AnalogTV>());
		this._panel.gameObject.SetActive(false);
		R.Ui.Pause.Enabled = true;
		R.Mode.ExitMode(Mode.AllMode.UI);
		return null;
	}

	public YieldInstruction CloseWithoutAnim()
	{
		R.Camera.Camera.enabled = true;
		this.Close();
		return null;
	}

	public YieldInstruction CloseWithAnim()
	{
		AnalogTV analogTV = R.Ui.CameraGO.GetComponent<AnalogTV>();
		R.Audio.PlayEffect(359, null);
		R.Camera.Camera.enabled = true;
		return DOTween.To(() => analogTV.Scale, delegate(float scale)
		{
			analogTV.Scale = scale;
		}, 0f, 0.5f).SetUpdate(true).OnComplete(delegate
		{
			this.CloseWithoutAnim();
		}).WaitForCompletion();
	}

	public Coroutine Print(string text, float interval = 0.1f)
	{
		base.StopCoroutine(this.PrintCoroutine(text, interval, null, true));
		return base.StartCoroutine(this.PrintCoroutine(text, interval, null, true));
	}

	public Coroutine Println(string text = "", float interval = 0.1f)
	{
		base.StopCoroutine(this.PrintCoroutine(text, interval, "\n", true));
		return base.StartCoroutine(this.PrintCoroutine(text, interval, "\n", true));
	}

	public YieldInstruction PrintShellPrompt()
	{
		this.PrintInstantly("$ ~ ");
		return null;
	}

	public YieldInstruction PrintInstantly(string text)
	{
		UILabel console = this._console;
		console.text += text;
		return null;
	}

	public YieldInstruction PrintlnInstantly(string text = "")
	{
		UILabel console = this._console;
		console.text = console.text + text + "\n";
		return null;
	}

	public YieldInstruction Clear()
	{
		this._console.text = string.Empty;
		return null;
	}

	private IEnumerator PrintCoroutine(string text, float interval, string end = null, bool withAudio = true)
	{
		Regex colorBBCode = new Regex("\\[([0-9a-fA-F]{6}|-)\\]");
		for (int i = 0; i < text.Length; i++)
		{
			Match match = colorBBCode.Match(text, i);
			if (match.Index == i)
			{
				UILabel console = this._console;
				console.text += match.Value;
				i += match.Length;
			}
			if (i < text.Length)
			{
				UILabel console2 = this._console;
				console2.text += text[i];
				if (withAudio)
				{
					R.Audio.PlayEffect(347, null);
				}
			}
			yield return WorldTime.WaitForSecondsIgnoreTimeScale(interval / UILanguage.CurrentLanguage.TypeSpeed);
		}
		if (!string.IsNullOrEmpty(end))
		{
			UILabel console3 = this._console;
			console3.text += end;
			if (withAudio)
			{
				R.Audio.PlayEffect(348, null);
			}
		}
		yield break;
	}

	public YieldInstruction ShowProgressBar(float defaultValue = 0f)
	{
		this._progressBar.value = defaultValue;
		return DOTween.To(() => this._progressBar.alpha, delegate(float alpha)
		{
			this._progressBar.alpha = alpha;
		}, 1f, 1f).SetUpdate(true).WaitForCompletion();
	}

	public YieldInstruction HideProgressBar()
	{
		return DOTween.To(() => this._progressBar.alpha, delegate(float alpha)
		{
			this._progressBar.alpha = alpha;
		}, 0f, 1f).SetUpdate(true).OnComplete(delegate
		{
			this._progressBar.value = 0f;
		}).WaitForCompletion();
	}

	public YieldInstruction SetProgressBarValue(float value)
	{
		this._progressBar.value = value;
		return null;
	}

	public YieldInstruction SetProgressBarValueWithAnim(float endValue, float duration)
	{
		return DOTween.To(() => this._progressBar.value, delegate(float value)
		{
			this._progressBar.value = value;
		}, endValue, duration).SetUpdate(true).WaitForCompletion();
	}

	public YieldInstruction ShowTexture(Texture texture, float scale = 1f, float alpha = 1f)
	{
		this._texture.mainTexture = texture;
		this.SetTextureSize(scale);
		this._texture.alpha = 0f;
		return DOTween.To(() => this._texture.alpha, delegate(float a)
		{
			this._texture.alpha = a;
		}, alpha, 1f).SetUpdate(true).WaitForCompletion();
	}

	public YieldInstruction ShowTextureFullScreen(Texture texture)
	{
		this._texture.mainTexture = texture;
		this.SetTextureFullScreen();
		this._texture.alpha = 0f;
		return DOTween.To(() => this._texture.alpha, delegate(float alpha)
		{
			this._texture.alpha = alpha;
		}, 1f, 1f).SetUpdate(true).WaitForCompletion();
	}

	public void SetTextureSize(float scale = 1f)
	{
		Texture mainTexture = this._texture.mainTexture;
		Vector2 a = new Vector2((float)mainTexture.width, (float)mainTexture.height) * R.Ui.CameraGO.GetComponent<Camera>().orthographicSize;
		a *= scale;
		this._texture.SetDimensions(Mathf.RoundToInt(a.x), Mathf.RoundToInt(a.y));
	}

	public void SetTextureFullScreen()
	{
		Vector2 vector = this._panel.GetViewSize() * R.Ui.CameraGO.GetComponent<Camera>().orthographicSize;
		this._texture.SetDimensions(Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.y));
	}

	public YieldInstruction HideTexture()
	{
		return DOTween.To(() => this._texture.alpha, delegate(float alpha)
		{
			this._texture.alpha = alpha;
		}, 0f, 1f).SetUpdate(true).WaitForCompletion();
	}

	public const string ShellPrompt = "$ ~ ";

	[SerializeField]
	private UILabel _console;

	[SerializeField]
	private UIProgressBar _progressBar;

	[SerializeField]
	private UITexture _texture;

	[SerializeField]
	private UIPanel _panel;
}
