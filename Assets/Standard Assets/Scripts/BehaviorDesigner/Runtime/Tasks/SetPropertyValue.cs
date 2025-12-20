using System;
using System.Reflection;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	[TaskDescription("Sets the property to the value specified. Returns success if the property was set.")]
	[HelpURL("http://www.opsive.com/assets/BehaviorDesigner/documentation.php?id=150")]
	[TaskCategory("Reflection")]
	[TaskIcon("{SkinColor}ReflectionIcon.png")]
	public class SetPropertyValue : Action
	{
		public override TaskStatus OnUpdate()
		{
			if (this.propertyValue == null)
			{
				UnityEngine.Debug.LogWarning("Unable to get field - field value is null");
				return TaskStatus.Failure;
			}
			Type typeWithinAssembly = TaskUtility.GetTypeWithinAssembly(this.componentName.Value);
			if (typeWithinAssembly == null)
			{
				UnityEngine.Debug.LogWarning("Unable to set property - type is null");
				return TaskStatus.Failure;
			}
			Component component = base.GetDefaultGameObject(this.targetGameObject.Value).GetComponent(typeWithinAssembly);
			if (component == null)
			{
				UnityEngine.Debug.LogWarning("Unable to set the property with component " + this.componentName.Value);
				return TaskStatus.Failure;
			}
			PropertyInfo property = component.GetType().GetProperty(this.propertyName.Value);
			property.SetValue(component, this.propertyValue.GetValue(), null);
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.targetGameObject = null;
			this.componentName = null;
			this.propertyName = null;
			this.propertyValue = null;
		}

		[Tooltip("The GameObject to set the property on")]
		public SharedGameObject targetGameObject;

		[Tooltip("The component to set the property on")]
		public SharedString componentName;

		[Tooltip("The name of the property")]
		public SharedString propertyName;

		[Tooltip("The value to set")]
		public SharedVariable propertyValue;
	}
}
