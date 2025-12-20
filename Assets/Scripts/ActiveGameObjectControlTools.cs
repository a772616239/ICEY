using System;
using SceneStory;
using UnityEngine;

public class ActiveGameObjectControlTools : BaseBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	public void SetBoss()
	{
		this.dahal.SetBoss();
	}

	public void ActiveGameObject()
	{
		this.activeTarget.SetActive(true);
	}

	public GameObject activeTarget;

	public DahalAnimTransformProxy dahal;
}
