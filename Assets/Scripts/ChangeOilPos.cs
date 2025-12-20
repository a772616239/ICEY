using System;
using UnityEngine;

public class ChangeOilPos : MonoBehaviour
{
	private void Start()
	{
		this.pos = Camera.main.WorldToViewportPoint(base.transform.position);
	}

	private void Update()
	{
		base.transform.position = Camera.main.ViewportToWorldPoint(this.pos);
	}

	public Vector3 pos;
}
