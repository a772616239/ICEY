using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace System.Collections
{
	public class TreeNode<T>
	{
		public TreeNode(T value)
		{
			this._value = value;
		}

		public TreeNode<T> this[int i]
		{
			get
			{
				return this._children[i];
			}
		}

		public TreeNode<T> Parent { get; private set; }

		public T Value
		{
			get
			{
				return this._value;
			}
		}

		public int ChildCount
		{
			get
			{
				return this._children.Count;
			}
		}

		public ReadOnlyCollection<TreeNode<T>> Children
		{
			get
			{
				return this._children.AsReadOnly();
			}
		}

		public TreeNode<T> AddChild(T value)
		{
			TreeNode<T> treeNode = new TreeNode<T>(value)
			{
				Parent = this
			};
			this._children.Add(treeNode);
			return treeNode;
		}

		public TreeNode<T>[] AddChildren(params T[] values)
		{
			return (from v in values
			select this.AddChild(v)).ToArray<TreeNode<T>>();
		}

		public bool RemoveChild(TreeNode<T> node)
		{
			return this._children.Remove(node);
		}

		public void ClearChild()
		{
			this._children.Clear();
		}

		public void Traverse(Action<T> action)
		{
			if (action != null && this.Value != null)
			{
				action(this.Value);
				for (int i = 0; i < this._children.Count; i++)
				{
					TreeNode<T> treeNode = this._children[i];
					treeNode.Traverse(action);
				}
			}
		}

		public IEnumerable<T> Flatten()
		{
			return new T[]
			{
				this.Value
			}.Union(this._children.SelectMany((TreeNode<T> x) => x.Flatten()));
		}

		private readonly T _value;

		private readonly List<TreeNode<T>> _children = new List<TreeNode<T>>();
	}
}
