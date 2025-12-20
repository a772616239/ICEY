using System;
using UnityEngine;

[ExecuteInEditMode]
public class MeshRendererLayer : MonoBehaviour
{
	private void Awake()
	{
		base.GetComponent<MeshRenderer>().sortingOrder = this.sortingOrder;
	}

	[ExecuteInEditMode]
	private void Update()
	{
		base.GetComponent<MeshRenderer>().sortingOrder = this.sortingOrder;
	}

	public int sortingOrder;

	public string sortingName;
}
