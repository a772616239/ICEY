using System;
using UnityEngine;

public interface IBlowee
{
	Vector2 GetDir(IBlowable blowable, Vector2 pos);

	float GetPower(IBlowable blowable, Vector2 pos);

	float GetMaxAccelSqr();

	void ParserIntegrator(Integrator integrator);

	GameObject GetGO();
}
