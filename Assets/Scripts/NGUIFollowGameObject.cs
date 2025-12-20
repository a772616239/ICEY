using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NGUIFollowGameObject : MonoBehaviour
{
	private void OnEnable()
	{
		SceneManager.sceneUnloaded += this.SceneUnloaded;
		this.UpdatePosition();
	}

	private void Update()
	{
		this.UpdatePosition();
	}

	private void OnDisable()
	{
		SceneManager.sceneUnloaded -= this.SceneUnloaded;
	}

	private void SceneUnloaded(Scene arg0)
	{
		UnityEngine.Object.Destroy(this);
	}

	private void UpdatePosition()
	{
		if (this.open)
		{
			if (!base.gameObject.activeSelf)
			{
				base.gameObject.SetActive(true);
			}
			if (this.isFollow)
			{
				if (this.gameObj != null)
				{
					this.followedPos = this.gameObj.position;
				}
				Vector3 pos = R.Camera.Camera.WorldToViewportPoint(this.followedPos + this.offset);
				if (UICamera.mainCamera != null)
				{
					this.position = this.UIViewportToWorldPoint(pos);
				}
			}
			base.transform.position = this.position;
		}
	}

	public void SetFollowTransform(Transform followed, bool _open = true)
	{
		this.gameObj = followed;
		Vector3 pos = Camera.main.WorldToViewportPoint(this.followedPos);
		if (UICamera.mainCamera != null)
		{
			this.position = this.UIViewportToWorldPoint(pos);
		}
		this.open = _open;
		this.isFollow = true;
	}

	public void SetFollowTransform(Transform followed, Vector2 offset, bool _open = true)
	{
		this.offset = offset;
		this.SetFollowTransform(followed, _open);
	}

	public void SetViewPosition(Vector2 pos, bool _open = true)
	{
		this.gameObj = null;
		this.position = this.UIViewportToWorldPoint(pos);
		this.open = _open;
		this.isFollow = false;
	}

	public void SetWorldPosition(Vector3 pos, bool follow = true, bool open = true)
	{
		this.gameObj = null;
		Vector3 pos2 = Camera.main.WorldToViewportPoint(pos);
		this.position = this.UIViewportToWorldPoint(pos2);
		this.open = open;
		this.isFollow = follow;
		this.followedPos = pos;
	}

	private Vector3 UIViewportToWorldPoint(Vector3 pos)
	{
		pos.z = 5f;
		return UICamera.mainCamera.ViewportToWorldPoint(pos);
	}

	public Transform gameObj;

	[HideInInspector]
	public Vector3 offset = Vector3.zero;

	public bool open;

	public bool isFollow = true;

	[HideInInspector]
	public Vector3 position;

	private Vector3 followedPos;
}
