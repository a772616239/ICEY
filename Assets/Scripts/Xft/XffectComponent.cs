using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Xft
{
	[ExecuteInEditMode]
	[AddComponentMenu("Xffect/XffectComponent")]
	public class XffectComponent : MonoBehaviour
	{
		public Camera MyCamera
		{
			get
			{
				if (this.mCamera == null || !this.mCamera.gameObject.activeInHierarchy || !this.mCamera.enabled)
				{
					this.FindMyCamera();
				}
				return this.mCamera;
			}
			set
			{
				this.mCamera = value;
			}
		}

		public void FindMyCamera()
		{
			int num = 1 << base.gameObject.layer;
			Camera[] array = UnityEngine.Object.FindObjectsOfType(typeof(Camera)) as Camera[];
			int i = 0;
			int num2 = array.Length;
			while (i < num2)
			{
				Camera camera = array[i];
				if ((camera.cullingMask & num) != 0)
				{
					this.mCamera = camera;
					return;
				}
				i++;
			}
		}

		private void Awake()
		{
			this.FindMyCamera();
		}

		private void DestoryMeshObject(GameObject obj)
		{
			UnityEngine.Object.Destroy(obj);
		}

		private MeshFilter CreateMeshObj(Material mat)
		{
			GameObject gameObject = new GameObject("xftmesh " + mat.name);
			gameObject.layer = base.gameObject.layer;
			this.MeshList.Add(gameObject);
			gameObject.AddComponent<MeshFilter>();
			gameObject.AddComponent<MeshRenderer>();
			XffectComponent.SetActive(gameObject, true);
			MeshFilter meshFilter = (MeshFilter)gameObject.GetComponent(typeof(MeshFilter));
			MeshRenderer meshRenderer = (MeshRenderer)gameObject.GetComponent(typeof(MeshRenderer));
			meshRenderer.castShadows = false;
			meshRenderer.receiveShadows = false;
			meshRenderer.GetComponent<Renderer>().sharedMaterial = mat;
			if (this.UseWith2DSprite)
			{
				meshRenderer.sortingLayerName = this.SortingLayerName;
				meshRenderer.sortingOrder = this.SortingOrder;
			}
			meshFilter.sharedMesh = new Mesh();
			return meshFilter;
		}

		public void Initialize()
		{
			if (this.EflList.Count > 0)
			{
				return;
			}
			List<GameObject> list = new List<GameObject>();
			IEnumerator enumerator = base.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					if (transform.gameObject.name.Contains("xftmesh"))
					{
						list.Add(transform.gameObject);
					}
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			IEnumerator enumerator2 = base.transform.GetEnumerator();
			try
			{
				while (enumerator2.MoveNext())
				{
					object obj2 = enumerator2.Current;
					Transform transform2 = (Transform)obj2;
					EffectLayer effectLayer = (EffectLayer)transform2.GetComponent(typeof(EffectLayer));
					if (!(effectLayer == null))
					{
						if (effectLayer.Material == null)
						{
							UnityEngine.Debug.LogWarning("effect layer: " + effectLayer.gameObject.name + " has no material, please assign a material first!");
						}
						else
						{
							Material material = effectLayer.Material;
							material.renderQueue = material.shader.renderQueue;
							material.renderQueue += effectLayer.Depth;
							this.EflList.Add(effectLayer);
							if (this.MergeSameMaterialMesh)
							{
								if (!this.MatDic.ContainsKey(material.name))
								{
									MeshFilter meshFilter = this.CreateMeshObj(material);
									this.MatDic[material.name] = new VertexPool(meshFilter.sharedMesh, material);
								}
							}
							else
							{
								MeshFilter meshFilter2 = this.CreateMeshObj(material);
								this.MatList.Add(new VertexPool(meshFilter2.sharedMesh, material));
							}
						}
					}
				}
			}
			finally
			{
				IDisposable disposable2;
				if ((disposable2 = (enumerator2 as IDisposable)) != null)
				{
					disposable2.Dispose();
				}
			}
			foreach (GameObject obj3 in list)
			{
				this.DestoryMeshObject(obj3);
			}
			foreach (GameObject gameObject in this.MeshList)
			{
				gameObject.transform.parent = base.transform;
				gameObject.transform.position = Vector3.zero;
				gameObject.transform.rotation = Quaternion.identity;
				Vector3 zero = Vector3.zero;
				zero.x = 1f / Mathf.Clamp(gameObject.transform.parent.lossyScale.x, 1f, float.MaxValue);
				zero.y = 1f / Mathf.Clamp(gameObject.transform.parent.lossyScale.y, 1f, float.MaxValue);
				zero.z = 1f / Mathf.Clamp(gameObject.transform.parent.lossyScale.z, 1f, float.MaxValue);
				gameObject.transform.localScale = zero * this.Scale;
			}
			for (int i = 0; i < this.EflList.Count; i++)
			{
				EffectLayer effectLayer2 = this.EflList[i];
				if (this.MergeSameMaterialMesh)
				{
					effectLayer2.Vertexpool = this.MatDic[effectLayer2.Material.name];
				}
				else if (this.EflList.Count != this.MatList.Count)
				{
					UnityEngine.Debug.LogError("something wrong with the no merge mesh mat list!");
					effectLayer2.Vertexpool = this.MatList[0];
				}
				else
				{
					effectLayer2.Vertexpool = this.MatList[i];
				}
			}
			base.transform.localScale = Vector3.one;
			foreach (EffectLayer effectLayer3 in this.EflList)
			{
				effectLayer3.StartCustom();
			}
			this.EventList.Clear();
			IEnumerator enumerator6 = base.transform.GetEnumerator();
			try
			{
				while (enumerator6.MoveNext())
				{
					object obj4 = enumerator6.Current;
					Transform transform3 = (Transform)obj4;
					XftEventComponent component = transform3.GetComponent<XftEventComponent>();
					if (!(component == null))
					{
						this.EventList.Add(component);
						component.Initialize(this);
					}
				}
			}
			finally
			{
				IDisposable disposable3;
				if ((disposable3 = (enumerator6 as IDisposable)) != null)
				{
					disposable3.Dispose();
				}
			}
			this.Initialized = true;
		}

		private void Start()
		{
			if (!this.Initialized)
			{
				this.Initialize();
			}
			this.LastTime = (double)Time.realtimeSinceStartup;
		}

		public void Update()
		{
			this.CurTime = (double)Time.realtimeSinceStartup;
			float num = (float)(this.CurTime - this.LastTime);
			if (num > 0.1f)
			{
				num = 0.0333f;
			}
			if (this.Paused)
			{
				this.LastTime = this.CurTime;
				return;
			}
			if (!this.UpdateWhenOffScreen && !this.IsInCameraView() && !this.EditView)
			{
				this.LastTime = this.CurTime;
				return;
			}
			if (!this.IgnoreTimeScale)
			{
				num *= Time.timeScale;
			}
			this.ElapsedTime += num;
			for (int i = 0; i < this.EflList.Count; i++)
			{
				if (this.EflList[i] == null)
				{
					return;
				}
				EffectLayer effectLayer = this.EflList[i];
				if (this.ElapsedTime > effectLayer.StartTime && XffectComponent.IsActive(effectLayer.gameObject))
				{
					effectLayer.FixedUpdateCustom(num);
				}
			}
			for (int j = 0; j < this.EventList.Count; j++)
			{
				XftEventComponent xftEventComponent = this.EventList[j];
				if (XffectComponent.IsActive(xftEventComponent.gameObject))
				{
					xftEventComponent.UpdateCustom(num);
				}
			}
			this.LastTime = this.CurTime;
		}

		public void ResetEditorEvents()
		{
			for (int i = 0; i < this.EventList.Count; i++)
			{
				XftEventComponent xftEventComponent = this.EventList[i];
				xftEventComponent.ResetCustom();
			}
		}

		private void DoFinish()
		{
			this.mIsActive = false;
			for (int i = 0; i < this.EflList.Count; i++)
			{
				EffectLayer effectLayer = this.EflList[i];
				effectLayer.Reset();
			}
			this.DeActive();
			this.ElapsedTime = 0f;
			if (this.AutoDestroy)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}

		private bool IsInCameraView()
		{
			for (int i = 0; i < this.MeshList.Count; i++)
			{
				GameObject gameObject = this.MeshList[i];
				if (gameObject != null)
				{
					MeshRenderer component = gameObject.GetComponent<MeshRenderer>();
					if (component.isVisible)
					{
						return true;
					}
				}
			}
			for (int j = 0; j < this.EflList.Count; j++)
			{
				EffectLayer effectLayer = this.EflList[j];
				Vector3 position = effectLayer.ClientTransform.position;
				Vector3 vector = this.MyCamera.WorldToViewportPoint(position);
				if (vector.x >= 0f && vector.x <= 1f && vector.y >= 0f && vector.y <= 1f && vector.z > 0f)
				{
					return true;
				}
			}
			return false;
		}

		private void UpdateMeshObj()
		{
			for (int i = 0; i < this.MeshList.Count; i++)
			{
				GameObject gameObject = this.MeshList[i];
				if (gameObject != null)
				{
					gameObject.transform.position = Vector3.zero;
					gameObject.transform.rotation = Quaternion.identity;
					Vector3 zero = Vector3.zero;
					zero.x = 1f / gameObject.transform.parent.lossyScale.x;
					zero.y = 1f / gameObject.transform.parent.lossyScale.y;
					zero.z = 1f;
					gameObject.transform.localScale = zero * this.Scale;
				}
			}
			if (this.MergeSameMaterialMesh)
			{
				foreach (KeyValuePair<string, VertexPool> keyValuePair in this.MatDic)
				{
					keyValuePair.Value.LateUpdate();
				}
			}
			else
			{
				for (int j = 0; j < this.MatList.Count; j++)
				{
					VertexPool vertexPool = this.MatList[j];
					vertexPool.LateUpdate();
				}
			}
		}

		private void LateUpdate()
		{
			if (!this.UpdateWhenOffScreen && !this.IsInCameraView() && !this.EditView)
			{
				return;
			}
			this.UpdateMeshObj();
			if (this.ElapsedTime > this.LifeTime && this.LifeTime >= 0f)
			{
				this.DoFinish();
			}
			else if (this.LifeTime < 0f && this.EflList.Count > 0)
			{
				float deltaTime = (float)(this.CurTime - this.LastTime);
				bool flag = true;
				for (int i = 0; i < this.EflList.Count; i++)
				{
					EffectLayer effectLayer = this.EflList[i];
					if (!effectLayer.EmitOver(deltaTime))
					{
						flag = false;
					}
				}
				if (flag)
				{
					this.DoFinish();
				}
			}
		}

		private void OnDrawGizmosSelected()
		{
		}

		public void Active()
		{
			if (!this.Initialized)
			{
				this.Initialize();
			}
			base.gameObject.SetActive(true);
			this.Reset();
		}

		public void DeActive()
		{
			for (int i = 0; i < this.EventList.Count; i++)
			{
				XftEventComponent xftEventComponent = this.EventList[i];
				xftEventComponent.ResetCustom();
			}
			base.gameObject.SetActive(false);
		}

		public bool IsPlaying
		{
			get
			{
				return this.mIsActive;
			}
		}

		public void Reset()
		{
			this.mIsActive = true;
			this.ElapsedTime = 0f;
			this.Start();
			for (int i = 0; i < this.EflList.Count; i++)
			{
				EffectLayer effectLayer = this.EflList[i];
				effectLayer.Reset();
			}
			for (int j = 0; j < this.EventList.Count; j++)
			{
				XftEventComponent xftEventComponent = this.EventList[j];
				xftEventComponent.ResetCustom();
			}
		}

		public void StopSmoothly(float fadeTime)
		{
			for (int i = 0; i < this.EflList.Count; i++)
			{
				EffectLayer effectLayer = this.EflList[i];
				effectLayer.StopSmoothly(fadeTime);
			}
		}

		public void ActiveNoInterrupt()
		{
			if (XffectComponent.IsActive(base.gameObject))
			{
				return;
			}
			this.Active();
		}

		public void SetScale(Vector2 scale, string eflName)
		{
			foreach (EffectLayer effectLayer in this.EflList)
			{
				if (effectLayer.gameObject.name == eflName)
				{
					effectLayer.SetScale(scale);
				}
			}
		}

		public void SetColor(Color color, string eflName)
		{
			foreach (EffectLayer effectLayer in this.EflList)
			{
				if (effectLayer.gameObject.name == eflName)
				{
					effectLayer.SetColor(color);
				}
			}
		}

		public void SetRotation(float angle, string eflName)
		{
			foreach (EffectLayer effectLayer in this.EflList)
			{
				if (effectLayer.gameObject.name == eflName)
				{
					effectLayer.SetRotation(angle);
				}
			}
		}

		public void SetGravityGoal(Transform goal, string eflName)
		{
			foreach (EffectLayer effectLayer in this.EflList)
			{
				if (effectLayer.gameObject.name == eflName && effectLayer.GravityAffectorEnable)
				{
					effectLayer.SetArractionAffectorGoal(goal);
				}
			}
		}

		public void SetCollisionGoalPos(Transform pos, string eflName)
		{
			foreach (EffectLayer effectLayer in this.EflList)
			{
				if (effectLayer.gameObject.name == eflName && effectLayer.UseCollisionDetection)
				{
					effectLayer.SetCollisionGoalPos(pos);
				}
			}
		}

		public void ResetCamera(Camera cam)
		{
		}

		public void SetClient(Transform client)
		{
			foreach (EffectLayer effectLayer in this.EflList)
			{
				effectLayer.SetClient(client);
			}
		}

		public void SetDirectionAxis(Vector3 axis)
		{
			foreach (EffectLayer effectLayer in this.EflList)
			{
				effectLayer.OriVelocityAxis = axis;
			}
		}

		public void SetEmitPosition(Vector3 pos)
		{
			foreach (EffectLayer effectLayer in this.EflList)
			{
				effectLayer.EmitPoint = pos;
			}
		}

		public void SetCollisionGoalPos(Transform pos)
		{
			foreach (EffectLayer effectLayer in this.EflList)
			{
				if (effectLayer.UseCollisionDetection)
				{
					effectLayer.SetCollisionGoalPos(pos);
				}
			}
		}

		public void SetArractionAffectorGoal(Transform goal)
		{
			foreach (EffectLayer effectLayer in this.EflList)
			{
				if (effectLayer.GravityAffectorEnable)
				{
					effectLayer.SetArractionAffectorGoal(goal);
				}
			}
		}

		public void SetArractionAffectorGoal(Transform goal, string eflName)
		{
			foreach (EffectLayer effectLayer in this.EflList)
			{
				if (effectLayer.gameObject.name == eflName && effectLayer.GravityAffectorEnable)
				{
					effectLayer.SetArractionAffectorGoal(goal);
				}
			}
		}

		public EffectNode EmitByPos(Vector3 pos)
		{
			if (this.EflList.Count > 1)
			{
				UnityEngine.Debug.LogWarning("EmitByPos only support one effect layer!");
			}
			EffectLayer effectLayer = this.EflList[0];
			return effectLayer.EmitByPos(pos);
		}

		public void StopEmit()
		{
			for (int i = 0; i < this.EflList.Count; i++)
			{
				EffectLayer effectLayer = this.EflList[i];
				effectLayer.StopEmit();
			}
		}

		public static bool IsActive(GameObject obj)
		{
			return obj.activeSelf;
		}

		public static void SetActive(GameObject obj, bool flag)
		{
			obj.SetActive(flag);
		}

		public static string CurVersion = "4.4.1";

		public string MyVersion = "4.4.1";

		private Dictionary<string, VertexPool> MatDic = new Dictionary<string, VertexPool>();

		private List<EffectLayer> EflList = new List<EffectLayer>();

		private List<XftEventComponent> EventList = new List<XftEventComponent>();

		private List<VertexPool> MatList = new List<VertexPool>();

		public float LifeTime = -1f;

		public bool IgnoreTimeScale;

		protected float ElapsedTime;

		protected bool Initialized;

		protected List<GameObject> MeshList = new List<GameObject>();

		protected double LastTime;

		protected double CurTime;

		public bool EditView;

		public float Scale = 1f;

		public int MaxFps = 60;

		public bool AutoDestroy;

		public bool MergeSameMaterialMesh = true;

		public bool Paused;

		public bool UpdateWhenOffScreen = true;

		public bool UseWith2DSprite;

		public string SortingLayerName = "Default";

		public int SortingOrder;

		public float PlaybackTime = 1f;

		protected bool mIsActive;

		protected Camera mCamera;
	}
}
