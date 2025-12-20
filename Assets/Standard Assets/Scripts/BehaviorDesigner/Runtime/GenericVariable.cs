using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime
{
	[Serializable]
	public class GenericVariable
	{
		public GenericVariable()
		{
			this.value = (Activator.CreateInstance(TaskUtility.GetTypeWithinAssembly("BehaviorDesigner.Runtime.SharedString")) as SharedVariable);
		}

		[SerializeField]
		public string type = "SharedString";

		[SerializeField]
		public SharedVariable value;
	}
}
