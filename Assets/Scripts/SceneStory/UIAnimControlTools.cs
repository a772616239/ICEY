using System;

namespace SceneStory
{
	public class UIAnimControlTools : BaseBehaviour
	{
		public void DisappearUI()
		{
			R.Ui.HideUI(true);
		}

		public void ShowUI()
		{
			R.Ui.ShowUI(true);
		}

		public void FadeOutUI()
		{
			R.Ui.HideUI(false);
		}

		public void FadeInUI()
		{
			R.Ui.ShowUI(false);
		}

		public void EnterStoryMode()
		{
			R.Mode.EnterMode(Mode.AllMode.Story);
		}

		public void ExitStoryMode()
		{
			R.Mode.ExitMode(Mode.AllMode.Story);
		}

		public void UIFadeOutAndInStoryMode()
		{
			this.FadeOutUI();
			this.EnterStoryMode();
		}

		public void UIFadeInAndOutStoryMode()
		{
			this.FadeInUI();
			this.ExitStoryMode();
		}

		public void UIEnterMovieMode()
		{
			R.Ui.EnterMovieMode();
		}

		public void UIExitMovieMode()
		{
			R.Ui.ExitMovieMode();
		}
	}
}
