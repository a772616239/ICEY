using System;
using UnityEngine;

namespace BasicTool
{
	[ExecuteInEditMode]
	public class Scene2DT3D : MonoBehaviour
	{
		private void Update()
		{
		}

		private void DealFlat(Transform t)
		{
			if (t.GetComponent<FlatTag>() != null)
			{
				if (this.isFlat)
				{
					t.localScale = new Vector3(t.localScale.x, t.localScale.y, this.flat);
				}
				else
				{
					t.localScale = new Vector3(t.localScale.x, t.localScale.y, 1f);
				}
			}
		}

		private void DealScale(Transform t)
		{
			if (t.GetComponent<ScaleTag>() != null)
			{
				float z = t.position.z;
				if (this.isScale)
				{
					t.localScale = new Vector3(z / 10f + 1f, z / 10f + 1f, t.localScale.z);
				}
				else
				{
					t.localScale = new Vector3(1f, 1f, t.localScale.z);
				}
			}
		}

		public bool isFlat = true;

		public bool isScale = true;

		public float flat = 0.001f;

		public bool open;
	}
}
