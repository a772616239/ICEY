using System;
using UnityEngine;

public class FixShaderQueue : MonoBehaviour
{
	private void Start()
	{
		if (base.GetComponent<Renderer>() != null)
		{
			base.GetComponent<Renderer>().sharedMaterial.renderQueue += this.AddQueue;
		}
		else
		{
			base.Invoke("SetProjectorQueue", 0.1f);
		}
	}

	private void SetProjectorQueue()
	{
		base.GetComponent<Projector>().material.renderQueue += this.AddQueue;
	}

	private void Update()
	{
	}

	public int AddQueue = 1;
}
