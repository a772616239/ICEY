using System;
using Colorful;
using DG.Tweening;
using UnityEngine;

public class FractalAnimation : BaseBehaviour
{
	private void Start()
	{
		this._mat = base.GetComponent<Fractal>().Mat;
		this._mat.color = new Color(0f, 0f, 0f, 0.8f);
		this._mat.DOColor(new Color(1f, 1f, 1f, 0.8f), 5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
		this._gaussianBlur = R.Camera.GameObject.AddComponent<GaussianBlur>();
		this._gaussianBlur.Shader = Shader.Find("Hidden/Colorful/Gaussian Blur");
		this._gaussianBlur.Passes = 3;
		this._gaussianBlur.Downscaling = 4f;
		DOTween.To(delegate(float a)
		{
			this._gaussianBlur.Amount = a;
		}, 0.655f, 1f, 3f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
	}

	private void Update()
	{
		base.transform.localRotation = base.transform.localRotation * Quaternion.Euler(0f, this._speed * Time.deltaTime, 0f);
	}

	private void OnDestroy()
	{
		UnityEngine.Object.Destroy(this._gaussianBlur);
	}

	[SerializeField]
	private float _speed;

	private Material _mat;

	private GaussianBlur _gaussianBlur;
}
