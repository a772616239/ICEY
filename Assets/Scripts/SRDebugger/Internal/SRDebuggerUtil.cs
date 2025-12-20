using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using SRF.Helpers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SRDebugger.Internal
{
	public static class SRDebuggerUtil
	{
		public static bool IsMobilePlatform
		{
			get
			{
				if (Application.isMobilePlatform)
				{
					return true;
				}
				switch (Application.platform)
				{
				case RuntimePlatform.MetroPlayerX86:
				case RuntimePlatform.MetroPlayerX64:
				case RuntimePlatform.MetroPlayerARM:
					return true;
				default:
					return false;
				}
			}
		}

		public static bool EnsureEventSystemExists()
		{
			if (!Settings.Instance.EnableEventSystemGeneration)
			{
				return false;
			}
			if (EventSystem.current != null)
			{
				return false;
			}
			EventSystem eventSystem = UnityEngine.Object.FindObjectOfType<EventSystem>();
			if (eventSystem != null && eventSystem.gameObject.activeSelf && eventSystem.enabled)
			{
				return false;
			}
			UnityEngine.Debug.LogWarning("[SRDebugger] No EventSystem found in scene - creating a default one.");
			SRDebuggerUtil.CreateDefaultEventSystem();
			return true;
		}

		public static void CreateDefaultEventSystem()
		{
			GameObject gameObject = new GameObject("EventSystem");
			gameObject.AddComponent<EventSystem>();
			gameObject.AddComponent<StandaloneInputModule>();
		}

		public static ICollection<OptionDefinition> ScanForOptions(object obj)
		{
			List<OptionDefinition> list = new List<OptionDefinition>();
			MemberInfo[] members = obj.GetType().GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.InvokeMethod | BindingFlags.GetProperty | BindingFlags.SetProperty);
			foreach (MemberInfo memberInfo in members)
			{
				CategoryAttribute attribute = SRReflection.GetAttribute<CategoryAttribute>(memberInfo);
				string category = (attribute != null) ? attribute.Category : "Default";
				SROptions.SortAttribute attribute2 = SRReflection.GetAttribute<SROptions.SortAttribute>(memberInfo);
				int sortPriority = (attribute2 != null) ? attribute2.SortPriority : 0;
				SROptions.DisplayNameAttribute attribute3 = SRReflection.GetAttribute<SROptions.DisplayNameAttribute>(memberInfo);
				string name = (attribute3 != null) ? attribute3.Name : memberInfo.Name;
				if (memberInfo is PropertyInfo)
				{
					PropertyInfo propertyInfo = memberInfo as PropertyInfo;
					if (propertyInfo.GetGetMethod() != null)
					{
						if ((propertyInfo.GetGetMethod().Attributes & MethodAttributes.Static) == MethodAttributes.PrivateScope)
						{
							list.Add(new OptionDefinition(name, category, sortPriority, new SRF.Helpers.PropertyReference(obj, propertyInfo)));
						}
					}
				}
				else if (memberInfo is MethodInfo)
				{
					MethodInfo methodInfo = memberInfo as MethodInfo;
					if (!methodInfo.IsStatic)
					{
						if (methodInfo.ReturnType == typeof(void) && methodInfo.GetParameters().Length <= 0)
						{
							list.Add(new OptionDefinition(name, category, sortPriority, new MethodReference(obj, methodInfo)));
						}
					}
				}
			}
			return list;
		}

		public static string GetNumberString(int value, int max, string exceedsMaxString)
		{
			if (value >= max)
			{
				return exceedsMaxString;
			}
			return value.ToString();
		}

		public static void ConfigureCanvas(Canvas canvas)
		{
			if (Settings.Instance.UseDebugCamera)
			{
				canvas.worldCamera = Service.DebugCamera.Camera;
				canvas.renderMode = RenderMode.ScreenSpaceCamera;
			}
		}
	}
}
