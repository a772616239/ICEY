using System;

public class AIAnimControlTools : BaseBehaviour
{
	public void AIOpen()
	{
		R.SceneData.CanAIRun = true;
	}

	public void AIClose()
	{
		R.SceneData.CanAIRun = false;
	}
}
