using System;
using System.Collections;
using DG.Tweening;
using ExtensionMethods;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class BossFocusAnim : BaseBehaviour
{
	private void Awake()
	{
		this._boxCollider2D = base.GetComponent<BoxCollider2D>();
		this._loopForTest = false;
		this._effect = UnityEngine.Object.Instantiate<GameObject>(this._effect);
		this._effect.SetActive(false);
	}

	private void OnTriggerEnter2D(Collider2D collider)
	{
		if (!collider.CompareTag("Player") || !this._isFirstTime)
		{
			return;
		}
		base.StartCoroutine(this.DoFocusCoroutine());
	}

	private IEnumerator DoFocusCoroutine()
	{
		this._isFirstTime = false;
		while (!R.Enemy.Boss)
		{
			yield return null;
		}
		GameObject boss = R.Enemy.Boss;
		R.Mode.EnterMode(Mode.AllMode.Story);
		R.SceneData.CanAIRun = false;
		while (!R.Ui.bossHpBar.Visible)
		{
			yield return null;
		}
		R.Ui.HideUI(false);
		R.Ui.bossHpBar.FadeTo(0f, 0.5f);
		R.Audio.PlayEffect(this._audioEffectId, null);
		do
		{
			R.Camera.Controller.IsFollowPivot = false;
			CameraFilterPack_Colors_Brightness brightness = CameraFilterUtils.Create<CameraFilterPack_Colors_Brightness>(null);
			brightness._Brightness = 1f;
			CameraFilterPack_Colors_Brightness.ChangeBrightness = 1f;
			this.FadeBrightness(this._blackMaskAlpha, this._focusTime);
			yield return R.Camera.Controller.CameraMoveTo(boss.GetComponent<EnemyBaseHurt>().center.position, this._focusTime, this._focusEase);
			this._effect.SetActive(true);
			this._effect.transform.position = UICamera.mainCamera.ViewportToWorldPoint(Vector3.one * 0.5f);
			Animation animation = this._effect.GetComponent<Animation>();
			while (animation.isPlaying)
			{
				yield return null;
			}
			UnityEngine.Object.Destroy(this._effect);
			this.FadeBrightness(1f, this._backTime);
			yield return R.Camera.Controller.CameraMoveTo(R.Player.Transform.position.AddY(2.4f), this._backTime, this._backEase);
			yield return new WaitForSeconds(0.1f);
			CameraFilterUtils.Remove<CameraFilterPack_Colors_Brightness>(null);
			R.Camera.Controller.IsFollowPivot = true;
		}
		while (this._loopForTest);
		R.Ui.ShowUI(false);
		R.Ui.bossHpBar.FadeTo(1f, 0.5f);
		R.SceneData.CanAIRun = true;
		R.Mode.ExitMode(Mode.AllMode.Story);
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	private void FadeBrightness(float endValue, float duration)
	{
		DOTween.To(() => CameraFilterPack_Colors_Brightness.ChangeBrightness, delegate(float a)
		{
			CameraFilterPack_Colors_Brightness.ChangeBrightness = a;
		}, endValue, duration);
	}

	[SerializeField]
	private bool _loopForTest;

	[SerializeField]
	private float _focusTime = 2f;

	[SerializeField]
	private float _backTime = 2f;

	[SerializeField]
	private Ease _focusEase;

	[SerializeField]
	private Ease _backEase;

	[SerializeField]
	private float _blackMaskAlpha = 0.8f;

	[SerializeField]
	private GameObject _effect;

	[SerializeField]
	private int _audioEffectId;

	private bool _isFirstTime = true;

	private BoxCollider2D _boxCollider2D;
}
