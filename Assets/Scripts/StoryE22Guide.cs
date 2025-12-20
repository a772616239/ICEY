using System;
using GameWorld;
using UnityEngine;

public class StoryE22Guide : BaseBehaviour
{
	private bool E22Email
	{
		get
		{
			return RoundStorage.Get("E22_Email", false);
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

	private void Start()
	{
		if (this.E22Email)
		{
			this.guideDown.SetActive(false);
			this.guideRight.SetActive(false);
		}
		this.guideUp.SetActive(this.E22Email);
		this.guideTopRight.SetActive(this.E22Email);
	}

	private bool BattleOver(string eventName, object sender, BattleEventArgs args)
	{
		if (args.Status == BattleEventArgs.BattleStatus.End)
		{
			this.guideRight.SetActive(true);
		}
		return true;
	}

	[SerializeField]
	private GameObject guideDown;

	[SerializeField]
	private GameObject guideUp;

	[SerializeField]
	private GameObject guideRight;

	[SerializeField]
	private GameObject guideTopRight;
}
