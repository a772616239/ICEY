using System;
using UnityEngine;

[AddComponentMenu("Advanced Platformer 2D/Material")]
public class APMaterial : BaseBehaviour
{
	public bool m_overrideFriction;

	public float m_dynFriction = 1f;

	public float m_staticFriction = 1f;

	public float m_groundBounciness;

	public APMaterial.BoolValue m_wallJump;

	public APMaterial.BoolValue m_wallSlide;

	public APMaterial.FloatValue m_wallFriction;

	public enum BoolValue
	{
		Default,
		True,
		False
	}

	[Serializable]
	public class FloatValue
	{
		public bool m_override;

		public float m_value;
	}
}
