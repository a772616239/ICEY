using System;
using UnityEngine;
using UnityEngine.UI;

namespace SRF.UI
{
	[ExecuteInEditMode]
	[AddComponentMenu("SRF/UI/Responsive (Enable)")]
	[RequireComponent(typeof(RectTransform))]
	public class ResponsiveResize : ResponsiveBase
	{
		protected override void Refresh()
		{
			Rect rect = base.RectTransform.rect;
			for (int i = 0; i < this.Elements.Length; i++)
			{
				ResponsiveResize.Element element = this.Elements[i];
				if (!(element.Target == null))
				{
					float num = float.MinValue;
					float num2 = -1f;
					for (int j = 0; j < element.SizeDefinitions.Length; j++)
					{
						ResponsiveResize.Element.SizeDefinition sizeDefinition = element.SizeDefinitions[j];
						if (sizeDefinition.ThresholdWidth <= rect.width && sizeDefinition.ThresholdWidth > num)
						{
							num = sizeDefinition.ThresholdWidth;
							num2 = sizeDefinition.ElementWidth;
						}
					}
					if (num2 > 0f)
					{
						element.Target.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, num2);
						LayoutElement component = element.Target.GetComponent<LayoutElement>();
						if (component != null)
						{
							component.preferredWidth = num2;
						}
					}
				}
			}
		}

		public ResponsiveResize.Element[] Elements = new ResponsiveResize.Element[0];

		[Serializable]
		public struct Element
		{
			public ResponsiveResize.Element.SizeDefinition[] SizeDefinitions;

			public RectTransform Target;

			[Serializable]
			public struct SizeDefinition
			{
				[Tooltip("Width to apply when over the threshold width")]
				public float ElementWidth;

				[Tooltip("Threshold over which this width will take effect")]
				public float ThresholdWidth;
			}
		}
	}
}
