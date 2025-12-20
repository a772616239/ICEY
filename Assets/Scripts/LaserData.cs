using System;
using ExtensionMethods;
using UnityEngine;

[Serializable]
public class LaserData
{
	public Vector3 point2
	{
		get
		{
			return this._point2;
		}
		set
		{
			this._point2 = value.SetZ(LayerManager.ZNum.Fx);
		}
	}

	public int type = 1;

	public int damage = 5;

	public bool canRotation = true;

	public bool canHitGround = true;

	public float laserWidth = 0.3f;

	public float rotateAngle = 40f;

	public float startAngle = -150f;

	public float rotateSpeed = 20f;

	public float laserLength = 300f;

	public float maxDistance = 40f;

	public float waitTime = 0.8f;

	public float appearTime = 0.2f;

	public Vector3 point1;

	private Vector3 _point2;
}
