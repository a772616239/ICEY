using System;
using UnityEngine;

public class TrapCameraLimited : MonoBehaviour
{
	private void OnEnable()
	{
		this.Init();
	}

	private void OnDisable()
	{
		this.Reset();
	}

	private void Update()
	{
		if (R.Camera.IsInView(base.gameObject))
		{
			this.CameraLimited();
		}
	}

	private void CameraLimited()
	{
		GameArea.CameraRange.xMax = ((!this.rightLimited) ? this.CameraRangeOrigin.xMax : this.right.position.x);
		GameArea.CameraRange.xMin = ((!this.leftLimited) ? this.CameraRangeOrigin.xMin : this.left.position.x);
		GameArea.CameraRange.yMax = ((!this.topLimited) ? this.CameraRangeOrigin.yMax : this.top.position.y);
		GameArea.CameraRange.yMin = ((!this.buttomLimited) ? this.CameraRangeOrigin.yMin : this.buttom.position.y);
	}

	private void Init()
	{
		this.CameraRangeOrigin = GameArea.CameraRange;
	}

	private void Reset()
	{
		GameArea.CameraRange = this.CameraRangeOrigin;
	}

	private Rect CameraRangeOrigin;

	private float orginMinX;

	private float orginMaxX;

	private float orginMinY;

	private float orginMaxY;

	[Header("是否限制右侧边界")]
	public bool rightLimited;

	[Header("右测边界值")]
	public Transform right;

	[Header("是否限制左侧边界")]
	public bool leftLimited;

	[Header("左测边界值")]
	public Transform left;

	[Header("是否限制上方边界")]
	public bool topLimited;

	[Header("上方边界值")]
	public Transform top;

	[Header("是否限制下方边界")]
	public bool buttomLimited;

	[Header("下方边界值")]
	public Transform buttom;
}
