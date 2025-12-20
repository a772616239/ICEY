using System;
using System.Collections;
using DG.Tweening;
using I2.Loc;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class StoryE8P2 : BaseBehaviour
{
	private void Start()
	{
		base.GetComponent<BoxCollider2D>().isTrigger = true;
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
		this._textMesh.text = ScriptLocalization.Story.e8s2;
		this._textMesh.gameObject.SetActive(true);
		yield return R.Audio.PlayVoiceOver("e8t4", null, false);
		yield return R.Audio.PlayVoiceOver("e8t5", null, false);
		this._textMesh.gameObject.SetActive(false);
		this._background.gameObject.SetActive(true);
		this._background.color = Color.clear;
		this._background.DOColor(Color.white, 1f);
		yield return R.Audio.PlayVoiceOver("e8t6", null, false);
		this._background.DOColor(Color.clear, 1f);
		this._background.gameObject.SetActive(false);
		this._videoRenderer.gameObject.SetActive(true);
		this._playVideo = PlayVideo.CreateVideo(this._videoRenderer);
		Coroutine coroutine = this._playVideo.Play("Movies/ps4/E8", "video0.mp4", false);
		yield return R.Audio.PlayVoiceOver("e8t7", null, false);
		yield return coroutine;
		this._videoRenderer.gameObject.SetActive(false);
		this._playVideo.Dispose();
		this._cross.gameObject.SetActive(true);
		R.Audio.PlayEffect(145, null);
		yield return R.Audio.PlayVoiceOver("e8t8", null, false);
		this._cross.gameObject.SetActive(false);
		this._animation.gameObject.SetActive(true);
		this._animation.Play();
		yield return new WaitForSeconds(this._animation.clip.length);
		this._animation.gameObject.SetActive(false);
		this._videoRenderer.gameObject.SetActive(true);
		this._playVideo = PlayVideo.CreateVideo(this._videoRenderer);
		coroutine = this._playVideo.Play("Movies/ps4/E8", "video1.mp4", false);
		yield return R.Audio.PlayVoiceOver("e8t9", null, false);
		yield return coroutine;
		this._videoRenderer.gameObject.SetActive(false);
		this._playVideo.Dispose();
		this._cross.gameObject.SetActive(true);
		R.Audio.PlayEffect(145, null);
		yield return R.Audio.PlayVoiceOver("e8t10", null, false);
		this._cross.gameObject.SetActive(false);
		this._animation.gameObject.SetActive(true);
		this._animation.Play();
		yield return new WaitForSeconds(this._animation.clip.length);
		this._animation.gameObject.SetActive(false);
		this._textMesh.text = ScriptLocalization.Story.e8s3;
		this._textMesh.gameObject.SetActive(true);
		yield return R.Audio.PlayVoiceOver("e8t11", null, false);
		this._textMesh.gameObject.SetActive(false);
		this._fatMan.gameObject.SetActive(true);
		this._thinMan.gameObject.SetActive(true);
		this._thinMan.color = Color.clear;
		this._fatMan.DOColor(Color.clear, 5f);
		this._thinMan.DOColor(Color.white, 5f);
		yield return R.Audio.PlayVoiceOver("e8t12", null, false);
		this._fatMan.gameObject.SetActive(false);
		this._thinMan.gameObject.SetActive(false);
		for (int i = 0; i < this._Stars.Length; i++)
		{
			this._Stars[i].gameObject.SetActive(true);
			this._Stars[i].color = Color.clear;
			R.Audio.PlayEffect(459, null);
			yield return this._Stars[i].DOColor(Color.white, 0.5f).WaitForCompletion();
		}
		yield return R.Audio.PlayVoiceOver("e8t13", null, false);
		for (int j = 0; j < this._Stars.Length; j++)
		{
			this._Stars[j].DOColor(Color.clear, 0.5f);
		}
		yield return new WaitForSeconds(0.5f);
		this._ubisoftIcon.SetActive(true);
		yield return R.Audio.PlayVoiceOver("e8t14", null, false);
		yield return R.Audio.PlayVoiceOver("e8t15", null, false);
		this._ubisoftIcon.SetActive(false);
		R.Ui.BlackScene.FadeBlack(0f, false);
		yield return R.Audio.PlayVoiceOver("e8t16", null, false);
		yield return new WaitForSeconds(5f);
		R.Ui.BlackScene.FadeTransparent(0.3f, false);
		yield return R.Ui.Terminal.OpenWithAnim(null);
		yield return R.Ui.Terminal.Println(ScriptLocalization.Story.e8s4, 0.1f);
		R.Trophy.AwardTrophy(10);
		yield return new WaitForSeconds(3f);
		yield return R.Ui.Terminal.CloseWithAnim();
		R.Ui.ShowUI(false);
		R.Ui.LevelSelect.OpenWithAnim(true, true);
		yield break;
	}

	[SerializeField]
	private Renderer _videoRenderer;

	[SerializeField]
	private TextMesh _textMesh;

	[SerializeField]
	private SpriteRenderer _background;

	[SerializeField]
	private TextMesh _cross;

	[SerializeField]
	private Animation _animation;

	[SerializeField]
	private SpriteRenderer _fatMan;

	[SerializeField]
	private SpriteRenderer _thinMan;

	[SerializeField]
	private SpriteRenderer[] _Stars;

	[SerializeField]
	private GameObject _ubisoftIcon;

	private PlayVideo _playVideo;

	private bool isFirst = true;
}
