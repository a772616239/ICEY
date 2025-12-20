using System;
using UnityEngine;

public class FireMeteorDrop : BaseBehaviour
{
	private void OnEnable()
	{
		this._gameObjectParicleSystem.OnParticleCreate += this.OnParticleCreate;
		this._gameObjectParicleSystem.OnParticleDestroy += this.OnParticleDestroy;
	}

	private void OnDisable()
	{
		this._gameObjectParicleSystem.OnParticleCreate -= this.OnParticleCreate;
		this._gameObjectParicleSystem.OnParticleDestroy -= this.OnParticleDestroy;
	}

	private void OnParticleCreate(object sender, EventArgs e)
	{
		GameObject gameObject = sender as GameObject;
		float distance = Physics2D.Raycast(gameObject.transform.position, Vector3.down, 100f, LayerManager.GroundMask).distance;
		EffectSettings component = gameObject.GetComponent<EffectSettings>();
		component.MoveSpeed = this._moveSpeed;
		component.MoveDistance = distance;
	}

	private void OnParticleDestroy(object sender, EventArgs e)
	{
	}

	[SerializeField]
	private GameObjectParicleSystem _gameObjectParicleSystem;

	[SerializeField]
	private float _moveSpeed = 6f;
}
