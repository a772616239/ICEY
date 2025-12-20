using System;
using LitJson;
using UnityEngine;

public class ShadeAttackChase : BaseBehaviour
{
	private float height
	{
		get
		{
			return Physics2D.Raycast(base.transform.position, Vector2.up * -1f, 100f, LayerManager.GroundMask | LayerManager.OneWayGroundMask).distance;
		}
	}

	private void Awake()
	{
		this.skeletonAnim = base.GetComponent<SkeletonAnimation>();
		this.jsonData = JsonMapper.ToObject(this.data.text);
		base.GetComponentInChildren<PlayerAtk>().SetData(this.jsonData["AirAtk7"], Incrementor.GetNextId());
	}

	private void OnEnable()
	{
		this.skeletonAnim.state.SetAnimation(0, this.GetAnim(), false);
		if (this.height >= 0.5f)
		{
			Vector3 euler = new Vector3(0f, 0f, UnityEngine.Random.Range(-30f, 30f));
			base.transform.localRotation = Quaternion.Euler(euler);
		}
		R.Audio.PlayEffect(200, new Vector3?(base.transform.position));
	}

	private void OnDisable()
	{
		this.skeletonAnim.Reset();
	}

	private string GetAnim()
	{
		if (this.height >= 0.5f)
		{
			return this.airAnim[UnityEngine.Random.Range(0, this.airAnim.Length)];
		}
		return this.groundAnim[UnityEngine.Random.Range(0, this.groundAnim.Length)];
	}

	private SkeletonAnimation skeletonAnim;

	[SerializeField]
	private TextAsset data;

	private JsonData jsonData;

	private string[] airAnim = new string[]
	{
		"ToLeft",
		"ToLeftAir"
	};

	private string[] groundAnim = new string[]
	{
		"Down",
		"Down2",
		"ToLeftGround",
		"ToLeftGround2"
	};
}
