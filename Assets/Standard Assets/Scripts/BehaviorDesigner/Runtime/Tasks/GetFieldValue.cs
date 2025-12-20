using System;
using System.Reflection;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
	[TaskDescription("Gets the value from the field specified. Returns success if the field was retrieved.")]
	[HelpURL("http://www.opsive.com/assets/BehaviorDesigner/documentation.php?id=147")]
	[TaskCategory("Reflection")]
	[TaskIcon("{SkinColor}ReflectionIcon.png")]
	public class GetFieldValue : Action
	{
		public override TaskStatus OnUpdate()
		{
			if (this.fieldValue == null)
			{
				UnityEngine.Debug.LogWarning("Unable to get field - field value is null");
				return TaskStatus.Failure;
			}
			Type typeWithinAssembly = TaskUtility.GetTypeWithinAssembly(this.componentName.Value);
			if (typeWithinAssembly == null)
			{
				UnityEngine.Debug.LogWarning("Unable to get field - type is null");
				return TaskStatus.Failure;
			}
			Component component = base.GetDefaultGameObject(this.targetGameObject.Value).GetComponent(typeWithinAssembly);
			if (component == null)
			{
				UnityEngine.Debug.LogWarning("Unable to get the field with component " + this.componentName.Value);
				return TaskStatus.Failure;
			}
			FieldInfo field = component.GetType().GetField(this.fieldName.Value);
			this.fieldValue.SetValue(field.GetValue(component));
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.targetGameObject = null;
			this.componentName = null;
			this.fieldName = null;
			this.fieldValue = null;
		}

		[Tooltip("The GameObject to get the field on")]
		public SharedGameObject targetGameObject;

		[Tooltip("The component to get the field on")]
		public SharedString componentName;

		[Tooltip("The name of the field")]
		public SharedString fieldName;

		[Tooltip("The value of the field")]
		[RequiredField]
		public SharedVariable fieldValue;
	}
}
