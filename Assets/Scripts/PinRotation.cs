using System;
using UnityEngine;

public class PinRotation : MonoBehaviour
{
	private void Start()
	{
		this.myTrans = base.transform;
	}

	private void Update()
	{
		base.transform.rotation = Quaternion.identity;
	}

	private Transform myTrans;
}
