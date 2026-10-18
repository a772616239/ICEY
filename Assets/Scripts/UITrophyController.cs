using System;
using Colorful;
using Core;
using UnityEngine;

public class UITrophyController : MonoBehaviour
{
	private void Update()
	{
		if (Core.Input.UI.Cancel.OnClick)
		{
			this.Close();
		}
	}

	public void Open()
	{
		this._analogTv = R.Ui.CameraGO.GetComponent<AnalogTV>();
		this._analogTv.enabled = false;
		R.Ui.Pause.Enabled = false;
		this._pauseGameObject.SetActive(false);
		this._trophyListGameObject.SetActive(true);
	}

	public void Close()
	{
		if (this._analogTv == null)
		{
			return;
		}
		this._analogTv.enabled = true;
		R.Ui.Pause.Enabled = true;
		this._pauseGameObject.SetActive(true);
		this._trophyListGameObject.SetActive(false);
	}

	[SerializeField]
	private GameObject _pauseGameObject;

	[SerializeField]
	private GameObject _trophyListGameObject;

	private AnalogTV _analogTv;
}
