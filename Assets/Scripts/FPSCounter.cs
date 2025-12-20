using System;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
	private void Awake()
	{
		this.style.fontSize = 20;
		this.style.normal.textColor = Color.white;
		Application.targetFrameRate = 60;
	}

	private void OnGUI()
	{
		float num = 1f / Time.smoothDeltaTime;
		GUI.Label(new Rect(10f, 5f, 50f, 20f), num.ToString("#,##0.0 fps"), this.style);
	}

	private GUIStyle style = new GUIStyle();
}
