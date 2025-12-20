using System;
using UnityEngine;

[AddComponentMenu("")]
public class MotionBlurGhost3D : MotionBlurGhost
{
	private void Awake()
	{
		this.m_rRenderer = base.GetComponent<Renderer>();
		this.CreateGhosthMeshIfNeeded();
	}

	public void SetMeshToGhost(Mesh a_rMeshToGhost)
	{
		this.CreateGhosthMeshIfNeeded();
		MotionBlurMeshUtility.CopyMesh(a_rMeshToGhost, ref this.m_oGhostMesh);
		this.m_oVertexColors = (this.m_oGhostMesh.colors.Clone() as Color[]);
		if (this.m_oVertexColors.Length <= 0)
		{
			this.m_oVertexColors = new Color[a_rMeshToGhost.vertexCount];
			for (int i = 0; i < this.m_oVertexColors.Length; i++)
			{
				this.m_oVertexColors[i] = Color.white;
			}
		}
	}

	public void UpdateMeshUV(Mesh a_rMeshToGhost)
	{
		this.m_oGhostMesh.uv = a_rMeshToGhost.uv;
	}

	public override void SetOpacity(float a_fOpacity)
	{
		for (int i = 0; i < this.m_oVertexColors.Length; i++)
		{
			this.m_oVertexColors[i].a = a_fOpacity;
		}
		this.m_oGhostMesh.colors = this.m_oVertexColors;
	}

	public override void Show()
	{
		if (!this.m_rRenderer.enabled)
		{
			this.m_rRenderer.enabled = true;
		}
	}

	public override void Hide()
	{
		if (this.m_rRenderer != null && this.m_rRenderer.enabled)
		{
			this.m_rRenderer.enabled = false;
		}
	}

	private void CreateGhosthMeshIfNeeded()
	{
		if (this.m_oGhostMesh == null)
		{
			this.m_oGhostMesh = new Mesh();
			MeshFilter component = base.GetComponent<MeshFilter>();
			component.sharedMesh = this.m_oGhostMesh;
		}
	}

	private Renderer m_rRenderer;

	private Color[] m_oVertexColors;

	private Mesh m_oGhostMesh;
}
