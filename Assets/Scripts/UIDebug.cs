using System;
using System.Collections.Generic;
using UnityEngine;

public class UIDebug : SingletonMono<UIDebug>
{
	private void Awake()
	{
		UIDebug._table = this.debugWidget.transform.GetChild(0);
		UIDebug.debugBtns = new Dictionary<int, GameObject>();
	}

	public static int Register(string name, EventDelegate.Callback open)
	{
		int firstNotInUseId = UIDebug.GetFirstNotInUseId();
		Transform transform;
		if (firstNotInUseId == 0)
		{
			transform = UIDebug._table.GetChild(0);
		}
		else
		{
			transform = UnityEngine.Object.Instantiate<Transform>(UIDebug._table.GetChild(0));
			transform.parent = UIDebug._table;
			transform.localScale = Vector3.one;
		}
		UIDebug.debugBtns.Add(firstNotInUseId, transform.gameObject);
		NGUITools.SetActive(transform.gameObject, true);
		UIButton component = transform.GetChild(0).GetComponent<UIButton>();
		UILabel component2 = transform.GetChild(1).GetComponent<UILabel>();
		component2.text = name;
		EventDelegate.Set(component.onClick, open);
		return firstNotInUseId;
	}

	public static void Unregister(int id)
	{
		UnityEngine.Object.Destroy(UIDebug.debugBtns[id]);
		UIDebug.debugBtns.Remove(id);
	}

	private static int GetFirstNotInUseId()
	{
		for (int i = 0; i < UIDebug.debugBtns.Count; i++)
		{
			if (!UIDebug.debugBtns.ContainsKey(i))
			{
				return i;
			}
		}
		return UIDebug.debugBtns.Count;
	}

	public void Open()
	{
		UIDebug.IsVisible = true;
		R.Mode.EnterMode(Mode.AllMode.UI);
		R.Ui.Pause.Enabled = false;
		R.PauseGame();
		this.debugWidget.gameObject.SetActive(true);
		Cursor.visible = true;
		Cursor.SetCursor(this._cursor, Vector2.zero, CursorMode.Auto);
	}

	public void Close()
	{
		UIDebug.IsVisible = false;
		R.Mode.ExitMode(Mode.AllMode.UI);
		R.Ui.Pause.Enabled = true;
		R.ResumeGame();
		this.debugWidget.gameObject.SetActive(false);
	}

	public static bool IsVisible;

	[SerializeField]
	private UIWidget debugWidget;

	private static Transform _table;

	[SerializeField]
	private Texture2D _cursor;

	private static Dictionary<int, GameObject> debugBtns;
}
