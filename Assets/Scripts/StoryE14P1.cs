using System;
using System.Collections;
using I2.Loc;
using UnityEngine;

public class StoryE14P1 : BaseBehaviour
{
	private bool IsFirstTime
	{
		get
		{
			return RoundStorage.Get("E14_p1IsFirstTime", true);
		}
		set
		{
			RoundStorage.Set("E14_p1IsFirstTime", value);
		}
	}

	private void Start()
	{
		if (this.IsFirstTime)
		{
			R.Audio.StopBGM(true);
			base.StartCoroutine(this.P1Coroutine());
		}
		else
		{
			this.musicPlayer.gameObject.SetActive(false);
		}
	}

	private IEnumerator P1Coroutine()
	{
		this.gate.openType = SceneGate.OpenType.None;
		yield return R.Audio.PlayVoiceOver("e14t1", null, false);
		this.musicPlayer.SetLimited(25);
		while (this.musicPlayer.currentHP > 25)
		{
			yield return null;
		}
		this.musicPlayer.ChangeState(MusicPlayerAction.State.ChangeMusic);
		R.Audio.PlayBGM(280, true);
		yield return R.Audio.PlayVoiceOver("e14t2", null, false);
		this.musicPlayer.SetLimited(20);
		while (this.musicPlayer.currentHP > 20)
		{
			yield return null;
		}
		this.musicPlayer.ChangeState(MusicPlayerAction.State.ChangeMusic);
		R.Audio.PlayBGM(289, true);
		yield return R.Audio.PlayVoiceOver("e14t3", null, false);
		this.musicPlayer.SetLimited(15);
		while (this.musicPlayer.currentHP > 15)
		{
			yield return null;
		}
		R.Audio.PlayBGM(472, true);
		for (int i = 0; i < this._timeSpans.Length; i++)
		{
			yield return new WaitForSeconds(this._timeSpans[i]);
			if (R.Settings.SubtitleVisiable)
			{
				string subtitle = ScriptLocalization.Get("Story/e14s2." + i);
				R.Ui.UISubtitle.FadeIn(subtitle);
			}
		}
		if (R.Settings.SubtitleVisiable)
		{
			R.Ui.UISubtitle.FadeOut();
		}
		SingletonMono<WorldTime>.Instance.TimeFrozen(1f, WorldTime.FrozenArgs.FrozenType.Player, false);
		R.Audio.StopBGM(true);
		CameraFilterUtils.Create<CameraFilterPack_VHS_Tracking>(null);
		yield return R.Audio.PlayVoiceOver("e14t4", null, false);
		SingletonMono<WorldTime>.Instance.FrozenResume();
		CameraFilterUtils.Remove<CameraFilterPack_VHS_Tracking>(null);
		yield return R.Audio.PlayVoiceOver("e14t5", null, false);
		this.musicPlayer.SetLimited(10);
		while (this.musicPlayer.currentHP > 10)
		{
			yield return null;
		}
		this.thunder.SetActive(true);
		AudioSource audio476 = R.Audio.PlayEffect(476, new Vector3?(this.musicPlayer.transform.position));
		audio476.loop = true;
		yield return R.Audio.PlayVoiceOver("e14t6", null, false);
		this.musicPlayer.SetLimited(5);
		while (this.musicPlayer.currentHP > 5)
		{
			yield return null;
		}
		UnityEngine.Object.Destroy(audio476.gameObject);
		this.thunder.GetComponent<ParticleSystem>().emissionRate = 10f;
		AudioSource audio477 = R.Audio.PlayEffect(477, new Vector3?(this.musicPlayer.transform.position));
		audio477.loop = true;
		this.musicPlayer.StartShake();
		R.Audio.PlayVoiceOver("e14t7", null, false);
		yield return new WaitForSeconds(1.2f);
		this.musicPlayer.SetLimited(0);
		this.musicPlayer.currentHP = 0;
		UnityEngine.Object.Destroy(audio477.gameObject);
		RoundStorage.Set("E14_P1", true);
		yield return R.Ui.Volume.Show(0f);
		yield return R.Ui.Volume.Hide();
		yield return R.Audio.PlayVoiceOver("e14t8", null, false);
		this.gate.openType = SceneGate.OpenType.Right;
		this.IsFirstTime = false;
		yield break;
	}

	[SerializeField]
	private MusicPlayerAction musicPlayer;

	[SerializeField]
	private SceneGate gate;

	[SerializeField]
	private GameObject thunder;

	private readonly float[] _timeSpans = new float[]
	{
		19.655f,
		4.745f,
		1.118f,
		3.918f,
		3.181f,
		3.263f,
		3.92f,
		5.321f,
		3.879f,
		4.243f,
		5.882f,
		3.358f,
		6.717f,
		7.166f,
		3.53f,
		4.846f,
		5.1f,
		6.399f,
		3.967f,
		6.184f
	};
}
