using System;
using GameWorld;
using UnityEngine;

public class PlayVoiceAtBattle : BaseBehaviour
{
	private void Start()
	{
		if (this.gate != null)
		{
			this.gate.openType = SceneGate.OpenType.None;
		}
	}

	private void OnEnable()
	{
		EventManager.RegisterEvent<BattleEventArgs>("Battle", new EventManager.FBEventHandler<BattleEventArgs>(this.BattleOver), EventManager.ListenerQueue.Game);
	}

	private void OnDisable()
	{
		EventManager.UnregisterEvent<BattleEventArgs>("Battle", new EventManager.FBEventHandler<BattleEventArgs>(this.BattleOver), EventManager.ListenerQueue.Game);
	}

	private bool BattleOver(string eventName, object sender, BattleEventArgs args)
	{
		if (args.Status == BattleEventArgs.BattleStatus.Begin)
		{
			R.Audio.PlayVoiceOver(this.beginVoice, null, false);
		}
		if (args.Status == BattleEventArgs.BattleStatus.End)
		{
			R.Audio.PlayVoiceOver(this.endVoice, null, false);
		}
		return true;
	}

	public string beginVoice;

	public string endVoice;

	[SerializeField]
	private SceneGate gate;
}
