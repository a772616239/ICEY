using System;
using UnityEngine;

public interface ILevelBlock
{
	void OnActive();

	void OnDeactive();

	void OnEnter();

	void OnLeave();

	bool IsActive();

	bool IsEntered();

	GameObject GetGo();
}
