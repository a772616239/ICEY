using System;
using UnityEngine;

public class UILevelSelectPasswordController : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnEnable()
	{
		EventDelegate.Add(this._input.onSubmit, new EventDelegate.Callback(this.OnSubmit));
	}

	private void OnDisable()
	{
		EventDelegate.Remove(this._input.onSubmit, new EventDelegate.Callback(this.OnSubmit));
	}

	private void OnSubmit()
	{
		if (this._input.label.text.ToUpper() == "HASTUR")
		{
			this._levelSelectController.LoadLevel("C6L1S1");
		}
	}

	[SerializeField]
	private GameObject _levelWidget;

	[SerializeField]
	private UILevelSelectController _levelSelectController;

	[SerializeField]
	private UIInput _input;
}
