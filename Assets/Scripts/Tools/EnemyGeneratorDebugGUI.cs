using System;
using DatabaseModel;
using UnityEngine;

namespace Tools
{
	public class EnemyGeneratorDebugGUI : DebugGuiBase
	{
		protected override string UIDebugButtonName
		{
			get
			{
				return "敌人生成";
			}
		}

		public override void OnDebugGUI()
		{
			Array values = Enum.GetValues(typeof(EnemyType));
			string text = values.GetValue(this.index).ToString();
			if (this.InVisible)
			{
				GUILayout.BeginVertical("box", new GUILayoutOption[0]);
				GUILayout.BeginHorizontal(new GUILayoutOption[0]);
				GUILayout.Label(string.Format("Current Enemy: {0}", text), new GUILayoutOption[0]);
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal(new GUILayoutOption[0]);
				if (GUILayout.Button("Previous", new GUILayoutOption[]
				{
					GUILayout.Width(50f),
					GUILayout.Height(30f)
				}))
				{
					this.index = (this.index - 1 + values.Length) % values.Length;
				}
				if (GUILayout.Button("Next", new GUILayoutOption[]
				{
					GUILayout.Width(50f),
					GUILayout.Height(30f)
				}))
				{
					this.index = (this.index + 1) % values.Length;
				}
				if (GUILayout.Button("Generate", new GUILayoutOption[]
				{
					GUILayout.Width(50f),
					GUILayout.Height(30f)
				}))
				{
					Vector2 value = R.Player.Transform.position;
					EnemyAttrData enemyAttrData = EnemyAttrData.FindBySceneNameAndType(LevelManager.SceneName, (EnemyType)Enum.Parse(typeof(EnemyType), text));
					value.x += UnityEngine.Random.Range(-3f, 3f);
					value.y = ((enemyAttrData.flyHeight != 0) ? MathfX.HeightToPos((float)enemyAttrData.flyHeight) : -3.69999981f);
					Singleton<EnemyGenerator>.Instance.GenerateEnemy(EnumTools.ToEnum<EnemyType>(text, false), new Vector2?(value), true, true);
				}
				GUILayout.EndHorizontal();
				GUILayout.EndVertical();
			}
		}

		protected override void GUIStart()
		{
		}

		private int index;
	}
}
