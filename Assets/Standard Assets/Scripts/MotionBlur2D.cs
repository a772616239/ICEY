using System;
using UnityEngine;

public class MotionBlur2D : MotionBlurBase
{
	public MotionBlur2D(MotionBlur b, SpriteRenderer sr) : base(b)
	{
		this.m_rSpriteRenderer = sr;
	}

	protected override void CreateGhosts(int a_iNumberOfGhostToCreate)
	{
		for (int i = 0; i < a_iNumberOfGhostToCreate; i++)
		{
			MotionBlurGhost2D motionBlurGhost2D = new GameObject(this.m_rMotionBlur.gameObject + "_Ghost")
			{
				transform = 
				{
					parent = this.m_rMotionBlur.transform
				}
			}.AddComponent<MotionBlurGhost2D>();
			motionBlurGhost2D.SetSpriteRendererToGhost(this.m_rSpriteRenderer);
			motionBlurGhost2D.Hide();
			this.m_oGhosts.Add(motionBlurGhost2D);
		}
		foreach (MotionBlurGhost motionBlurGhost in this.m_oGhosts)
		{
			MotionBlurGhost2D motionBlurGhost2D2 = (MotionBlurGhost2D)motionBlurGhost;
			motionBlurGhost2D2.GetComponent<Renderer>().enabled = false;
		}
		this.m_rMotionBlur.GetComponent<Renderer>().enabled = false;
		this.m_rMotionBlur.GetComponent<Renderer>().enabled = true;
		foreach (MotionBlurGhost motionBlurGhost2 in this.m_oGhosts)
		{
			MotionBlurGhost2D motionBlurGhost2D3 = (MotionBlurGhost2D)motionBlurGhost2;
			motionBlurGhost2D3.GetComponent<Renderer>().enabled = true;
		}
	}

	protected override void UpdateTransformHistory()
	{
		Vector3 vector = this.m_rMotionBlur.localVelocity * Time.deltaTime;
		if (vector != Vector3.zero)
		{
			foreach (MotionBlurBase.TransformSave transformSave in this.m_oTransformHistory)
			{
				transformSave.position += vector;
			}
		}
		this.m_oTransformHistory.Add(new MotionBlur2D.TransformSave2D(this.m_rMotionBlur.transform, this.m_rSpriteRenderer));
		float num = Time.time - this.m_rMotionBlur.persistenceDuration;
		while (this.m_oTransformHistory.Count > 2)
		{
			MotionBlurBase.TransformSave transformSave2 = this.m_oTransformHistory[1];
			if (transformSave2.saveTime >= num)
			{
				break;
			}
			this.m_oTransformHistory.RemoveAt(0);
		}
	}

	protected override void PlaceGhostInterpolate(MotionBlurGhost rGhost, MotionBlurBase.TransformSave rTransformSaveBegin, MotionBlurBase.TransformSave rTransformSaveEnd, float fCurrentGhostCurrentTimeRangePositionInPercent)
	{
		base.PlaceGhostInterpolate(rGhost, rTransformSaveBegin, rTransformSaveEnd, fCurrentGhostCurrentTimeRangePositionInPercent);
		if (this.m_rMotionBlur.progressiveSpriteColor)
		{
			MotionBlur2D.TransformSave2D.LoadInterpolatedTransform(((MotionBlurGhost2D)rGhost).m_rSpriteRendererFilter, (MotionBlur2D.TransformSave2D)rTransformSaveBegin, (MotionBlur2D.TransformSave2D)rTransformSaveEnd, fCurrentGhostCurrentTimeRangePositionInPercent);
		}
		else
		{
			((MotionBlurGhost2D)rGhost).SetSpriteRendererToGhost(this.m_rSpriteRenderer);
		}
	}

	private SpriteRenderer m_rSpriteRenderer;

	protected class TransformSave2D : MotionBlurBase.TransformSave
	{
		public TransformSave2D(Transform a_rTransform, SpriteRenderer a_rRenderer) : base(a_rTransform)
		{
			this.color = a_rRenderer.color;
			this.sprite = a_rRenderer.sprite;
			this.sortingOrder = a_rRenderer.sortingOrder;
			this.sortingLayerID = a_rRenderer.sortingLayerID;
		}

		public static void LoadInterpolatedTransform(SpriteRenderer a_rSpriteRenderer, MotionBlur2D.TransformSave2D a_rTransformBegin, MotionBlur2D.TransformSave2D a_rTransformEnd, float a_fInterpolationPercent)
		{
			a_rSpriteRenderer.color = a_rTransformEnd.color;
			a_rSpriteRenderer.sprite = a_rTransformEnd.sprite;
			a_rSpriteRenderer.sortingLayerID = a_rTransformEnd.sortingLayerID;
			a_rSpriteRenderer.sortingOrder = a_rTransformEnd.sortingOrder;
		}

		private Color color;

		private Sprite sprite;

		private int sortingLayerID;

		private int sortingOrder;
	}
}
