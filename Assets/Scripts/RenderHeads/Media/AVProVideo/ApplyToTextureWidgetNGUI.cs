using System;
using UnityEngine;

namespace RenderHeads.Media.AVProVideo
{
	[AddComponentMenu("AVPro Video/Display NGUI")]
	public class ApplyToTextureWidgetNGUI : MonoBehaviour
	{
		private void Update()
		{
			if (this._mediaPlayer != null)
			{
				if (this._mediaPlayer.TextureProducer != null)
				{
					Texture texture = this._mediaPlayer.TextureProducer.GetTexture(0);
					if (texture != null)
					{
						if (this._mediaPlayer.TextureProducer.RequiresVerticalFlip())
						{
							this._uiTexture.flip = UIBasicSprite.Flip.Vertically;
						}
						this._uiTexture.mainTexture = texture;
					}
				}
			}
			else
			{
				this._uiTexture.mainTexture = this._defaultTexture;
			}
			if (this._makePixelPerfect)
			{
				this._uiTexture.MakePixelPerfect();
			}
		}

		public void OnDisable()
		{
		}

		private void OnDestroy()
		{
			this._uiTexture.mainTexture = this._defaultTexture;
		}

		public UITexture _uiTexture;

		public MediaPlayer _mediaPlayer;

		public Texture2D _defaultTexture;

		[SerializeField]
		private bool _makePixelPerfect;
	}
}
