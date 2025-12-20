using System;
using System.Collections.Generic;
using GameWorld;
using UnityEngine;

[ExecuteInEditMode]
public class CameraFVController : BaseBehaviour
{
	private void Awake()
	{
		this.InitCameraFVList();
	}

	private void Update()
	{
		this.UpdateFVAndOffset();
	}

	private void InitCameraFVList()
	{
		if (base.transform.childCount != this._cameraFV.Count)
		{
			this._cameraFV.Clear();
			for (int i = 0; i < base.transform.childCount; i++)
			{
				this._cameraFV.Add(base.transform.GetChild(i).GetComponent<CameraFV>());
			}
			this._cameraFV.Sort((CameraFV l1, CameraFV l2) => l1.transform.position.x.CompareTo(l2.transform.position.x));
		}
	}

	private void UpdateFVAndOffset()
	{
		PlayerManager player = R.Player;
		if (player != null)
		{
			float x = player.Transform.position.x;
			int num = this.PlayerAt(x);
			if (num == 0)
			{
				CameraFV cameraFV = this._cameraFV[0];
				float fv = cameraFV.fv;
				float offsetY = cameraFV.offsetY;
				SingletonMono<CameraController>.Instance.FieldOfView = fv;
				SingletonMono<CameraController>.Instance.ManualOffsetY = offsetY;
			}
			else if (num == this._cameraFV.Count)
			{
				CameraFV cameraFV2 = this._cameraFV[num - 1];
				float fv2 = cameraFV2.fv;
				float offsetY2 = cameraFV2.offsetY;
				SingletonMono<CameraController>.Instance.FieldOfView = fv2;
				SingletonMono<CameraController>.Instance.ManualOffsetY = offsetY2;
			}
			else
			{
				CameraFV cameraFV3 = this._cameraFV[num - 1];
				CameraFV cameraFV4 = this._cameraFV[num];
				float fv3 = cameraFV3.fv;
				float fv4 = cameraFV4.fv;
				float offsetY3 = cameraFV3.offsetY;
				float offsetY4 = cameraFV4.offsetY;
				SingletonMono<CameraController>.Instance.FieldOfView = Mathf.Lerp(fv4, fv3, this.getLerpPercent(cameraFV4.Position.x, cameraFV3.Position.x, x));
				SingletonMono<CameraController>.Instance.ManualOffsetY = Mathf.Lerp(offsetY4, offsetY3, this.getLerpPercent(cameraFV4.Position.x, cameraFV3.Position.x, x));
			}
		}
		else
		{
			CameraFV cameraFV5 = this._cameraFV[0];
			float fv5 = cameraFV5.fv;
			SingletonMono<CameraController>.Instance.FieldOfView = fv5;
		}
	}

	private int PlayerAt(float playerPositionX)
	{
		for (int i = 0; i < this._cameraFV.Count; i++)
		{
			if (playerPositionX < this._cameraFV[i].Position.x)
			{
				return i;
			}
		}
		return this._cameraFV.Count;
	}

	private float getLerpPercent(float x1, float x2, float current)
	{
		return (current - x1) / (x2 - x1);
	}

	[SerializeField]
	private List<CameraFV> _cameraFV = new List<CameraFV>();
}
