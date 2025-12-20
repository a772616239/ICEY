using System;
using I2.Loc;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class StoryLabel : BaseBehaviour
{
	private bool HasShown
	{
		get
		{
			return RoundStorage.Get("StoryLabel" + LevelManager.SceneName, false);
		}
		set
		{
			RoundStorage.Set("StoryLabel" + LevelManager.SceneName, value);
		}
	}

	private void Start()
	{
		this._text = ScriptLocalization.Get("Story/" + this._storyStringKey);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!collision.CompareTag("Player"))
		{
			return;
		}
		if (this.HasShown)
		{
			return;
		}
		R.Ui.Toast.Show(this._text, this._normalSize, this._duration * 2f + 1f, false);
		this.HasShown = true;
	}

	[SerializeField]
	private string _storyStringKey;

	[SerializeField]
	private int _normalSize = 40;

	[SerializeField]
	private float _duration = 1f;

	private string _text;
}
