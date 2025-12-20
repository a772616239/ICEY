using System;
using System.Collections.Generic;
using System.Linq;
using CIS;
using UnityEngine;

public class LevelBlock : MonoBehaviour, ILevelBlock
{
	public virtual void OnActive()
	{
		base.gameObject.SetActive(true);
		Component[] componentsInChildren = base.gameObject.GetComponentsInChildren(typeof(ILevelItem));
		foreach (Component component in componentsInChildren)
		{
			this.items.Add((ILevelItem)component, (ILevelItem)component);
		}
		this.checkpoints = new List<LevelCheckPoint>(base.gameObject.GetComponentsInChildren<LevelCheckPoint>());
		foreach (LevelCheckPoint levelCheckPoint in this.checkpoints)
		{
			levelCheckPoint.levelBlock = this;
		}
		this.checkpoints = (from x in this.checkpoints
		orderby x.index
		select x).ToList<LevelCheckPoint>();
		this.isActive = true;
	}

	public virtual void OnDeactive()
	{
		this.items.Clear();
		this.checkpoints.Clear();
		base.gameObject.SetActive(false);
		this.isActive = false;
	}

	public virtual void OnEnter()
	{
		this.isEntered = true;
		foreach (KeyValuePair<ILevelItem, ILevelItem> keyValuePair in this.items)
		{
			keyValuePair.Value.OnEntertBlock(this);
		}
	}

	public virtual void OnLeave()
	{
		this.isEntered = false;
		foreach (KeyValuePair<ILevelItem, ILevelItem> keyValuePair in this.items)
		{
			keyValuePair.Value.OnLeaveBlock(this);
		}
	}

	public virtual void OnReset()
	{
		foreach (KeyValuePair<ILevelItem, ILevelItem> keyValuePair in this.items)
		{
			keyValuePair.Value.OnReset();
		}
	}

	public virtual void OnPause(bool isPause)
	{
		foreach (KeyValuePair<ILevelItem, ILevelItem> keyValuePair in this.items)
		{
			keyValuePair.Value.OnPause(isPause);
		}
	}

	public virtual void RegisterPlayer(IPlayerController playerController)
	{
		ILevelItem levelItem = (ILevelItem)playerController.GameObject.GetComponent(typeof(ILevelItem));
		if (!this.items.ContainsKey(levelItem))
		{
			this.items.Add(levelItem, levelItem);
		}
	}

	public virtual void UnRegisterPlayer(IPlayerController playerController)
	{
		ILevelItem key = (ILevelItem)playerController.GameObject.GetComponent(typeof(ILevelItem));
		if (this.items.ContainsKey(key))
		{
			this.items.Remove(key);
		}
	}

	public virtual void SetupFirtBlock()
	{
		if (this.checkpoints.Count == 0)
		{
			UnityEngine.Debug.LogError("Checkpoint not found!");
			return;
		}
		IPlayerController playerController = SingletonMonoBehaviourClass<GameLogicMgr>.instance.playerController;
		LevelCheckPoint levelCheckPoint = this.checkpoints[0];
		playerController.SetRespawnPoint(levelCheckPoint.transform.position);
		playerController.GameObject.transform.position = levelCheckPoint.transform.position;
		float z = SingletonMonoBehaviourClass<GameLogicMgr>.instance.cameraController.GetGO().transform.position.z;
		SingletonMonoBehaviourClass<GameLogicMgr>.instance.cameraController.GetGO().transform.position = new Vector3(levelCheckPoint.transform.position.x, levelCheckPoint.transform.position.y, z);
		SingletonMonoBehaviourClass<GameLogicMgr>.instance.cameraController.SetCurrentPos(levelCheckPoint.transform.position.x, levelCheckPoint.transform.position.y);
	}

	public virtual GameObject GetGo()
	{
		return base.gameObject;
	}

	public virtual void OnPickingTime(float time)
	{
	}

	public bool IsActive()
	{
		return this.isActive;
	}

	public bool IsEntered()
	{
		return this.isEntered;
	}

	public int index;

	public LevelBlock previous;

	public LevelBlock next;

	public GameSection section;

	private bool isActive;

	private bool isEntered;

	protected Dictionary<ILevelItem, ILevelItem> items = new Dictionary<ILevelItem, ILevelItem>();

	protected List<LevelCheckPoint> checkpoints;
}
