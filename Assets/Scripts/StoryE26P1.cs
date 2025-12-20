using System;
using System.Collections;
using I2.Loc;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class StoryE26P1 : BaseBehaviour
{
	private bool e26p1
	{
		get
		{
			return this._isUCEY && !SaveStorage.Get("E26_Finish", false);
		}
	}

	private bool _isUCEY
	{
		get
		{
			return R.GameData.RoleName == "UCEY" || R.GameData.RoleName == "佑希";
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!collision.CompareTag("Player"))
		{
			return;
		}
		R.Ui.BloodPalace.OpenScoreboard(true);
		if (this.e26p1 && !this._p1Running)
		{
			this._doorAnimator.enabled = false;
			this._211Gate.openType = SceneGate.OpenType.None;
			this._p1Running = true;
			base.StartCoroutine(this.P1Coroutine());
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (!collision.CompareTag("Player"))
		{
			return;
		}
		if (this.e26p1)
		{
			return;
		}
		R.Ui.BloodPalace.CloseScoreboard(true);
	}

	private IEnumerator P1Coroutine()
	{
		yield return new WaitForSeconds(5f);
		R.Ui.BloodPalace.CloseScoreboard(true);
		yield return R.Audio.PlayVoiceOver("e26t1", null, false);
		yield return R.Audio.PlayVoiceOver("e26t2", null, false);
		yield return R.Audio.PlayVoiceOver("e26t3", null, false);
		yield return R.Ui.Terminal.OpenWithAnim(null);
		yield return new WaitForSeconds(1f);
		yield return R.Ui.Terminal.PrintlnInstantly(ScriptLocalization.Story.e26s1);
		yield return new WaitForSeconds(1f);
		yield return R.Ui.Terminal.Println(ScriptLocalization.Story.e26s2, 0.1f);
		yield return new WaitForSeconds(1f);
		yield return R.Ui.Terminal.Println(ScriptLocalization.Story.e26s3, 0.1f);
		yield return new WaitForSeconds(1f);
		yield return R.Ui.Terminal.Println(ScriptLocalization.Story.e26s4, 0.1f);
		yield return new WaitForSeconds(1f);
		yield return R.Ui.Terminal.Println(ScriptLocalization.Story.e26s5, 0.1f);
		yield return new WaitForSeconds(1f);
		yield return R.Ui.Terminal.Println(ScriptLocalization.Story.e26s6, 0.1f);
		yield return new WaitForSeconds(1f);
		yield return R.Ui.Terminal.Println(ScriptLocalization.Story.e26s7, 0.1f);
		yield return new WaitForSeconds(1f);
		yield return R.Ui.Terminal.Println(ScriptLocalization.Story.e26s8, 0.1f);
		yield return new WaitForSeconds(1f);
		yield return R.Ui.Terminal.PrintlnInstantly(ScriptLocalization.Story.e21s24);
		yield return new WaitForSeconds(1f);
		yield return R.Ui.Terminal.PrintlnInstantly(ScriptLocalization.Story.e21s22);
		yield return new WaitForSeconds(2f);
		yield return R.Ui.Terminal.CloseWithAnim();
		R.Ui.BloodPalace.CloseScoreboard(false);
		this._palace7Gate.Enter(false);
		yield break;
	}

	private bool _p1Running;

	[SerializeField]
	private SceneGate _palace7Gate;

	[SerializeField]
	private SceneGate _211Gate;

	[SerializeField]
	private Animator _doorAnimator;
}
