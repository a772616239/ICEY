using System;
using UnityEngine;
using Xft;

public class ReleaseYourEffect : MonoBehaviour
{
	private void Start()
	{
		this.YourEffect.Initialize();
	}

	private void InstantiateEffect()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.YourEffect.gameObject, Vector3.zero, Quaternion.identity);
		XffectComponent component = gameObject.GetComponent<XffectComponent>();
		component.Active();
	}

	private void ReleasePooledEffect(string effectName)
	{
		XffectComponent effect = this.EffectPool.GetEffect(effectName);
		effect.Active();
	}

	private void OnGUI()
	{
		if (GUI.Button(new Rect(0f, 0f, 350f, 30f), "instantiate a new effect"))
		{
			this.InstantiateEffect();
		}
		if (GUI.Button(new Rect(0f, 30f, 350f, 30f), "instantiate effect from EffectCache"))
		{
			this.ReleasePooledEffect("sample_effect");
		}
	}

	public XffectComponent YourEffect;

	public XffectCache EffectPool;
}
