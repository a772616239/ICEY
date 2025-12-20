using System;
using UnityEngine;

namespace Tools
{
	public class FrozenTimeDebugGUI : DebugGuiBase
	{
		protected override string UIDebugButtonName
		{
			get
			{
				return "帧冻结测试";
			}
		}

		protected override void GUIStart()
		{
			float num = (float)Screen.width;
			this.GUIArea = new Rect(num / 2f - 320f, 20f, 640f, 500f);
		}

		public override void OnDebugGUI()
		{
			GUILayout.BeginArea(this.GUIArea);
			GUILayout.BeginVertical("box", new GUILayoutOption[0]);
			GUILayout.BeginHorizontal(new GUILayoutOption[]
			{
				GUILayout.Width(600f)
			});
			GUILayout.Label(string.Format("frozen frames: {0}", this.frozenClip), new GUILayoutOption[0]);
			GUILayout.FlexibleSpace();
			this.frozenClip = Mathf.RoundToInt(GUILayout.HorizontalSlider((float)this.frozenClip, 0f, 60f, new GUILayoutOption[]
			{
				GUILayout.Width(400f)
			}));
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal(new GUILayoutOption[]
			{
				GUILayout.Width(600f)
			});
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("Test", new GUILayoutOption[]
			{
				GUILayout.Width(200f)
			}))
			{
				SingletonMono<WorldTime>.Instance.TimeFrozenByFixedFrame(this.frozenClip, WorldTime.FrozenArgs.FrozenType.All, true);
			}
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();
			GUILayout.EndArea();
		}

		private Rect GUIArea;

		private int frozenClip = 10;
	}
}
