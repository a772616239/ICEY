using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Xft
{
	[AddComponentMenu("Xffect/XffectCache")]
	public class XffectCache : MonoBehaviour
	{
		private void Awake()
		{
			this.mInited = false;
			this.Init();
		}

		public bool IsInited()
		{
			return this.mInited;
		}

		public void Init()
		{
			IEnumerator enumerator = base.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					XffectComponent component = transform.GetComponent<XffectComponent>();
					if (component != null)
					{
						component.Initialize();
						if (!this.EffectDic.ContainsKey(transform.name))
						{
							this.EffectDic[transform.name] = new List<XffectComponent>();
						}
						this.EffectDic[transform.name].Add(component);
					}
					CompositeXffect component2 = transform.GetComponent<CompositeXffect>();
					if (component2 != null)
					{
						component2.Initialize();
						if (!this.CompEffectDic.ContainsKey(transform.name))
						{
							this.CompEffectDic[transform.name] = new List<CompositeXffect>();
						}
						this.CompEffectDic[transform.name].Add(component2);
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
			this.mInited = true;
		}

		public XffectComponent AddXffect(string name)
		{
			Transform transform = base.transform.Find(name);
			if (transform == null)
			{
				UnityEngine.Debug.Log("object:" + name + "doesn't exist!");
				return null;
			}
			Transform transform2 = UnityEngine.Object.Instantiate<Transform>(transform, Vector3.zero, Quaternion.identity);
			transform2.parent = base.transform;
			XffectComponent.SetActive(transform2.gameObject, false);
			transform2.gameObject.name = transform.gameObject.name;
			XffectComponent component = transform2.GetComponent<XffectComponent>();
			this.EffectDic[name].Add(component);
			if (component != null)
			{
				component.Initialize();
			}
			return component;
		}

		public bool ContainsEffect(string name)
		{
			return this.EffectDic.ContainsKey(name);
		}

		public XffectComponent GetEffect(string name)
		{
			if (!this.EffectDic.ContainsKey(name))
			{
				UnityEngine.Debug.LogError("there is no effect:" + name + " in effect cache!");
				return null;
			}
			List<XffectComponent> list = this.EffectDic[name];
			if (list == null)
			{
				return null;
			}
			foreach (XffectComponent xffectComponent in list)
			{
				if (!(xffectComponent == null))
				{
					if (!XffectComponent.IsActive(xffectComponent.gameObject))
					{
						return xffectComponent;
					}
				}
			}
			return this.AddXffect(name);
		}

		public CompositeXffect GetCompositeXffect(string name)
		{
			List<CompositeXffect> list = this.CompEffectDic[name];
			if (list == null)
			{
				return null;
			}
			foreach (CompositeXffect compositeXffect in list)
			{
				if (!XffectComponent.IsActive(compositeXffect.gameObject))
				{
					return compositeXffect;
				}
			}
			return null;
		}

		public EffectNode EmitNode(string eftName, Vector3 pos)
		{
			List<XffectComponent> list = this.EffectDic[eftName];
			if (list == null)
			{
				UnityEngine.Debug.LogError(base.name + ": cache doesnt exist!");
				return null;
			}
			if (list.Count > 1)
			{
				UnityEngine.Debug.LogWarning("EmitNode() only support only-one xffect cache!");
			}
			XffectComponent xffectComponent = list[0];
			if (!XffectComponent.IsActive(xffectComponent.gameObject))
			{
				xffectComponent.Active();
			}
			EffectNode effectNode = xffectComponent.EmitByPos(pos);
			if (effectNode == null)
			{
				UnityEngine.Debug.LogError("emit node error! may be node max count is not enough:" + eftName);
			}
			return effectNode;
		}

		public XffectComponent ReleaseEffect(string name, Vector3 pos)
		{
			XffectComponent effect = this.GetEffect(name);
			if (effect == null)
			{
				UnityEngine.Debug.LogWarning("can't find available effect in cache!:" + name);
				return null;
			}
			effect.Active();
			effect.SetClient(base.transform);
			effect.SetEmitPosition(pos);
			return effect;
		}

		public XffectComponent ReleaseEffect(string name, Transform client)
		{
			XffectComponent effect = this.GetEffect(name);
			if (effect == null)
			{
				UnityEngine.Debug.LogWarning("can't find available effect in cache!:" + name);
				return null;
			}
			effect.Active();
			effect.SetClient(client);
			return effect;
		}

		public XffectComponent ReleaseEffect(string name)
		{
			XffectComponent effect = this.GetEffect(name);
			if (effect == null)
			{
				return null;
			}
			effect.Active();
			return effect;
		}

		public void StopEffect(string eftName, float fadeTime)
		{
			List<XffectComponent> list = this.EffectDic[eftName];
			if (list == null)
			{
				UnityEngine.Debug.LogError(base.name + ": cache doesnt exist!");
				return;
			}
			for (int i = 0; i < list.Count; i++)
			{
				XffectComponent xffectComponent = list[i];
				if (XffectComponent.IsActive(xffectComponent.gameObject))
				{
					xffectComponent.StopSmoothly(fadeTime);
				}
			}
		}

		public void DeActiveEffect(string eftName)
		{
			List<XffectComponent> list = this.EffectDic[eftName];
			if (list == null)
			{
				UnityEngine.Debug.LogError(base.name + ": cache doesnt exist!");
				return;
			}
			for (int i = 0; i < list.Count; i++)
			{
				XffectComponent xffectComponent = list[i];
				if (XffectComponent.IsActive(xffectComponent.gameObject))
				{
					xffectComponent.DeActive();
				}
			}
		}

		public bool IsEffectActive(string eftName)
		{
			List<XffectComponent> list = this.EffectDic[eftName];
			if (list == null)
			{
				UnityEngine.Debug.LogError(base.name + ": cache doesnt exist!");
				return true;
			}
			if (list.Count > 1)
			{
				UnityEngine.Debug.LogWarning("DeActive() only support one Xffect cache!");
			}
			XffectComponent xffectComponent = list[0];
			return XffectComponent.IsActive(xffectComponent.gameObject);
		}

		public void StopEffect(string eftName)
		{
			List<XffectComponent> list = this.EffectDic[eftName];
			if (list == null)
			{
				UnityEngine.Debug.LogError(base.name + ": cache doesnt exist!");
				return;
			}
			if (list.Count > 1)
			{
				UnityEngine.Debug.LogWarning("DeActive() only support one Xffect cache!");
			}
			XffectComponent xffectComponent = list[0];
			if (!XffectComponent.IsActive(xffectComponent.gameObject))
			{
				return;
			}
			xffectComponent.StopEmit();
		}

		public int GetAvailableEffectCount(string eftName)
		{
			List<XffectComponent> list = this.EffectDic[eftName];
			if (list == null)
			{
				return 0;
			}
			int num = 0;
			for (int i = 0; i < list.Count; i++)
			{
				XffectComponent xffectComponent = list[i];
				if (!(xffectComponent == null))
				{
					if (!XffectComponent.IsActive(xffectComponent.gameObject))
					{
						num++;
					}
				}
			}
			return num;
		}

		private Dictionary<string, List<XffectComponent>> EffectDic = new Dictionary<string, List<XffectComponent>>();

		private Dictionary<string, List<CompositeXffect>> CompEffectDic = new Dictionary<string, List<CompositeXffect>>();

		protected bool mInited;
	}
}
