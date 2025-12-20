using System;
using System.Collections;
using UnityEngine;

public class RandomRotate : MonoBehaviour
{
	private void Start()
	{
		this.deltaTime = 1f / (float)this.fps;
		this.rangeX = (float)UnityEngine.Random.Range(0, 10);
		this.rangeY = (float)UnityEngine.Random.Range(0, 10);
		this.rangeZ = (float)UnityEngine.Random.Range(0, 10);
	}

	private void OnBecameVisible()
	{
		this.isVisible = true;
		base.StartCoroutine(this.UpdateRotation());
	}

	private void OnBecameInvisible()
	{
		this.isVisible = false;
	}

	private IEnumerator UpdateRotation()
	{
		while (this.isVisible)
		{
			if (this.isRotate)
			{
				base.transform.Rotate(this.deltaTime * Mathf.Sin(Time.time + this.rangeX) * (float)this.x, this.deltaTime * Mathf.Sin(Time.time + this.rangeY) * (float)this.y, this.deltaTime * Mathf.Sin(Time.time + this.rangeZ) * (float)this.z);
			}
			yield return new WaitForSeconds(this.deltaTime);
		}
		yield break;
	}

	public bool isRotate = true;

	public int fps = 30;

	public int x = 100;

	public int y = 200;

	public int z = 300;

	private float rangeX;

	private float rangeY;

	private float rangeZ;

	private float deltaTime;

	private bool isVisible;
}
