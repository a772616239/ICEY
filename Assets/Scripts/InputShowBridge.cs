using System;
using Core;
using UnityEngine;

public class InputShowBridge : BaseBehaviour
{
	private bool _hasBridgeOpend
	{
		get
		{
			return RoundStorage.Get("E2_hasBridgeOpend", false);
		}
		set
		{
			RoundStorage.Set("E2_hasBridgeOpend", value);
		}
	}

	private void Start()
	{
		if (this._hasBridgeOpend)
		{
			this._prompt.gameObject.SetActive(false);
		}
	}

	private void Update()
	{
		if (Mathf.Abs(R.Player.Transform.position.x - base.transform.position.x) < 2f && Core.Input.Game.Search.OnClick && !this.activeObj.activeSelf)
		{
			this._hasBridgeOpend = true;
			this.activeObj.SetActive(true);
			this.bridgeCollider.SetActive(true);
			this._e2p9.SetActive(false);
			this._prompt.FadeOut();
			this._prompt.GetComponent<BoxCollider2D>().enabled = false;
		}
	}

	[SerializeField]
	private GameObject activeObj;

	[SerializeField]
	private GateEnterPrompt _prompt;

	[SerializeField]
	private GameObject bridgeCollider;

	[SerializeField]
	private GameObject _e2p9;
}
