using System;
using I2.Loc;
using UnityEngine;

public class UIControlOptionsController : MonoBehaviour
{
	private void Start()
	{
		this.UpdateLabel();
	}

	private void OnDisable()
	{
		R.Settings.Save();
	}

	private void UpdateLabel()
	{
		this._label.text = ((!R.Settings.DynamicJoystickOpen) ? ScriptLocalization.ui_start.off : ScriptLocalization.ui_start.on);
	}

	private void OnDynamicJoystickStateChange(bool open)
	{
		R.Settings.DynamicJoystickOpen = open;
		SingletonMono<MobileInputPlayer>.Instance.UpdateRadiusAndPosition();
		this.UpdateLabel();
	}

	public void DynamicJoystickStateOpen()
	{
		this.OnDynamicJoystickStateChange(true);
	}

	public void DynamicJoystickStateClose()
	{
		this.OnDynamicJoystickStateChange(false);
	}

	[SerializeField]
	private UILabel _label;
}
