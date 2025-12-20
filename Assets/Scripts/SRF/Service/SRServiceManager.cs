using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SRF.Components;
using SRF.Helpers;
using UnityEngine;

namespace SRF.Service
{
	[AddComponentMenu("SRF/Service/Service Manager")]
	public class SRServiceManager : SRAutoSingleton<SRServiceManager>
	{
		public static bool IsLoading
		{
			get
			{
				return SRServiceManager.LoadingCount > 0;
			}
		}

		public static T GetService<T>() where T : class
		{
			T t = SRServiceManager.GetServiceInternal(typeof(T)) as T;
			if (t == null && (!SRServiceManager._hasQuit || false))
			{
				UnityEngine.Debug.LogWarning("Service {0} not found. (HasQuit: {1})".Fmt(new object[]
				{
					typeof(T).Name,
					SRServiceManager._hasQuit
				}));
			}
			return t;
		}

		public static object GetService(Type t)
		{
			object serviceInternal = SRServiceManager.GetServiceInternal(t);
			if (serviceInternal == null && (!SRServiceManager._hasQuit || false))
			{
				UnityEngine.Debug.LogWarning("Service {0} not found. (HasQuit: {1})".Fmt(new object[]
				{
					t.Name,
					SRServiceManager._hasQuit
				}));
			}
			return serviceInternal;
		}

		private static object GetServiceInternal(Type t)
		{
			if (SRServiceManager._hasQuit || !Application.isPlaying)
			{
				return null;
			}
			SRList<SRServiceManager.Service> services = SRAutoSingleton<SRServiceManager>.Instance._services;
			int i = 0;
			while (i < services.Count)
			{
				SRServiceManager.Service service = services[i];
				if (t.IsAssignableFrom(service.Type))
				{
					if (service.Object == null)
					{
						SRServiceManager.UnRegisterService(t);
						break;
					}
					return service.Object;
				}
				else
				{
					i++;
				}
			}
			return SRAutoSingleton<SRServiceManager>.Instance.AutoCreateService(t);
		}

		public static bool HasService<T>() where T : class
		{
			return SRServiceManager.HasService(typeof(T));
		}

		public static bool HasService(Type t)
		{
			if (SRServiceManager._hasQuit || !Application.isPlaying)
			{
				return false;
			}
			SRList<SRServiceManager.Service> services = SRAutoSingleton<SRServiceManager>.Instance._services;
			for (int i = 0; i < services.Count; i++)
			{
				SRServiceManager.Service service = services[i];
				if (t.IsAssignableFrom(service.Type))
				{
					return service.Object != null;
				}
			}
			return false;
		}

		public static void RegisterService<T>(object service) where T : class
		{
			SRServiceManager.RegisterService(typeof(T), service);
		}

		private static void RegisterService(Type t, object service)
		{
			if (SRServiceManager._hasQuit)
			{
				return;
			}
			if (SRServiceManager.HasService(t))
			{
				if (SRServiceManager.GetServiceInternal(t) == service)
				{
					return;
				}
				throw new Exception("Service already registered for type " + t.Name);
			}
			else
			{
				SRServiceManager.UnRegisterService(t);
				if (!t.IsInstanceOfType(service))
				{
					throw new ArgumentException("service {0} must be assignable from type {1}".Fmt(new object[]
					{
						service.GetType(),
						t
					}));
				}
				SRAutoSingleton<SRServiceManager>.Instance._services.Add(new SRServiceManager.Service
				{
					Object = service,
					Type = t
				});
				return;
			}
		}

		public static void UnRegisterService<T>() where T : class
		{
			SRServiceManager.UnRegisterService(typeof(T));
		}

		private static void UnRegisterService(Type t)
		{
			if (SRServiceManager._hasQuit || !SRAutoSingleton<SRServiceManager>.HasInstance)
			{
				return;
			}
			if (!SRServiceManager.HasService(t))
			{
				return;
			}
			SRList<SRServiceManager.Service> services = SRAutoSingleton<SRServiceManager>.Instance._services;
			for (int i = services.Count - 1; i >= 0; i--)
			{
				SRServiceManager.Service service = services[i];
				if (service.Type == t)
				{
					services.RemoveAt(i);
				}
			}
		}

		protected override void Awake()
		{
			SRServiceManager._hasQuit = false;
			base.Awake();
			UnityEngine.Object.DontDestroyOnLoad(base.CachedGameObject);
			base.CachedGameObject.hideFlags = HideFlags.NotEditable;
		}

		protected void UpdateStubs()
		{
			if (this._serviceStubs != null)
			{
				return;
			}
			this._serviceStubs = new List<SRServiceManager.ServiceStub>();
			List<Type> list = new List<Type>();
			Assembly assembly = typeof(SRServiceManager).Assembly;
			try
			{
				list.AddRange(assembly.GetExportedTypes());
			}
			catch (Exception exception)
			{
				UnityEngine.Debug.LogError("[SRServiceManager] Error loading assembly {0}".Fmt(new object[]
				{
					assembly.FullName
				}), this);
				UnityEngine.Debug.LogException(exception);
			}
			foreach (Type type in list)
			{
				this.ScanType(type);
			}
		}

		protected object AutoCreateService(Type t)
		{
			this.UpdateStubs();
			foreach (SRServiceManager.ServiceStub serviceStub in this._serviceStubs)
			{
				if (serviceStub.InterfaceType == t)
				{
					object obj;
					if (serviceStub.Constructor != null)
					{
						obj = serviceStub.Constructor();
					}
					else
					{
						Type type = serviceStub.Type;
						if (type == null)
						{
							type = serviceStub.Selector();
						}
						obj = SRServiceManager.DefaultServiceConstructor(t, type);
					}
					if (!SRServiceManager.HasService(t))
					{
						SRServiceManager.RegisterService(t, obj);
					}
					return obj;
				}
			}
			return null;
		}

		protected void OnApplicationQuit()
		{
			SRServiceManager._hasQuit = true;
		}

		private static object DefaultServiceConstructor(Type serviceIntType, Type implType)
		{
			if (typeof(MonoBehaviour).IsAssignableFrom(implType))
			{
				GameObject gameObject = new GameObject("_S_" + serviceIntType.Name);
				return gameObject.AddComponent(implType);
			}
			if (typeof(ScriptableObject).IsAssignableFrom(implType))
			{
				return ScriptableObject.CreateInstance(implType);
			}
			return Activator.CreateInstance(implType);
		}

		private void ScanType(Type type)
		{
			ServiceAttribute attribute = SRReflection.GetAttribute<ServiceAttribute>(type);
			if (attribute != null)
			{
				this._serviceStubs.Add(new SRServiceManager.ServiceStub
				{
					Type = type,
					InterfaceType = attribute.ServiceType
				});
			}
			SRServiceManager.ScanTypeForConstructors(type, this._serviceStubs);
			SRServiceManager.ScanTypeForSelectors(type, this._serviceStubs);
		}

		private static void ScanTypeForSelectors(Type t, List<SRServiceManager.ServiceStub> stubs)
		{
			MethodInfo[] staticMethods = SRServiceManager.GetStaticMethods(t);
			MethodInfo[] array = staticMethods;
			for (int i = 0; i < array.Length; i++)
			{
				MethodInfo methodInfo = array[i];
				ServiceSelectorAttribute attrib = SRReflection.GetAttribute<ServiceSelectorAttribute>(methodInfo);
				if (attrib != null)
				{
					if (methodInfo.ReturnType != typeof(Type))
					{
						UnityEngine.Debug.LogError("ServiceSelector must have return type of Type ({0}.{1}())".Fmt(new object[]
						{
							t.Name,
							methodInfo.Name
						}));
					}
					else if (methodInfo.GetParameters().Length > 0)
					{
						UnityEngine.Debug.LogError("ServiceSelector must have no parameters ({0}.{1}())".Fmt(new object[]
						{
							t.Name,
							methodInfo.Name
						}));
					}
					else
					{
						SRServiceManager.ServiceStub serviceStub = stubs.FirstOrDefault((SRServiceManager.ServiceStub p) => p.InterfaceType == attrib.ServiceType);
						if (serviceStub == null)
						{
							serviceStub = new SRServiceManager.ServiceStub
							{
								InterfaceType = attrib.ServiceType
							};
							stubs.Add(serviceStub);
						}
						serviceStub.Selector = (Func<Type>)Delegate.CreateDelegate(typeof(Func<Type>), methodInfo);
					}
				}
			}
		}

		private static void ScanTypeForConstructors(Type t, List<SRServiceManager.ServiceStub> stubs)
		{
			MethodInfo[] staticMethods = SRServiceManager.GetStaticMethods(t);
			MethodInfo[] array = staticMethods;
			for (int i = 0; i < array.Length; i++)
			{
				MethodInfo methodInfo = array[i];
				ServiceConstructorAttribute attrib = SRReflection.GetAttribute<ServiceConstructorAttribute>(methodInfo);
				if (attrib != null)
				{
					if (methodInfo.ReturnType != attrib.ServiceType)
					{
						UnityEngine.Debug.LogError("ServiceConstructor must have return type of {2} ({0}.{1}())".Fmt(new object[]
						{
							t.Name,
							methodInfo.Name,
							attrib.ServiceType
						}));
					}
					else if (methodInfo.GetParameters().Length > 0)
					{
						UnityEngine.Debug.LogError("ServiceConstructor must have no parameters ({0}.{1}())".Fmt(new object[]
						{
							t.Name,
							methodInfo.Name
						}));
					}
					else
					{
						SRServiceManager.ServiceStub serviceStub = stubs.FirstOrDefault((SRServiceManager.ServiceStub p) => p.InterfaceType == attrib.ServiceType);
						if (serviceStub == null)
						{
							serviceStub = new SRServiceManager.ServiceStub
							{
								InterfaceType = attrib.ServiceType
							};
							stubs.Add(serviceStub);
						}
						MethodInfo m = methodInfo;
						serviceStub.Constructor = (() => m.Invoke(null, null));
					}
				}
			}
		}

		private static MethodInfo[] GetStaticMethods(Type t)
		{
			return t.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
		}

		public const bool EnableLogging = false;

		public static int LoadingCount;

		private readonly SRList<SRServiceManager.Service> _services = new SRList<SRServiceManager.Service>();

		private List<SRServiceManager.ServiceStub> _serviceStubs;

		private static bool _hasQuit;

		[Serializable]
		private class Service
		{
			public object Object;

			public Type Type;
		}

		[Serializable]
		private class ServiceStub
		{
			public override string ToString()
			{
				string text = this.InterfaceType.Name + " (";
				if (this.Type != null)
				{
					text = text + "Type: " + this.Type;
				}
				else if (this.Selector != null)
				{
					text = text + "Selector: " + this.Selector;
				}
				else if (this.Constructor != null)
				{
					text = text + "Constructor: " + this.Constructor;
				}
				return text + ")";
			}

			public Func<object> Constructor;

			public Type InterfaceType;

			public Func<Type> Selector;

			public Type Type;
		}
	}
}
