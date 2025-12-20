using System;

namespace GameWorld
{
	public class AssessmentEventArgs : EventArgs
	{
		public AssessmentEventArgs(AssessmentEventArgs.EventType type)
		{
			this.Type = type;
		}

		public readonly AssessmentEventArgs.EventType Type;

		public enum EventType
		{
			AddFlashTime,
			CurrentComboFinish,
			ContinueGame
		}
	}
}
