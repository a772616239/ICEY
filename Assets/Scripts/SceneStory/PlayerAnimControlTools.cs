using System;

namespace SceneStory
{
	public class PlayerAnimControlTools : BaseBehaviour
	{
		private PlayerAction pac
		{
			get
			{
				return R.Player.Action;
			}
		}

		public void ControlPlayerStartMove()
		{
			this.move = true;
		}

		public void ControlPlayerStopMove()
		{
			this.move = false;
			if (this.pac != null)
			{
				this.pac.pab.move.Move(0);
			}
		}

		public void ControlPlayerIsShow(PlayerAnimControlTools.BoolEnum show)
		{
			if (this.pac != null)
			{
				Log.Warning("没找到玩家");
			}
			this.pac.gameObject.SetActive(show == PlayerAnimControlTools.BoolEnum.True);
		}

		public void ControlPlayerTurnRound(PlayerAnimControlTools.Dir dir)
		{
			if (this.pac != null)
			{
				this.pac.TurnRound((int)dir);
			}
		}

		public void InputPause()
		{
			InputSetting.Stop(false);
		}

		public void InputResume()
		{
			InputSetting.Resume(false);
		}

		private void Update()
		{
			if (this.move)
			{
				this.pac.pab.move.Move(3);
			}
		}

		private bool move;

		public enum BoolEnum
		{
			True = 1,
			False = 0
		}

		public enum Dir
		{
			Left = -1,
			Right = 1
		}
	}
}
