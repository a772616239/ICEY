using System;
using System.Collections.Generic;
using CIS;
using UnityEngine;

[ExecuteInEditMode]
public class MultiScrollerMgr : MonoBehaviour, ILevelItem
{
	public static MultiScrollerMgr Instance
	{
		get
		{
			if (MultiScrollerMgr._instance == null)
			{
				MultiScrollerMgr._instance = UnityEngine.Object.FindObjectOfType<MultiScrollerMgr>();
			}
			return MultiScrollerMgr._instance;
		}
	}

	public void GenerateAnchor()
	{
		MultiScroller multiScroller = this.currScroller;
		List<ScrollGroup> list = new List<ScrollGroup>();
		for (int i = 0; i < multiScroller.groups.Count; i++)
		{
			ScrollGroup scrollGroup = multiScroller.groups[i];
			ScrollGroup scrollGroup2 = new ScrollGroup();
			for (int j = 0; j < scrollGroup.pages.Count; j++)
			{
				ScrollPage scrollPage = scrollGroup.pages[j];
				ScrollPage scrollPage2 = new ScrollPage();
				scrollPage2.initPos = scrollPage.trans.position;
				scrollPage2.initPos.y = scrollPage2.initPos.y - base.transform.position.y;
				scrollPage2.initPos.x = scrollPage2.initPos.x - base.transform.position.x;
				scrollPage2.originalPos = scrollPage.trans.localPosition;
				scrollGroup2.pages.Add(scrollPage2);
			}
			list.Add(scrollGroup2);
		}
		this.currAnchor.groups = list;
	}

	public void LoadCurrAnchor()
	{
		this.LoadAnchor(this.currAnchor);
	}

	public void ResetCameraToCurrAnchor()
	{
		Vector3 position = base.transform.parent.position;
		base.transform.parent.position = new Vector3(this.currAnchor.transform.position.x, this.currAnchor.transform.position.y, position.z);
		this.LoadAnchor(this.currAnchor);
	}

	public void LoadAnchor(ScrollerAnchor anchor)
	{
		for (int i = 0; i < anchor.groups.Count; i++)
		{
			ScrollGroup scrollGroup = anchor.groups[i];
			for (int j = 0; j < scrollGroup.pages.Count; j++)
			{
				ScrollPage scrollPage = scrollGroup.pages[j];
				ScrollPage scrollPage2 = this.currScroller.groups[i].pages[j];
				scrollPage2.initPos.y = scrollPage.initPos.y;
				scrollPage2.initPos.x = scrollPage.initPos.x;
				scrollPage2.originalPos = scrollPage.originalPos;
				scrollPage2.trans.localPosition = scrollPage2.originalPos;
			}
		}
		this.initCameraY = anchor.transform.position.y;
		this.initCameraX = anchor.transform.position.x;
	}

	public void Reset()
	{
		this.reseting = false;
		this.lastCamPosX = this.camTrans.position.x;
		this.lastCamPosY = this.camTrans.position.y;
	}

	public void ResetScrollerPos()
	{
		foreach (ScrollGroup scrollGroup in this.currScroller.groups)
		{
			for (int i = 0; i < scrollGroup.pages.Count; i++)
			{
				ScrollPage scrollPage = scrollGroup.pages[i];
				scrollGroup.pageSnake[i].id = scrollPage.id;
				scrollGroup.pageSnake[i].index = scrollPage.index;
				scrollGroup.pageSnake[i].initPos = scrollPage.initPos;
				scrollGroup.pageSnake[i].originalPos = scrollPage.originalPos;
				scrollGroup.pageSnake[i].trans = scrollPage.trans;
			}
		}
	}

	public void ReCalInitPos()
	{
		if (this.currScroller == null)
		{
			return;
		}
		List<ScrollGroup> list = this.currScroller.groups;
		this.initCameraY = this.camTrans.position.y;
		this.initCameraX = this.camTrans.position.x;
		foreach (ScrollGroup scrollGroup in list)
		{
			foreach (ScrollPage scrollPage in scrollGroup.pages)
			{
				scrollPage.initPos.y = scrollPage.trans.position.y - this.initCameraY;
				scrollPage.initPos.x = scrollPage.trans.position.x - this.initCameraX;
			}
		}
	}

	public void OnReset()
	{
		this.reseting = true;
		this.Reset();
	}

	public void OnPause(bool isPause)
	{
	}

	public void EnableScroller(bool enable)
	{
		this.isEnable = enable;
	}

	private void Awake()
	{
	}

	public void StartPin()
	{
	}

	public void AssemblePages()
	{
		float num = this.ScreenWidth / 2f;
		List<ScrollGroup> list = this.currScroller.groups;
		foreach (ScrollGroup scrollGroup in list)
		{
			int childCount = scrollGroup.trans.childCount;
			for (int i = 0; i < childCount; i++)
			{
				UnityEngine.Object.DestroyImmediate(scrollGroup.trans.GetChild(0).gameObject);
			}
			scrollGroup.pages.Clear();
			scrollGroup.pageSnake.Clear();
			float x = scrollGroup.sprite.bounds.size.x;
			scrollGroup.width = x;
			float num2 = x / 2f;
			float num3 = this.ScreenWidth / x;
			int num4 = (int)Mathf.Ceil(num3);
			if (num3 - 1f > 0.01f)
			{
				num4++;
			}
			float num5 = -num - x + num2;
			for (int j = 0; j < num4; j++)
			{
				GameObject gameObject = new GameObject("page" + (j + 1));
				gameObject.transform.parent = scrollGroup.trans;
				SpriteRenderer spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
				spriteRenderer.sprite = scrollGroup.sprite;
				spriteRenderer.sortingOrder = scrollGroup.order;
				ScrollPage scrollPage = new ScrollPage();
				scrollPage.id = UnityEngine.Random.Range(-1000000, 1000000);
				scrollPage.trans = gameObject.transform;
				scrollGroup.pages.Add(scrollPage);
				scrollGroup.pageSnake.Add(scrollPage);
				gameObject.transform.localPosition = new Vector2(num5, 0f);
				num5 += x;
			}
		}
	}

	private void Start()
	{
		this.ScreenWidth = 1920f;
		this.ScreenHeight = 1080f;
		this.lastCamPosX = this.camTrans.position.x;
		this.lastCamPosY = this.camTrans.position.y;
	}

	private void Update()
	{
		if (!this.isEnable)
		{
			return;
		}
		float num = 0f;
		if (Application.isPlaying)
		{
			num = SingletonMonoBehaviourClass<TimeMgr>.instance.deltaTime;
		}
		float num2 = -(this.camTrans.position.x - this.lastCamPosX);
		float num3 = -(this.camTrans.position.y - this.lastCamPosY);
		List<ScrollGroup> list;
		if (this.currScroller == null)
		{
			list = this.groups;
		}
		else
		{
			list = this.currScroller.groups;
		}
		foreach (ScrollGroup scrollGroup in list)
		{
			int count = scrollGroup.pageSnake.Count;
			for (int i = 0; i < scrollGroup.pages.Count; i++)
			{
				ScrollPage scrollPage = scrollGroup.pages[i];
				ScrollPage scrollPage2 = scrollGroup.pageSnake[count - 1];
				ScrollPage scrollPage3 = scrollGroup.pageSnake[0];
				Transform trans = scrollPage.trans;
				this.tempPosVec3 = trans.localPosition;
				if (scrollPage.id == scrollGroup.pageSnake[count - 1].id && this.tempPosVec3.x < this.ScreenWidth / 2f - scrollGroup.width / 2f)
				{
					ScrollPage scrollPage4 = scrollGroup.pageSnake[0];
					scrollPage4.trans.localPosition = scrollPage4.trans.localPosition + new Vector3(scrollGroup.width * (float)count, 0f, 0f);
					if (scrollGroup.newXCalMethod)
					{
						scrollGroup.pages[scrollPage3.index].initPos.x = scrollGroup.pages[scrollPage3.index].initPos.x + scrollGroup.width * (float)count;
					}
					scrollGroup.pageSnake.RemoveAt(0);
					scrollGroup.pageSnake.Add(scrollPage4);
				}
				else if (scrollPage.id == scrollGroup.pageSnake[0].id && this.tempPosVec3.x > -this.ScreenWidth / 2f + scrollGroup.width / 2f)
				{
					ScrollPage scrollPage5 = scrollGroup.pageSnake[count - 1];
					scrollPage5.trans.localPosition = scrollPage5.trans.localPosition - new Vector3(scrollGroup.width * (float)count, 0f, 0f);
					if (scrollGroup.newXCalMethod)
					{
						scrollGroup.pages[scrollPage2.index].initPos.x = scrollGroup.pages[scrollPage2.index].initPos.x - scrollGroup.width * (float)count;
					}
					scrollGroup.pageSnake.RemoveAt(count - 1);
					scrollGroup.pageSnake.Insert(0, scrollPage5);
				}
				if (scrollGroup.autoScroll)
				{
					this.tempPosVec3.x = this.tempPosVec3.x + num * scrollGroup.speedX;
					this.tempPosVec3.y = this.tempPosVec3.y + num * scrollGroup.speedY;
				}
				else
				{
					if (scrollGroup.newXCalMethod)
					{
						float num4 = this.camTrans.position.x - this.initCameraX;
						this.tempPosVec3.x = scrollPage.initPos.x - num4 * scrollGroup.speedX;
					}
					else
					{
						this.tempPosVec3.x = this.tempPosVec3.x + num2 * num * scrollGroup.speedX;
					}
					if (scrollGroup.newYCalMethod)
					{
						float num5 = this.camTrans.position.y - this.initCameraY;
						this.tempPosVec3.y = scrollPage.initPos.y - num5 * scrollGroup.speedY;
					}
					else
					{
						this.tempPosVec3.y = this.tempPosVec3.y + num3 * num * scrollGroup.speedY;
					}
				}
				if (scrollGroup.UpLimitTrans != null && scrollGroup.DownLimitTrnas != null)
				{
					if (this.tempPosVec3.y > scrollGroup.UpLimitTrans.localPosition.y)
					{
						this.tempPosVec3.y = scrollGroup.UpLimitTrans.localPosition.y;
					}
					if (this.tempPosVec3.y < scrollGroup.DownLimitTrnas.localPosition.y)
					{
						this.tempPosVec3.y = scrollGroup.DownLimitTrnas.localPosition.y;
					}
				}
				trans.localPosition = this.tempPosVec3;
			}
		}
		this.lastCamPosX = this.camTrans.position.x;
		this.lastCamPosY = this.camTrans.position.y;
	}

	public void OnEntertBlock(ILevelBlock block)
	{
	}

	public void OnLeaveBlock(ILevelBlock block)
	{
	}

	public virtual void OnPickingTime(float time)
	{
	}

	public List<ScrollPage> pageList;

	public float ScreenWidth;

	public float ScreenHeight;

	public List<ScrollGroup> groups;

	public MultiScroller currScroller;

	public ScrollerAnchor currAnchor;

	public Transform camTrans;

	private bool isEnable = true;

	public bool scrollInEditor;

	private static MultiScrollerMgr _instance;

	private bool reseting;

	private float initCameraX;

	private float initCameraY;

	private Vector3 tempPosVec3;

	private float lastCamPosX;

	private float lastCamPosY;
}
