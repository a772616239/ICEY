using System;
using UnityEngine;

public interface IInputEventHandler
{
	void KeyDown(KeyCode key);

	void KeyUp(KeyCode key);
}
