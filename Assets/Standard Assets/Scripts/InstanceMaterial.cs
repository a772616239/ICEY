using System;
using UnityEngine;

public class InstanceMaterial : MonoBehaviour
{
	private void Start()
	{
		base.GetComponent<Renderer>().material = this.Material;
	}

	private void Update()
	{
	}

	public Material Material;
}
