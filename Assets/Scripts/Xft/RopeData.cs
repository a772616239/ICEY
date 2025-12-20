using System;
using System.Collections.Generic;
using UnityEngine;

namespace Xft
{
	public class RopeData
	{
		public void Init(EffectLayer owner)
		{
			this.Owner = owner;
			this.Vertexsegment = owner.GetVertexPool().GetRopeVertexSeg(owner.MaxENodes);
			this.dummyNode = new EffectNode(0, owner.ClientTransform, false, owner);
			List<Affector> affectorList = owner.InitAffectors(this.dummyNode);
			this.dummyNode.SetAffectorList(affectorList);
			this.dummyNode.SetRenderType(4);
			this.dummyNode.Init(Vector3.zero, 0f, -1f, 0, 1f, 1f, Color.clear, Vector2.zero, Vector2.one);
		}

		protected void RefreshData()
		{
			this.NodeList.Clear();
			for (int i = 0; i < this.Owner.MaxENodes; i++)
			{
				EffectNode effectNode = this.Owner.ActiveENodes[i];
				if (effectNode != null)
				{
					this.NodeList.Add(effectNode);
				}
			}
			this.NodeList.Sort();
		}

		public void Update(float deltaTime)
		{
			this.RefreshData();
			if (this.NodeList.Count < 2)
			{
				return;
			}
			this.dummyNode.Update(deltaTime);
			this.ClearDeadVerts();
			this.UpdateVertices();
			this.UpdateIndices();
		}

		protected void ClearNodeVert(EffectNode node)
		{
			int num = this.Vertexsegment.VertStart + node.Index * 2;
			VertexPool pool = this.Vertexsegment.Pool;
			pool.Vertices[num] = this.Owner.ClientTransform.position;
			pool.Colors[num] = Color.clear;
			pool.Vertices[num + 1] = this.Owner.ClientTransform.position;
			pool.Colors[num + 1] = Color.clear;
			pool.VertChanged = true;
			pool.ColorChanged = true;
		}

		public void ClearDeadVerts()
		{
			for (int i = 0; i < this.Owner.MaxENodes; i++)
			{
				EffectNode effectNode = this.Owner.AvailableENodes[i];
				if (effectNode != null)
				{
					this.ClearNodeVert(effectNode);
				}
			}
			this.Vertexsegment.ClearIndices();
		}

		public void UpdateIndices()
		{
			int num = 0;
			VertexPool pool = this.Vertexsegment.Pool;
			for (int i = this.NodeList.Count - 1; i >= 0; i--)
			{
				EffectNode effectNode = this.NodeList[i];
				EffectNode effectNode2 = (i - 1 < 0) ? null : this.NodeList[i - 1];
				if (effectNode2 == null)
				{
					break;
				}
				int num2 = this.Vertexsegment.VertStart + effectNode.Index * 2;
				int num3 = this.Vertexsegment.VertStart + effectNode2.Index * 2;
				int num4 = this.Vertexsegment.IndexStart + num * 6;
				pool.Indices[num4] = num2;
				pool.Indices[num4 + 1] = num2 + 1;
				pool.Indices[num4 + 2] = num3;
				pool.Indices[num4 + 3] = num2 + 1;
				pool.Indices[num4 + 4] = num3 + 1;
				pool.Indices[num4 + 5] = num3;
				num++;
			}
			pool.IndiceChanged = true;
		}

		public void UpdateVertices()
		{
			float num = 0f;
			Vector2 lowerLeftUV = this.dummyNode.LowerLeftUV;
			Vector2 uvdimensions = this.dummyNode.UVDimensions;
			uvdimensions.y = -uvdimensions.y;
			lowerLeftUV.y = 1f - lowerLeftUV.y;
			float num2 = this.Owner.RopeUVLen;
			if (this.Owner.RopeFixUVLen)
			{
				float num3 = 0f;
				for (int i = 0; i < this.NodeList.Count - 1; i++)
				{
					num3 += (this.NodeList[i + 1].GetWorldPos() - this.NodeList[i].GetWorldPos()).magnitude;
				}
				num2 = num3;
			}
			for (int j = this.NodeList.Count - 1; j >= 0; j--)
			{
				EffectNode effectNode = this.NodeList[j];
				EffectNode effectNode2 = (j + 1 >= this.NodeList.Count) ? null : this.NodeList[j + 1];
				EffectNode effectNode3 = (j - 1 < 0) ? null : this.NodeList[j - 1];
				Vector3 lhs;
				if (effectNode3 == null)
				{
					lhs = effectNode.GetWorldPos() - effectNode2.GetWorldPos();
				}
				else if (effectNode2 == null)
				{
					lhs = effectNode3.GetWorldPos() - effectNode.GetWorldPos();
				}
				else
				{
					lhs = effectNode3.GetWorldPos() - effectNode2.GetWorldPos();
				}
				Vector3 position = this.Owner.MyCamera.transform.position;
				Vector3 rhs = position - effectNode.GetWorldPos();
				Vector3 vector = Vector3.Cross(lhs, rhs);
				vector.Normalize();
				vector *= this.Owner.RopeWidth * 0.5f * effectNode.Scale.x;
				Vector3 vector2 = effectNode.GetWorldPos() - vector;
				Vector3 vector3 = effectNode.GetWorldPos() + vector;
				VertexPool pool = this.Vertexsegment.Pool;
				float num4 = num / num2 * Mathf.Abs(uvdimensions.y);
				Vector2 zero = Vector2.zero;
				int num5 = this.Vertexsegment.VertStart + effectNode.Index * 2;
				pool.Vertices[num5] = vector2;
				pool.Colors[num5] = effectNode.Color;
				zero.x = lowerLeftUV.x + uvdimensions.x;
				zero.y = lowerLeftUV.y - num4;
				pool.UVs[num5] = zero;
				pool.Vertices[num5 + 1] = vector3;
				pool.Colors[num5 + 1] = effectNode.Color;
				zero.x = lowerLeftUV.x;
				zero.y = lowerLeftUV.y - num4;
				pool.UVs[num5 + 1] = zero;
				if (effectNode3 != null)
				{
					num += (effectNode3.GetWorldPos() - effectNode.GetWorldPos()).magnitude;
				}
				else
				{
					num += (effectNode.GetWorldPos() - effectNode2.GetWorldPos()).magnitude;
				}
			}
			this.Vertexsegment.Pool.UVChanged = true;
			this.Vertexsegment.Pool.VertChanged = true;
			this.Vertexsegment.Pool.ColorChanged = true;
		}

		public List<EffectNode> NodeList = new List<EffectNode>();

		public VertexPool.VertexSegment Vertexsegment;

		public EffectLayer Owner;

		public EffectNode dummyNode;
	}
}
