using System;
using UnityEngine;

public class PlayerPath : MonoBehaviour
{
	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(new Vector3(UnityEngine.Input.mousePosition.x, UnityEngine.Input.mousePosition.y, 0f));
			RaycastHit raycastHit;
			if (Physics.Raycast(ray, out raycastHit))
			{
				this.playerSelected = true;
				this.MoveOnFloor();
				this.trailReference.Clear();
			}
		}
		else if (Input.GetMouseButtonUp(0))
		{
			this.playerSelected = false;
		}
		if (Input.GetMouseButton(0) && this.playerSelected)
		{
			this.MoveOnFloor();
		}
	}

	private void MoveOnFloor()
	{
		RaycastHit raycastHit;
		if (Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(UnityEngine.Input.mousePosition.x, UnityEngine.Input.mousePosition.y, 0f)), out raycastHit, 3.40282347E+38f, 2147483647 & 1 << LayerMask.NameToLayer("Water")))
		{
			this.trailReference.transform.position = raycastHit.point + new Vector3(0f, 0.02f, 0f);
		}
	}

	public SplineTrailRenderer trailReference;

	private bool playerSelected;
}
