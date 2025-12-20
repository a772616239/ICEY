using System;
using System.Collections.Generic;
using UnityEngine;

public class ParticleTester : MonoBehaviour
{
	private void Awake()
	{
		this.expBool = (this.flrBool = false);
		this.systemType = ParticleTester.SystemType.None;
		this.savedSystemType = ParticleTester.SystemType.None;
		this.particleSystems = Resources.LoadAll("Explosions", typeof(GameObject));
		this.loadFlareSystems = Resources.LoadAll("Flares", typeof(GameObject));
		this.loadDirectionalSystems = Resources.LoadAll("Directional", typeof(GameObject));
		this.flareSystems = new List<ParticleTester.FlareSystem>();
		this.directionalSystems = new List<ParticleTester.FlareSystem>();
		foreach (UnityEngine.Object @object in this.loadFlareSystems)
		{
			ParticleTester.FlareSystem flareSystem = new ParticleTester.FlareSystem();
			flareSystem.name = @object.name;
			flareSystem.particleObject = (UnityEngine.Object.Instantiate(@object, Vector3.zero, Quaternion.identity) as GameObject);
			flareSystem.particleSystems = flareSystem.particleObject.GetComponentsInChildren<ParticleSystem>();
			flareSystem.particleObject.SetActive(false);
			flareSystem.toggleFlag = false;
			flareSystem.particleObject.transform.parent = base.transform;
			this.flareSystems.Add(flareSystem);
		}
		foreach (UnityEngine.Object object2 in this.loadDirectionalSystems)
		{
			ParticleTester.FlareSystem flareSystem2 = new ParticleTester.FlareSystem();
			flareSystem2.name = object2.name;
			flareSystem2.particleObject = (UnityEngine.Object.Instantiate(object2, Vector3.zero, Quaternion.identity) as GameObject);
			flareSystem2.particleSystems = flareSystem2.particleObject.GetComponentsInChildren<ParticleSystem>();
			flareSystem2.particleObject.SetActive(false);
			flareSystem2.toggleFlag = false;
			flareSystem2.particleObject.transform.parent = base.transform;
			this.directionalSystems.Add(flareSystem2);
		}
	}

	private void OnGUI()
	{
		GUILayout.BeginHorizontal(new GUILayoutOption[0]);
		if (GUILayout.Toggle(this.expBool, "Explosions", new GUILayoutOption[0]))
		{
			this.expBool = this.SetBool();
			this.systemType = ParticleTester.SystemType.Explosions;
		}
		if (GUILayout.Toggle(this.flrBool, "Flares", new GUILayoutOption[0]))
		{
			this.flrBool = this.SetBool();
			this.systemType = ParticleTester.SystemType.Flares;
		}
		if (GUILayout.Toggle(this.flmBool, "Flames", new GUILayoutOption[0]))
		{
			this.flmBool = this.SetBool();
			this.systemType = ParticleTester.SystemType.Flames;
		}
		GUILayout.EndHorizontal();
		GUILayout.Space(20f);
		this.scrollPosition = GUILayout.BeginScrollView(this.scrollPosition, new GUILayoutOption[]
		{
			GUILayout.Width(250f),
			GUILayout.Height(550f)
		});
		ParticleTester.SystemType systemType = this.systemType;
		if (systemType != ParticleTester.SystemType.Explosions)
		{
			if (systemType != ParticleTester.SystemType.Flares)
			{
				if (systemType == ParticleTester.SystemType.Flames)
				{
					foreach (ParticleTester.FlareSystem flareSystem in this.directionalSystems)
					{
						flareSystem.toggleFlag = GUILayout.Toggle(flareSystem.toggleFlag, flareSystem.name, new GUILayoutOption[0]);
						if (flareSystem.toggleFlag != flareSystem.savedToggleFlag)
						{
							flareSystem.particleObject.SetActive(flareSystem.toggleFlag);
							if (flareSystem.toggleFlag)
							{
								foreach (ParticleSystem particleSystem in flareSystem.particleSystems)
								{
									particleSystem.Clear();
									particleSystem.Play();
								}
								flareSystem.toggleFlag = this.SetDirBool();
							}
							flareSystem.savedToggleFlag = flareSystem.toggleFlag;
						}
					}
				}
			}
			else
			{
				foreach (ParticleTester.FlareSystem flareSystem2 in this.flareSystems)
				{
					flareSystem2.toggleFlag = GUILayout.Toggle(flareSystem2.toggleFlag, flareSystem2.name, new GUILayoutOption[0]);
					if (flareSystem2.toggleFlag != flareSystem2.savedToggleFlag)
					{
						flareSystem2.particleObject.SetActive(flareSystem2.toggleFlag);
						if (flareSystem2.toggleFlag)
						{
							foreach (ParticleSystem particleSystem2 in flareSystem2.particleSystems)
							{
								particleSystem2.Clear();
								particleSystem2.Play();
							}
							flareSystem2.toggleFlag = this.SetFlareBool();
						}
						flareSystem2.savedToggleFlag = flareSystem2.toggleFlag;
					}
				}
			}
		}
		else
		{
			foreach (GameObject gameObject in this.particleSystems)
			{
				if (GUILayout.Button(gameObject.name, new GUILayoutOption[0]))
				{
					GameObject obj = UnityEngine.Object.Instantiate<GameObject>(gameObject, Vector3.zero, Quaternion.identity);
					UnityEngine.Object.Destroy(obj, 10f);
				}
			}
		}
		GUILayout.EndScrollView();
	}

	private void Update()
	{
		if (this.systemType != this.savedSystemType)
		{
			this.SetNoFlares();
			this.savedSystemType = this.systemType;
		}
	}

	private bool SetBool()
	{
		this.expBool = (this.flrBool = (this.flmBool = false));
		return true;
	}

	private bool SetFlareBool()
	{
		foreach (ParticleTester.FlareSystem flareSystem in this.flareSystems)
		{
			flareSystem.toggleFlag = false;
		}
		return true;
	}

	private bool SetDirBool()
	{
		foreach (ParticleTester.FlareSystem flareSystem in this.directionalSystems)
		{
			flareSystem.toggleFlag = false;
		}
		return true;
	}

	private void SetNoFlares()
	{
		foreach (ParticleTester.FlareSystem flareSystem in this.flareSystems)
		{
			flareSystem.toggleFlag = false;
			flareSystem.savedToggleFlag = false;
			flareSystem.particleObject.SetActive(false);
		}
		foreach (ParticleTester.FlareSystem flareSystem2 in this.directionalSystems)
		{
			flareSystem2.toggleFlag = false;
			flareSystem2.savedToggleFlag = false;
			flareSystem2.particleObject.SetActive(false);
		}
	}

	private ParticleTester.SystemType systemType;

	public UnityEngine.Object[] particleSystems;

	public UnityEngine.Object[] loadFlareSystems;

	public UnityEngine.Object[] loadDirectionalSystems;

	private List<ParticleTester.FlareSystem> flareSystems;

	private List<ParticleTester.FlareSystem> directionalSystems;

	private bool expBool;

	private bool flrBool;

	private bool flmBool;

	private Vector2 scrollPosition;

	private ParticleTester.SystemType savedSystemType;

	private enum SystemType
	{
		None,
		Explosions,
		Flares,
		Flames
	}

	private class FlareSystem
	{
		public string name;

		public GameObject particleObject;

		public ParticleSystem[] particleSystems;

		public bool toggleFlag;

		public bool savedToggleFlag;
	}
}
