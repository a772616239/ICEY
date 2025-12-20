using System;
using UnityEngine;

public class StoryE9Guide2 : BaseBehaviour
{
	private bool IsE9BossDie
	{
		get
		{
			return RoundStorage.Get("E9_BossDie", false);
		}
	}

	private void Start()
	{
		this.guideLeft.SetActive(!this.IsE9BossDie);
		this.guideRight.SetActive(this.IsE9BossDie);
	}

	[SerializeField]
	private GameObject guideLeft;

	[SerializeField]
	private GameObject guideRight;
}
