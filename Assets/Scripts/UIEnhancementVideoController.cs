using System;
using System.Collections;
using UnityEngine;

public class UIEnhancementVideoController : MonoBehaviour
{
	public void OnEnable()
	{
		this._playVideo = PlayVideo.CreateVideo(this._renderer);
	}

	public void OnDisable()
	{
		this._currentItem = null;
		this._playVideo.Dispose();
	}

	private void Update()
	{
		EnhancementItem current = UIEnhancementItem.Current;
		if (current != null && !current.Equals(this._currentItem))
		{
			this._currentItem = current;
			if (Array.IndexOf<string>(this._notExistVideos, current.Name) != -1)
			{
				this._playVideo.Stop();
				return;
			}
			if (current.Name == "attack" && current.Level == 3)
			{
				this.Play("Movies/ps4/enhancement", "attack_3.mp4", current);
			}
			else
			{
				this.Play("Movies/ps4/enhancement", string.Format("{0}.mp4", current.Name), current);
			}
		}
	}

	private void Play(string path, string name, EnhancementItem enhancementItem)
	{
		base.StartCoroutine(this.PlayCoroutine(path, name, enhancementItem));
	}

	private IEnumerator PlayCoroutine(string path, string name, EnhancementItem enhancementItem)
	{
		yield return new WaitForSeconds(0.5f);
		if (enhancementItem.Name != this._currentItem.Name)
		{
			yield break;
		}
		this._playVideo.Play(path, name, true);
		yield break;
	}

	[SerializeField]
	private MeshRenderer _renderer;

	private EnhancementItem _currentItem;

	private PlayVideo _playVideo;

	private readonly string[] _notExistVideos = new string[]
	{
		"maxEnergy",
		"maxHP"
	};
}
