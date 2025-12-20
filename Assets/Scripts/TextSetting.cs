using System;
using UnityEngine;

[RequireComponent(typeof(TextMesh))]
public class TextSetting : BaseBehaviour
{
	private void Awake()
	{
		this._textMesh = base.GetComponent<TextMesh>();
	}

	private void Start()
	{
		this._textMesh.text = this._ps4Str;
	}

	[SerializeField]
	private string _ps4Str;

	private TextMesh _textMesh;
}
