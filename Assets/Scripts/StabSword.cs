using System;
using System.Collections;
using UnityEngine;

public class StabSword : BaseBehaviour
{
	private void Start()
	{
	}

	private void OnEnable()
	{
		if (SingletonMono<WorldTime>.Instance != null)
		{
		}
	}

	private void OnDisable()
	{
		if (SingletonMono<WorldTime>.Instance != null)
		{
		}
		base.StopCoroutine("MultiStab");
	}

	private void OnDestroy()
	{
	}

	private void Update()
	{
	}

	public void ShowStabSword()
	{
		base.StartCoroutine(this.MultiStab(20));
	}

	private IEnumerator MultiStab(int times)
	{
		for (;;)
		{
			if (!this.isPause)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.stabEffect);
				gameObject.transform.parent = base.transform;
				gameObject.transform.localScale = new Vector3(0.8333334f, 0.8333334f, 1f);
				gameObject.transform.localPosition = new Vector3(0.2f, -0.1f, 0f);
				gameObject.transform.localRotation = Quaternion.Euler(0f, 0f, (float)UnityEngine.Random.Range(-16, 17));
			}
			yield return new WaitForSeconds(0.03f);
		}
		yield break;
	}

	private void ClipFrozen(object obj, EventArgs e)
	{
		if (this.isPause)
		{
			return;
		}
		this.isPause = true;
		this.newEffectAnim.speed = 0f;
		this.oldEffectAnim.speed = 0f;
	}

	private void ClipResume(object obj, EventArgs e)
	{
		if (!this.isPause)
		{
			return;
		}
		this.newEffectAnim.speed = 1f;
		this.oldEffectAnim.speed = 1f;
		this.isPause = false;
	}

	[SerializeField]
	private GameObject stabEffect;

	[SerializeField]
	private Animator newEffectAnim;

	[SerializeField]
	private Animator oldEffectAnim;

	public bool isPause;
}
