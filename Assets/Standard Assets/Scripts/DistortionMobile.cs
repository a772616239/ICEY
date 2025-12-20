using System;
using System.Collections;
using UnityEngine;

public class DistortionMobile : MonoBehaviour
{
	private void OnEnable()
	{
		this.fpsMove = new WaitForSeconds(1f / (float)this.FPSWhenMoveCamera);
		this.fpsStatic = new WaitForSeconds(1f / (float)this.FPSWhenStaticCamera);
	}

	private void Update()
	{
		if (!this.isInitialized)
		{
			this.Initialize();
			base.StartCoroutine(this.RepeatCameraMove());
			base.StartCoroutine(this.RepeatCameraStatic());
		}
		if (Vector3.SqrMagnitude(this.instanceCameraTransform.position - this.oldPosition) <= 1E-05f && this.instanceCameraTransform.rotation == this.oldRotation)
		{
			this.frameCountWhenCameraIsStatic++;
			if (this.frameCountWhenCameraIsStatic >= 50)
			{
				this.isStaticUpdate = true;
			}
		}
		else
		{
			this.frameCountWhenCameraIsStatic = 0;
			this.isStaticUpdate = false;
		}
		this.oldPosition = this.instanceCameraTransform.position;
		this.oldRotation = this.instanceCameraTransform.rotation;
		if (this.canUpdateCamera)
		{
			this.cameraInstance.enabled = true;
			this.canUpdateCamera = false;
		}
		else if (this.cameraInstance.enabled)
		{
			this.cameraInstance.enabled = false;
		}
	}

	private IEnumerator RepeatCameraMove()
	{
		for (;;)
		{
			if (!this.isStaticUpdate)
			{
				this.canUpdateCamera = true;
			}
			yield return this.fpsMove;
		}
		yield break;
	}

	private IEnumerator RepeatCameraStatic()
	{
		for (;;)
		{
			if (this.isStaticUpdate)
			{
				this.canUpdateCamera = true;
			}
			yield return this.fpsStatic;
		}
		yield break;
	}

	private void OnBecameVisible()
	{
		if (this.goCamera != null)
		{
			this.goCamera.SetActive(true);
		}
	}

	private void OnBecameInvisible()
	{
		if (this.goCamera != null)
		{
			this.goCamera.SetActive(false);
		}
	}

	private void Initialize()
	{
		this.goCamera = new GameObject("RenderTextureCamera");
		this.cameraInstance = this.goCamera.AddComponent<Camera>();
		Camera main = Camera.main;
		this.cameraInstance.CopyFrom(main);
		this.cameraInstance.depth += 1f;
		this.cameraInstance.cullingMask = this.CullingMask;
		this.cameraInstance.renderingPath = this.RenderingPath;
		this.goCamera.transform.parent = main.transform;
		this.renderTexture = new RenderTexture(Mathf.RoundToInt((float)Screen.width * this.TextureScale), Mathf.RoundToInt((float)Screen.height * this.TextureScale), 16, this.RenderTextureFormat);
		this.renderTexture.DiscardContents();
		this.renderTexture.filterMode = this.FilterMode;
		this.cameraInstance.targetTexture = this.renderTexture;
		this.instanceCameraTransform = this.cameraInstance.transform;
		this.oldPosition = this.instanceCameraTransform.position;
		Shader.SetGlobalTexture("_GrabTextureMobile", this.renderTexture);
		this.isInitialized = true;
	}

	private void OnDisable()
	{
		if (this.goCamera)
		{
			UnityEngine.Object.DestroyImmediate(this.goCamera);
			this.goCamera = null;
		}
		if (this.renderTexture)
		{
			UnityEngine.Object.DestroyImmediate(this.renderTexture);
			this.renderTexture = null;
		}
		this.isInitialized = false;
	}

	public float TextureScale = 1f;

	public RenderTextureFormat RenderTextureFormat;

	public FilterMode FilterMode;

	public LayerMask CullingMask = -17;

	public RenderingPath RenderingPath;

	public int FPSWhenMoveCamera = 40;

	public int FPSWhenStaticCamera = 20;

	private RenderTexture renderTexture;

	private Camera cameraInstance;

	private GameObject goCamera;

	private Vector3 oldPosition;

	private Quaternion oldRotation;

	private Transform instanceCameraTransform;

	private bool canUpdateCamera;

	private bool isStaticUpdate;

	private WaitForSeconds fpsMove;

	private WaitForSeconds fpsStatic;

	private const int dropedFrames = 50;

	private int frameCountWhenCameraIsStatic;

	private bool isInitialized;
}
