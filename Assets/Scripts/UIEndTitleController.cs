using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class UIEndTitleController : MonoBehaviour
{
	public YieldInstruction Show(bool type)
	{
		return base.StartCoroutine(this.ShowCoroutine(type));
	}

	private IEnumerator ShowCoroutine(bool type)
	{
		UILabel label = (!type) ? this._falseLabel : this._trueLabel;
		bool uiState = R.Ui.IsHide;
		R.Mode.EnterMode(Mode.AllMode.Story);
		if (!uiState)
		{
			R.Ui.HideUI(true);
		}
		int offset = label.height + UITools.ScreenHeight;
		this._background.gameObject.SetActive(true);
		label.transform.parent.gameObject.SetActive(true);
		yield return DOTween.To(delegate(float a)
		{
			this._background.alpha = a;
		}, 0f, 1f, 1f).WaitForCompletion();
		yield return label.transform.parent.DOLocalMoveY((float)offset, (float)offset / (float)this._speed, false).SetEase(Ease.Linear).WaitForCompletion();
		yield return new WaitForSeconds(1f);
		if (type)
		{
			yield return DOTween.To(delegate(float a)
			{
				label.alpha = a;
			}, 1f, 0f, 5f).WaitForCompletion();
		}
		yield return DOTween.To(delegate(float a)
		{
			this._background.alpha = a;
		}, 1f, 0f, 1f).WaitForCompletion();
		label.transform.parent.gameObject.SetActive(false);
		this._background.gameObject.SetActive(false);
		label.transform.parent.localPosition = new Vector3(0f, (float)(-(float)UITools.ScreenHeight), 0f);
		label.alpha = 1f;
		if (!uiState)
		{
			R.Ui.ShowUI(false);
		}
		R.Mode.ExitMode(Mode.AllMode.Story);
		yield break;
	}

	[SerializeField]
	private UISprite _background;

	[SerializeField]
	private UILabel _falseLabel;

	[SerializeField]
	private UILabel _trueLabel;

	[SerializeField]
	private int _speed;
}
