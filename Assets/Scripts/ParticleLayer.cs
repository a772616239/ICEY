using System;
using UnityEngine;

public class ParticleLayer : MonoBehaviour
{
	private void Awake()
	{
		base.GetComponent<ParticleSystem>().GetComponent<Renderer>().sortingOrder = this.sortingOrder;
	}

	public int sortingOrder;
}
