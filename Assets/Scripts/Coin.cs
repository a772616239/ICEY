using System;
using System.Collections;
using UnityEngine;

public class Coin : BaseBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	public IEnumerator GenerateCoinCoroutine(float randomRange, Vector3 position, int count, int coinValue)
	{
		for (int i = 0; i < count; i++)
		{
			GameObject obj = UnityEngine.Object.Instantiate<GameObject>(this.coin, position + new Vector3(0f, 2f), Quaternion.Euler(Vector3.zero));
			obj.GetComponent<RedMoney>().coinValue = coinValue;
			obj.GetComponent<Rigidbody2D>().gravityScale = 0.8f;
			obj.GetComponent<Rigidbody2D>().velocity = new Vector2(UnityEngine.Random.Range(-randomRange, randomRange), 17f);
			yield return new WaitForSeconds(0.05f);
		}
		yield break;
	}

	public void GenerateCoin(float randomRange, int count, int coinValue)
	{
		base.StartCoroutine(this.GenerateCoinCoroutine(randomRange, base.transform.position, count, coinValue));
	}

	public GameObject coin;
}
