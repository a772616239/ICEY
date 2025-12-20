using System;
using I2.Loc;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class GateEnterPrompt : BaseBehaviour
{
	private void Start()
	{
		this._info[0] = ScriptLocalization.ui_detailInfo.coordinate + ScriptLocalization.Get("ui/detailInfo/coordinate/" + this._coordinateId);
		this._info[1] = ScriptLocalization.ui_detailInfo.altitude + this._altitude + ScriptLocalization.ui_detailInfo.meter;
		this._info[2] = ScriptLocalization.Get("mobile/detailInfo/" + this._promptId);
	}

	private void Update()
	{
		if (R.Mode.IsInStoryMode() && this._promptAnim)
		{
			this._promptAnim.GetComponent<DetailInfoAnim>().FadeOut(0f);
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			this._promptAnim = UnityEngine.Object.Instantiate<GameObject>(this.detailUI).GetComponent<DetailInfoAnim>();
			this._promptAnim.StartAnim(this._info);
			this._promptAnim.GetComponent<NGUIFollowGameObject>().SetFollowTransform(base.transform, true);
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Player") && this._promptAnim)
		{
			this._promptAnim.FadeOut(0f);
		}
	}

	private void OnDestroy()
	{
		if (this._promptAnim != null)
		{
			UnityEngine.Object.Destroy(this._promptAnim.gameObject);
		}
	}

	public void FadeOut()
	{
		if (this._promptAnim != null)
		{
			this._promptAnim.FadeOut(0f);
		}
	}

	[SerializeField]
	private GameObject detailUI;

	[SerializeField]
	private int _coordinateId;

	[SerializeField]
	private int _altitude;

	[SerializeField]
	private int _promptId;

	private string[] _info = new string[3];

	private DetailInfoAnim _promptAnim;
}
