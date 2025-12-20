using System;
using UnityEngine;

[ExecuteInEditMode]
public class ProgressBarLabelPercentageBinding : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
		if (this.progressBar && this.label)
		{
			this.label.text = (this.progressBar.value * 100f).ToString("f1") + "%";
		}
	}

	[SerializeField]
	private UIProgressBar progressBar;

	[SerializeField]
	private UILabel label;
}
