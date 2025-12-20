using System;
using System.Collections;
using I2.Loc;
using UnityEngine;

public class StoryE11P4 : BaseBehaviour
{
	private void Start()
	{
		base.GetComponent<BoxCollider2D>().isTrigger = true;
		this._videoRenderer.gameObject.SetActive(false);
	}

	private void OnDestroy()
	{
		UnityEngine.Object.Destroy(this._videoRenderer);
		if (this._playVideo != null)
		{
			this._playVideo.Dispose();
		}
	}

	public void OnTriggerEnter2D(Collider2D collision)
	{
		if (this.isFirst && collision.CompareTag("Player"))
		{
			this.isFirst = false;
			base.StartCoroutine(this.Sequence0());
		}
	}

	private IEnumerator Sequence0()
	{
		yield return R.Audio.PlayVoiceOver("e11t7", null, false);
		yield return R.Audio.PlayVoiceOver("e11t8", null, false);
		this._videoRenderer.gameObject.SetActive(true);
		this._playVideo = PlayVideo.CreateVideo(this._videoRenderer);
		Coroutine video = this._playVideo.Play("Movies/ps4", "koi.mp4", false);
		yield return R.Audio.PlayVoiceOver("e11t23", null, false);
		yield return video;
		yield return R.Audio.PlayVoiceOver("e11t9", null, false);
		yield return new WaitForSeconds(R.Audio.PlayEffect(457, null).clip.length);
		yield return R.Audio.PlayVoiceOver("e11t11", null, false);
		UnityEngine.Object.Destroy(this._videoRenderer);
		this._playVideo.Dispose();
		yield return R.Ui.Terminal.OpenWithAnim(new Color?(Color.white));
		yield return R.Ui.Terminal.PrintShellPrompt();
		yield return R.Ui.Terminal.Println(ScriptLocalization.Story.e11s3, 0.1f);
		yield return new WaitForSeconds(1f);
		yield return R.Ui.Terminal.Print(ScriptLocalization.Story.e11s2, 0.1f);
		yield return R.Ui.Terminal.ShowProgressBar(0f);
		yield return R.Ui.Terminal.SetProgressBarValueWithAnim(1f, 3f);
		yield return R.Ui.Terminal.HideProgressBar();
		yield return R.Ui.Terminal.CloseWithAnim();
		this._gate2.Enter(false);
		yield break;
	}

	[SerializeField]
	private SceneGate _gate2;

	[SerializeField]
	private MeshRenderer _videoRenderer;

	private PlayVideo _playVideo;

	private bool isFirst = true;
}
