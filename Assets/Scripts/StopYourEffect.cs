using System;
using UnityEngine;
using Xft;

public class StopYourEffect : MonoBehaviour
{
	private void OnGUI()
	{
		if (GUI.Button(new Rect(0f, 0f, 350f, 30f), "Reset"))
		{
			this.SampleLoop.Active();
		}
		if (GUI.Button(new Rect(0f, 30f, 350f, 30f), "stop effect immediately"))
		{
			this.SampleLoop.DeActive();
		}
		if (GUI.Button(new Rect(0f, 60f, 350f, 30f), "stop effect softly"))
		{
			this.SampleLoop.StopSmoothly(1f);
		}
	}

	public XffectComponent SampleLoop;
}
