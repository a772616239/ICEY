using System;
using UnityEngine;

public class StoryE11P5 : BaseBehaviour
{
	private void Start()
	{
		R.Ui.HideUI(true);
		this._shi.SetActive(true);
		if (!R.Mode.IsInShiMode())
		{
			R.Mode.EnterMode(Mode.AllMode.Shi);
		}
		SingletonMono<MobileInputPlayer>.Instance.EnterSHI();
		R.Trophy.AwardTrophy(14);
		R.Audio.PlayVoiceOver("e11t15", null, false);
		R.Player.GameObject.SetActive(false);
		if (R.Windy)
		{
			R.Windy.SetActive(false);
		}
	}

	private void OnDestroy()
	{
		SingletonMono<MobileInputPlayer>.Instance.ExitSHI();
		R.Player.GameObject.SetActive(true);
		if (R.Windy)
		{
			R.Windy.SetActive(true);
		}
		UnityEngine.Object.Destroy(this._shi);
	}

	[SerializeField]
	private GameObject _shi;
}
