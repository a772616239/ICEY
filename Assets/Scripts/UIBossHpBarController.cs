using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public class UIBossHpBarController : MonoBehaviour
{
	private void Awake()
	{
		this.cursorTweenAlpha = this.cursorSprite.GetComponent<TweenAlpha>();
		base.StartCoroutine(this.HPValueHistoryRecorder());
		base.StartCoroutine(this.HPChangeAnim());
	}

	private void Update()
	{
		if (UIBossHpBarController.boss == null)
		{
			return;
		}
		this.BindData();
		if (this.bossData.FullCurrentHp <= 0)
		{
			this.Disappear();
		}
	}

	private IEnumerator HPChangeAnim()
	{
		for (;;)
		{
			if (this.play)
			{
				this.play = false;
				this.cursorTweenAlpha.enabled = true;
				this.cursorSprite.enabled = true;
				float start = this.hp2Bar.value;
				float targe = this.hp1Bar.value;
				for (float x = 0f; x < 1f; x += 0.02f)
				{
					float current = start + (targe - start) * x;
					this.hp2Bar.value = current;
					yield return new WaitForSeconds(0.02f);
				}
				this.cursorTweenAlpha.enabled = false;
				this.cursorSprite.alpha = 0f;
			}
			yield return null;
		}
		yield break;
	}

	private IEnumerator HPValueHistoryRecorder()
	{
		for (;;)
		{
			this.hpValueHistory.Enqueue(this.hp1Bar.value);
			if (this.hpValueHistory.Count > 10)
			{
				this.hpValueHistory.Dequeue();
			}
			yield return new WaitForSeconds(0.1f);
		}
		yield break;
	}

	private void BindData()
	{
		this.bossData.Update();
		this.hp1Bar.value = (float)this.bossData.CurrentAppearHp / Math.Max(1f, (float)this.bossData.MaxHps[this.bossData.CurrentHpBarIndex]);
		this.hp1Bar.value = (float)this.bossData.CurrentAppearHp / (float)Mathf.Max(this.bossData.MaxHps[this.bossData.CurrentHpBarIndex], 1);
		if (this.hpValueHistory.Count >= 10 && (double)Mathf.Abs(this.hp1Bar.value - this.hpValueHistory.Peek()) < 0.0001 && (double)Mathf.Abs(this.hp1Bar.value - this.hpValueWhenLastPlayAnim) > 0.0001)
		{
			this.hpValueWhenLastPlayAnim = this.hp1Bar.value;
			this.play = true;
		}
		if ((double)Math.Abs(this.hp1Bar.value - 1f) < 0.0001)
		{
			this.hp2Bar.value = 1f;
			this.cursorTweenAlpha.enabled = false;
			this.cursorSprite.alpha = 0f;
			this.hpValueWhenLastPlayAnim = 1f;
		}
		int num = this.bossData.HpBarCount - this.bossData.CurrentHpBarIndex;
		this._numLabel.alpha = (float)((num != 1) ? 1 : 0);
		this._numLabel.text = StringTools.Int2String[this.bossData.HpBarCount - this.bossData.CurrentHpBarIndex];
	}

	public void Create(EnemyAttribute enemy, List<int> phaseHp)
	{
		if (!this.Visible)
		{
			UIBossHpBarController.boss = enemy;
			this.bossData = new UIBossHpBarController.BossHpBarData(phaseHp);
			this.FadeTo(1f, 1f, delegate()
			{
				this.Visible = true;
			});
		}
		else
		{
			Log.Error("重复生成Boss血条");
		}
	}

	public void Disappear()
	{
		if (this.Visible)
		{
			this.Visible = false;
			this.FadeTo(0f, 1f);
			UIBossHpBarController.boss = null;
			base.StopCoroutine(this.HPValueHistoryRecorder());
			base.StopCoroutine(this.HPChangeAnim());
		}
		else
		{
			Log.Error("重复隐藏Boss血条");
		}
	}

	public YieldInstruction FadeTo(float endValue, float duration)
	{
		return DOTween.To(() => this._widget.alpha, delegate(float alpha)
		{
			this._widget.alpha = alpha;
		}, endValue, duration).WaitForCompletion();
	}

	public YieldInstruction FadeTo(float endValue, float duration, TweenCallback onComplete)
	{
		return DOTween.To(() => this._widget.alpha, delegate(float alpha)
		{
			this._widget.alpha = alpha;
		}, endValue, duration).OnComplete(onComplete).WaitForCompletion();
	}

	private static EnemyAttribute boss;

	private UIBossHpBarController.BossHpBarData bossData;

	[SerializeField]
	private UIWidget _widget;

	[SerializeField]
	private UILabel _numLabel;

	[SerializeField]
	private UIProgressBar hp1Bar;

	[SerializeField]
	private UIProgressBar hp2Bar;

	[SerializeField]
	private UISprite cursorSprite;

	private TweenAlpha cursorTweenAlpha;

	private bool play;

	public bool Visible;

	private readonly Queue<float> hpValueHistory = new Queue<float>();

	private float hpValueWhenLastPlayAnim = 1f;

	private class BossHpBarData
	{
		public BossHpBarData(List<int> phaseHp)
		{
			this.MaxHps = phaseHp;
			this.HpBarCount = phaseHp.Count;
		}

		public int FullMaxHp
		{
			get
			{
				return UIBossHpBarController.boss.maxHp;
			}
		}

		public int FullCurrentHp
		{
			get
			{
				return UIBossHpBarController.boss.currentHp;
			}
		}

		public void Update()
		{
			int num = 0;
			for (int i = this.HpBarCount - 1; i >= 0; i--)
			{
				num += this.MaxHps[i];
				if (this.FullCurrentHp <= num)
				{
					this.CurrentHpBarIndex = i;
					this.CurrentAppearHp = this.FullCurrentHp - (num - this.MaxHps[i]);
					break;
				}
			}
		}

		public readonly int HpBarCount;

		public readonly List<int> MaxHps;

		public int CurrentHpBarIndex;

		public int CurrentAppearHp;
	}
}
