using System;
using UnityEngine;

[AddComponentMenu("MotionBlur/MotionBlur")]
public class MotionBlur : MonoBehaviour
{
	private void Awake()
	{
		if (base.GetComponent<SpriteRenderer>() != null)
		{
			this.m_oMotionBlurBase = new MotionBlur2D(this, base.GetComponent<SpriteRenderer>());
		}
		else if (base.GetComponent<MeshFilter>() != null)
		{
			this.m_oMotionBlurBase = new MotionBlur3D(this, base.GetComponent<MeshFilter>());
		}
		else
		{
			UnityEngine.Debug.LogError("The motion blur cannot be apply on an object without renderer.");
		}
		this.m_oMotionBlurBase.Awake();
	}

	private void OnDisable()
	{
		this.m_oMotionBlurBase.OnDisable();
	}

	private void LateUpdate()
	{
		this.m_oMotionBlurBase.LateUpdate();
	}

	public MotionBlurBase.EMotionBlurMode movementMode;

	public float persistenceDuration = 0.25f;

	public int quality = 25;

	public float opacity = 1f;

	public MotionBlurBase.EFadeInType fadeType = MotionBlurBase.EFadeInType.Circular;

	public Vector3 localVelocity;

	public Vector3 offset = new Vector3(0f, 0f, 1E-05f);

	public bool progressiveSpriteColor;

	public bool progressiveScale = true;

	private MotionBlurBase m_oMotionBlurBase;

	[HideInInspector]
	public Material mat;
}
