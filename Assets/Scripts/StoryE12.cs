using System;
using System.Collections;
using Core;
using DG.Tweening;
using I2.Loc;
using UnityEngine;

public class StoryE12 : BaseBehaviour
{
	private bool E12
	{
		get
		{
			return RoundStorage.Get("E12", false);
		}
		set
		{
			RoundStorage.Set("E12", value);
		}
	}

	private void Start()
	{
		if (!this.E12)
		{
			base.StartCoroutine(this.MainCoroutine());
		}
	}

	private void OnDestroy()
	{
		this.E12 = true;
		UnityEngine.Object.Destroy(this._videoRenderer);
		SingletonMono<MobileInputPlayer>.Instance.HideL2R2(true);
		if (this._playVideo != null)
		{
			this._playVideo.Dispose();
		}
	}

	private IEnumerator MainCoroutine()
	{
		R.Audio.PlayBGM(354, true);
		this._phone.Name = ScriptLocalization.Story.e12s1;
		this._phone.InCall();
		SingletonMono<MobileInputPlayer>.Instance.ShowL2R2(ScriptLocalization.ui_phone.accept, ScriptLocalization.ui_phone.decline);
		while (!Core.Input.Game.R2.OnPressed)
		{
			if (Core.Input.Game.L2.OnPressed)
			{
				SingletonMono<MobileInputPlayer>.Instance.HideL2R2(true);
				base.StartCoroutine(this.AnswerThePhone());
				yield break;
			}
			yield return null;
		}
		SingletonMono<MobileInputPlayer>.Instance.HideL2R2(true);
		this.NotAnswerThePhone();
        yield break;
    }

	private void NotAnswerThePhone()
	{
		R.Audio.StopBGM(false);
		R.Audio.PlayEffect(367, new Vector3?(R.Player.Transform.position));
		this._phone.CallEnd(false);
		SingletonMono<MobileInputPlayer>.Instance.MainControllerVisiable = true;
		this.E12 = true;
	}

	private IEnumerator AnswerThePhone()
	{
		R.Audio.StopBGM(false);
		R.Audio.PlayEffect(367, new Vector3?(R.Player.Transform.position));
		SingletonMono<MobileInputPlayer>.Instance.MainControllerVisiable = false;
		this._phone.Recive(false);
		this._phone.Name = ScriptLocalization.Story.e12s2;
		this.gate1.openType = SceneGate.OpenType.None;
		this.gate2.openType = SceneGate.OpenType.None;
		yield return R.Audio.PlayVoiceOver("e12t1", null, false);
		this._phone.CallEnd(false);
		SingletonMono<MobileInputPlayer>.Instance.MainControllerVisiable = true;
		AudioSource audioEffect = R.Audio.PlayEffect(366, new Vector3?(R.Player.Transform.position));
		yield return new WaitForSeconds(audioEffect.clip.length);
		R.Audio.PlayEffect(355, new Vector3?(R.Player.Transform.position));
		CameraFilterUtils.Create<CameraFilterPack_TV_PlanetMars>(null);
		this.text.gameObject.SetActive(true);
		this.text.text = ScriptLocalization.Story.e12s3;
		DOTween.To(() => this.text.color, delegate(Color color)
		{
			this.text.color = color;
		}, Color.white, 0.3f);
		yield return new WaitForSeconds(3.5f);
		this.text.gameObject.SetActive(false);
		CameraFilterUtils.Remove<CameraFilterPack_TV_PlanetMars>(null);
		yield return R.Audio.PlayVoiceOver("e12t2", null, false);
		R.Audio.PlayEffect(379, new Vector3?(R.Player.Transform.position));
		yield return new WaitForSeconds(1.45f);
		R.Camera.Controller.CameraShake(0.5f, 0.2f, CameraController.ShakeTypeEnum.Rect, false);
		R.Audio.PlayEffect(377, new Vector3?(R.Player.Transform.position));
		yield return new WaitForSeconds(5f);
		yield return R.Audio.PlayVoiceOver("e12t3", null, false);
		R.Audio.PlayEffect(378, new Vector3?(R.Player.Transform.position));
		yield return new WaitForSeconds(3f);
		for (int i = 0; i < 13; i++)
		{
			R.Audio.PlayEffect(367, null);
			yield return new WaitForSeconds(0.15f);
		}
		this._phone.Name = ScriptLocalization.Story.e12s5;
		SingletonMono<MobileInputPlayer>.Instance.MainControllerVisiable = false;
		this._phone.DialOut();
		yield return new WaitForSeconds(3f);
		this._phone.Name = ScriptLocalization.Story.e12s6;
		this._phone.Recive(true);
		yield return R.Audio.PlayVoiceOver("e12t4", null, false);
		this._phone.CallEnd(true);
		SingletonMono<MobileInputPlayer>.Instance.MainControllerVisiable = true;
		R.Audio.PlayEffect(366, new Vector3?(R.Player.Transform.position));
		yield return R.Audio.PlayVoiceOver("e12t5", null, false);
		R.Audio.PlayEffect(363, new Vector3?(R.Player.Transform.position));
		yield return R.Ui.uiNotifacation.Show(ScriptLocalization.Story.e12s7, 2f);
		yield return R.Audio.PlayVoiceOver("e12t6", null, false);
		yield return R.Ui.Terminal.OpenWithAnim(new Color?(Color.white));
		yield return R.Ui.Terminal.Print(ScriptLocalization.Story.e12s8, 0.1f);
		yield return R.Ui.Terminal.Println("......", 0.5f);
		yield return R.Ui.Terminal.Println(ScriptLocalization.Story.e12s9 + DateTime.Now, 0.1f);
		yield return R.Ui.Terminal.Println(ScriptLocalization.Story.e12s10, 0.1f);
		yield return new WaitForSeconds(2f);
		yield return R.Ui.Terminal.ShowTexture(this.texture_Girls, 0.8f, 1f);
		YieldInstruction t8 = R.Audio.PlayVoiceOver("e12t7", null, false);
		yield return new WaitForSeconds(5f);
		yield return t8;
		yield return R.Ui.Terminal.HideTexture();
		yield return new WaitForSeconds(2f);
		yield return R.Ui.Terminal.ShowTexture(this.texture_MediaPlayer, 0.8f, 1f);
		yield return R.Audio.PlayVoiceOver("e12t8", null, false);
		yield return R.Ui.Terminal.HideTexture();
		yield return R.Ui.Terminal.CloseWithAnim();
		yield return R.Audio.PlayVoiceOver("e12t9", null, false);
		YieldInstruction t9 = R.Audio.PlayVoiceOver("e12t10", null, false);
		yield return R.Ui.Terminal.OpenWithAnim(null);
		yield return R.Ui.Terminal.Println(ScriptLocalization.Story.e12s11, 0.1f);
		yield return t9;
		R.Audio.PlayVoiceOver("e12t11", null, false);
		yield return R.Ui.Terminal.ShowProgressBar(0f);
		yield return R.Ui.Terminal.SetProgressBarValueWithAnim(1f, 15f);
		this.screenInScene.SetActive(true);
		yield return R.Audio.PlayVoiceOver("e12t12", null, false);
		yield return R.Ui.Terminal.HideProgressBar();
		yield return R.Ui.Terminal.CloseWithAnim();
		yield return new WaitForSeconds(1f);
		yield return R.Audio.PlayVoiceOver("e12t13", null, false);
		yield return R.Ui.Terminal.OpenWithAnim(new Color?(Color.white));
		yield return R.Ui.Terminal.Println(ScriptLocalization.Story.e12s12, 0.1f);
		yield return R.Ui.Terminal.ShowProgressBar(0f);
		if (UIAudioLanguage.IsChinese)
		{
			R.Audio.PlayEffect(474, null);
		}
		else
		{
			R.Audio.PlayEffect(482, null);
		}
		yield return R.Ui.Terminal.SetProgressBarValueWithAnim(1f, 12f);
		yield return R.Ui.Terminal.HideProgressBar();
		yield return R.Ui.Terminal.CloseWithoutAnim();
		this._playVideo = PlayVideo.CreateVideo(this._videoRenderer);
		this._videoRenderer.gameObject.SetActive(true);
		Coroutine video = this._playVideo.Play("Movies/ps4", string.Format("{0}.mp4", (!UILanguage.IsJapanese) ? "atf" : "atf_jp"), false);
		yield return R.Audio.PlayVoiceOver("e12t15", null, false);
		yield return video;
		UnityEngine.Object.Destroy(this._videoRenderer.gameObject);
		this._playVideo.Dispose();
		R.Ui.BlackScene.Alpha = 1f;
		R.Trophy.AwardTrophy(15);
		yield return new WaitForSeconds(3f);
		yield return R.Audio.PlayVoiceOver("e12t14", null, false);
		R.Ui.BlackScene.FadeTransparent(0f, false);
		yield return R.Ui.Terminal.OpenWithAnim(new Color?(Color.red));
		yield return R.Ui.Terminal.Println(ScriptLocalization.Story.e12s13, 0.1f);
		yield return new WaitForSeconds(2f);
		R.Ui.Terminal.CloseWithoutAnim();
		yield return R.Ui.Terminal.OpenWithAnim(null);
		yield return R.Ui.Terminal.Println(ScriptLocalization.Story.e12s14, 0.1f);
		yield return new WaitForSeconds(3f);
		yield return R.Ui.Terminal.CloseWithAnim();
		this.E12 = true;
		yield return R.Ui.LevelSelect.OpenWithAnim(true, true);
		yield break;
	}

	[SerializeField]
	private SceneGate gate1;

	[SerializeField]
	private SceneGate gate2;

	[SerializeField]
	private GameObject screenInScene;

	[SerializeField]
	private Renderer _videoRenderer;

	[SerializeField]
	private Texture texture_Girls;

	[SerializeField]
	private Texture texture_MediaPlayer;

	[SerializeField]
	private TextMesh text;

	[SerializeField]
	private PhoneController _phone;

	private PlayVideo _playVideo;
}
