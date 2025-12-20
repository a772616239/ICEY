using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSpineColor : BaseBehaviour
{
	private void Awake()
	{
		this._gameObjects = new GameObject[this._renderers.Length];
		this._materials = new Material[this._renderers.Length];
		for (int i = 0; i < this._renderers.Length; i++)
		{
			this._gameObjects[i] = this._renderers[i].gameObject;
			this._materials[i] = this._renderers[i].sharedMaterial;
		}
		this.ColorBecomeNormal();
	}

	private void OnEnable()
	{
		SceneManager.sceneUnloaded += this.OnSceneUnloaded;
		this.ColorBecomeNormal();
	}

	private void OnDisable()
	{
		SceneManager.sceneUnloaded -= this.OnSceneUnloaded;
		this.ColorBecomeNormal();
	}

	private void OnSceneUnloaded(Scene arg0)
	{
		this.ColorBecomeNormal();
	}

	private void Update()
	{
		for (int i = 0; i < this._renderers.Length; i++)
		{
			if (this._gameObjects[i].activeSelf && this._renderers[i].enabled)
			{
				this._materials[i].SetColor("_Color", this._tint);
				this._materials[i].SetFloat("_EmissionStrength", this._emissionStrength);
			}
		}
	}

	private void OnDestroy()
	{
		this._emissionStrength = 1f;
		this._tint = ChangeSpineColor.DefaultColor;
	}

	private void ColorBecomeNormal()
	{
		this._emissionStrength = 0f;
		this._tint = ChangeSpineColor.DefaultColor;
	}

	public IEnumerator EnergyBallColorChange()
	{
		this._tint = new Color(0.4f, 0.6f, 0.9f, 1f);
		yield return null;
		for (int i = 0; i < 30; i++)
		{
			this._tint.a = Mathf.Lerp(1f, 0f, (float)i / 30f);
			yield return null;
		}
		this._tint = ChangeSpineColor.DefaultColor;
		yield break;
	}

	public void HurtChange(Color color)
	{
	}

	public void TurnOnEmission()
	{
		if (this._breatheLightTweener != null)
		{
			this._breatheLightTweener.Pause<Tweener>();
		}
		this._emissionStrength = 1f;
	}

	public void TurnOnBreatheLight()
	{
		if (this._breatheLightTweener == null)
		{
			this._breatheLightTweener = DOTween.To(delegate(float s)
			{
				this._emissionStrength = s;
			}, 1f, 0f, 1f).SetLoops(-1, LoopType.Yoyo);
		}
		if (this._breatheLightTweener.IsPlaying())
		{
			return;
		}
		this._breatheLightTweener.Restart(true);
	}

	public void TurnOffAll()
	{
		this._breatheLightTweener.Pause<Tweener>();
		this._emissionStrength = 0f;
	}

	private static readonly Color DefaultColor = new Color(1f, 1f, 1f, 0f);

	[SerializeField]
	private Color _tint = ChangeSpineColor.DefaultColor;

	[Range(0f, 1f)]
	[SerializeField]
	private float _emissionStrength;

	[SerializeField]
	private Renderer[] _renderers;

	private GameObject[] _gameObjects;

	private Material[] _materials;

	private Tweener _breatheLightTweener;
}
