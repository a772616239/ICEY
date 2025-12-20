using System;
using System.Collections.Generic;
using UnityEngine;

namespace Xft
{
	[ExecuteInEditMode]
	public class XftCameraEffectComp : MonoBehaviour
	{
		private void Awake()
		{
			base.enabled = false;
		}

		public void ResetEvent(CameraEffectEvent e)
		{
			for (int i = 0; i < this.m_eventList.Count; i++)
			{
				if (this.m_eventList[i].EffectType == e.EffectType)
				{
					this.m_eventList.RemoveAt(i);
					break;
				}
			}
			if (this.m_eventList.Count == 0)
			{
				base.enabled = false;
			}
		}

		public void AddEvent(CameraEffectEvent e)
		{
			for (int i = 0; i < this.m_eventList.Count; i++)
			{
				if (this.m_eventList[i].EffectType == e.EffectType)
				{
					UnityEngine.Debug.LogWarning("can't add camera effect duplicated:" + e.EffectType);
					return;
				}
			}
			this.m_eventList.Add(e);
			this.m_eventList.Sort();
			base.enabled = true;
		}

		private void OnPreRender()
		{
			for (int i = 0; i < this.m_eventList.Count; i++)
			{
				this.m_eventList[i].OnPreRender();
			}
		}

		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (this.m_eventList.Count == 0)
			{
				return;
			}
			RenderTexture temporary = RenderTexture.GetTemporary(source.width, source.height, 0);
			RenderTexture temporary2 = RenderTexture.GetTemporary(source.width, source.height, 0);
			this.m_eventList[0].OnRenderImage(source, temporary);
			bool flag = true;
			for (int i = 1; i < this.m_eventList.Count; i++)
			{
				if (flag)
				{
					this.m_eventList[i].OnRenderImage(temporary, temporary2);
					temporary.DiscardContents();
				}
				else
				{
					this.m_eventList[i].OnRenderImage(temporary2, temporary);
					temporary2.DiscardContents();
				}
				flag = !flag;
			}
			if (flag)
			{
				Graphics.Blit(temporary, destination);
			}
			else
			{
				Graphics.Blit(temporary2, destination);
			}
			RenderTexture.ReleaseTemporary(temporary);
			RenderTexture.ReleaseTemporary(temporary2);
		}

		protected List<CameraEffectEvent> m_eventList = new List<CameraEffectEvent>();
	}
}
