using System;
using UnityEngine;

public interface ICameraController
{
	void ReloadLimitBorder(Transform left, Transform right, Transform down, Transform up);

	void SetCurrentPos(float x, float y);

	GameObject GetGO();
}
