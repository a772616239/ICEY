using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UISaveProgressCircleController : MonoBehaviour
{
	private void OnEnable()
	{
		SceneManager.sceneUnloaded += this.OnSceneUnloaded;
	}

	private void OnDisable()
	{
		SceneManager.sceneUnloaded -= this.OnSceneUnloaded;
	}

	public void Appear()
	{
		this._circle.SetActive(true);
	}

	public void Disappear()
	{
		this._circle.SetActive(false);
	}

	public void OnSceneUnloaded(Scene arg0)
	{
		this.Disappear();
	}

	[SerializeField]
	private GameObject _circle;
}
