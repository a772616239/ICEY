using System;
using UnityEngine;

public class AutoDisableEffect : MonoBehaviour
{
	private void Update()
	{
		if (Math.Abs(this.delayTime) < 1.401298E-45f)
		{
			return;
		}
		if (this._timeFromEnable > this.delayTime)
		{
			this.Disable(base.transform);
		}
		else
		{
			this._timeFromEnable += Time.deltaTime;
		}
	}

	public void Disable(Transform transf)
	{
		this._timeFromEnable = 0f;
		transf.gameObject.SetActive(false);
		transf.parent = R.Effect.transform;
		transf.position = Vector3.zero;
		transf.rotation = Quaternion.Euler(Vector3.zero);
		transf.localScale = Vector3.one;
	}

	[SerializeField]
	private float delayTime = 2f;

	private float _timeFromEnable;
}
