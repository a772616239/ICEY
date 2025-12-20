using System;
using UnityEngine;

namespace Tools
{
	public class PlayerAttributeDebugGUI : DebugGuiBase
	{
		protected override string UIDebugButtonName
		{
			get
			{
				return "属性查看";
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
			GUILayout.Label(string.Concat(new object[]
			{
				"HP：           ",
				R.Player.Attribute.currentHP,
				"/",
				R.Player.Attribute.maxHP
			}), new GUILayoutOption[0]);
			GUILayout.Label(string.Concat(new object[]
			{
				"Energy：       ",
				R.Player.Attribute.currentEnergy,
				"/",
				R.Player.Attribute.maxEnergy
			}), new GUILayoutOption[0]);
			GUILayout.Label("Coin：       " + R.Equipment.CoinNum, new GUILayoutOption[0]);
			GUILayout.EndVertical();
			GUILayout.EndArea();
		}

		private Rect GUIArea;
	}
}
