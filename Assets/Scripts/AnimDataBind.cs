using System;
using UnityEngine;

[ExecuteInEditMode]
public class AnimDataBind : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		this.UpdateTarget();
	}

	[ContextMenu("Update Now")]
	public void UpdateTarget()
	{
		if (this.target != null && this.target.isValid)
		{
			this.target.Set(this.x);
			for (int i = 0; i < this.args.Length; i++)
			{
				this.propertyArgs[i].Set(this.args[i]);
			}
		}
	}

	public float[] args;

	public bool editMode = true;

	public PropertyReference[] propertyArgs;

	public PropertyReference target;

	public float x;
}
