using System;

public interface ILevelItem
{
	void OnReset();

	void OnPause(bool isPause);

	void OnEntertBlock(ILevelBlock block);

	void OnLeaveBlock(ILevelBlock block);

	void OnPickingTime(float time);
}
