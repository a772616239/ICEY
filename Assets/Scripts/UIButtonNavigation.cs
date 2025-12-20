using System;
using AnimationOrTween;
using Core;
using UnityEngine;

[RequireComponent(typeof(UIButton))]
public class UIButtonNavigation : MonoBehaviour
{
	public UIButton button
	{
		get
		{
			UIButton result;
			if ((result = this._button) == null)
			{
				result = (this._button = base.GetComponent<UIButton>());
			}
			return result;
		}
	}

	public static UIButtonNavigation current
	{
		get
		{
			GameObject hoveredObject = UICamera.hoveredObject;
			if (hoveredObject == null)
			{
				return null;
			}
			return hoveredObject.GetComponent<UIButtonNavigation>();
		}
	}

	public bool isColliderEnabled
	{
		get
		{
			if (!base.enabled || !base.gameObject.activeInHierarchy)
			{
				return false;
			}
			Collider component = base.GetComponent<Collider>();
			if (component != null)
			{
				return component.enabled;
			}
			Collider2D component2 = base.GetComponent<Collider2D>();
			return component2 != null && component2.enabled;
		}
	}

	protected virtual void OnEnable()
	{
		UIButtonNavigation.list.Add(this);
		if (this.mStarted)
		{
			this.Start();
		}
	}

	private void Start()
	{
		this.mStarted = true;
		if (this.startsSelected && this.isColliderEnabled)
		{
			UICamera.hoveredObject = base.gameObject;
		}
	}

	protected virtual void OnDisable()
	{
		UIButtonNavigation.list.Remove(this);
	}

	private static bool IsActive(GameObject go)
	{
		if (!go || !go.activeInHierarchy)
		{
			return false;
		}
		Collider component = go.GetComponent<Collider>();
		if (component != null)
		{
			return component.enabled;
		}
		Collider2D component2 = go.GetComponent<Collider2D>();
		return component2 != null && component2.enabled;
	}

	public GameObject GetLeft()
	{
		if (UIButtonNavigation.IsActive(this.onLeft))
		{
			return this.onLeft;
		}
		if (this.constraint == UIButtonNavigation.Constraint.Vertical || this.constraint == UIButtonNavigation.Constraint.Explicit)
		{
			return null;
		}
		return this.Get(Vector3.left, 1f, 2f);
	}

	public GameObject GetRight()
	{
		if (UIButtonNavigation.IsActive(this.onRight))
		{
			return this.onRight;
		}
		if (this.constraint == UIButtonNavigation.Constraint.Vertical || this.constraint == UIButtonNavigation.Constraint.Explicit)
		{
			return null;
		}
		return this.Get(Vector3.right, 1f, 2f);
	}

	public GameObject GetUp()
	{
		if (UIButtonNavigation.IsActive(this.onUp))
		{
			return this.onUp;
		}
		if (this.constraint == UIButtonNavigation.Constraint.Horizontal || this.constraint == UIButtonNavigation.Constraint.Explicit)
		{
			return null;
		}
		return this.Get(Vector3.up, 2f, 1f);
	}

	public GameObject GetDown()
	{
		if (UIButtonNavigation.IsActive(this.onDown))
		{
			return this.onDown;
		}
		if (this.constraint == UIButtonNavigation.Constraint.Horizontal || this.constraint == UIButtonNavigation.Constraint.Explicit)
		{
			return null;
		}
		return this.Get(Vector3.down, 2f, 1f);
	}

	public GameObject Get(Vector3 myDir, float x = 1f, float y = 1f)
	{
		Transform transform = base.transform;
		myDir = transform.TransformDirection(myDir);
		Vector3 center = UIButtonNavigation.GetCenter(base.gameObject);
		float num = float.MaxValue;
		GameObject result = null;
		for (int i = 0; i < UIButtonNavigation.list.size; i++)
		{
			UIButtonNavigation uibuttonNavigation = UIButtonNavigation.list[i];
			if (!(uibuttonNavigation == this) && uibuttonNavigation.constraint != UIButtonNavigation.Constraint.Explicit && uibuttonNavigation.isColliderEnabled)
			{
				UIWidget component = uibuttonNavigation.GetComponent<UIWidget>();
				if (!(component != null) || component.alpha != 0f)
				{
					Vector3 direction = UIButtonNavigation.GetCenter(uibuttonNavigation.gameObject) - center;
					float num2 = Vector3.Dot(myDir, direction.normalized);
					if (num2 >= 0.707f)
					{
						direction = transform.InverseTransformDirection(direction);
						direction.x *= x;
						direction.y *= y;
						float sqrMagnitude = direction.sqrMagnitude;
						if (sqrMagnitude <= num)
						{
							result = uibuttonNavigation.gameObject;
							num = sqrMagnitude;
						}
					}
				}
			}
		}
		return result;
	}

	protected static Vector3 GetCenter(GameObject go)
	{
		UIWidget component = go.GetComponent<UIWidget>();
		UICamera uicamera = UICamera.FindCameraForLayer(go.layer);
		if (uicamera != null)
		{
			Vector3 vector = go.transform.position;
			if (component != null)
			{
				Vector3[] worldCorners = component.worldCorners;
				vector = (worldCorners[0] + worldCorners[2]) * 0.5f;
			}
			vector = uicamera.cachedCamera.WorldToScreenPoint(vector);
			vector.z = 0f;
			return vector;
		}
		if (component != null)
		{
			Vector3[] worldCorners2 = component.worldCorners;
			return (worldCorners2[0] + worldCorners2[2]) * 0.5f;
		}
		return go.transform.position;
	}

	public virtual void OnNavigate()
	{
		if (UIPopupList.isOpen)
		{
			return;
		}
		GameObject gameObject = null;
		if (Core.Input.UI.Left.OnPressedForSeveralSeconds(0.333333343f, true))
		{
			gameObject = this.GetLeft();
		}
		else if (Core.Input.UI.Right.OnPressedForSeveralSeconds(0.333333343f, true))
		{
			gameObject = this.GetRight();
		}
		else if (Core.Input.UI.Up.OnPressedForSeveralSeconds(0.333333343f, true))
		{
			gameObject = this.GetUp();
		}
		else if (Core.Input.UI.Down.OnPressedForSeveralSeconds(0.333333343f, true))
		{
			gameObject = this.GetDown();
		}
		if (gameObject != null)
		{
			R.Audio.PlayEffect(3, null);
		}
		if (Core.Input.UI.Confirm.OnClick)
		{
			gameObject = this.onClick;
			if (gameObject != null)
			{
				UIKeyInput.SaveHoveredObject();
			}
			this.button.SendMessage("OnClick");
			UIPlayAnimation[] components = base.GetComponents<UIPlayAnimation>();
			if (components != null)
			{
				for (int i = 0; i < components.Length; i++)
				{
					if (components[i].enabled && components[i].trigger == Trigger.OnClick)
					{
						Direction playDirection = components[i].playDirection;
						if (playDirection != Direction.Forward)
						{
							if (playDirection != Direction.Reverse)
							{
								if (playDirection == Direction.Toggle)
								{
									components[i].Play(true);
								}
							}
							else
							{
								components[i].Play(true, false);
							}
						}
						else
						{
							components[i].Play(true, false);
						}
					}
				}
				R.Audio.PlayEffect(139, null);
			}
		}
		if (gameObject != null)
		{
			this.button.state = UIButtonColor.State.Normal;
			UIKeyInput.HoveredObject = gameObject;
		}
	}

	public virtual void OnKey(KeyCode key)
	{
	}

	public static BetterList<UIButtonNavigation> list = new BetterList<UIButtonNavigation>();

	private UIButton _button;

	public UIButtonNavigation.Constraint constraint;

	public GameObject onUp;

	public GameObject onDown;

	public GameObject onLeft;

	public GameObject onRight;

	public GameObject onClick;

	public GameObject onTab;

	public bool startsSelected;

	[NonSerialized]
	private bool mStarted;

	public enum Constraint
	{
		None,
		Vertical,
		Horizontal,
		Explicit
	}
}
