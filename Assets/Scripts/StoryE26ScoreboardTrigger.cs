using System;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class StoryE26ScoreboardTrigger : BaseBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!collision.CompareTag("Player"))
		{
			return;
		}
		R.Ui.BloodPalace.OpenScoreboard(true);
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (!collision.CompareTag("Player"))
		{
			return;
		}
		R.Ui.BloodPalace.CloseScoreboard(true);
	}

	private void OnDisable()
	{
		R.Ui.BloodPalace.CloseScoreboard(false);
	}
}
