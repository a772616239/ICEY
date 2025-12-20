using System;
using UnityEngine;

[AddComponentMenu("FantaBlade/添加一个Editor模式显示编辑器")]
public class PropertyInScene : MonoBehaviour
{
	private void OnDrawGizmos()
	{
		if (string.IsNullOrEmpty(this.property))
		{
			this.property = base.name;
		}
		if (this.show)
		{
			DebugX.DrawText(this.property, base.transform.position);
			if (this.showGizmos)
			{
				Color col = this.color;
				DebugX.DrawPoint(base.transform.position, col, 0.1f);
			}
		}
	}

	[SerializeField]
	public bool show = true;

	[SerializeField]
	public bool showGizmos = true;

	[SerializeField]
	public string property;

	[SerializeField]
	public Color color = new Color(0.7f, 0.3f, 0.15f);
}
