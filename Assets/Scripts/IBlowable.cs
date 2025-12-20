using System;
using UnityEngine;

public interface IBlowable
{
	void _OnWindEnter(IBlowee wind);

	void _OnWindExit(IBlowee wind);

	Bounds GetBounds();
}
