using System;
using CIS;
using UnityEngine;

public class Integrator : MonoBehaviour
{
	public Vector2 Integrate(Vector2 oldPos)
	{
		this.vel += this.accel * SingletonMonoBehaviourClass<TimeMgr>.instance.deltaTime;
		return oldPos + this.vel * SingletonMonoBehaviourClass<TimeMgr>.instance.deltaTime;
	}

	public Vector2 accel;

	public Vector2 vel;
}
