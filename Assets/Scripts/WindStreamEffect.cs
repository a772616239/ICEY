using System;
using CIS;
using UnityEngine;

public class WindStreamEffect : MonoBehaviour
{
	private void Awake()
	{
		this.renderner = base.GetComponent<Renderer>();
		this.renderner.sortingOrder = this.sortingOrder;
	}

	private void Start()
	{
		this.mat = this.renderner.material;
		this.mat.SetFloat("_Seed", UnityEngine.Random.Range(0f, 50f));
	}

	private void Update()
	{
		if (SingletonMonoBehaviourClass<GameLogicMgr>.instance.IsPause())
		{
			return;
		}
		this.time += Time.deltaTime;
		this.mat.SetVector("_Speed", new Vector4(this.vel.x, this.vel.y, 0f, 0f));
		this.mat.SetFloat("_Strength", this.strength);
		this.mat.SetColor("_TintColor", this.tintColor);
		this.mat.SetFloat("_Distortion1", this.factor1);
		this.mat.SetFloat("_Distortion2", this.factor2);
		this.mat.SetFloat("_Gradient", this.gradient);
		this.mat.SetFloat("_Extend", this.extend);
		this.mat.SetVector("_MyTime", new Vector4(this.time / 20f, this.time, this.time * 2f, this.time * 3f));
		this.renderner.sortingOrder = this.sortingOrder;
	}

	public int sortingOrder;

	public Vector2 vel;

	public float strength;

	public Color tintColor;

	public float factor1;

	public float factor2;

	public float extend;

	public float gradient;

	private float time;

	private Material mat;

	private Renderer renderner;

	private SpriteRenderer sprite;
}
