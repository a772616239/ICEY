using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityVector3
{
	[TaskCategory("Basic/Vector3")]
	[TaskDescription("Performs a math operation on two Vector3s: Add, Subtract, Multiply, Divide, Min, or Max.")]
	public class Operator : Action
	{
		public override TaskStatus OnUpdate()
		{
			Operator.Operation operation = this.operation;
			if (operation != Operator.Operation.Add)
			{
				if (operation != Operator.Operation.Subtract)
				{
					if (operation == Operator.Operation.Scale)
					{
						this.storeResult.Value = Vector3.Scale(this.firstVector3.Value, this.secondVector3.Value);
					}
				}
				else
				{
					this.storeResult.Value = this.firstVector3.Value - this.secondVector3.Value;
				}
			}
			else
			{
				this.storeResult.Value = this.firstVector3.Value + this.secondVector3.Value;
			}
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.operation = Operator.Operation.Add;
			this.firstVector3 = (this.secondVector3 = (this.storeResult = Vector3.zero));
		}

		[Tooltip("The operation to perform")]
		public Operator.Operation operation;

		[Tooltip("The first Vector3")]
		public SharedVector3 firstVector3;

		[Tooltip("The second Vector3")]
		public SharedVector3 secondVector3;

		[Tooltip("The variable to store the result")]
		public SharedVector3 storeResult;

		public enum Operation
		{
			Add,
			Subtract,
			Scale
		}
	}
}
