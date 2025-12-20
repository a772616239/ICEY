using System;
using System.Collections;
using GameWorld;
using I2.Loc;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class StoryE24P1 : BaseBehaviour
{
	private void OnEnable()
	{
		EventManager.RegisterEvent<BattleEventArgs>("Battle", new EventManager.FBEventHandler<BattleEventArgs>(this.OnBattle), EventManager.ListenerQueue.Game);
	}

	private void OnDisable()
	{
		EventManager.UnregisterEvent<BattleEventArgs>("Battle", new EventManager.FBEventHandler<BattleEventArgs>(this.OnBattle), EventManager.ListenerQueue.Game);
	}

	public void OnTriggerEnter2D(Collider2D collision)
	{
		if (this._hasTrigger || !collision.CompareTag("Player"))
		{
			return;
		}
		this._hasTrigger = true;
		base.StartCoroutine(this.Sequence0());
	}

	private bool OnBattle(string eventName, object sender, BattleEventArgs battleEventArgs)
	{
		BattleEventArgs.BattleStatus status = battleEventArgs.Status;
		if (status != BattleEventArgs.BattleStatus.Begin)
		{
			if (status != BattleEventArgs.BattleStatus.End)
			{
				throw new ArgumentOutOfRangeException();
			}
			this._collider.enabled = true;
			this._guideBoard.SetActive(true);
		}
		else
		{
			this._gate1.openType = SceneGate.OpenType.None;
		}
		return true;
	}

	private IEnumerator Sequence0()
	{
		yield return R.Audio.PlayVoiceOverArray(new string[]
		{
			"e24t1",
			"e24t2"
		}, 0f, null, 0f);
		yield return new WaitForSeconds(2f);
		yield return R.Audio.PlayVoiceOver("e24t3", null, false);
		yield return R.Ui.Terminal.OpenWithAnim(null);
		yield return R.Ui.Terminal.Println(ScriptLocalization.Story.e24s1, 0.1f);
		yield return R.Ui.Terminal.ShowProgressBar(0f);
		yield return R.Ui.Terminal.SetProgressBarValueWithAnim(1f, 3f);
		yield return new WaitForSeconds(1f);
		yield return R.Ui.Terminal.HideProgressBar();
		yield return R.Ui.Terminal.CloseWithAnim();
		LevelManager.LoadLevelByGateId("C2L2S1", SceneGate.OpenType.Left);
		yield break;
	}

	private bool _hasTrigger;

	[SerializeField]
	private SceneGate _gate1;

	[SerializeField]
	private GameObject _guideBoard;

	[SerializeField]
	private Collider2D _collider;
}
