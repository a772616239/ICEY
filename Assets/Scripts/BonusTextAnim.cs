using System;
using System.Collections;
using UnityEngine;

public class BonusTextAnim : MonoBehaviour
{
	private void Start()
	{
		this.label = base.GetComponent<UILabel>();
		base.StartCoroutine(this.AnimCoroutine());
	}

	private void Update()
	{
	}

	private IEnumerator AnimCoroutine()
	{
		base.GetComponent<AudioSource>().Play();
		this.label.text = string.Empty;
		while (!string.IsNullOrEmpty(this.Text))
		{
			for (int i = 0; i < 3; i++)
			{
				char randomChar = "abcdefghijklmnopqrstuvwxyz"[UnityEngine.Random.Range(0, "abcdefghijklmnopqrstuvwxyz".Length)];
				randomChar = ((!char.IsLower(this.Text[0])) ? char.ToUpper(randomChar) : randomChar);
				UILabel uilabel = this.label;
				uilabel.text += randomChar;
				yield return new WaitForSeconds(0.02f);
				this.label.text = this.label.text.Remove(this.label.text.Length - 1);
			}
			UILabel uilabel2 = this.label;
			uilabel2.text += this.Text[0];
			this.Text = this.Text.Remove(0, 1);
			yield return new WaitForSeconds(0.02f);
		}
		base.GetComponent<AudioSource>().Play();
		yield return new WaitForSeconds(2f);
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	public string Text;

	private UILabel label;

	private const string Alphabat = "abcdefghijklmnopqrstuvwxyz";
}
