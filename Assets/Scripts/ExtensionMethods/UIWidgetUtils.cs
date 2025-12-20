using System;
using DG.Tweening;

namespace ExtensionMethods
{
	public static class UIWidgetUtils
	{
		public static Tweener DOFade(this UIWidget widget, float endValue, float duration)
		{
			return DOTween.To(() => widget.alpha, delegate(float a)
			{
				widget.alpha = a;
			}, endValue, duration);
		}
	}
}
