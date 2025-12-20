using System;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Renderer))]
[RequireComponent(typeof(MeshFilter))]
public class PlaneInScene : MonoBehaviour
{
	public void Awake()
	{
		this.shader = Shader.Find("Sprites/Default");
		base.GetComponent<Renderer>().sharedMaterial = new Material(this.shader);
		Mesh mesh = new Mesh();
		base.GetComponent<MeshFilter>().sharedMesh = mesh;
		this._mesh = mesh;
		this._renderer = base.GetComponent<Renderer>();
	}

	public void OnEnable()
	{
		this._renderer.enabled = true;
	}

	public void OnDisable()
	{
		this._renderer.enabled = false;
	}

	public void Update()
	{
		if (Application.isPlaying)
		{
			return;
		}
	}

	private Mesh _mesh;

	private Renderer _renderer;

	[SerializeField]
	private Color color = new Color(0f, 1f, 1f, 0.2f);

	[SerializeField]
	private Vector2 offset = new Vector2(0f, 0f);

	[HideInInspector]
	private Shader shader;

	[SerializeField]
	private Vector2 size = new Vector2(100f, 100f);
}
