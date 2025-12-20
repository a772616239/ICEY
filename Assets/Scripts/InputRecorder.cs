using System;
using System.Collections.Generic;
using System.IO;
using CIS;
using LitJson;
using UnityEngine;

public class InputRecorder : MonoBehaviour, ILevelItem, IInputEventHandler
{
	private void Start()
	{
		if (this.replayTarget != null)
		{
			this.replayTargetHandler = (IInputEventHandler)this.replayTarget.GetComponent(typeof(IInputEventHandler));
		}
		SingletonMonoBehaviourClass<InputMgr>.instance.RegisterInputEventHandler(this);
		this.demo = PlayerControllerDemo.Instance;
	}

	private void Update()
	{
		if (this.recordMode == InputRecorder.RecordMode.Record)
		{
			if (this.startRecordInput)
			{
				this.frameCounter++;
			}
		}
		else if (this.recordMode == InputRecorder.RecordMode.Replay && this.startRecordInput)
		{
			this.frameCounter++;
			InputRecorder.InputSlot inputSlot = null;
			if (this.inputRecord.slots.Count > 0)
			{
				inputSlot = this.inputRecord.slots[0];
			}
			int num = 0;
			while (inputSlot != null && this.frameCounter == inputSlot.frame)
			{
				if (inputSlot.type == 0)
				{
					this.replayTargetHandler.KeyDown(inputSlot.key);
				}
				else
				{
					this.replayTargetHandler.KeyUp(inputSlot.key);
				}
				this.inputRecord.slots.RemoveAt(0);
				if (this.inputRecord.slots.Count > 0)
				{
					inputSlot = this.inputRecord.slots[0];
				}
				else
				{
					inputSlot = null;
				}
				num++;
			}
			if (num > 1)
			{
			}
		}
	}

	public void StartRecord()
	{
		UnityEngine.Debug.Log("[StartRecord]");
		this.startRecordInput = true;
		if (this.recordMode == InputRecorder.RecordMode.Record)
		{
			this.inputRecord = new InputRecorder.InputRecord();
		}
		else if (this.recordMode == InputRecorder.RecordMode.Replay)
		{
			TextAsset textAsset = Resources.Load<TextAsset>(this.fileName);
			if (textAsset == null)
			{
				UnityEngine.Debug.LogError("inputRecord asset not found!");
				return;
			}
			this.inputRecord = JsonMapper.ToObject<InputRecorder.InputRecord>(textAsset.text);
		}
	}

	public void EndRecord()
	{
		UnityEngine.Debug.Log("[EndRecord]");
		this.startRecordInput = false;
		this.frameCounter = 0;
		if (this.recordMode == InputRecorder.RecordMode.Record)
		{
			string contents = JsonMapper.ToJson(this.inputRecord);
			File.WriteAllText(Application.dataPath + "/Resources/" + this.fileName + ".txt", contents);
		}
	}

	public void KeyDown(KeyCode key)
	{
		if (this.recordMode == InputRecorder.RecordMode.Record && this.startRecordInput)
		{
			this.inputRecord.slots.Add(new InputRecorder.InputSlot(key, this.frameCounter, 0));
		}
	}

	public void KeyUp(KeyCode key)
	{
		if (this.recordMode == InputRecorder.RecordMode.Record && this.startRecordInput)
		{
			this.inputRecord.slots.Add(new InputRecorder.InputSlot(key, this.frameCounter, 1));
		}
	}

	public void OnReset()
	{
	}

	public void OnPause(bool isPause)
	{
	}

	public void OnEntertBlock(ILevelBlock block)
	{
		UnityEngine.Debug.Log("[OnEntertBlock]");
		if (this.startBlock == block)
		{
			this.startRecordInput = true;
			this.fileName = block.GetGo().name + "_test";
			this.StartRecord();
		}
	}

	public void OnLeaveBlock(ILevelBlock block)
	{
		if (this.startRecordInput)
		{
			this.EndRecord();
		}
	}

	public void OnPickingTime(float time)
	{
	}

	public LevelBlock startBlock;

	public InputRecorder.RecordMode recordMode;

	public GameObject replayTarget;

	private string fileName;

	private bool startRecordInput;

	private int frameCounter;

	private InputRecorder.InputRecord inputRecord = new InputRecorder.InputRecord();

	private IInputEventHandler replayTargetHandler;

	private PlayerControllerDemo demo;

	public class InputSlot
	{
		public InputSlot(KeyCode key, int frame, int type)
		{
			this.key = key;
			this.frame = frame;
			this.type = type;
		}

		public KeyCode key;

		public int type;

		public int frame;
	}

	public class InputRecord
	{
		public InputRecord()
		{
			this.slots = new List<InputRecorder.InputSlot>();
		}

		public List<InputRecorder.InputSlot> slots;
	}

	public enum RecordMode
	{
		Record,
		Replay
	}
}
