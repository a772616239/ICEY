using System;
using ExtensionMethods;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SetZAxisByPlayerInCollision : BaseBehaviour
{
	public void Awake()
	{
		this._zOrigin = base.transform.localPosition.z;
	}

	public void OnTriggerEnter2D(Collider2D collision)
	{
		if (!collision.CompareTag("Player"))
		{
			return;
		}
		if (!this._out)
		{
			this._zOrigin = base.transform.localPosition.z;
			base.transform.localPosition = base.transform.localPosition.SetZ(this._z);
		}
		else
		{
			base.transform.localPosition = base.transform.localPosition.SetZ(this._zOrigin);
		}
	}

	public void OnTriggerExit2D(Collider2D collision)
	{
		if (!collision.CompareTag("Player"))
		{
			return;
		}
		if (!this._out)
		{
			base.transform.localPosition = base.transform.localPosition.SetZ(this._zOrigin);
		}
		else
		{
			this._zOrigin = base.transform.localPosition.z;
			base.transform.localPosition = base.transform.localPosition.SetZ(this._z);
		}
	}

	private float _zOrigin;

	[Header("需要设置的 LocalPosition z")]
	[SerializeField]
	private float _z;

	[Header("离开碰撞区域设值")]
	[SerializeField]
	private bool _out;
}
