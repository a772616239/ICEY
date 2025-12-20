using System;
using UnityEngine;

namespace Xft
{
	public class CollisionParam
	{
		public CollisionParam(GameObject obj, Vector3 pos, Vector3 dir)
		{
			this.m_collideObject = obj;
			this.m_collidePos = pos;
			this.m_direction = dir;
		}

		public GameObject CollideObject
		{
			get
			{
				return this.m_collideObject;
			}
			set
			{
				this.m_collideObject = value;
			}
		}

		public Vector3 CollidePos
		{
			get
			{
				return this.m_collidePos;
			}
			set
			{
				this.m_collidePos = value;
			}
		}

		public Vector3 CollideDir
		{
			get
			{
				return this.m_direction;
			}
		}

		protected GameObject m_collideObject;

		protected Vector3 m_collidePos;

		protected Vector3 m_direction;
	}
}
