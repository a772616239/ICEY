using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ScrollGroup
{
	public ScrollGroup()
	{
		this.pages = new List<ScrollPage>();
	}

	public List<ScrollPage> pages;

	public Transform trans;

	public float width;

	public float speedX;

	public float speedY;

	public string name;

	public bool autoScroll;

	public Transform UpLimitTrans;

	public Transform DownLimitTrnas;

	public Sprite sprite;

	public int order;

	public bool needReset;

	public bool newYCalMethod;

	public bool newXCalMethod;

	public List<ScrollPage> pageSnake;
}
