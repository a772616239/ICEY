using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class RepeatSpriteBoundary : MonoBehaviour
{
	private void Awake()
	{
		this.sprite = base.GetComponent<SpriteRenderer>();
		if (!SpritePivotAlignment.GetSpriteAlignment(base.gameObject).Equals(SpriteAlignment.TopRight))
		{
			UnityEngine.Debug.LogError("You forgot change the sprite pivot to Top Right.");
		}
		Vector2 vector = new Vector2(this.sprite.bounds.size.x / base.transform.localScale.x, this.sprite.bounds.size.y / base.transform.localScale.y);
		GameObject gameObject = new GameObject();
		SpriteRenderer spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
		gameObject.transform.position = base.transform.position;
		spriteRenderer.sprite = this.sprite.sprite;
		spriteRenderer.sortingLayerID = this.sprite.sortingLayerID;
		spriteRenderer.sortingLayerName = this.sprite.sortingLayerName;
		spriteRenderer.sortingOrder = this.sprite.sortingOrder;
		int num = 0;
		int num2 = (int)Mathf.Round(this.sprite.bounds.size.y);
		while ((float)num * vector.y < (float)num2)
		{
			int num3 = 0;
			int num4 = (int)Mathf.Round(this.sprite.bounds.size.x);
			while ((float)num3 * vector.x < (float)num4)
			{
				GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
				gameObject2.transform.position = base.transform.position - new Vector3(vector.x * (float)num3, vector.y * (float)num, 0f);
				gameObject2.transform.parent = base.transform;
				num3++;
			}
			num++;
		}
		UnityEngine.Object.Destroy(gameObject);
		this.sprite.enabled = false;
	}

	private SpriteRenderer sprite;
}
