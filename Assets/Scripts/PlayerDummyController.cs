using System;
using UnityEngine;

public class PlayerDummyController : MonoBehaviour
{
	public UISpriteAnimation anime
	{
		get
		{
			if (this._anime == null)
			{
				this._anime = base.GetComponent<UISpriteAnimation>();
			}
			return this._anime;
		}
	}

	public UISprite sprite
	{
		get
		{
			if (this._sprite == null)
			{
				this._sprite = base.GetComponent<UISprite>();
			}
			return this._sprite;
		}
	}

	private UISpriteAnimation _anime;

	private UISprite _sprite;
}
