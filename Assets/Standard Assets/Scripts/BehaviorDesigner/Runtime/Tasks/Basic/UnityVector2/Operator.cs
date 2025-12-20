using System;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Basic.UnityVector2
{
	[TaskCategory("Basic/Vector2")]
	[TaskDescription("Performs a math operation on two Vector2s: Add, Subtract, Multiply, Divide, Min, or Max.")]
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
						this.storeResult.Value = Vector2.Scale(this.firstVector2.Value, this.secondVector2.Value);
					}
				}
				else
				{
					this.storeResult.Value = this.firstVector2.Value - this.secondVector2.Value;
				}
			}
			else
			{
				this.storeResult.Value = this.firstVector2.Value + this.secondVector2.Value;
			}
			return TaskStatus.Success;
		}

		public override void OnReset()
		{
			this.operation = Operator.Operation.Add;
			this.firstVector2 = (this.secondVector2 = (this.storeResult = Vector2.zero));
		}

		[Tooltip("The operation to perform")]
		public Operator.Operation operation;

		[Tooltip("The first Vector2")]
		public SharedVector2 firstVector2;

		[Tooltip("The second Vector2")]
		public SharedVector2 secondVector2;

		[Tooltip("The variable to store the result")]
		public SharedVector2 storeResult;

		public enum Operation
		{
			Add,
			Subtract,
			Scale
		}
	}
}
