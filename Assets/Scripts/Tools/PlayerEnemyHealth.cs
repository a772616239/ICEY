using System;
using UnityEngine;

namespace Tools
{
	public class PlayerEnemyHealth : DebugGuiBase
	{
		protected override string UIDebugButtonName
		{
			get
			{
				return "血量控制";
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
			GUILayout.Label("HP " + R.Player.Attribute.currentHP, new GUILayoutOption[0]);
			GUILayout.BeginHorizontal(new GUILayoutOption[]
			{
				GUILayout.Width(600f)
			});
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("Kill All Enemies", new GUILayoutOption[]
			{
				GUILayout.Width(400f)
			}))
			{
				this.KillAllEnemy();
			}
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal(new GUILayoutOption[]
			{
				GUILayout.Width(600f)
			});
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("Full HP", new GUILayoutOption[]
			{
				GUILayout.Width(400f)
			}))
			{
				this.PlayerHPRecover();
			}
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal(new GUILayoutOption[]
			{
				GUILayout.Width(600f)
			});
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("Kill yourself", new GUILayoutOption[]
			{
				GUILayout.Width(400f)
			}))
			{
				this.KillPlayer();
			}
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal(new GUILayoutOption[]
			{
				GUILayout.Width(600f)
			});
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("HP -20", new GUILayoutOption[]
			{
				GUILayout.Width(400f)
			}))
			{
				R.Player.Attribute.currentHP -= 20;
			}
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			GUILayout.EndVertical();
			GUILayout.EndArea();
		}

		private void KillAllEnemy()
		{
			R.Enemy.KillAllEnemy();
		}

		private void PlayerHPRecover()
		{
			PlayerAction.Reborn();
		}

		private void KillPlayer()
		{
			R.Player.Kill();
		}

		private Rect GUIArea;
	}
}
