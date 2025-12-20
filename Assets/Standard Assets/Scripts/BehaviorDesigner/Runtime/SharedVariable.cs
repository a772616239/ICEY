using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime
{
	public abstract class SharedVariable
	{
		public bool IsShared
		{
			get
			{
				return this.mIsShared;
			}
			set
			{
				this.mIsShared = value;
			}
		}

		public bool IsGlobal
		{
			get
			{
				return this.mIsGlobal;
			}
			set
			{
				this.mIsGlobal = value;
			}
		}

		public string Name
		{
			get
			{
				return this.mName;
			}
			set
			{
				this.mName = value;
			}
		}

		public string PropertyMapping
		{
			get
			{
				return this.mPropertyMapping;
			}
			set
			{
				this.mPropertyMapping = value;
			}
		}

		public GameObject PropertyMappingOwner
		{
			get
			{
				return this.mPropertyMappingOwner;
			}
			set
			{
				this.mPropertyMappingOwner = value;
			}
		}

		public bool NetworkSync
		{
			get
			{
				return this.mNetworkSync;
			}
			set
			{
				this.mNetworkSync = value;
			}
		}

		public bool IsNone
		{
			get
			{
				return this.mIsShared && string.IsNullOrEmpty(this.mName);
			}
		}

		public void ValueChanged()
		{
		}

		public virtual void InitializePropertyMapping(BehaviorSource behaviorSource)
		{
		}

		public abstract object GetValue();

		public abstract void SetValue(object value);

		[SerializeField]
		private bool mIsShared;

		[SerializeField]
		private bool mIsGlobal;

		[SerializeField]
		private string mName;

		[SerializeField]
		private string mPropertyMapping;

		[SerializeField]
		private GameObject mPropertyMappingOwner;

		[SerializeField]
		private bool mNetworkSync;
	}
}
