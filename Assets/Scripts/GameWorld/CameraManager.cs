using System;
using UnityEngine;

namespace GameWorld
{
	public class CameraManager
	{
		public GameObject GameObject
		{
			get
			{
				GameObject result;
				if ((result = this._gameObject) == null)
				{
					result = (this._gameObject = this.Camera.gameObject);
				}
				return result;
			}
		}

		public Transform Transform
		{
			get
			{
				Transform result;
				if ((result = this._transform) == null)
				{
					result = (this._transform = this.Camera.transform);
				}
				return result;
			}
		}

		public Camera Camera
		{
			get
			{
				Camera result;
				if ((result = this._camera) == null)
				{
					result = (this._camera = Camera.main);
				}
				return result;
			}
		}

		public CameraController Controller
		{
			get
			{
				return SingletonMono<CameraController>.Instance;
			}
		}

		public T GetComponent<T>()
		{
			return this.GameObject.GetComponent<T>();
		}

		public T AddComponent<T>() where T : Component
		{
			return this.GameObject.AddComponent<T>();
		}

		public bool IsInView(GameObject go)
		{
			Vector3 vector = this.Camera.WorldToViewportPoint(go.transform.position);
			return vector.x > 0f && vector.x < 1f;
		}

		private Camera _camera;

		private GameObject _gameObject;

		private Transform _transform;
	}
}
