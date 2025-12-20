using System;
using UnityEngine;

public class EnergyBallGenerator : BaseBehaviour
{
	public static EnergyBallGenerator Instance
	{
		get
		{
			if (EnergyBallGenerator.myInstance == null)
			{
				EnergyBallGenerator.myInstance = (UnityEngine.Object.FindObjectOfType(typeof(EnergyBallGenerator)) as EnergyBallGenerator);
			}
			return EnergyBallGenerator.myInstance;
		}
	}

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void Generate(Vector3 generatePos, int number, int typeId)
	{
		for (int i = 0; i < number; i++)
		{
			Transform transform = UnityEngine.Object.Instantiate<Transform>(this.energyBallArray[typeId]);
			transform.position = new Vector3(generatePos.x, generatePos.y, -0.01f);
		}
	}

	public void GenerateCircle(Vector3 generatePos, int number, int typeId)
	{
		generatePos = new Vector3(generatePos.x, generatePos.y, -0.01f);
		for (int i = 0; i < number; i++)
		{
			Transform transform = UnityEngine.Object.Instantiate<Transform>(this.energyBallArray[typeId]);
			transform.position = new Vector3(generatePos.x, generatePos.y, -0.01f);
			Vector3 target = new Vector3(generatePos.x + Mathf.Sin(6.28318548f / (float)number * (float)i), generatePos.y + Mathf.Cos(6.28318548f / (float)number * (float)i), -0.01f);
			transform.position = Vector3.MoveTowards(generatePos, target, 40f * Time.deltaTime);
		}
	}

	[SerializeField]
	private Transform[] energyBallArray;

	public static EnergyBallGenerator myInstance;
}
