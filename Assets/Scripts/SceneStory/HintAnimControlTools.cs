using System;
using UnityEngine;

namespace SceneStory
{
	[ExecuteInEditMode]
	[AddComponentMenu("FantaBlade/剧情动画编辑器/编辑器辅助工具/动画标签提示工具")]
	public class HintAnimControlTools : BaseBehaviour
	{
		public void SetHint(string hint)
		{
			this.hint = hint;
		}

		private void OnDrawGizmos()
		{
			if (string.IsNullOrEmpty(this.hint))
			{
				this.hint = base.name;
			}
			if (this.show)
			{
				DebugX.DrawText(this.hint, base.transform.position);
				Color col = this.color;
				DebugX.DrawPoint(base.transform.position, col, 0.1f);
			}
		}

		[SerializeField]
		private bool show = true;

		[SerializeField]
		private string hint;

		[SerializeField]
		private Color color = new Color(0.7f, 0.3f, 0.15f);
	}
}
