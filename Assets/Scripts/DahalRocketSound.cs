using System;
using UnityEngine;

public class DahalRocketSound : BaseBehaviour
{
	private void Start()
	{
	}

	private void OnEnable()
	{
		R.Audio.PlayEffect(UnityEngine.Random.Range(136, 139), new Vector3?(base.transform.position));
	}

	private void Update()
	{
	}
}
