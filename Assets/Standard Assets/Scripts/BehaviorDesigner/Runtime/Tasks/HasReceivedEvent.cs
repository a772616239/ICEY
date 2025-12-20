using System;

namespace BehaviorDesigner.Runtime.Tasks
{
	[TaskDescription("Returns success as soon as the event specified by eventName has been received.")]
	[HelpURL("http://www.opsive.com/assets/BehaviorDesigner/documentation.php?id=123")]
	[TaskIcon("{SkinColor}HasReceivedEventIcon.png")]
	public class HasReceivedEvent : Conditional
	{
		public override void OnStart()
		{
			if (!this.registered)
			{
				base.Owner.RegisterEvent(this.eventName.Value, new System.Action(this.ReceivedEvent));
				base.Owner.RegisterEvent<object>(this.eventName.Value, new System.Action<object>(this.ReceivedEvent));
				base.Owner.RegisterEvent<object, object>(this.eventName.Value, new System.Action<object, object>(this.ReceivedEvent));
				base.Owner.RegisterEvent<object, object, object>(this.eventName.Value, new Action<object, object, object>(this.ReceivedEvent));
				this.registered = true;
			}
		}

		public override TaskStatus OnUpdate()
		{
			return (!this.eventReceived) ? TaskStatus.Failure : TaskStatus.Success;
		}

		public override void OnEnd()
		{
			if (this.eventReceived)
			{
				base.Owner.UnregisterEvent(this.eventName.Value, new System.Action(this.ReceivedEvent));
				base.Owner.UnregisterEvent<object>(this.eventName.Value, new System.Action<object>(this.ReceivedEvent));
				base.Owner.UnregisterEvent<object, object>(this.eventName.Value, new System.Action<object, object>(this.ReceivedEvent));
				base.Owner.UnregisterEvent<object, object, object>(this.eventName.Value, new Action<object, object, object>(this.ReceivedEvent));
				this.registered = false;
			}
			this.eventReceived = false;
		}

		private void ReceivedEvent()
		{
			this.eventReceived = true;
		}

		private void ReceivedEvent(object arg1)
		{
			this.ReceivedEvent();
			if (this.storedValue1 != null && !this.storedValue1.IsNone)
			{
				this.storedValue1.SetValue(arg1);
			}
		}

		private void ReceivedEvent(object arg1, object arg2)
		{
			this.ReceivedEvent();
			if (this.storedValue1 != null && !this.storedValue1.IsNone)
			{
				this.storedValue1.SetValue(arg1);
			}
			if (this.storedValue2 != null && !this.storedValue2.IsNone)
			{
				this.storedValue2.SetValue(arg2);
			}
		}

		private void ReceivedEvent(object arg1, object arg2, object arg3)
		{
			this.ReceivedEvent();
			if (this.storedValue1 != null && !this.storedValue1.IsNone)
			{
				this.storedValue1.SetValue(arg1);
			}
			if (this.storedValue2 != null && !this.storedValue2.IsNone)
			{
				this.storedValue2.SetValue(arg2);
			}
			if (this.storedValue3 != null && !this.storedValue3.IsNone)
			{
				this.storedValue3.SetValue(arg3);
			}
		}

		public override void OnBehaviorComplete()
		{
			base.Owner.UnregisterEvent(this.eventName.Value, new System.Action(this.ReceivedEvent));
			base.Owner.UnregisterEvent<object>(this.eventName.Value, new System.Action<object>(this.ReceivedEvent));
			base.Owner.UnregisterEvent<object, object>(this.eventName.Value, new System.Action<object, object>(this.ReceivedEvent));
			base.Owner.UnregisterEvent<object, object, object>(this.eventName.Value, new Action<object, object, object>(this.ReceivedEvent));
			this.eventReceived = false;
			this.registered = false;
		}

		public override void OnReset()
		{
			this.eventName = string.Empty;
		}

		[Tooltip("The name of the event to receive")]
		public SharedString eventName = string.Empty;

		[Tooltip("Optionally store the first sent argument")]
		[SharedRequired]
		public SharedVariable storedValue1;

		[Tooltip("Optionally store the second sent argument")]
		[SharedRequired]
		public SharedVariable storedValue2;

		[Tooltip("Optionally store the third sent argument")]
		[SharedRequired]
		public SharedVariable storedValue3;

		private bool eventReceived;

		private bool registered;
	}
}
