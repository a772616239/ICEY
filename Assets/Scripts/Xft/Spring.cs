using System;
using UnityEngine;

namespace Xft
{
	public class Spring
	{
		public Spring(Transform transform, Spring.TransformType modifier)
		{
			this.m_transform = transform;
			this.Modifier = modifier;
			this.RefreshTransformType();
		}

		public Transform Transform
		{
			set
			{
				this.m_transform = value;
				this.RefreshTransformType();
			}
		}

		public bool Done
		{
			get
			{
				return this.m_done;
			}
			set
			{
				this.m_done = value;
			}
		}

		public void FixedUpdate()
		{
			if (this.m_velocityFadeInEndTime > Time.time)
			{
				this.m_velocityFadeInCap = Mathf.Clamp01(1f - (this.m_velocityFadeInEndTime - Time.time) / this.m_velocityFadeInLength);
			}
			else
			{
				this.m_velocityFadeInCap = 1f;
			}
			if (this.Modifier != this.m_currentTransformType)
			{
				this.RefreshTransformType();
			}
			this.m_transformFunction();
		}

		private void Position()
		{
			this.Calculate();
			this.m_transform.localPosition = this.State;
		}

		private void PositionAdditive()
		{
			this.Calculate();
			this.m_transform.localPosition += this.State;
		}

		private void Rotation()
		{
			this.Calculate();
			this.m_transform.localEulerAngles = this.State;
		}

		private void RotationAdditive()
		{
			this.Calculate();
			this.m_transform.localEulerAngles += this.State;
		}

		private void Scale()
		{
			this.Calculate();
			this.m_transform.localScale = this.State;
		}

		private void ScaleAdditive()
		{
			this.Calculate();
			this.m_transform.localScale += this.State;
		}

		public void RefreshTransformType()
		{
			switch (this.Modifier)
			{
			case Spring.TransformType.Position:
				this.State = this.m_transform.localPosition;
				this.m_transformFunction = new Spring.TransformDelegate(this.Position);
				break;
			case Spring.TransformType.PositionAdditive:
				this.State = this.m_transform.localPosition;
				this.m_transformFunction = new Spring.TransformDelegate(this.PositionAdditive);
				break;
			case Spring.TransformType.Rotation:
				this.State = this.m_transform.localEulerAngles;
				this.m_transformFunction = new Spring.TransformDelegate(this.Rotation);
				break;
			case Spring.TransformType.RotationAdditive:
				this.State = this.m_transform.localEulerAngles;
				this.m_transformFunction = new Spring.TransformDelegate(this.RotationAdditive);
				break;
			case Spring.TransformType.Scale:
				this.State = this.m_transform.localScale;
				this.m_transformFunction = new Spring.TransformDelegate(this.Scale);
				break;
			case Spring.TransformType.ScaleAdditive:
				this.State = this.m_transform.localScale;
				this.m_transformFunction = new Spring.TransformDelegate(this.ScaleAdditive);
				break;
			}
			this.m_currentTransformType = this.Modifier;
			this.RestState = this.State;
		}

		protected void Calculate()
		{
			if (this.State == this.RestState)
			{
				this.m_done = true;
				return;
			}
			Vector3 a = this.RestState - this.State;
			this.m_velocity += Vector3.Scale(a, this.Stiffness);
			this.m_velocity = Vector3.Scale(this.m_velocity, this.Damping);
			this.m_velocity = Vector3.ClampMagnitude(this.m_velocity, this.MaxVelocity);
			if (Mathf.Abs(this.m_velocity.sqrMagnitude) > this.MinVelocity * this.MinVelocity)
			{
				this.Move();
			}
			else
			{
				this.Reset();
			}
		}

		public void AddForce(Vector3 force)
		{
			force *= this.m_velocityFadeInCap;
			this.m_velocity += force;
			this.m_velocity = Vector3.ClampMagnitude(this.m_velocity, this.MaxVelocity);
			this.Move();
			this.m_done = false;
		}

		public void AddForce(float x, float y, float z)
		{
			this.AddForce(new Vector3(x, y, z));
		}

		protected void Move()
		{
			this.State += this.m_velocity;
			this.State = new Vector3(Mathf.Clamp(this.State.x, this.MinState.x, this.MaxState.x), Mathf.Clamp(this.State.y, this.MinState.y, this.MaxState.y), Mathf.Clamp(this.State.z, this.MinState.z, this.MaxState.z));
		}

		public void Reset()
		{
			this.m_velocity = Vector3.zero;
			this.State = this.RestState;
			this.m_done = true;
		}

		public void Stop()
		{
			this.m_velocity = Vector3.zero;
		}

		public void ForceVelocityFadeIn(float seconds)
		{
			this.m_velocityFadeInLength = seconds;
			this.m_velocityFadeInEndTime = Time.time + seconds;
			this.m_velocityFadeInCap = 0f;
		}

		public Spring.TransformType Modifier;

		protected Spring.TransformDelegate m_transformFunction;

		public Vector3 State = Vector3.zero;

		protected Spring.TransformType m_currentTransformType;

		protected Vector3 m_velocity = Vector3.zero;

		public Vector3 RestState = Vector3.zero;

		public Vector3 Stiffness = new Vector3(0.5f, 0.5f, 0.5f);

		public Vector3 Damping = new Vector3(0.75f, 0.75f, 0.75f);

		protected float m_velocityFadeInCap = 1f;

		protected float m_velocityFadeInEndTime;

		protected float m_velocityFadeInLength;

		public float MaxVelocity = 10000f;

		public float MinVelocity = 1E-07f;

		public Vector3 MaxState = new Vector3(10000f, 10000f, 10000f);

		public Vector3 MinState = new Vector3(-10000f, -10000f, -10000f);

		protected Transform m_transform;

		protected bool m_done;

		public enum TransformType
		{
			Position,
			PositionAdditive,
			Rotation,
			RotationAdditive,
			Scale,
			ScaleAdditive
		}

		protected delegate void TransformDelegate();
	}
}
