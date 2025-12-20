using System;
using UnityEngine;

public class UIHurtNumController : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	public void SetHurtNum(int damage)
	{
		this.label.text = damage.ToString();
	}

	[SerializeField]
	private UILabel label;
}
