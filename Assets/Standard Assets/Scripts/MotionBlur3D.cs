using System;
using UnityEngine;

public class MotionBlur3D : MotionBlurBase
{
	public MotionBlur3D(MotionBlur b, MeshFilter mf) : base(b)
	{
		this.m_rMeshFilter = mf;
	}

	protected override void TakeCareOfParametersUpdate()
	{
		base.TakeCareOfParametersUpdate();
		if (this.m_oGhosts.Count > 0)
		{
			this.TakeCareOfMeshUpdate();
			if (this.m_bUpdateMeshUVRequested)
			{
				this.UpdateMeshUV();
				this.m_bUpdateMeshUVRequested = false;
			}
		}
	}

	private void TakeCareOfMaterialUpdate(MotionBlurGhost a_rFirstGhost)
	{
		if (this.m_rMotionBlur.GetComponent<Renderer>().sharedMaterial != a_rFirstGhost.GetComponent<Renderer>().sharedMaterial)
		{
			foreach (MotionBlurGhost motionBlurGhost in this.m_oGhosts)
			{
				MotionBlurGhost3D motionBlurGhost3D = (MotionBlurGhost3D)motionBlurGhost;
				motionBlurGhost3D.GetComponent<Renderer>().sharedMaterials = this.m_rMotionBlur.GetComponent<Renderer>().sharedMaterials;
			}
		}
	}

	private bool AreMaterialsEqual(Material[] a_rMaterialsA, Material[] a_rMaterialsB)
	{
		if (a_rMaterialsA.Length != a_rMaterialsB.Length)
		{
			return false;
		}
		foreach (Material x in a_rMaterialsA)
		{
			bool flag = false;
			foreach (Material y in a_rMaterialsB)
			{
				if (x == y)
				{
					flag = true;
				}
			}
			if (!flag)
			{
				return false;
			}
		}
		return true;
	}

	private Material[] CopyMaterials(Material[] a_rMaterialsFrom)
	{
		Material[] array = new Material[a_rMaterialsFrom.Length];
		a_rMaterialsFrom.CopyTo(array, 0);
		return array;
	}

	private void UpdateMeshUV()
	{
		foreach (MotionBlurGhost motionBlurGhost in this.m_oGhosts)
		{
			MotionBlurGhost3D motionBlurGhost3D = (MotionBlurGhost3D)motionBlurGhost;
			motionBlurGhost3D.UpdateMeshUV(this.m_rLastSetMeshToGhost);
		}
	}

	public void RequestMeshUVUpdate()
	{
		this.m_bUpdateMeshUVRequested = true;
	}

	private void TakeCareOfMeshUpdate()
	{
		if (this.m_rMeshFilter.sharedMesh != this.m_rLastSetMeshToGhost)
		{
			this.m_rLastSetMeshToGhost = this.m_rMeshFilter.sharedMesh;
			foreach (MotionBlurGhost motionBlurGhost in this.m_oGhosts)
			{
				MotionBlurGhost3D motionBlurGhost3D = (MotionBlurGhost3D)motionBlurGhost;
				motionBlurGhost3D.SetMeshToGhost(this.m_rLastSetMeshToGhost);
			}
		}
	}

	protected override void CreateGhosts(int a_iNumberOfGhostToCreate)
	{
		this.m_rLastSetMeshToGhost = (this.m_rMeshFilter.sharedMesh ?? this.m_rMeshFilter.mesh);
		for (int i = 0; i < a_iNumberOfGhostToCreate; i++)
		{
			GameObject gameObject = new GameObject(this.m_rMotionBlur.gameObject + "_Ghost");
			gameObject.transform.parent = this.m_rMotionBlur.transform;
			Renderer renderer = gameObject.AddComponent<MeshRenderer>();
			renderer.material = this.m_rMotionBlur.mat;
			renderer.enabled = false;
			gameObject.AddComponent<MeshFilter>();
			MotionBlurGhost3D motionBlurGhost3D = gameObject.AddComponent<MotionBlurGhost3D>();
			motionBlurGhost3D.SetMeshToGhost(this.m_rLastSetMeshToGhost);
			this.m_oGhosts.Add(motionBlurGhost3D);
		}
	}

	protected override void PlaceGhostInterpolate(MotionBlurGhost rGhost, MotionBlurBase.TransformSave rTransformSaveBegin, MotionBlurBase.TransformSave rTransformSaveEnd, float fCurrentGhostCurrentTimeRangePositionInPercent)
	{
		base.PlaceGhostInterpolate(rGhost, rTransformSaveBegin, rTransformSaveEnd, fCurrentGhostCurrentTimeRangePositionInPercent);
		rGhost.Show();
	}

	private MeshFilter m_rMeshFilter;

	private Mesh m_rLastSetMeshToGhost;

	private bool m_bUpdateMeshUVRequested;
}
