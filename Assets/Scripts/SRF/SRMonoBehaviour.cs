using System;
using System.Diagnostics;
using UnityEngine;

namespace SRF
{
	public abstract class SRMonoBehaviour : MonoBehaviour
	{
		public Transform CachedTransform
		{
			[DebuggerNonUserCode]
			[DebuggerStepThrough]
			get
			{
				if (this._transform == null)
				{
					this._transform = base.transform;
				}
				return this._transform;
			}
		}

		public Collider CachedCollider
		{
			[DebuggerStepThrough]
			[DebuggerNonUserCode]
			get
			{
				if (this._collider == null)
				{
					this._collider = base.GetComponent<Collider>();
				}
				return this._collider;
			}
		}

		public Collider2D CachedCollider2D
		{
			[DebuggerStepThrough]
			[DebuggerNonUserCode]
			get
			{
				if (this._collider2D == null)
				{
					this._collider2D = base.GetComponent<Collider2D>();
				}
				return this._collider2D;
			}
		}

		public Rigidbody CachedRigidBody
		{
			[DebuggerStepThrough]
			[DebuggerNonUserCode]
			get
			{
				if (this._rigidBody == null)
				{
					this._rigidBody = base.GetComponent<Rigidbody>();
				}
				return this._rigidBody;
			}
		}

		public Rigidbody2D CachedRigidBody2D
		{
			[DebuggerStepThrough]
			[DebuggerNonUserCode]
			get
			{
				if (this._rigidbody2D == null)
				{
					this._rigidbody2D = base.GetComponent<Rigidbody2D>();
				}
				return this._rigidbody2D;
			}
		}

		public GameObject CachedGameObject
		{
			[DebuggerStepThrough]
			[DebuggerNonUserCode]
			get
			{
				if (this._gameObject == null)
				{
					this._gameObject = base.gameObject;
				}
				return this._gameObject;
			}
		}

		public new Transform transform
		{
			get
			{
				return this.CachedTransform;
			}
		}

		[DebuggerNonUserCode]
		[DebuggerStepThrough]
		protected void AssertNotNull(object value, string fieldName = null)
		{
			SRDebugUtil.AssertNotNull(value, fieldName, this);
		}

		[DebuggerNonUserCode]
		[DebuggerStepThrough]
		protected void Assert(bool condition, string message = null)
		{
			SRDebugUtil.Assert(condition, message, this);
		}

		[Conditional("UNITY_EDITOR")]
		[DebuggerNonUserCode]
		[DebuggerStepThrough]
		protected void EditorAssertNotNull(object value, string fieldName = null)
		{
			this.AssertNotNull(value, fieldName);
		}

		[Conditional("UNITY_EDITOR")]
		[DebuggerNonUserCode]
		[DebuggerStepThrough]
		protected void EditorAssert(bool condition, string message = null)
		{
			this.Assert(condition, message);
		}

		private Collider _collider;

		private Transform _transform;

		private Rigidbody _rigidBody;

		private GameObject _gameObject;

		private Rigidbody2D _rigidbody2D;

		private Collider2D _collider2D;
	}
}
