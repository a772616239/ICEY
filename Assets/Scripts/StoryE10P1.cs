using System;
using System.Collections;
using DG.Tweening;
using I2.Loc;
using UnityEngine;

public class StoryE10P1 : BaseBehaviour
{
	private void Start()
	{
		this.first = true;
		this.gate1.openType = SceneGate.OpenType.None;
		this.gate2.openType = SceneGate.OpenType.None;
		this.text.text = ScriptLocalization.Story.e10s1;
		this.text.transform.DOScale(Vector3.one, 3f).SetLoops(-1);
		DOTween.ToAlpha(() => this.text.color, delegate(Color c)
		{
			this.text.color = c;
		}, 0f, 3f).SetLoops(-1);
	}

	public void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player") && this.first)
		{
			this.first = false;
			base.StartCoroutine(this.MainCoroutine());
		}
	}

	private IEnumerator MainCoroutine()
	{
		YieldInstruction t = R.Audio.PlayVoiceOver("e10t1", null, false);
		R.Audio.PlayEffect(369, new Vector3?(R.Player.Transform.position));
		yield return t;
		this.gate2.openType = SceneGate.OpenType.Left;
		yield break;
	}

	[SerializeField]
	private SceneGate gate1;

	[SerializeField]
	private SceneGate gate2;

	[SerializeField]
	private TextMesh text;

	private bool first;
}
