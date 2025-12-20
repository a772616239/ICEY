using System;
using System.Collections.Generic;
using DatabaseModel;
using SaveDataModel;
using UnityEngine;

public class UIEnhancementItemsController : MonoBehaviour
{
	private void Awake()
	{
		this._scrollView = base.GetComponentInChildren<UIScrollView>();
		this._grid = this._scrollView.gameObject.GetComponentInChildren<UIGrid>();
		this._items = this.InstantiateAndGetUIEnhancementItems();
		base.gameObject.SetActive(false);
	}

	private List<UIEnhancementItem> InstantiateAndGetUIEnhancementItems()
	{
		Transform transform = this._grid.transform;
		Transform child = this._grid.GetChild(0);
		bool flag = true;
		foreach (KeyValuePair<string, DatabaseModel.Enhancement> keyValuePair in DB.Enhancements)
		{
			Transform transform2;
			if (flag)
			{
				flag = false;
				transform2 = child;
				transform2.GetComponent<UIToggle>().value = true;
			}
			else
			{
				transform2 = UnityEngine.Object.Instantiate<Transform>(child);
				transform2.parent = transform;
				transform2.localScale = Vector3.one;
			}
			transform2.GetComponent<UIEnhancementItem>().Name = keyValuePair.Key;
		}
		List<Transform> childList = this._grid.GetChildList();
		List<UIEnhancementItem> list = new List<UIEnhancementItem>();
		for (int i = 0; i < childList.Count; i++)
		{
			list.Add(childList[i].GetComponent<UIEnhancementItem>());
		}
		return list;
	}

	public void OnClick()
	{
		EnhancementItem current = UIEnhancementItem.Current;
		if (current.CoinEnough && current.Unlockable)
		{
			SaveDataModel.Enhancement enhancement;
			string name;
			(enhancement = R.Player.Enhancement)[name = current.Name] = enhancement[name] + 1;
			R.Equipment.CoinNum -= current.Cost;
			R.Player.Enhancement.Post(current.Name, R.Player.Enhancement[current.Name]);
		}
	}

	private UIScrollView _scrollView;

	private UIGrid _grid;

	private List<UIEnhancementItem> _items;
}
