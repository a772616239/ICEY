using System;
using UnityEngine;

public interface IInputPlayer
{
	Vector2 GetJoystick(string axis);

	Vector2 GetJoystickRaw(string axis);

	bool GetButton(string buttonName);

	void SetVibration(float leftMotorValue, float rightMotorValue);
}
