using System;
using System.Collections;
using System.Text.RegularExpressions;
using Core;
using I2.Loc;
using UnityEngine;

public class StoryE16P5 : BaseBehaviour
{
	private void Start()
	{
		this.gate1.openType = SceneGate.OpenType.None;
		this.gate2.openType = SceneGate.OpenType.None;
		base.StartCoroutine(this.P14Coroutine());
	}

	private void OnDestroy()
	{
		SingletonMono<MobileInputPlayer>.Instance.HideL2R2(true);
	}

	private void SetMenu(string l, string r)
	{
		this._phone.LeftBtn = l;
		this._phone.RightBtn = r;
		Regex regex = new Regex("<size=\\d+>|</size>");
		SingletonMono<MobileInputPlayer>.Instance.ShowL2R2(regex.Replace(l, string.Empty), regex.Replace(r, string.Empty));
	}

	private IEnumerator P14Coroutine()
	{
		R.Audio.PlayBGM(354, true);
		yield return new WaitForSeconds(3f);
		this._phone.Name = ScriptLocalization.Story.e16s2;
		this._phone.InCall();
		SingletonMono<MobileInputPlayer>.Instance.MainControllerVisiable = false;
		this.SetMenu(string.Empty, string.Empty);
		yield return R.Audio.PlayVoiceOver("e16t13", null, false);
		this.SetMenu(ScriptLocalization.mobile.e16s3, ScriptLocalization.mobile.e16s4);
		while (!Core.Input.Game.L2.OnPressed)
		{
			if (Core.Input.Game.R2.OnPressed)
			{
				SingletonMono<MobileInputPlayer>.Instance.HideL2R2(false);
				base.StartCoroutine(this.P20Coroutine());
		
				yield break;
			}
			yield return null;
		}
		SingletonMono<MobileInputPlayer>.Instance.HideL2R2(false);
		base.StartCoroutine(this.P15Coroutine());
        yield break;
    }

	private IEnumerator P15Coroutine()
	{
		yield return R.Audio.PlayVoiceOver("e16t15", null, false);
		this.SetMenu(ScriptLocalization.mobile.e16s5, ScriptLocalization.mobile.e16s6);
		while (!Core.Input.Game.L2.OnPressed)
		{
			if (Core.Input.Game.R2.OnPressed)
			{
				SingletonMono<MobileInputPlayer>.Instance.HideL2R2(false);
				base.StartCoroutine(this.P15UnsureCoroutine());

				yield break;
			}
			yield return null;
		}
		SingletonMono<MobileInputPlayer>.Instance.HideL2R2(false);
		base.StartCoroutine(this.P16Coroutine());
        yield break;
    }

	private IEnumerator P16Coroutine()
	{
		yield return R.Audio.PlayVoiceOver("e16t16", null, false);
		this.SetMenu(ScriptLocalization.mobile.e16s7, ScriptLocalization.mobile.e16s8);
		while (!Core.Input.Game.L2.OnPressed)
		{
			if (Core.Input.Game.R2.OnPressed)
			{
				SingletonMono<MobileInputPlayer>.Instance.HideL2R2(false);
				base.StartCoroutine(this.P15UnsureCoroutine());
			
				yield break;
			}
			yield return null;
		}
		SingletonMono<MobileInputPlayer>.Instance.HideL2R2(false);
		base.StartCoroutine(this.P17Coroutine());
        yield break;
    }

	private IEnumerator P17Coroutine()
	{
		yield return R.Audio.PlayVoiceOver("e16t17", null, false);
		yield return new WaitForSeconds(R.Audio.PlayEffect(353, null).clip.length);
		this.SetMenu(ScriptLocalization.mobile.e16s9, ScriptLocalization.mobile.e16s10);
		yield return new WaitForSeconds(1.5f);
		while (!Core.Input.Game.L2.OnPressed && !Core.Input.Game.R2.OnPressed)
		{
			yield return null;
		}
		SingletonMono<MobileInputPlayer>.Instance.HideL2R2(false);
		base.StartCoroutine(this.P19Coroutine());
		yield break;
	}

	private IEnumerator P19Coroutine()
	{
		yield return R.Audio.PlayVoiceOver("e16t19", null, false);
		this.SetMenu(ScriptLocalization.mobile.e16s4, ScriptLocalization.mobile.e16s4);
		while (!Core.Input.Game.L2.OnPressed && !Core.Input.Game.R2.OnPressed)
		{
			yield return null;
		}
		SingletonMono<MobileInputPlayer>.Instance.HideL2R2(false);
		base.StartCoroutine(this.P20Coroutine());
		yield break;
	}

	private IEnumerator P20Coroutine()
	{
		R.Audio.StopBGM(true);
		this._phone.CallEnd(false);
		SingletonMono<MobileInputPlayer>.Instance.MainControllerVisiable = true;
		yield return R.Audio.PlayVoiceOver("e16t20", null, false);
		this.gate1.openType = SceneGate.OpenType.Right;
		yield break;
	}

	private IEnumerator P15UnsureCoroutine()
	{
		yield return R.Audio.PlayVoiceOver("e16t24", null, false);
		this.SetMenu(ScriptLocalization.mobile.e16s3, ScriptLocalization.mobile.e16s4);
		while (!Core.Input.Game.L2.OnPressed)
		{
			if (Core.Input.Game.R2.OnPressed)
			{
				SingletonMono<MobileInputPlayer>.Instance.HideL2R2(false);
				base.StartCoroutine(this.P20Coroutine());
				yield break;
			}
			yield return null;
		}
		SingletonMono<MobileInputPlayer>.Instance.HideL2R2(false);
		base.StartCoroutine(this.P15Coroutine());
        yield break;
    }

	[SerializeField]
	private PhoneController _phone;

	[SerializeField]
	private SceneGate gate1;

	[SerializeField]
	private SceneGate gate2;
}
