using System;
using UnityEngine;

public class LocalSprite : BaseBehaviour
{
	private void Start()
	{
		this._renderer = base.GetComponent<SpriteRenderer>();
		this._sprite_cn = this._renderer.sprite;
		if (!UILanguage.IsSimplifiedChinese)
		{
			this._renderer.sprite = this.sprite;
			this._setChinese = false;
		}
	}

	private void Update()
	{
		if (!UILanguage.IsSimplifiedChinese && this._setChinese)
		{
			this._renderer.sprite = this.sprite;
			this._setChinese = false;
		}
		else if (UILanguage.IsSimplifiedChinese && !this._setChinese)
		{
			this._renderer.sprite = this._sprite_cn;
			this._setChinese = true;
		}
	}

	public Sprite sprite;

	private Sprite _sprite_cn;

	private SpriteRenderer _renderer;

	private bool _setChinese = true;
}
