using System;
using I2.Loc;
using UnityEngine;

public class PhoneController : BaseBehaviour
{
	public string LeftBtn
	{
		set
		{
			this._receiveText.text = value;
		}
	}

	public string RightBtn
	{
		set
		{
			this._cancelText.text = value;
		}
	}

	public string Name
	{
		get
		{
			return this._nameText.text;
		}
		set
		{
			this._nameText.text = value;
		}
	}

	public void DialOut()
	{
		this._receiveText.text = string.Empty;
		this._cancelText.text = ScriptLocalization.ui_phone.decline;
		this._animator.Play("IntoCall_Broken");
	}

	public void InCall()
	{
		this._receiveText.text = ScriptLocalization.ui_phone.accept;
		this._cancelText.text = ScriptLocalization.ui_phone.decline;
		this._animator.Play("Received");
	}

	public void Recive(bool isBreak = false)
	{
		this._receiveText.text = string.Empty;
		this._cancelText.text = ScriptLocalization.ui_phone.decline;
		this._animator.Play((!isBreak) ? "IntoInCall" : "IntoInCall_Broken");
	}

	public void CallEnd(bool isBreak = false)
	{
		this._receiveText.text = string.Empty;
		this._cancelText.text = string.Empty;
		this._animator.Play((!isBreak) ? "CallEnd" : "CallEnd_Broken");
	}

	[SerializeField]
	private Animator _animator;

	[SerializeField]
	private TextMesh _receiveText;

	[SerializeField]
	private TextMesh _cancelText;

	[SerializeField]
	private TextMesh _nameText;
}
