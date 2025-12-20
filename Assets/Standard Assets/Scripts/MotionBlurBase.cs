using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class MotionBlurBase
{
	public MotionBlurBase(MotionBlur b)
	{
		this.m_rMotionBlur = b;
	}

	public void Awake()
	{
		this.CreateGhosts(this.m_rMotionBlur.quality);
	}

	protected virtual void TakeCareOfParametersUpdate()
	{
		int count = this.m_oGhosts.Count;
		int quality = this.m_rMotionBlur.quality;
		if (quality > count)
		{
			this.DestroyGhosts(count);
			this.CreateGhosts(quality);
		}
		else if (quality < count)
		{
			this.DestroyGhosts(count - quality);
		}
	}

	public void OnDisable()
	{
		this.HideGhosts();
		this.m_oTransformHistory.Clear();
	}

	public void LateUpdate()
	{
		this.TakeCareOfParametersUpdate();
		this.UpdateTransformHistory();
		this.UpdateGhosts();
	}

	private void DestroyGhosts(int a_iNumberOfGhostToDestroy)
	{
		a_iNumberOfGhostToDestroy = Mathf.Clamp(a_iNumberOfGhostToDestroy, 0, this.m_oGhosts.Count);
		for (int i = 0; i < a_iNumberOfGhostToDestroy; i++)
		{
			UnityEngine.Object.Destroy(this.m_oGhosts[i].gameObject);
		}
		this.m_oGhosts.RemoveRange(0, a_iNumberOfGhostToDestroy);
	}

	protected virtual void UpdateTransformHistory()
	{
		Vector3 vector = this.m_rMotionBlur.localVelocity * Time.deltaTime;
		if (vector != Vector3.zero)
		{
			foreach (MotionBlurBase.TransformSave transformSave in this.m_oTransformHistory)
			{
				transformSave.position += vector;
			}
		}
		this.m_oTransformHistory.Add(new MotionBlurBase.TransformSave(this.m_rMotionBlur.transform));
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

	protected void ApplyGhostFade(MotionBlurGhost a_rGhost, float a_fPercentFade)
	{
		a_rGhost.SetOpacity(MotionBlurInterpolate.Ease(this.GetFadeEaseType())(0f, 1f, 1f - a_fPercentFade, 1f) * this.m_rMotionBlur.opacity);
	}

	private MotionBlurInterpolate.EaseType GetFadeEaseType()
	{
		switch (this.m_rMotionBlur.fadeType)
		{
		case MotionBlurBase.EFadeInType.Linear:
			return MotionBlurInterpolate.EaseType.Linear;
		case MotionBlurBase.EFadeInType.Circular:
			return MotionBlurInterpolate.EaseType.EaseInCirc;
		case MotionBlurBase.EFadeInType.Exponential:
			return MotionBlurInterpolate.EaseType.EaseInExpo;
		default:
			return MotionBlurInterpolate.EaseType.EaseInCirc;
		}
	}

	protected void UpdateGhosts()
	{
		int num = 0;
		MotionBlurBase.EMotionBlurMode movementMode = this.m_rMotionBlur.movementMode;
		if (movementMode != MotionBlurBase.EMotionBlurMode.World)
		{
			if (movementMode == MotionBlurBase.EMotionBlurMode.Local)
			{
				this.UpdateGhostLocalMode(ref num);
			}
		}
		else
		{
			this.UpdateGhostWorldMode(ref num);
		}
		this.HideUnusedGhost(ref num);
	}

	private void HideUnusedGhost(ref int a_iCurrentGhostIndex)
	{
		int count = this.m_oGhosts.Count;
		while (a_iCurrentGhostIndex < count)
		{
			MotionBlurGhost motionBlurGhost = this.m_oGhosts[a_iCurrentGhostIndex];
			motionBlurGhost.Hide();
			a_iCurrentGhostIndex++;
		}
	}

	private void UpdateGhostLocalMode(ref int a_iCurrentGhostIndex)
	{
		MotionBlurBase.TransformSave rTransformSaveBegin = new MotionBlurBase.TransformSave(this.m_rMotionBlur.transform);
		MotionBlurBase.TransformSave transformSave = new MotionBlurBase.TransformSave(this.m_rMotionBlur.transform);
		transformSave.position += this.m_rMotionBlur.persistenceDuration * this.m_rMotionBlur.localVelocity;
		transformSave.saveTime -= this.m_rMotionBlur.persistenceDuration;
		this.PlaceGhost(rTransformSaveBegin, transformSave, ref a_iCurrentGhostIndex);
	}

	protected abstract void CreateGhosts(int a_iNumberOfGhostToCreate);

	protected virtual float PlaceGhost(MotionBlurBase.TransformSave rTransformSaveBegin, MotionBlurBase.TransformSave rTransformSaveEnd, ref int a_iCurrentGhostIndex)
	{
		float num = this.m_rMotionBlur.persistenceDuration / (float)this.m_rMotionBlur.quality;
		float saveTime = rTransformSaveBegin.saveTime;
		float saveTime2 = rTransformSaveEnd.saveTime;
		float num2 = saveTime - saveTime2;
		float num3 = (float)a_iCurrentGhostIndex * num;
		float time = Time.time;
		float num4 = time - num3;
		float num5 = num4 - saveTime2;
		int num6 = 1 + Mathf.FloorToInt(num5 / num);
		num6 = Mathf.Clamp(num6, 0, this.m_oGhosts.Count - a_iCurrentGhostIndex);
		for (int i = 0; i < num6; i++)
		{
			num3 = (float)a_iCurrentGhostIndex * num;
			num4 = time - num3;
			float num7 = (saveTime - num4) / num2;
			MotionBlurGhost motionBlurGhost = this.m_oGhosts[a_iCurrentGhostIndex];
			num7 = Mathf.Max(num7, 0f);
			this.PlaceGhostInterpolate(motionBlurGhost, rTransformSaveBegin, rTransformSaveEnd, num7);
			motionBlurGhost.transform.position += this.m_rMotionBlur.offset;
			this.ApplyGhostFade(motionBlurGhost, num3 / this.m_rMotionBlur.persistenceDuration);
			a_iCurrentGhostIndex++;
		}
		return num4 - saveTime2;
	}

	protected virtual void PlaceGhostInterpolate(MotionBlurGhost rGhost, MotionBlurBase.TransformSave rTransformSaveBegin, MotionBlurBase.TransformSave rTransformSaveEnd, float fCurrentGhostCurrentTimeRangePositionInPercent)
	{
		MotionBlurBase.TransformSave.LoadInterpolatedTransform(rGhost.transform, rTransformSaveBegin, rTransformSaveEnd, fCurrentGhostCurrentTimeRangePositionInPercent);
		if (!this.m_rMotionBlur.progressiveScale)
		{
			rGhost.SetTransformToGhost(this.m_rMotionBlur.transform);
		}
	}

	private void UpdateGhostWorldMode(ref int a_iCurrentGhostIndex)
	{
		float num = this.m_rMotionBlur.persistenceDuration / (float)this.m_rMotionBlur.quality;
		int count = this.m_oTransformHistory.Count;
		float num2 = 0f;
		for (int i = count - 2; i >= 0; i--)
		{
			MotionBlurBase.TransformSave transformSave = this.m_oTransformHistory[i + 1];
			MotionBlurBase.TransformSave transformSave2 = this.m_oTransformHistory[i];
			float saveTime = transformSave.saveTime;
			float saveTime2 = transformSave2.saveTime;
			float num3 = saveTime - saveTime2;
			if (num2 + num3 >= num)
			{
				num2 = this.PlaceGhost(transformSave, transformSave2, ref a_iCurrentGhostIndex);
			}
			else
			{
				num2 += num3;
			}
		}
	}

	private void HideGhosts()
	{
		foreach (MotionBlurGhost motionBlurGhost in this.m_oGhosts)
		{
			motionBlurGhost.Hide();
		}
	}

	public MotionBlur m_rMotionBlur;

	protected List<MotionBlurBase.TransformSave> m_oTransformHistory = new List<MotionBlurBase.TransformSave>();

	protected List<MotionBlurGhost> m_oGhosts = new List<MotionBlurGhost>();

	protected class TransformSave
	{
		public TransformSave(Transform a_rTransform)
		{
			this.SaveFromTransform(a_rTransform);
		}

		public void SaveFromTransform(Transform a_rTransform)
		{
			this.position = a_rTransform.position;
			this.rotation = a_rTransform.rotation;
			this.lossyScale = a_rTransform.lossyScale;
			this.saveTime = Time.time;
		}

		public virtual void LoadToTransform(Transform a_rTransform)
		{
			a_rTransform.position = this.position;
			a_rTransform.rotation = this.rotation;
			Transform parent = a_rTransform.parent;
			a_rTransform.parent = null;
			a_rTransform.localScale = this.lossyScale;
			a_rTransform.parent = parent;
		}

		public static void LoadInterpolatedTransform(Transform a_rTransform, MotionBlurBase.TransformSave a_rTransformBegin, MotionBlurBase.TransformSave a_rTransformEnd, float a_fInterpolationPercent)
		{
			a_rTransform.position = a_rTransformBegin.position + (a_rTransformEnd.position - a_rTransformBegin.position) * a_fInterpolationPercent;
			a_rTransform.rotation = a_rTransformBegin.rotation;
			Transform parent = a_rTransform.parent;
			a_rTransform.parent = null;
			a_rTransform.localScale = a_rTransformBegin.lossyScale;
			a_rTransform.parent = parent;
		}

		public Vector3 position;

		public Quaternion rotation;

		public Vector3 lossyScale;

		public float saveTime;
	}

	public enum EMotionBlurMode
	{
		World,
		Local
	}

	public enum EFadeInType
	{
		Linear,
		Circular,
		Exponential
	}
}
