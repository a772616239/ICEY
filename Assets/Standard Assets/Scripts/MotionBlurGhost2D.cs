using System;
using UnityEngine;

[AddComponentMenu("")]
public class MotionBlurGhost2D : MotionBlurGhost
{
	public void SetSpriteRendererToGhost(SpriteRenderer a_rMeshToGhost)
	{
		this.m_rSpriteRendererFilter = MotionBlurGhost.CopyComponent(a_rMeshToGhost, base.gameObject);
	}

	public override void SetOpacity(float a_fOpacity)
	{
		this.m_rPreviousColor = this.m_rSpriteRendererFilter.color;
		Color color = this.m_rSpriteRendererFilter.color;
		color.a = a_fOpacity;
		this.m_rSpriteRendererFilter.color = color;
	}

	public override void Hide()
	{
		this.SetOpacity(0f);
	}

	public override void Show()
	{
		this.SetOpacity(this.m_rPreviousColor.a);
	}

	public SpriteRenderer m_rSpriteRendererFilter;

	private Color m_rPreviousColor;
}
