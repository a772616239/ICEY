using System;
using I2.Loc;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Guideboard : BaseBehaviour
{
	private bool HasShown { get; set; }

	private void Start()
	{
		this._levelName = ScriptLocalization.Get("ui/levelName/" + LevelManager.SceneName);
		if (this._normalSize == 144)
		{
			this._normalSize = 108;
		}
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
		R.Ui.LevelName.Show(this._levelName, this._normalSize, this._duration, true);
		this.HasShown = true;
	}

	private string _levelName;

	[SerializeField]
	private int _normalSize = 108;

	[SerializeField]
	private float _duration = 1f;
}
