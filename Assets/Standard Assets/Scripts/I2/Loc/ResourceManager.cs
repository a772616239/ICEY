using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace I2.Loc
{
	public class ResourceManager : MonoBehaviour
	{
		public static ResourceManager pInstance
		{
			get
			{
				bool flag = ResourceManager.mInstance == null;
				if (ResourceManager.mInstance == null)
				{
					ResourceManager.mInstance = (ResourceManager)UnityEngine.Object.FindObjectOfType(typeof(ResourceManager));
				}
				if (ResourceManager.mInstance == null)
				{
					GameObject gameObject = new GameObject("I2ResourceManager", new Type[]
					{
						typeof(ResourceManager)
					});
					gameObject.hideFlags |= HideFlags.HideAndDontSave;
					ResourceManager.mInstance = gameObject.GetComponent<ResourceManager>();
					if (ResourceManager._003C_003Ef__mg_0024cache0 == null)
					{
						ResourceManager._003C_003Ef__mg_0024cache0 = new UnityAction<Scene, LoadSceneMode>(ResourceManager.MyOnLevelWasLoaded);
					}
					SceneManager.sceneLoaded += ResourceManager._003C_003Ef__mg_0024cache0;
				}
				if (flag && Application.isPlaying)
				{
					UnityEngine.Object.DontDestroyOnLoad(ResourceManager.mInstance.gameObject);
				}
				return ResourceManager.mInstance;
			}
		}

		public static void MyOnLevelWasLoaded(Scene scene, LoadSceneMode mode)
		{
			LocalizationManager.UpdateSources();
		}

		public T GetAsset<T>(string Name) where T : UnityEngine.Object
		{
			T t = this.FindAsset(Name) as T;
			if (t != null)
			{
				return t;
			}
			return this.LoadFromResources<T>(Name);
		}

		private UnityEngine.Object FindAsset(string Name)
		{
			if (this.Assets != null)
			{
				int i = 0;
				int num = this.Assets.Length;
				while (i < num)
				{
					if (this.Assets[i] != null && this.Assets[i].name == Name)
					{
						return this.Assets[i];
					}
					i++;
				}
			}
			return null;
		}

		public bool HasAsset(UnityEngine.Object Obj)
		{
			return this.Assets != null && Array.IndexOf<UnityEngine.Object>(this.Assets, Obj) >= 0;
		}

		public T LoadFromResources<T>(string Path) where T : UnityEngine.Object
		{
			if (string.IsNullOrEmpty(Path))
			{
				return (T)((object)null);
			}
			UnityEngine.Object @object;
			if (this.mResourcesCache.TryGetValue(Path, out @object) && @object != null)
			{
				return @object as T;
			}
			T t = (T)((object)null);
			if (Path.EndsWith("]", StringComparison.OrdinalIgnoreCase))
			{
				int num = Path.LastIndexOf("[", StringComparison.OrdinalIgnoreCase);
				int length = Path.Length - num - 2;
				string value = Path.Substring(num + 1, length);
				Path = Path.Substring(0, num);
				T[] array = Resources.LoadAll<T>(Path);
				int i = 0;
				int num2 = array.Length;
				while (i < num2)
				{
					if (array[i].name.Equals(value))
					{
						t = array[i];
						break;
					}
					i++;
				}
			}
			else
			{
				t = Resources.Load<T>(Path);
			}
			if (t == null)
			{
				t = this.LoadFromBundle<T>(Path);
			}
			if (t != null)
			{
				this.mResourcesCache[Path] = t;
			}
			if (!this.mCleaningScheduled)
			{
				base.Invoke("CleanResourceCache", 0.1f);
				this.mCleaningScheduled = true;
			}
			return t;
		}

		public T LoadFromBundle<T>(string path) where T : UnityEngine.Object
		{
			int i = 0;
			int count = this.mBundleManagers.Count;
			while (i < count)
			{
				if (this.mBundleManagers[i] != null)
				{
					T t = this.mBundleManagers[i].LoadFromBundle<T>(path);
					if (t != null)
					{
						return t;
					}
				}
				i++;
			}
			return (T)((object)null);
		}

		public void CleanResourceCache()
		{
			this.mResourcesCache.Clear();
			Resources.UnloadUnusedAssets();
			base.CancelInvoke();
			this.mCleaningScheduled = false;
		}

		private static ResourceManager mInstance;

		public List<IResourceManager_Bundles> mBundleManagers = new List<IResourceManager_Bundles>();

		public UnityEngine.Object[] Assets;

		private Dictionary<string, UnityEngine.Object> mResourcesCache = new Dictionary<string, UnityEngine.Object>();

		private bool mCleaningScheduled;

		[CompilerGenerated]
		private static UnityAction<Scene, LoadSceneMode> _003C_003Ef__mg_0024cache0;
	}
}
