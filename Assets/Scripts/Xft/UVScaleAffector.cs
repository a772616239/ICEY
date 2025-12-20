using System;
using UnityEngine;

namespace Xft
{
	public class UVScaleAffector : Affector
	{
		public UVScaleAffector(EffectNode node) : base(node, AFFECTORTYPE.UVScaleAffector)
		{
		}

		public override void Update(float deltaTime)
		{
			Vector2 uvdimensions = this.Node.UVDimensions;
			uvdimensions.x += this.Node.Owner.UVScaleXSpeed * deltaTime;
			uvdimensions.y += this.Node.Owner.UVScaleYSpeed * deltaTime;
			this.Node.UVDimensions = uvdimensions;
		}
	}
}
