using System;
using DatabaseModel;
using LitJson;
using UnityEngine;

public class AirBombAnimEvent : BaseBehaviour
{
	private void Start()
	{
		this._jsonData = SingletonMono<EnemyDataPreload>.Instance.attack[EnemyType.空雷];
	}

	public void DestroySelf()
	{
		base.Invoke("RealDestroy", 2f);
		base.gameObject.SetActive(false);
	}

	private void RealDestroy()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public void PlayAudio(int id)
	{
		R.Audio.PlayEffect(id, new Vector3?(base.transform.position));
	}

	public void PlayAtk1Effect()
	{
		float num = (R.GameData.Difficulty > 1) ? 1.5f : 1f;
		int damage = (int)((float)EnemyAttrData.FindBySceneNameAndType(LevelManager.SceneName, EnemyType.空雷).atk * num);
		Transform transform = R.Effect.Generate(162, base.transform, default(Vector3), default(Vector3), default(Vector3), true);
		EnemyBullet componentInChildren = transform.GetComponentInChildren<EnemyBullet>();
		componentInChildren.SetAtkData(this._jsonData[base.GetComponent<StateMachine>().currentState]);
		componentInChildren.damage = damage;
		componentInChildren.origin = null;
		componentInChildren.EnemyTypeOfShooter = EnemyType.空雷;
	}

	private JsonData _jsonData;
}
