using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIEnemyPointController : MonoBehaviour
{
	private void OnEnable()
	{
		SceneManager.sceneUnloaded += this.OnSceneUnloaded;
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (this.enemy == null)
		{
			NGUITools.Destroy(base.gameObject);
			return;
		}
		this.enemyPosition = Camera.main.WorldToViewportPoint(this.enemy.transform.position).x;
		NGUITools.SetActive(this.point.gameObject, this.enemyPosition < 0f || this.enemyPosition > 1f);
		if (this.enemyPosition < 0f)
		{
			this.point.transform.localScale = new Vector3(-1f, 1f, 1f);
			this.point.transform.localPosition = new Vector3((float)(-(float)UITools.ScreenWidth + 256), 1f / (this.enemyPosition / 100f - 2f / (float)UITools.ScreenHeight));
		}
		if (this.enemyPosition > 1f)
		{
			this.enemyPosition -= 1f;
			this.point.transform.localScale = Vector3.one;
			this.point.transform.localPosition = new Vector3((float)(UITools.ScreenWidth - 256), 1f / (-this.enemyPosition / 100f - 2f / (float)UITools.ScreenHeight));
		}
		if (this.enemy.currentHp <= 0)
		{
			NGUITools.Destroy(base.gameObject);
		}
	}

	private void OnDisable()
	{
		SceneManager.sceneUnloaded -= this.OnSceneUnloaded;
	}

	private void OnSceneUnloaded(Scene arg0)
	{
		NGUITools.Destroy(base.gameObject);
	}

	[SerializeField]
	private UISprite point;

	[SerializeField]
	public EnemyAttribute enemy;

	[SerializeField]
	private float enemyPosition;
}
