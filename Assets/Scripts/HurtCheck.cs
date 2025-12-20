using System;
using UnityEngine;

public class HurtCheck : BaseBehaviour
{
	[SerializeField]
	public HurtCheck.BodyType bodyType;

	public enum BodyType
	{
		Horn,
		Body,
		Tail
	}
}
