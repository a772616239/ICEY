using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class DetailInfoAnim : MonoBehaviour
{
	private void Awake()
	{
		for (int i = 0; i < this.label.Length; i++)
		{
			this.label[i].text = string.Empty;
		}
	}

	public void StartAnim(params string[] info)
	{
		this._text = info;
		this.pointAnim.Play();
		this.widget.alpha = 0f;
		DOTween.To(delegate(float a)
		{
			this.widget.alpha = a;
		}, 0f, 1f, 1f);
		for (int i = 0; i < this.label.Length; i++)
		{
			base.StartCoroutine(this.ShowTextAnimCoroutine(i));
		}
	}

	public Coroutine FadeOut(float delayTime = 0f)
	{
		return base.StartCoroutine(this.FadeOutAnimCoroutine(delayTime));
	}

	private IEnumerator FadeOutAnimCoroutine(float delayTime)
	{
		yield return new WaitForSeconds(delayTime);
		for (int i = 0; i < this.label.Length; i++)
		{
			while (this.label[i].alpha > 0f)
			{
				this.label[i].transform.position += new Vector3(0f, 1f * Time.deltaTime);
				this.label[i].alpha -= 4f * Time.deltaTime;
				yield return new WaitForFixedUpdate();
			}
		}
		NGUITools.Destroy(base.gameObject);
		yield break;
	}

	private IEnumerator ShowTextAnimCoroutine(int index)
	{
		foreach (char str in this._text[index])
		{
			float duration = 0.5f;
			float delayTime = 0.02f;
			int times = Mathf.CeilToInt(duration / (float)this._text[index].Length / delayTime);
			for (int i = 1; i < times; i++)
			{
				UILabel uilabel = this.label[index];
				uilabel.text += this.randomString[UnityEngine.Random.Range(0, this.randomString.Length)];
				yield return new WaitForSeconds(delayTime);
				this.label[index].text = this.label[index].text.Remove(this.label[index].text.Length - 1);
			}
			UILabel uilabel2 = this.label[index];
			uilabel2.text += str;
		}
		yield break;
	}

	[SerializeField]
	private UILabel[] label;

	[SerializeField]
	private UIWidget widget;

	[SerializeField]
	private Animation pointAnim;

	private string[] _text = new string[3];

	private string randomString = "如果你也厌倦了这个浮华的市场如果你同样追求完美想尽情宣泄那天马行空的创造力加入我们体验不同寻常的游戏创造之路我们可以一起愉快地用显示器砸制作人";
}
