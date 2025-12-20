using System;
using UnityEngine;

public class OilAtScreen : MonoBehaviour
{
	private void Start()
	{
		Vector3 b = new Vector3(UnityEngine.Random.Range(-2.5f, 2.5f), UnityEngine.Random.Range(-3f, 0f), -1f);
		Transform transform = UnityEngine.Object.Instantiate<Transform>(this.oil[UnityEngine.Random.Range(0, this.oil.Length)], base.transform.position + b, Quaternion.Euler(0f, 0f, 0f));
		transform.localScale = new Vector3(0.6f, 0.6f, 1f);
	}

	private void Update()
	{
	}

	[SerializeField]
	private Transform[] oil;
}
