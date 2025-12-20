using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(UILabel), typeof(UICallback))]
public class UISpriteInLabel : MonoBehaviour, ICallBackable
{
	[ContextMenu("Execute")]
	private void OnModifyLocalization()
	{
		base.StartCoroutine(this.OnModifyLocalizationCoroutine());
	}

	public void OnCall()
	{
		this.OnModifyLocalization();
	}

	private IEnumerator OnModifyLocalizationCoroutine()
	{
		if (Application.isPlaying)
		{
			yield return null;
		}
		if (this._label == null)
		{
			this._label = base.GetComponent<UILabel>();
		}
		string text = this._label.text;
		text = text.Substring(0, text.IndexOf(this._target, StringComparison.Ordinal));
		NGUIText.finalSize = this._label.defaultFontSize;
		this._label.UpdateNGUIText();
		this._printedSize = NGUIText.CalculatePrintedSize(text);
		Vector2 pivotOffset = -Vector2.Scale(this._label.pivotOffset - 0.5f * Vector2.one, this._label.printedSize);
		float x = pivotOffset.x - this._label.printedSize.x / 2f + this._printedSize.x + NGUIText.CalculatePrintedSize(this._target).x / 2f;
		float nOfTarget = this._printedSize.y / (float)this._label.finalFontSize;
		float i = this._label.printedSize.y / (float)this._label.finalFontSize;
		float y = pivotOffset.y + (i - 2f * nOfTarget + 1f) * (float)this._label.finalFontSize * 0.5f * (i - 1f);
		this._sprite.localPosition = new Vector3(x, y, this._sprite.localPosition.z);
		yield break;
	}

	private UILabel _label;

	private Vector2 _printedSize;

	[SerializeField]
	[Header("替换的目标Sprite")]
	private Transform _sprite;

	[Header("替换的目标字符")]
	[SerializeField]
	private string _target;

	[Header("在使用键盘时是否生效")]
	[SerializeField]
	private bool _useInKeybordMode;
}
