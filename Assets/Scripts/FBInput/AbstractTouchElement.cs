using System;
using UnityEngine;

namespace FBInput
{
	public abstract class AbstractTouchElement : BaseBehaviour
	{
		protected bool IsActive
		{
			get
			{
				return this._fingerId != -1;
			}
		}

		protected virtual void Update()
		{
			if (UnityEngine.Input.touchCount > 0)
			{
				Touch touch = default(Touch);
				if (this.IsActive)
				{
					for (int i = 0; i < UnityEngine.Input.touchCount; i++)
					{
						touch = UnityEngine.Input.GetTouch(i);
						if (touch.fingerId == this._fingerId)
						{
							break;
						}
					}
				}
				else
				{
					for (int j = 0; j < UnityEngine.Input.touchCount; j++)
					{
						touch = UnityEngine.Input.GetTouch(j);
						if (touch.phase == TouchPhase.Began)
						{
							Vector2 vector = UITools.ScreenPositionToLocalPosition(touch.position);
							if (this.TestTouchArea(vector))
							{
								this._fingerId = touch.fingerId;
								this.OnTouchDown(vector);
							}
						}
					}
				}
				if (this.IsActive)
				{
					Vector2 localPosition = UITools.ScreenPositionToLocalPosition(touch.position);
					if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
					{
						this._fingerId = -1;
						this.OnTouchUp(localPosition);
					}
					else
					{
						this.OnTouch(localPosition);
					}
				}
			}
			else
			{
				if (!this.IsActive && Input.GetMouseButton(0))
				{
					Vector2 vector2 = UITools.ScreenPositionToLocalPosition(UnityEngine.Input.mousePosition);
					if (this.TestTouchArea(vector2))
					{
						this._fingerId = 0;
						this.OnTouchDown(vector2);
					}
				}
				if (this.IsActive)
				{
					Vector2 localPosition2 = UITools.ScreenPositionToLocalPosition(UnityEngine.Input.mousePosition);
					if (!Input.GetMouseButton(0))
					{
						this._fingerId = -1;
						this.OnTouchUp(localPosition2);
					}
					else
					{
						this.OnTouch(localPosition2);
					}
				}
			}
		}

		protected virtual void OnTouchDown(Vector2 touchPosition)
		{
		}

		protected virtual void OnTouchUp(Vector2 localPosition)
		{
		}

		protected virtual void OnTouch(Vector2 localPosition)
		{
		}

		protected virtual bool TestTouchArea(Vector2 localPosition)
		{
			return true;
		}

		private int _fingerId = -1;
	}
}
