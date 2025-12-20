using System;
using System.Collections.Generic;

[Serializable]
public class EnemyQueueData
{
	public EnemyType CheckEnemy;

	public bool Random;

	public bool Limited = true;

	public float DelayTime = 0.5f;

	public AreaRect enemyQueueArea = default(AreaRect);

	public List<EnemyType> EnemyType = new List<EnemyType>();
}
