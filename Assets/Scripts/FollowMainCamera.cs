using System;
using UnityEngine;

public class FollowMainCamera : MonoBehaviour
{
	private void Update()
	{
		if (R.Camera.Transform != null)
		{
			base.transform.position = R.Camera.Transform.position;
		}
	}
}
