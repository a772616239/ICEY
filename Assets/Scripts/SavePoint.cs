using System;
using Core;
using UnityEngine;

public class SavePoint : BaseBehaviour
{
	private bool _hasRecovered
	{
		get
		{
			return RoundStorage.Get(LevelManager.SceneName + "HasRecovered", false);
		}
		set
		{
			RoundStorage.Set(LevelManager.SceneName + "HasRecovered", value);
		}
	}

	private bool HasConquer
	{
		get
		{
			return SaveStorage.Get(LevelManager.SceneName + "HasConquer", false);
		}
		set
		{
			SaveStorage.Set(LevelManager.SceneName + "HasConquer", value);
		}
	}

	private void Start()
	{
		this.HasConquer = true;
		R.Ui.Enhancement.OnFinish += this.OnEnhancementFinish;
	}

	private void OnDestroy()
	{
		if (!SingletonMono<UIController>.ApplicationIsQuitting)
		{
			R.Ui.Enhancement.OnFinish -= this.OnEnhancementFinish;
			R.Ui.SaveProgressCircle.Disappear();
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!collision.CompareTag("Player") || this._hasSaved)
		{
			return;
		}
		this._hasSaved = true;
		if (!this._hasRecovered)
		{
			R.Player.Attribute.AllAttributeRecovery();
			this._hasRecovered = true;
		}
		this.Save();
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (!collision.CompareTag("Player"))
		{
			return;
		}
		if (Core.Input.Game.Search.OnClick)
		{
			this._prompt.FadeOut();
			this._animator.SetBool("IsShopping", true);
		}
	}

	private void Save()
	{
		R.GameData.Save(false);
		this._animator.SetTrigger("Save");
	}

	private void OnEnhancementFinish(object sender, EventArgs e)
	{
		this._animator.SetBool("IsShopping", false);
	}

	private bool _hasSaved;

	[SerializeField]
	private Animator _animator;

	[SerializeField]
	private GateEnterPrompt _prompt;
}
