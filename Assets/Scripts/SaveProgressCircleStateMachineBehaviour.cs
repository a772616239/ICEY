using System;
using UnityEngine;

public class SaveProgressCircleStateMachineBehaviour : StateMachineBehaviour
{
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		R.Ui.SaveProgressCircle.Appear();
	}

	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		R.Ui.SaveProgressCircle.Disappear();
	}
}
