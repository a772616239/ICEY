using System;
using UnityEngine;

public class BaseBehaviour : MonoBehaviour
{
	public new GameObject gameObject
	{
		get
		{
			GameObject result;
			if ((result = this._gameObject) == null)
			{
				result = (this._gameObject = base.gameObject);
			}
			return result;
		}
	}

	public new Transform transform
	{
		get
		{
			Transform result;
			if ((result = this._transform) == null)
			{
				result = (this._transform = base.transform);
			}
			return result;
		}
	}

	private GameObject _gameObject;

	private Transform _transform;
}
