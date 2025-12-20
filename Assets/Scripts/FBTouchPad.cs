using System;
using FBInput;
using UnityEngine;

public class FBTouchPad : AbstractTouchElement
{
	public Vector2 Axis { get; private set; }

	private bool _dynamicJoystickOpen
	{
		get
		{
			return R.Settings.DynamicJoystickOpen;
		}
	}

	private void Awake()
	{
		this.UpdateRadiusAndPosition();
		this._lastAnchor = new Vector2(100000f, 100000f);
		this.SetDefaultState();
	}

	protected override void OnTouchDown(Vector2 touchPosition)
	{
		if ((touchPosition - this._lastAnchor).sqrMagnitude > 4f * this._radiusSqr && this._dynamicJoystickOpen)
		{
			base.transform.localPosition = touchPosition;
			this._anchor = touchPosition;
		}
		this.BackgroundSprite.color = Color.white;
	}

	protected override void OnTouchUp(Vector2 localPosition)
	{
		this._lastAnchor = this._anchor;
		this.SetDefaultState();
	}

	protected override void OnTouch(Vector2 localPosition)
	{
		Vector2 vector = localPosition - this._anchor;
		if (vector.sqrMagnitude < this._radiusSqr)
		{
			vector /= this._radius;
		}
		else
		{
			vector = vector.normalized;
		}
		if (vector.sqrMagnitude >= FBTouchPad.DeadZoneRadiusSqr)
		{
			this.Axis = ((!base.IsActive) ? Vector2.zero : vector);
			float num = 57.29578f * Mathf.Atan2(this.Axis.y, this.Axis.x);
			num = (float)Mathf.RoundToInt(num / 45f) * 45f;
			this.HintHandle.rotation = Quaternion.Euler(num * Vector3.forward);
			this.HintSprite.color = Color.white;
		}
		else
		{
			this.HintSprite.color = new Color(1f, 1f, 1f, 0f);
		}
	}

	private void SetDefaultState()
	{
		this.Axis = Vector2.zero;
		this.BackgroundSprite.color = new Color(1f, 1f, 1f, 0.5f);
		this.HintSprite.color = new Color(1f, 1f, 1f, 0f);
	}

	protected override bool TestTouchArea(Vector2 screenPosition)
	{
		return (this._dynamicJoystickOpen || (screenPosition - this._anchor).sqrMagnitude <= 4f * this._radiusSqr) && screenPosition.x < (float)UITools.ScreenWidth / 2f;
	}

	public void UpdateRadiusAndPosition()
	{
		this._radius = ((!this._dynamicJoystickOpen) ? (this.Radius * 1.5f) : this.Radius);
		this._radiusSqr = this._radius * this._radius;
		base.transform.localPosition = this._defaultPosition;
		this._anchor = base.transform.localPosition;
	}

	public static readonly float DeadZoneRadius = 0.1f;

	private static readonly float DeadZoneRadiusSqr = FBTouchPad.DeadZoneRadius * FBTouchPad.DeadZoneRadius;

	[SerializeField]
	public UISprite BackgroundSprite;

	[SerializeField]
	public Transform HintHandle;

	[SerializeField]
	public UISprite HintSprite;

	[SerializeField]
	public float Radius;

	[SerializeField]
	private Vector3 _defaultPosition;

	private float _radius;

	private Vector2 _anchor;

	private Vector2 _lastAnchor;

	private float _radiusSqr;
}
