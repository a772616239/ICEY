using System;
using System.Collections;
using UnityEngine;

public class Fractal : MonoBehaviour
{
	private void Start()
	{
		this._root = new TreeNode<Transform>(this.CreateAndSetPrimitive(Vector3.zero, 1f, base.transform));
		this.Repaint(1, this._root);
	}

	private void Update()
	{
	}

	private void Repaint(int level, TreeNode<Transform> parent)
	{
		if (level > this._maxLevel)
		{
			if (parent.ChildCount != 0)
			{
				for (int i = 0; i < parent.Children.Count; i++)
				{
					TreeNode<Transform> treeNode = parent.Children[i];
					treeNode.Traverse(delegate(Transform g)
					{
						if (g != null)
						{
							UnityEngine.Object.Destroy(g.gameObject);
						}
					});
					treeNode.ClearChild();
				}
				parent.ClearChild();
			}
			return;
		}
		for (int j = 0; j < 8; j++)
		{
			if (parent.ChildCount < j + 1)
			{
				parent.AddChild(this.CreateAndSetPrimitive(this.ChildPositions()[j], this._scale, parent.Value));
			}
			else
			{
				this.SetPrimitive(parent[j].Value, this.ChildPositions()[j], this._scale);
			}
			this.Repaint(level + 1, parent[j]);
		}
	}

	private void Clear()
	{
		this._root.Traverse(delegate(Transform g)
		{
			if (g != null)
			{
				UnityEngine.Object.Destroy(g.gameObject);
			}
		});
		this._root.ClearChild();
		this._root = new TreeNode<Transform>(this.CreateAndSetPrimitive(Vector3.zero, 1f, null));
	}

	private Transform CreateAndSetPrimitive(Vector3 position, float scale, Transform parent)
	{
		return this.SetPrimitive(this.CreatePrimitive(parent), position, scale);
	}

	private Transform CreatePrimitive(Transform parent)
	{
		Transform transform = GameObject.CreatePrimitive(this._sharpType).transform;
		transform.GetComponent<Renderer>().sharedMaterial = this.Mat;
		transform.parent = parent;
		return transform;
	}

	private Transform SetPrimitive(Transform primitive, Vector3 position, float scale)
	{
		primitive.transform.localPosition = position;
		primitive.transform.localScale = new Vector3(scale, scale, scale);
		return primitive;
	}

	private Vector3[] ChildPositions()
	{
		Vector3 rootPosition = this._rootPosition;
		Vector3 vector = new Vector3(rootPosition.x - this._distance, rootPosition.y + this._distance, rootPosition.z + this._distance);
		Vector3 vector2 = new Vector3(rootPosition.x + this._distance, rootPosition.y + this._distance, rootPosition.z + this._distance);
		Vector3 vector3 = new Vector3(rootPosition.x - this._distance, rootPosition.y - this._distance, rootPosition.z + this._distance);
		Vector3 vector4 = new Vector3(rootPosition.x + this._distance, rootPosition.y - this._distance, rootPosition.z + this._distance);
		Vector3 vector5 = new Vector3(rootPosition.x - this._distance, rootPosition.y + this._distance, rootPosition.z - this._distance);
		Vector3 vector6 = new Vector3(rootPosition.x + this._distance, rootPosition.y + this._distance, rootPosition.z - this._distance);
		Vector3 vector7 = new Vector3(rootPosition.x - this._distance, rootPosition.y - this._distance, rootPosition.z - this._distance);
		Vector3 vector8 = new Vector3(rootPosition.x + this._distance, rootPosition.y - this._distance, rootPosition.z - this._distance);
		return new Vector3[]
		{
			vector,
			vector2,
			vector3,
			vector4,
			vector5,
			vector6,
			vector7,
			vector8
		};
	}

	[SerializeField]
	private PrimitiveType _sharpType;

	[SerializeField]
	private float _distance = 1f;

	[SerializeField]
	private float _scale = 0.5f;

	[SerializeField]
	private Vector3 _rootPosition;

	[Range(0f, 10f)]
	[SerializeField]
	private int _maxLevel;

	private TreeNode<Transform> _root;

	[SerializeField]
	public Material Mat;
}
