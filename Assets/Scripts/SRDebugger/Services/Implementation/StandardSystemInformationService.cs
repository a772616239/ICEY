using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using SRF;
using SRF.Service;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SRDebugger.Services.Implementation
{
	[Service(typeof(ISystemInformationService))]
	public class StandardSystemInformationService : ISystemInformationService
	{
		public StandardSystemInformationService()
		{
			this.CreateDefaultSet();
		}

		public IEnumerable<string> GetCategories()
		{
			return this._info.Keys;
		}

		public IList<InfoEntry> GetInfo(string category)
		{
			IList<InfoEntry> result;
			if (!this._info.TryGetValue(category, out result))
			{
				UnityEngine.Debug.LogError("[SystemInformationService] Category not found: {0}".Fmt(new object[]
				{
					category
				}));
				return new InfoEntry[0];
			}
			return result;
		}

		public void Add(InfoEntry info, string category = "Default")
		{
			IList<InfoEntry> list;
			if (!this._info.TryGetValue(category, out list))
			{
				list = new List<InfoEntry>();
				this._info.Add(category, list);
			}
			if (list.Any((InfoEntry p) => p.Title == info.Title))
			{
				throw new ArgumentException("An InfoEntry object with the same title already exists in that category.", "info");
			}
			list.Add(info);
		}

		public Dictionary<string, Dictionary<string, object>> CreateReport(bool includePrivate = false)
		{
			Dictionary<string, Dictionary<string, object>> dictionary = new Dictionary<string, Dictionary<string, object>>();
			foreach (KeyValuePair<string, IList<InfoEntry>> keyValuePair in this._info)
			{
				Dictionary<string, object> dictionary2 = new Dictionary<string, object>();
				foreach (InfoEntry infoEntry in keyValuePair.Value)
				{
					if (!infoEntry.IsPrivate || includePrivate)
					{
						dictionary2.Add(infoEntry.Title, infoEntry.Value);
					}
				}
				dictionary.Add(keyValuePair.Key, dictionary2);
			}
			return dictionary;
		}

		private void CreateDefaultSet()
		{
			this._info.Add("System", new InfoEntry[]
			{
				InfoEntry.Create("Operating System", SystemInfo.operatingSystem, false),
				InfoEntry.Create("Device Name", SystemInfo.deviceName, true),
				InfoEntry.Create("Device Type", SystemInfo.deviceType, false),
				InfoEntry.Create("Device Model", SystemInfo.deviceModel, false),
				InfoEntry.Create("CPU Type", SystemInfo.processorType, false),
				InfoEntry.Create("CPU Count", SystemInfo.processorCount, false),
				InfoEntry.Create("System Memory", SRFileUtil.GetBytesReadable((long)SystemInfo.systemMemorySize * 1024L * 1024L), false)
			});
			this._info.Add("Unity", new InfoEntry[]
			{
				InfoEntry.Create("Version", Application.unityVersion, false),
				InfoEntry.Create("Debug", UnityEngine.Debug.isDebugBuild, false),
				InfoEntry.Create("Unity Pro", Application.HasProLicense(), false),
				InfoEntry.Create("Genuine", "{0} ({1})".Fmt(new object[]
				{
					(!Application.genuine) ? "No" : "Yes",
					(!Application.genuineCheckAvailable) ? "Untrusted" : "Trusted"
				}), false),
				InfoEntry.Create("System Language", Application.systemLanguage, false),
				InfoEntry.Create("Platform", Application.platform, false),
				InfoEntry.Create("IL2CPP", "No", false),
				InfoEntry.Create("SRDebugger Version", "1.6.2", false)
			});
			Dictionary<string, IList<InfoEntry>> info = this._info;
			string key = "Display";
			InfoEntry[] array = new InfoEntry[4];
			array[0] = InfoEntry.Create("Resolution", () => Screen.width + "x" + Screen.height, false);
			array[1] = InfoEntry.Create("DPI", () => Screen.dpi, false);
			array[2] = InfoEntry.Create("Fullscreen", () => Screen.fullScreen, false);
			array[3] = InfoEntry.Create("Orientation", () => Screen.orientation, false);
			info.Add(key, array);
			Dictionary<string, IList<InfoEntry>> info2 = this._info;
			string key2 = "Runtime";
			InfoEntry[] array2 = new InfoEntry[4];
			array2[0] = InfoEntry.Create("Play Time", () => Time.unscaledTime, false);
			array2[1] = InfoEntry.Create("Level Play Time", () => Time.timeSinceLevelLoad, false);
			array2[2] = InfoEntry.Create("Current Level", delegate()
			{
				Scene activeScene = SceneManager.GetActiveScene();
				return "{0} (Index: {1})".Fmt(new object[]
				{
					activeScene.name,
					activeScene.buildIndex
				});
			}, false);
			array2[3] = InfoEntry.Create("Quality Level", () => string.Concat(new object[]
			{
				QualitySettings.names[QualitySettings.GetQualityLevel()],
				" (",
				QualitySettings.GetQualityLevel(),
				")"
			}), false);
			info2.Add(key2, array2);
			TextAsset textAsset = (TextAsset)Resources.Load("UnityCloudBuildManifest.json");
			Dictionary<string, object> dictionary = (!(textAsset != null)) ? null : (Json.Deserialize(textAsset.text) as Dictionary<string, object>);
			if (dictionary != null)
			{
				List<InfoEntry> list = new List<InfoEntry>(dictionary.Count);
				foreach (KeyValuePair<string, object> keyValuePair in dictionary)
				{
					if (keyValuePair.Value != null)
					{
						string value = keyValuePair.Value.ToString();
						list.Add(InfoEntry.Create(StandardSystemInformationService.GetCloudManifestPrettyName(keyValuePair.Key), value, false));
					}
				}
				this._info.Add("Build", list);
			}
			this._info.Add("Features", new InfoEntry[]
			{
				InfoEntry.Create("Location", SystemInfo.supportsLocationService, false),
				InfoEntry.Create("Accelerometer", SystemInfo.supportsAccelerometer, false),
				InfoEntry.Create("Gyroscope", SystemInfo.supportsGyroscope, false),
				InfoEntry.Create("Vibration", SystemInfo.supportsVibration, false)
			});
			this._info.Add("Graphics", new InfoEntry[]
			{
				InfoEntry.Create("Device Name", SystemInfo.graphicsDeviceName, false),
				InfoEntry.Create("Device Vendor", SystemInfo.graphicsDeviceVendor, false),
				InfoEntry.Create("Device Version", SystemInfo.graphicsDeviceVersion, false),
				InfoEntry.Create("Max Tex Size", SystemInfo.maxTextureSize, false),
				InfoEntry.Create("NPOT Support", SystemInfo.npotSupport, false),
				InfoEntry.Create("Render Textures", "{0} ({1})".Fmt(new object[]
				{
					(!SystemInfo.supportsRenderTextures) ? "No" : "Yes",
					SystemInfo.supportedRenderTargetCount
				}), false),
				InfoEntry.Create("3D Textures", SystemInfo.supports3DTextures, false),
				InfoEntry.Create("Compute Shaders", SystemInfo.supportsComputeShaders, false),
				InfoEntry.Create("Image Effects", SystemInfo.supportsImageEffects, false),
				InfoEntry.Create("Cubemaps", SystemInfo.supportsRenderToCubemap, false),
				InfoEntry.Create("Shadows", SystemInfo.supportsShadows, false),
				InfoEntry.Create("Stencil", SystemInfo.supportsStencil, false),
				InfoEntry.Create("Sparse Textures", SystemInfo.supportsSparseTextures, false)
			});
		}

		private static string GetCloudManifestPrettyName(string name)
		{
			if (name != null)
			{
				if (name == "scmCommitId")
				{
					return "Commit";
				}
				if (name == "scmBranch")
				{
					return "Branch";
				}
				if (name == "cloudBuildTargetName")
				{
					return "Build Target";
				}
				if (name == "buildStartTime")
				{
					return "Build Date";
				}
			}
			return name.Substring(0, 1).ToUpper() + name.Substring(1);
		}

		private readonly Dictionary<string, IList<InfoEntry>> _info = new Dictionary<string, IList<InfoEntry>>();

		[CompilerGenerated]
		private static Func<object> _003C_003Ef__mg_0024cache0;

		[CompilerGenerated]
		private static Func<object> _003C_003Ef__mg_0024cache1;

		[CompilerGenerated]
		private static Func<object> _003C_003Ef__mg_0024cache2;

		[CompilerGenerated]
		private static Func<object> _003C_003Ef__mg_0024cache3;

		[CompilerGenerated]
		private static Func<object> _003C_003Ef__mg_0024cache4;
	}
}
