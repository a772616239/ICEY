using System;
using UnityEngine;

public class ShowUIWhenStart : MonoBehaviour
{
	private void Start()
	{
		if (R.Ui.IsHide)
		{
			R.Ui.ShowUI(false);
		}
	}
}
