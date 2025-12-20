using System;
using UnityEngine;

public class UITrophyItemsController : BaseBehaviour
{
	private void Awake()
	{
		this._scrollView = base.GetComponent<UIScrollView>();
		this._grid = this._scrollView.gameObject.GetComponentInChildren<UIGrid>();
		this.InstantiateAndGetUiTrophyItems();
	}

	private void InstantiateAndGetUiTrophyItems()
	{
		Transform transform = this._grid.transform;
		Transform child = this._grid.GetChild(0);
		for (int i = 1; i <= 33; i++)
		{
			Transform transform2 = UnityEngine.Object.Instantiate<Transform>(child);
			transform2.parent = transform;
			transform2.localScale = Vector3.one;
			transform2.GetComponent<UITrophyItem>().TrophyId = i;
		}
	}

	private UIScrollView _scrollView;

	private UIGrid _grid;
}
