using System;
using LitJson;
using UnityEngine;

public class ShadeAttackSkill : BaseBehaviour
{
	private void Start()
	{
		this.activeIndex = 0;
		if (this.data != null)
		{
			this.jsonData = JsonMapper.ToObject(this.data.text);
		}
	}

	private void Update()
	{
		if (this.activeIndex >= this.animArray.Length)
		{
			return;
		}
		this.activeTime += Time.deltaTime;
		if (this.activeTime > this.activeTimeArray[this.activeIndex])
		{
			this.Active();
			this.activeTime = 0f;
		}
	}

	private void Active()
	{
		if (this.activeIndex >= this.animArray.Length)
		{
			return;
		}
		this.animArray[this.activeIndex].SetActive(true);
		this.animArray[this.activeIndex].GetComponentInChildren<PlayerAtk>().SetData(this.jsonData["ShadeAtkSkill"], Incrementor.GetNextId());
		R.Audio.PlayEffect(200, new Vector3?(base.transform.position));
		this.activeIndex++;
	}

	private void OnDestroy()
	{
		bool isOnGround = R.Player.Attribute.isOnGround;
		R.Player.TimeController.SetSpeed(new Vector2(0f, 0f));
		R.Player.Rigidbody2D.gravityScale = 1f;
		R.Player.Action.ChangeState((!isOnGround) ? PlayerAction.StateEnum.RollEndFrame : PlayerAction.StateEnum.Atk14, 1f);
	}

	[SerializeField]
	private TextAsset data;

	private JsonData jsonData;

	[SerializeField]
	private GameObject[] animArray;

	[SerializeField]
	private float[] activeTimeArray;

	private int activeIndex;

	private float activeTime;
}
