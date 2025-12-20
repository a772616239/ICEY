using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UI2DSprite), typeof(UI2DSpriteAnimation))]
public class UIFrameAnimation : MonoBehaviour
{
	private void Awake()
	{
		this._animation = base.GetComponent<UI2DSpriteAnimation>();
		this._2DSprite = base.GetComponent<UI2DSprite>();
	}

	public void Preload(string name)
	{
		this._spritesesCache.Add(name, Resources.LoadAll<Sprite>("UI/Frames/" + name));
	}

	public void Play(string name)
	{
		Sprite[] array;
		if (this._spritesesCache.ContainsKey(name))
		{
			array = this._spritesesCache[name];
		}
		else
		{
			array = Resources.LoadAll<Sprite>("UI/Frames/" + name);
			this._spritesesCache.Add(name, array);
		}
		this._2DSprite.sprite2D = null;
		this._animation.frames = array;
		if (array.Length != 0)
		{
			this._animation.ResetToBeginning();
			this._animation.Play();
		}
	}

	public void Pause()
	{
		this._animation.Pause();
	}

	public void ResetToBeginning()
	{
		this._animation.ResetToBeginning();
	}

	private Dictionary<string, Sprite[]> _spritesesCache = new Dictionary<string, Sprite[]>();

	private const string Path = "UI/Frames/";

	private UI2DSpriteAnimation _animation;

	private UI2DSprite _2DSprite;
}
