using System;
using UnityEngine;

public struct EnemyJsonObject
{
	public EnemyJsonObject(string name, Vector2 position, string guid)
	{
		this.name = name;
		this.position.x = position.x;
		this.position.y = position.y;
		this.guid = guid;
	}

	public string name;

	public Vector2 position;

	public string guid;
}
