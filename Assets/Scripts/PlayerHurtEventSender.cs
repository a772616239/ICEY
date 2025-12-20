using System;
using GameWorld;
using LitJson;
using UnityEngine;

public class PlayerHurtEventSender : DebugGuiBase
{
	protected override string UIDebugButtonName
	{
		get
		{
			return "主角受伤";
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
		GUILayout.FlexibleSpace();
		if (GUILayout.Button("测试一下", new GUILayoutOption[]
		{
			GUILayout.Width(200f)
		}))
		{
			this.SendEvent();
		}
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();
		GUILayout.EndArea();
	}

	private void SendEvent()
	{
		PlayerHurtAtkEventArgs args = new PlayerHurtAtkEventArgs(R.Player.GameObject, base.gameObject, base.gameObject, 0, Incrementor.GetNextId(), new JsonData(), false);
		EventManager.PostEvent<Transform, PlayerHurtAtkEventArgs>("PlayerHurtAtk", base.transform, args);
	}

	private string effectDescription;

	private Rect GUIArea;
}
