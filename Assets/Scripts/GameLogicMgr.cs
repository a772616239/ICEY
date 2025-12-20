using System;
using System.Collections;
using System.Collections.Generic;
using CIS;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GameLogicMgr : SingletonMonoBehaviourClass<GameLogicMgr>
{
	public bool IsGameEnd
	{
		get
		{
			return this.isGameEnd;
		}
	}

	public void EndPlay()
	{
		float num = Time.realtimeSinceStartup - this.startTime;
		UnityEngine.Debug.Log("totalTime: " + num);
	}

	public void StartPlay()
	{
		if (this.startTime < 0f)
		{
			this.startTime = Time.realtimeSinceStartup;
		}
	}

	public GameSection currGameSection
	{
		get
		{
			return this._currGameSection;
		}
	}

	private void Awake()
	{
		GameSection[] componentsInChildren = base.GetComponentsInChildren<GameSection>(true);
		foreach (GameSection gameSection in componentsInChildren)
		{
			this.gameSectionDic.Add(gameSection.sectionName, gameSection);
		}
	}

	public IEnumerator LoadSectionAsync(string fileName, Action<GameObject> callback)
	{
		ResourceRequest resourceRequest = Resources.LoadAsync<GameObject>(fileName);
		while (!resourceRequest.isDone)
		{
			yield return 0;
		}
		GameObject go = (GameObject)resourceRequest.asset;
		UnityEngine.Object.Instantiate<GameObject>(go);
		if (callback != null)
		{
			callback(go);
		}
		yield break;
	}

	public void LoadSection(string fileName, Action<GameObject> callback)
	{
		GameObject obj = Resources.Load<GameObject>(fileName);
		if (callback != null)
		{
			callback(obj);
		}
	}

	private void Start()
	{
		if (this.debug)
		{
			return;
		}
		this.playerController = (IPlayerController)this.playerControllerGo.GetComponent(typeof(IPlayerController));
		this.cameraController = (ICameraController)this.cameraControllerGo.GetComponent(typeof(ICameraController));
		if (!this.gameSectionDic.ContainsKey("first"))
		{
			UnityEngine.Debug.LogError("First section not found! Must have a 'first' section");
		}
		GameSection section = this.gameSectionDic["first"];
		this.PushSection(section, false);
	}

	public void ReLoadCurrentSection()
	{
	}

	public void ClearLevelTimeText()
	{
		this.startTime = -1f;
	}

	public void RegisterLevelItem(GameSection secion, ILevelItem item)
	{
		if (secion != null)
		{
		}
	}

	public GameSection GetGameSection(string sectionName)
	{
		if (this.gameSectionDic.ContainsKey(sectionName))
		{
			return this.gameSectionDic[sectionName];
		}
		return null;
	}

	public void GameEnd()
	{
		this.isGameEnd = true;
		if (this._currGameSection != null)
		{
			this._currGameSection.OnSectionLeave(null);
		}
	}

	public void PushSection(GameSection section, bool needMask = false)
	{
		if (this.isGameEnd)
		{
			return;
		}
		if (section != null && section.isEnd)
		{
			this.GameEnd();
			return;
		}
		if (this._currGameSection != null)
		{
			this._currGameSection.OnSectionLeave(section);
			if (needMask)
			{
				SpriteRenderer mask = SingletonMonoBehaviourClass<SHICameraController>.instance.overlayMask;
				mask.enabled = true;
				Color black = Color.black;
				black.a = 0f;
				mask.color = black;
				mask.DOFade(1f, 0f).OnComplete(delegate
				{
					mask.DOFade(0f, 2f);
				});
			}
			this._currGameSection = section;
			if (!section.isEnd)
			{
				section.OnSectionEnter(this._currGameSection);
			}
		}
		else
		{
			this._currGameSection = section;
			section.OnSectionEnter(null);
			section.SetupFirtBlock();
		}
	}

	public void SkipSection()
	{
		if (this._currGameSection != null)
		{
			this._currGameSection.OnSectionSkip();
		}
	}

	public void Pause(bool _isPuase, bool timeScale = false, bool showTutorial = false)
	{
		this.isPause = _isPuase;
		if (showTutorial)
		{
			this.tutorialSprite.gameObject.SetActive(true);
		}
		if (_isPuase)
		{
			if (timeScale)
			{
				Time.timeScale = 0f;
			}
			if (showTutorial)
			{
				SpriteRenderer overlayMask = SingletonMonoBehaviourClass<SHICameraController>.instance.overlayMask;
				Color black = Color.black;
				black.a = 0f;
				overlayMask.color = black;
				overlayMask.enabled = true;
				overlayMask.DOFade(0.65f, 0.2f).SetUpdate(true);
				DOTween.ToAlpha(() => this.tutorialSprite.color, delegate(Color x)
				{
					this.tutorialSprite.color = x;
				}, 1f, 0.2f).SetUpdate(true);
			}
		}
		else if (showTutorial)
		{
			SpriteRenderer mask = SingletonMonoBehaviourClass<SHICameraController>.instance.overlayMask;
			mask.DOFade(0f, 0.2f).SetUpdate(true).OnComplete(delegate
			{
				mask.enabled = false;
			});
			DOTween.ToAlpha(() => this.tutorialSprite.color, delegate(Color x)
			{
				this.tutorialSprite.color = x;
			}, 0f, 0.2f).SetUpdate(true).OnComplete(delegate
			{
				if (timeScale)
				{
					Time.timeScale = 1f;
				}
				this.tutorialSprite.gameObject.SetActive(false);
			});
		}
		else if (timeScale)
		{
			Time.timeScale = 1f;
		}
		if (this._currGameSection != null)
		{
			this._currGameSection.OnSectionPause(this.isPause);
		}
	}

	public bool IsPause()
	{
		return this.isPause;
	}

	public void Reset()
	{
		if (this._currGameSection != null)
		{
			this._currGameSection.OnSectionReset();
		}
	}

	public void ChangePlayerController(GameObject playerGo)
	{
		this.playerControllerGo = playerGo;
		this.playerController = (IPlayerController)this.playerControllerGo.GetComponent(typeof(IPlayerController));
		if (this.playerControllerGo == null || this.playerController == null)
		{
			UnityEngine.Debug.LogError("failed to change player controller");
		}
	}

	private GameSection _currGameSection;

	public GameObject playerControllerGo;

	public IPlayerController playerController;

	public GameObject cameraControllerGo;

	public ICameraController cameraController;

	public Image tutorialSprite;

	private bool isPause;

	private bool isGameEnd;

	private Hashtable sectionMap = new Hashtable();

	private Dictionary<string, GameSection> gameSectionDic = new Dictionary<string, GameSection>();

	private float startTime = -1f;

	public bool debug;
}
