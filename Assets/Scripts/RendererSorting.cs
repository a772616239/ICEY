using System;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class RendererSorting : MonoBehaviour
{
	private void Start()
	{
		this.SetSortingLayerAndOrder();
	}

	public void SetSortingLayerAndOrder()
	{
		Renderer component = base.GetComponent<Renderer>();
		if (!string.IsNullOrEmpty(this.sortingLayerName))
		{
			component.sortingLayerName = this.sortingLayerName;
			component.sortingOrder = this.sortOrder;
		}
	}

	public int sortOrder;

	public string sortingLayerName;

	public int sortingLayerID;
}
