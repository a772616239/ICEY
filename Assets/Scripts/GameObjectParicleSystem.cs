using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using ExtensionMethods;
using UnityEngine;

public class GameObjectParicleSystem : MonoBehaviour
{
	//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event EventHandler OnParticleCreate;

	//[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event EventHandler OnParticleDestroy;

	private Rect Shape
	{
		get
		{
			return new Rect(base.transform.position - this._box / 2f, this._box);
		}
	}

	private void Start()
	{
		base.StartCoroutine(this.Emission());
	}

	private IEnumerator Emission()
	{
		for (;;)
		{
			if (this._count < this._maxParticles)
			{
				base.StartCoroutine(this.ParticleLifetime());
			}
			yield return new WaitForSeconds(1f / (float)this._rate);
		}
		yield break;
	}

	private IEnumerator ParticleLifetime()
	{
		Vector3 position = new Vector3(UnityEngine.Random.Range(this.Shape.xMin, this.Shape.xMax), UnityEngine.Random.Range(this.Shape.yMin, this.Shape.yMax), UnityEngine.Random.Range(base.transform.position.z - this._box.z, base.transform.position.z + this._box.z));
		Vector3 eulerAngles = Vector3.zero.SetZ(UnityEngine.Random.Range(this._startRotation.x, this._startRotation.y));
		Vector3 localScale = Vector3.one * UnityEngine.Random.Range(this._startSize.x, this._startSize.y);
		GameObject particle;
		if (this._usePool && this._gameObjectStack.Count != 0)
		{
			particle = this._gameObjectStack.Pop();
			particle.transform.position = position;
			particle.transform.eulerAngles = eulerAngles;
			particle.transform.localScale = localScale;
			particle.SetActive(true);
		}
		else
		{
			particle = UnityEngine.Object.Instantiate<GameObject>(this._particleGameObject, position, Quaternion.Euler(eulerAngles));
			if (particle == null)
			{
				throw new NullReferenceException();
			}
			particle.transform.parent = base.transform;
		}
		this._count++;
		if (this.OnParticleCreate != null)
		{
			this.OnParticleCreate(particle, EventArgs.Empty);
		}
		yield return new WaitForSeconds(this._startLifetime);
		if (this.OnParticleDestroy != null)
		{
			this.OnParticleDestroy(particle, EventArgs.Empty);
		}
		if (!this._usePool || this._count > this._maxParticles)
		{
			UnityEngine.Object.Destroy(particle);
		}
		else
		{
			particle.SetActive(false);
			this._gameObjectStack.Push(particle);
		}
		this._count--;
		yield break;
	}

	public void OnDrawGizmosSelected()
	{
		DebugX.DrawCube(base.transform.position, Color.cyan / 2f, this._box);
	}

	[SerializeField]
	private GameObject _particleGameObject;

	[SerializeField]
	private bool _usePool = true;

	[SerializeField]
	private float _startLifetime = 5f;

	[SerializeField]
	private Vector2 _startSize = Vector2.one;

	[SerializeField]
	private Vector2 _startRotation = Vector2.zero;

	[SerializeField]
	private int _maxParticles = 10;

	[SerializeField]
	private int _rate = 1;

	[SerializeField]
	private Vector3 _box = Vector2.one;

	private Stack<GameObject> _gameObjectStack = new Stack<GameObject>();

	private int _count;
}
