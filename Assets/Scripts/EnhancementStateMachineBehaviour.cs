using System;
using UnityEngine;

public class EnhancementStateMachineBehaviour : StateMachineBehaviour
{
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		InputSetting.Stop(false);
	}

	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		InputSetting.Resume(false);
		R.Ui.Enhancement.OpenWithAnim();
	}
}
