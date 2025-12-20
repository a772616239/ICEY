using System;
using UnityEngine;

public static class SpritePivotAlignment
{
	public static SpriteAlignment GetSpriteAlignment(GameObject SpriteObject)
	{
		BoxCollider2D boxCollider2D = SpriteObject.AddComponent<BoxCollider2D>();
		boxCollider2D.enabled = false;
		float x = boxCollider2D.offset.x;
		float y = boxCollider2D.offset.y;
		if (x > 0f && y < 0f)
		{
			return SpriteAlignment.TopLeft;
		}
		if (x < 0f && y < 0f)
		{
			return SpriteAlignment.TopRight;
		}
		if (x == 0f && y < 0f)
		{
			return SpriteAlignment.TopCenter;
		}
		if (x > 0f && y == 0f)
		{
			return SpriteAlignment.LeftCenter;
		}
		if (x < 0f && y == 0f)
		{
			return SpriteAlignment.RightCenter;
		}
		if (x > 0f && y > 0f)
		{
			return SpriteAlignment.BottomLeft;
		}
		if (x < 0f && y > 0f)
		{
			return SpriteAlignment.BottomRight;
		}
		if (x == 0f && y > 0f)
		{
			return SpriteAlignment.BottomCenter;
		}
		if (x == 0f && y == 0f)
		{
			return SpriteAlignment.Center;
		}
		return SpriteAlignment.Custom;
	}
}
