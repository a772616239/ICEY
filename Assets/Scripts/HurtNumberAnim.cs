using System;
using System.Collections;
using UnityEngine;

public class HurtNumberAnim : MonoBehaviour
{
	private void Start()
	{
		this._label = base.GetComponentInChildren<UILabel>();
		this._widget = base.GetComponent<UIWidget>();
		this._widget.transform.position += new Vector3(UnityEngine.Random.Range(-0.15f, 0.15f), UnityEngine.Random.Range(-0.15f, 0.15f));
		this._fontSize = this._label.fontSize;
		this._label.fontSize = 0;
		base.StartCoroutine(this.AnimationCoroutine());
	}

	private IEnumerator AnimationCoroutine()
	{
		while (this._widget.alpha < 1f || this._label.fontSize < this._fontSize)
		{
			if (!WorldTime.IsPausing)
			{
				if (this._widget.alpha < 1f)
				{
					this._widget.alpha += 8f * Time.deltaTime;
				}
				if (this._label.fontSize < this._fontSize)
				{
					HurtNumberAnim.TypeEnum typeEnum = this.type;
					if (typeEnum != HurtNumberAnim.TypeEnum.Normal)
					{
						if (typeEnum == HurtNumberAnim.TypeEnum.Crit)
						{
							this._label.fontSize += 30;
						}
					}
					else
					{
						this._label.fontSize += 40;
					}
				}
			}
			yield return null;
		}
		for (int i = 0; i < 30; i++)
		{
			if (!WorldTime.IsPausing)
			{
				this._widget.transform.position += new Vector3(0f, Time.deltaTime * 0.5f);
			}
			else
			{
				i--;
			}
			yield return null;
		}
		while (this._widget.alpha > 0f)
		{
			if (!WorldTime.IsPausing)
			{
				this._widget.alpha -= 4f * Time.deltaTime;
				HurtNumberAnim.TypeEnum typeEnum2 = this.type;
				if (typeEnum2 != HurtNumberAnim.TypeEnum.Normal)
				{
					if (typeEnum2 == HurtNumberAnim.TypeEnum.Crit)
					{
						this._label.fontSize -= 40;
					}
				}
				else
				{
					this._widget.transform.position += new Vector3(0f, Time.deltaTime * 5f);
					this._label.fontSize -= 10;
				}
			}
			yield return null;
		}
		yield break;
	}

	[SerializeField]
	private HurtNumberAnim.TypeEnum type;

	private UILabel _label;

	private UIWidget _widget;

	private Vector3 _lastRandomDelta = Vector3.zero;

	private int _fontSize;

	private enum TypeEnum
	{
		Normal,
		Crit
	}
}
