using System;
using UnityEngine;

namespace SceneStory
{
	[ExecuteInEditMode]
	public class SceneStorySpriteColorLinked : BaseBehaviour
	{
		private void Start()
		{
		}

		private void Update()
		{
			if (this.linkedSprite != null)
			{
				this.linkedSprite.color = this.color;
				if (!this.linkedSprite.gameObject.activeSelf)
				{
					this.linkedSprite.gameObject.SetActive(true);
				}
			}
		}

		public Color color = Color.black;

		public SpriteRenderer linkedSprite;
	}
}
