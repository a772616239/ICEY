using System;
using DG.Tweening;
using UnityEngine;

public class UIHitsGradeController : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		if (this.hitNum < 40)
		{
			this._demageText.alpha = 0f;
		}
		else
		{
			this._demageText.alpha = 1f;
			if (this.hitNum < 80)
			{
				this._infoLabel.text = "+10%";
			}
			else if (this.hitNum < 120)
			{
				this._infoLabel.text = "+20%";
			}
			else
			{
				this._infoLabel.text = "+50%";
			}
		}
	}

	public void BeHited()
	{
		if (!this.hitsNumLabel.isActiveAndEnabled && !this.hitBar.isActiveAndEnabled && this.hitNum == 1)
		{
			this.hitsNumLabel.gameObject.SetActive(true);
			this.hitBar.gameObject.SetActive(true);
		}
		this.hitNum++;
		this.hitsNumLabel.text = this.hitNum.ToString();
		if (!DOTween.IsTweening(this.hitsNumLabel.transform, false))
		{
			this.hitsNumLabel.transform.DOShakePosition(0.2f, (float)this.shakeRange * Vector3.one, 100, 90f, false, true).OnStart(delegate
			{
				this.hitsNumLabel.color = Color.yellow;
			}).OnComplete(delegate
			{
				this.hitsNumLabel.color = Color.white;
			});
		}
	}

	public void HideHitNumAndBar()
	{
		this.hitNum = 0;
		this.hitsNumLabel.gameObject.SetActive(false);
		this.hitBar.gameObject.SetActive(false);
	}

	public float Hitbar
	{
		get
		{
			return this.hitBar.value;
		}
		set
		{
			this.hitBar.value = Mathf.Clamp01(value);
		}
	}

	[SerializeField]
	private UILabel hitsNumLabel;

	[SerializeField]
	private UIProgressBar hitBar;

	[SerializeField]
	private UILabel _demageText;

	[SerializeField]
	private UILabel _infoLabel;

	[SerializeField]
	[Range(0f, 100f)]
	private int shakeRange;

	private int hitNum;
}
