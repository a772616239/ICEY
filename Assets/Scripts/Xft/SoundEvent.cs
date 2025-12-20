using System;
using UnityEngine;

namespace Xft
{
	public class SoundEvent : XftEvent
	{
		public SoundEvent(XftEventComponent owner) : base(XEventType.Sound, owner)
		{
		}

		protected AudioSource PlaySound(AudioClip clip, float volume, float pitch)
		{
			volume *= GlobalConfig.SoundVolume;
			if (clip != null)
			{
				if (SoundEvent.m_Listener == null)
				{
					SoundEvent.m_Listener = (UnityEngine.Object.FindObjectOfType(typeof(AudioListener)) as AudioListener);
					if (SoundEvent.m_Listener == null)
					{
						Camera camera = Camera.main;
						if (camera == null)
						{
							camera = (UnityEngine.Object.FindObjectOfType(typeof(Camera)) as Camera);
						}
						if (camera != null)
						{
							SoundEvent.m_Listener = camera.gameObject.AddComponent<AudioListener>();
						}
					}
				}
				if (SoundEvent.m_Listener != null)
				{
					AudioSource audioSource = SoundEvent.m_Listener.GetComponent<AudioSource>();
					if (audioSource == null)
					{
						audioSource = SoundEvent.m_Listener.gameObject.AddComponent<AudioSource>();
					}
					audioSource.pitch = pitch;
					audioSource.loop = this.m_owner.IsSoundLoop;
					if (!this.m_owner.IsSoundLoop)
					{
						audioSource.PlayOneShot(clip, volume);
					}
					else
					{
						audioSource.clip = clip;
						audioSource.volume = volume;
						audioSource.Play();
					}
					return audioSource;
				}
			}
			return null;
		}

		public override void Reset()
		{
			base.Reset();
			if (SoundEvent.m_Listener != null && SoundEvent.m_Listener.GetComponent<AudioSource>() != null && this.m_owner.IsSoundLoop)
			{
				SoundEvent.m_Listener.GetComponent<AudioSource>().Stop();
			}
		}

		public override void OnBegin()
		{
			base.OnBegin();
			this.PlaySound(this.m_owner.Clip, this.m_owner.Volume, this.m_owner.Pitch);
		}

		private static AudioListener m_Listener;
	}
}
