using System;
using System.Collections;
using UnityEngine;

public class EnergyBall : BaseBehaviour
{
	private float deltaTime
	{
		get
		{
			return Time.time - this.startTime;
		}
	}

	private void OnEnable()
	{
		this.startTime = Time.time;
		this.player = R.Player.Transform;
		base.transform.Find("Tail").gameObject.SetActive(true);
		base.transform.Find("Flash").gameObject.SetActive(true);
		Vector2 vector = new Vector2(((this.player.GetComponent<Collider2D>().bounds.center - base.transform.position).x <= 0f) ? UnityEngine.Random.Range(0f, 1f) : UnityEngine.Random.Range(-1f, 0f), UnityEngine.Random.Range(-1f, 1f));
		this.randomDir = vector.normalized * 5f;
		base.GetComponent<Rigidbody2D>().velocity = this.randomDir;
		base.gameObject.GetComponent<Animator>().Play("EnergyBallFade");
		base.transform.Find("Tail").localPosition = Vector3.zero;
		Light component = this.light.GetComponent<Light>();
		component.intensity = 3f;
		base.StartCoroutine(this.UseBall());
	}

	private void Update()
	{
	}

	private IEnumerator UseBall()
	{
		while (this.deltaTime <= this.waitTime)
		{
			base.GetComponent<Rigidbody2D>().AddForce(new Vector2(-base.GetComponent<Rigidbody2D>().velocity.y, base.GetComponent<Rigidbody2D>().velocity.x) * 6f);
			if (Vector2.Angle(base.GetComponent<Rigidbody2D>().velocity, base.transform.position - this.player.position) > 160f && this.deltaTime <= this.waitTime)
			{
				break;
			}
			yield return new WaitForFixedUpdate();
		}
		base.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 0f));
		while (this.isMove)
		{
			base.transform.position = Vector3.MoveTowards(base.transform.position, this.player.position + this.repairPosition, 0.02f * this.speed * this.deltaTime);
			yield return new WaitForFixedUpdate();
			if (Vector3.Distance(base.transform.position, this.player.position + this.repairPosition) < 0.4f)
			{
				this.EnergyBallFunction(this.energyType);
				base.transform.Find("Tail").gameObject.SetActive(false);
				base.gameObject.GetComponent<Animator>().Play("EnergyBallCollision");
				base.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
				base.transform.Find("Flash").gameObject.SetActive(false);
				base.StartCoroutine(this.player.GetComponent<ChangeSpineColor>().EnergyBallColorChange());
				base.StartCoroutine(this.FadeInLight());
				break;
			}
		}
		yield break;
	}

	private void EnergyBallFunction(EnergyBall.EnergyBallType energyType)
	{
		if (energyType != EnergyBall.EnergyBallType.Armor)
		{
		}
	}

	private IEnumerator FadeInLight()
	{
		Light i = this.light.GetComponent<Light>();
		while (i.range < 6f)
		{
			i.range += 0.2f;
			yield return new WaitForSeconds(0.04f);
		}
		i.range = 4f;
		i.intensity = 0f;
		yield break;
	}

	[SerializeField]
	private bool isMove = true;

	[SerializeField]
	private float speed = 1f;

	[SerializeField]
	private float minDistance = 0.1f;

	[SerializeField]
	private float MaxDistance = 2.5f;

	[SerializeField]
	private EnergyBall.EnergyBallType energyType;

	[SerializeField]
	private int energyValue = 10;

	[SerializeField]
	private float waitTime;

	[SerializeField]
	private Transform light;

	private float startTime;

	private Transform player;

	private Vector3 repairPosition = new Vector3(0f, 1.2f, -0.1f);

	private Vector2 randomDir;

	public enum EnergyBallType
	{
		Armor = 1,
		Skill
	}
}
