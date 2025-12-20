using System;
using System.Collections;
using System.Runtime.CompilerServices;

namespace MonsterLove.StateMachine
{
	public class StateMapping
	{
		public StateMapping(Enum state)
		{
			if (StateMapping._003C_003Ef__mg_0024cache0 == null)
			{
				StateMapping._003C_003Ef__mg_0024cache0 = new StateMapping.DoNothingCoroutine(StateMachineEngine.DoNothingCoroutine);
			}
			this.Enter = StateMapping._003C_003Ef__mg_0024cache0;
			if (StateMapping._003C_003Ef__mg_0024cache1 == null)
			{
				StateMapping._003C_003Ef__mg_0024cache1 = new StateMapping.DoNothingCoroutine(StateMachineEngine.DoNothingCoroutine);
			}
			this.Exit = StateMapping._003C_003Ef__mg_0024cache1;
			if (StateMapping._003C_003Ef__mg_0024cache2 == null)
			{
				StateMapping._003C_003Ef__mg_0024cache2 = new Action(StateMachineEngine.DoNothing);
			}
			this.Update = StateMapping._003C_003Ef__mg_0024cache2;
			if (StateMapping._003C_003Ef__mg_0024cache3 == null)
			{
				StateMapping._003C_003Ef__mg_0024cache3 = new Action(StateMachineEngine.DoNothing);
			}
			this.LateUpdate = StateMapping._003C_003Ef__mg_0024cache3;
			if (StateMapping._003C_003Ef__mg_0024cache4 == null)
			{
				StateMapping._003C_003Ef__mg_0024cache4 = new Action(StateMachineEngine.DoNothing);
			}
			this.FixedUpdate = StateMapping._003C_003Ef__mg_0024cache4;
			
			this.state = state;
		}

		public Enum state;

		public StateMapping.DoNothingCoroutine Enter;

		public StateMapping.DoNothingCoroutine Exit;

		public Action Update;

		public Action LateUpdate;

		public Action FixedUpdate;

		[CompilerGenerated]
		private static StateMapping.DoNothingCoroutine _003C_003Ef__mg_0024cache0;

		[CompilerGenerated]
		private static StateMapping.DoNothingCoroutine _003C_003Ef__mg_0024cache1;

		[CompilerGenerated]
		private static Action _003C_003Ef__mg_0024cache2;

		[CompilerGenerated]
		private static Action _003C_003Ef__mg_0024cache3;

		[CompilerGenerated]
		private static Action _003C_003Ef__mg_0024cache4;

		public delegate IEnumerator DoNothingCoroutine(object userData = null);

		public delegate void DoNothing(object userData = null);
	}
}
