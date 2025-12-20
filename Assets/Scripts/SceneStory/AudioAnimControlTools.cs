using System;
using UnityEngine;

namespace SceneStory
{
	public class AudioAnimControlTools : BaseBehaviour
	{
		public void PlayBGMLoop(int clipID)
		{
			R.Audio.PlayBGM(clipID, true);
		}

		public void PlayEffect(int clipID)
		{
			R.Audio.PlayEffect(clipID, new Vector3?(this.effectAuidoPos.position));
		}

		public void PlayVoiceOver(string key)
		{
			R.Audio.PlayVoiceOver(key, null, false);
		}

		[Header("这个效果发生的位置")]
		public Transform effectAuidoPos;
	}
}
