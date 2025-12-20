using System;
using UnityEngine;

public class ShaderPrinter : MonoBehaviour
{
	private void Start()
	{
		this.mat = base.GetComponent<Renderer>().material;
	}

	private void Update()
	{
		if (UnityEngine.Input.GetKeyDown(KeyCode.P))
		{
			UnityEngine.Debug.Log("range: " + this.mat.GetFloat("range"));
		}
	}

	private Material mat;
}
