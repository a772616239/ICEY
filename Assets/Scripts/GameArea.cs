using System;
using UnityEngine;

[ExecuteInEditMode]
public class GameArea : SingletonMono<GameArea>
{
	public void Awake()
	{
		this.Init();
	}

	[ContextMenu("Init")]
	private void Init()
	{
		GameArea.MapRange.min = this.bottomLeft.position;
		GameArea.MapRange.max = this.TopRight.position;
		GameArea.CameraRange = GameArea.MapRange;
		GameArea.PlayerRange.min = new Vector2(GameArea.CameraRange.min.x - 2f, GameArea.CameraRange.min.y);
		GameArea.PlayerRange.max = new Vector2(GameArea.CameraRange.max.x + 2f, GameArea.CameraRange.max.y);
		GameArea.EnemyRange = GameArea.CameraRange;
	}

	public void OnDrawGizmos()
	{
		if (this._showCamera)
		{
			DebugX.DrawRect(GameArea.CameraRange, this._cameraRangeColor, 0f);
		}
		if (this._showPlayer)
		{
			DebugX.DrawRect(GameArea.PlayerRange, this._playerRangeColor, 0f);
		}
		if (this._showEnemy)
		{
			DebugX.DrawRect(GameArea.EnemyRange, this._enemyRangeColor, 0f);
		}
		if (this._showMap)
		{
			DebugX.DrawRect(GameArea.MapRange, this._mapRangeColor, 0f);
		}
	}

	[HideInInspector]
	public static Rect MapRange;

	[HideInInspector]
	public static Rect CameraRange;

	[HideInInspector]
	public static Rect PlayerRange;

	[HideInInspector]
	public static Rect EnemyRange;

	[SerializeField]
	private bool _showMap;

	[SerializeField]
	private Color _mapRangeColor = Color.red;

	[SerializeField]
	private bool _showCamera;

	[SerializeField]
	private Color _cameraRangeColor = Color.green;

	[SerializeField]
	private bool _showPlayer;

	[SerializeField]
	private Color _playerRangeColor = Color.cyan;

	[SerializeField]
	private bool _showEnemy;

	[SerializeField]
	private Color _enemyRangeColor = Color.yellow;

	[SerializeField]
	private Transform bottomLeft;

	[SerializeField]
	private Transform TopRight;
}
