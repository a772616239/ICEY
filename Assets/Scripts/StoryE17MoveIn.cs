using System;

public class StoryE17MoveIn : BaseBehaviour
{
	private int E17MoveIn
	{
		get
		{
			return RoundStorage.Get("E17_MoveIn", 0);
		}
		set
		{
			RoundStorage.Set("E17_MoveIn", value);
		}
	}

	private bool E17P1
	{
		get
		{
			return RoundStorage.Get("E17P1", true);
		}
		set
		{
			RoundStorage.Set("E17P1", value);
		}
	}

	private void Start()
	{
		if (!this.E17P1)
		{
			this.E17MoveIn = 1;
		}
	}
}
