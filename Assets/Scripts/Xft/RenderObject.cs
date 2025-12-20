using System;
using UnityEngine;

namespace Xft
{
	public class RenderObject
	{
		protected float Fps
		{
			get
			{
				float num = 1f / (float)this.Node.Owner.Owner.MaxFps;
				if (!this.Node.Owner.Owner.IgnoreTimeScale)
				{
					num *= Time.timeScale;
				}
				return num;
			}
		}

		public virtual void Initialize(EffectNode node)
		{
			this.Node = node;
		}

		public virtual void Reset()
		{
		}

		public virtual void Update(float deltaTime)
		{
		}

		public virtual void ApplyShaderParam(float x, float y)
		{
		}

		public EffectNode Node;
	}
}
