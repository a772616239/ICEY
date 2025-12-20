using System;
using System.Collections;
using UnityEngine;

public class StoryE7P2 : BaseBehaviour
{
	private bool _e7p5
	{
		get
		{
			return RoundStorage.Get("E7_P5  ", false);
		}
		set
		{
			RoundStorage.Set("E7_P5", value);
		}
	}

	private bool _e7p6
	{
		get
		{
			return RoundStorage.Get("E7_P6", false);
		}
		set
		{
			RoundStorage.Set("E7_P6", value);
		}
	}

	private bool _e7p7
	{
		get
		{
			return RoundStorage.Get("E7_P7", false);
		}
		set
		{
			RoundStorage.Set("E7_P7", value);
		}
	}

	private bool _e7p8
	{
		get
		{
			return RoundStorage.Get("E7_P8", false);
		}
	}

	private bool gateClose
	{
		get
		{
			return RoundStorage.Get("E7_CloseGate", false);
		}
		set
		{
			RoundStorage.Set("E7_CloseGate", value);
		}
	}

	private void Start()
	{
		if (this.gateClose)
		{
			this.gate.openType = SceneGate.OpenType.None;
			this._battleZoneGate.Appear();
		}
		this._e7p5 = true;
		if (this._e7p6)
		{
			this._e7p7 = true;
			base.StartCoroutine(this.Sequence0());
		}
		if (this._e7p8 && !this.gateClose)
		{
			R.Audio.PlayVoiceOver("e7t10", null, false);
			this.gateClose = true;
			this.gate.openType = SceneGate.OpenType.None;
			this._battleZoneGate.Appear();
		}
	}

	private IEnumerator Sequence0()
	{
		yield return R.Audio.PlayVoiceOver("e7t8", null, false);
		this.gateClose = true;
		this.gate.openType = SceneGate.OpenType.None;
		this._battleZoneGate.Appear();
		yield break;
	}

	[SerializeField]
	private SceneGate gate;

	[SerializeField]
	private BattleZoneGate _battleZoneGate;
}
