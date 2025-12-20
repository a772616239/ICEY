using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using ExtensionMethods;
using UnityEngine;

public class UITrophyNotificationController : MonoBehaviour
{
	public void AwardTrophy(string trophyName, string spriteName)
	{
		this._trophyQueue.Enqueue(new UITrophyNotificationController.Trophy(trophyName, spriteName));
		if (!this._isPlaying)
		{
			this.AwardTrophy();
		}
	}

	private Coroutine AwardTrophy()
	{
		UITrophyNotificationController.Trophy trophy = this._trophyQueue.Dequeue();
		this._trophyName.text = trophy.TrophyName;
		this._trophyIcon.spriteName = trophy.SpriteName;
		this._widget.alpha = 1f;
		this._widget.transform.localPosition = new Vector3(0f, (float)this._widget.height, 0f);
		return base.StartCoroutine(this.AwardTrophyCoroutine());
	}

	private IEnumerator AwardTrophyCoroutine()
	{
		this._isPlaying = true;
		this._panel.gameObject.SetActive(true);
		yield return this._widget.transform.DOLocalMoveY(0f, 0.5f, false).WaitForCompletion();
		yield return new WaitForSeconds(5f);
		yield return this._widget.DOFade(0f, 0.5f).WaitForCompletion();
		this._panel.gameObject.SetActive(false);
		this._isPlaying = false;
		if (this._trophyQueue.Count != 0)
		{
			this.AwardTrophy();
		}
		yield break;
	}

	[SerializeField]
	private UIPanel _panel;

	[SerializeField]
	private UIWidget _widget;

	[SerializeField]
	private UILabel _trophyName;

	[SerializeField]
	private UISprite _trophyIcon;

	private bool _isPlaying;

	private readonly Queue<UITrophyNotificationController.Trophy> _trophyQueue = new Queue<UITrophyNotificationController.Trophy>();

	private class Trophy
	{
		public Trophy(string trophyName, string spriteName)
		{
			this.TrophyName = trophyName;
			this.SpriteName = spriteName;
		}

		public string TrophyName { get; set; }

		public string SpriteName { get; set; }
	}
}
