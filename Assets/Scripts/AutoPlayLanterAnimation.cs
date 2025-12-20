using System;
using System.Collections;
using UnityEngine;

public class AutoPlayLanterAnimation : MonoBehaviour
{
	private void Start()
	{
	}

	private IEnumerator PlayAnimation(float time)
	{
		yield return new WaitForSeconds(time);
		if (this.lanterAnimator != null)
		{
			this.lanterAnimator.speed = this.speed;
			this.lanterAnimator.Play("hit3");
		}
		float z = UnityEngine.Random.Range(-40f, 40f);
		if (this.light1Animator != null)
		{
			this.light1Animator.speed = this.speed;
			this.light1Animator.transform.rotation = Quaternion.Euler(0f, 0f, z);
			this.light1Animator.Play("lanternLight");
		}
		if (this.light2Animator != null)
		{
			this.light2Animator.speed = this.speed;
			this.light2Animator.transform.rotation = Quaternion.Euler(0f, 0f, z + UnityEngine.Random.Range(50f, 110f));
			this.light2Animator.Play("lanternLightAlter");
		}
		if (this.backgroundAnimator != null)
		{
			this.backgroundAnimator.speed = this.speed;
			this.backgroundAnimator.Play("lanternLightBackground");
		}
		yield break;
	}

	private void Update()
	{
		if (UnityEngine.Input.GetKeyDown(KeyCode.Space))
		{
			base.StartCoroutine(this.PlayAnimation(this.delay));
		}
	}

	public Animator lanterAnimator;

	public Animator light1Animator;

	public Animator light2Animator;

	public Animator backgroundAnimator;

	public float speed;

	public float delay;
}
