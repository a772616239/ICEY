using System;
using System.Collections.Generic;
using System.Linq;
using CIS;
using UnityEngine;

public class GameSection : MonoBehaviour
{
	protected void Awake()
	{
		this.FindCameraBoderByDefinedName(this.resourceNode);
		this.SetupBlocks(this.resourceNode);
	}

	protected void Start()
	{
		this.logicMgr = SingletonMonoBehaviourClass<GameLogicMgr>.instance;
	}

	protected void SetupBlocks(GameObject target)
	{
		LevelBlock[] componentsInChildren = target.GetComponentsInChildren<LevelBlock>(true);
		this.levelBlocks = new List<LevelBlock>(componentsInChildren);
		this.levelBlocks = (from x in this.levelBlocks
		orderby x.index
		select x).ToList<LevelBlock>();
		for (int i = 0; i < this.levelBlocks.Count; i++)
		{
			this.levelBlocks[i].section = this;
			if (i == 0)
			{
				this.levelBlocks[i].next = ((this.levelBlocks.Count <= 1) ? null : this.levelBlocks[1]);
				this.levelBlocks[i].previous = null;
			}
			else if (i == this.levelBlocks.Count - 1)
			{
				this.levelBlocks[i].next = null;
				this.levelBlocks[i].previous = this.levelBlocks[i - 1];
			}
			else
			{
				this.levelBlocks[i].next = this.levelBlocks[i + 1];
				this.levelBlocks[i].previous = this.levelBlocks[i - 1];
			}
		}
	}

	public virtual void SetupFirtBlock()
	{
		if (this.currBlock != null)
		{
			this.currBlock.SetupFirtBlock();
		}
	}

	public virtual void OnSectionLeave(GameSection section)
	{
		this.SetCurrentBlock(null);
	}

	public virtual LevelBlock GetBlock(int index)
	{
		if (index < 0)
		{
			return null;
		}
		if (index < this.levelBlocks.Count)
		{
			return this.levelBlocks[index];
		}
		return null;
	}

	public virtual void OnSectionReset()
	{
		if (this.currBlock != null)
		{
			this.currBlock.OnReset();
			StoryE11P6.OnShiPlayerDie();
		}
	}

	public virtual void OnSectionPause(bool pause)
	{
		if (this.currBlock != null)
		{
			this.currBlock.OnPause(pause);
		}
	}

	public virtual void OnSectionEnter(GameSection section)
	{
		this.SetCameraBoundary();
		if (this.levelBlocks.Count > 0)
		{
			this.SetCurrentBlock(this.levelBlocks[0]);
		}
	}

	public virtual void Update()
	{
	}

	public virtual void SetCurrentBlock(LevelBlock block)
	{
		if (block != null)
		{
			this.currBlock = block;
			if (!block.IsActive())
			{
				block.OnActive();
			}
			if (!block.IsEntered())
			{
				block.OnEnter();
			}
			if (block.previous != null)
			{
				if (block.previous.IsEntered())
				{
					block.previous.OnLeave();
				}
				block.previous.UnRegisterPlayer(SingletonMonoBehaviourClass<GameLogicMgr>.instance.playerController);
				if (block.previous.previous != null && block.previous.previous.IsActive())
				{
					block.previous.previous.OnDeactive();
				}
			}
			block.RegisterPlayer(SingletonMonoBehaviourClass<GameLogicMgr>.instance.playerController);
			if (block.next != null)
			{
				if (block.previous == null && !block.next.IsActive())
				{
					block.next.OnActive();
				}
				if (block.next.next != null && !block.next.next.IsActive())
				{
					block.next.next.OnActive();
				}
			}
		}
		else if (this.currBlock != null && this.currBlock.IsEntered())
		{
			this.currBlock.OnLeave();
		}
	}

	public virtual void OnSectionSkip()
	{
	}

	public void AddBlock(LevelBlock block)
	{
		this.levelBlocks.Add(block);
	}

	public void RemoveLevelBlock(LevelBlock block)
	{
		for (int i = 0; i < this.levelBlocks.Count; i++)
		{
			if (this.levelBlocks[i] == block)
			{
				this.levelBlocks.RemoveAt(i);
				break;
			}
		}
	}

	protected void FindCameraBoderByDefinedName(GameObject resourceNode)
	{
		this.cameraUpLimitTrans = resourceNode.transform.Find(GameSection.upBorderName);
		this.cameraDownLimitTrans = resourceNode.transform.Find(GameSection.downBorderName);
		this.cameraRightLimitTrans = resourceNode.transform.Find(GameSection.rightBorderName);
		this.cameraLeftLimitTrans = resourceNode.transform.Find(GameSection.leftBorderName);
		if (this.cameraUpLimitTrans == null || this.cameraDownLimitTrans == null || this.cameraRightLimitTrans == null || this.cameraLeftLimitTrans == null)
		{
			UnityEngine.Debug.LogError("camera boundaries can not be null!");
		}
	}

	protected void SetCameraBoundary()
	{
		ICameraController cameraController = SingletonMonoBehaviourClass<GameLogicMgr>.instance.cameraController;
		if (this.cameraUpLimitTrans == null || this.cameraDownLimitTrans == null || this.cameraRightLimitTrans == null || this.cameraLeftLimitTrans == null)
		{
			UnityEngine.Debug.LogError("camera boundaries can not be null!");
		}
		if (cameraController == null)
		{
			UnityEngine.Debug.LogError("camera controller new not found please attatch it to your camera node");
		}
		cameraController.ReloadLimitBorder(this.cameraUpLimitTrans, this.cameraDownLimitTrans, this.cameraLeftLimitTrans, this.cameraRightLimitTrans);
	}

	protected GameLogicMgr logicMgr;

	protected Transform cameraUpLimitTrans;

	protected Transform cameraDownLimitTrans;

	protected Transform cameraRightLimitTrans;

	protected Transform cameraLeftLimitTrans;

	public string sectionName;

	public MultiScroller scroller;

	public GameObject resourceNode;

	public bool isEnd;

	protected List<LevelBlock> levelBlocks = new List<LevelBlock>();

	protected LevelBlock currBlock;

	public static string upBorderName = "upBorder";

	public static string downBorderName = "downBorder";

	public static string leftBorderName = "leftBorder";

	public static string rightBorderName = "rightBorder";
}
