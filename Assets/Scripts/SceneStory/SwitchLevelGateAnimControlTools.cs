using System;

namespace SceneStory
{
	public class SwitchLevelGateAnimControlTools : BaseBehaviour
	{
		public void SwitchLevel()
		{
			if (R.Mode.IsInBattleMode())
			{
				R.Mode.ExitMode(Mode.AllMode.Battle);
			}
			this.levelGate.Enter(false);
		}

		public SceneGate levelGate;
	}
}
