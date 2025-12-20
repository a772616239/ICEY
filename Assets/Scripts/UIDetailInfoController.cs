using System;
using UnityEngine;

public class UIDetailInfoController : MonoBehaviour
{
	private void Start()
	{
		this.container.gameObject.SetActive(true);
	}

	private void Update()
	{
		if (this.play)
		{
			this.container.alpha = 0f;
			this.container.transform.position -= new Vector3(2f, 0f, 0f);
			this.play = false;
		}
		if (this.container.alpha < 1f)
		{
			this.container.alpha += 1f * Time.deltaTime;
			this.container.transform.position += new Vector3(1f * Time.deltaTime, 0f, 0f);
		}
		else
		{
			this.play = false;
		}
	}

	[SerializeField]
	private UIWidget container;

	[SerializeField]
	private bool play;
}
