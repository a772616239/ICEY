using System;
using UnityEngine;

public class SewerLight : BaseBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		this.SetLamp(Mathf.Abs(R.Player.Transform.position.x - base.transform.position.x) < this.distance);
	}

	private void SetLamp(bool on)
	{
		this.lampOn.SetActive(on);
		this.sewerLight.SetActive(on);
	}

	[SerializeField]
	[Header("触发距离")]
	private float distance = 5f;

	[SerializeField]
	private GameObject lampOff;

	[SerializeField]
	private GameObject lampOn;

	[SerializeField]
	private GameObject sewerLight;
}
