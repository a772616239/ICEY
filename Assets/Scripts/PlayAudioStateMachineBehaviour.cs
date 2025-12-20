using System;
using UnityEngine;

public class PlayAudioStateMachineBehaviour : StateMachineBehaviour
{
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		R.Audio.PlayEffect(this._id, new Vector3?(animator.transform.position));
	}

	[SerializeField]
	private int _id;
}
