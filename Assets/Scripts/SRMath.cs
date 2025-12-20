using System;
using UnityEngine;

public static class SRMath
{
	public static float Ease(float from, float to, float t, SRMath.EaseType type)
	{
		switch (type)
		{
		case SRMath.EaseType.Linear:
			return SRMath.TweenFunctions.Linear(t, from, to, 1f);
		case SRMath.EaseType.QuadEaseOut:
			return SRMath.TweenFunctions.QuadEaseOut(t, from, to, 1f);
		case SRMath.EaseType.QuadEaseIn:
			return SRMath.TweenFunctions.QuadEaseIn(t, from, to, 1f);
		case SRMath.EaseType.QuadEaseInOut:
			return SRMath.TweenFunctions.QuadEaseInOut(t, from, to, 1f);
		case SRMath.EaseType.QuadEaseOutIn:
			return SRMath.TweenFunctions.QuadEaseOutIn(t, from, to, 1f);
		case SRMath.EaseType.ExpoEaseOut:
			return SRMath.TweenFunctions.ExpoEaseOut(t, from, to, 1f);
		case SRMath.EaseType.ExpoEaseIn:
			return SRMath.TweenFunctions.ExpoEaseIn(t, from, to, 1f);
		case SRMath.EaseType.ExpoEaseInOut:
			return SRMath.TweenFunctions.ExpoEaseInOut(t, from, to, 1f);
		case SRMath.EaseType.ExpoEaseOutIn:
			return SRMath.TweenFunctions.ExpoEaseOutIn(t, from, to, 1f);
		case SRMath.EaseType.CubicEaseOut:
			return SRMath.TweenFunctions.CubicEaseOut(t, from, to, 1f);
		case SRMath.EaseType.CubicEaseIn:
			return SRMath.TweenFunctions.CubicEaseIn(t, from, to, 1f);
		case SRMath.EaseType.CubicEaseInOut:
			return SRMath.TweenFunctions.CubicEaseInOut(t, from, to, 1f);
		case SRMath.EaseType.CubicEaseOutIn:
			return SRMath.TweenFunctions.CubicEaseOutIn(t, from, to, 1f);
		case SRMath.EaseType.QuartEaseOut:
			return SRMath.TweenFunctions.QuartEaseOut(t, from, to, 1f);
		case SRMath.EaseType.QuartEaseIn:
			return SRMath.TweenFunctions.QuartEaseIn(t, from, to, 1f);
		case SRMath.EaseType.QuartEaseInOut:
			return SRMath.TweenFunctions.QuartEaseInOut(t, from, to, 1f);
		case SRMath.EaseType.QuartEaseOutIn:
			return SRMath.TweenFunctions.QuartEaseOutIn(t, from, to, 1f);
		case SRMath.EaseType.QuintEaseOut:
			return SRMath.TweenFunctions.QuintEaseOut(t, from, to, 1f);
		case SRMath.EaseType.QuintEaseIn:
			return SRMath.TweenFunctions.QuintEaseIn(t, from, to, 1f);
		case SRMath.EaseType.QuintEaseInOut:
			return SRMath.TweenFunctions.QuintEaseInOut(t, from, to, 1f);
		case SRMath.EaseType.QuintEaseOutIn:
			return SRMath.TweenFunctions.QuintEaseOutIn(t, from, to, 1f);
		case SRMath.EaseType.CircEaseOut:
			return SRMath.TweenFunctions.CircEaseOut(t, from, to, 1f);
		case SRMath.EaseType.CircEaseIn:
			return SRMath.TweenFunctions.CircEaseIn(t, from, to, 1f);
		case SRMath.EaseType.CircEaseInOut:
			return SRMath.TweenFunctions.CircEaseInOut(t, from, to, 1f);
		case SRMath.EaseType.CircEaseOutIn:
			return SRMath.TweenFunctions.CircEaseOutIn(t, from, to, 1f);
		case SRMath.EaseType.SineEaseOut:
			return SRMath.TweenFunctions.SineEaseOut(t, from, to, 1f);
		case SRMath.EaseType.SineEaseIn:
			return SRMath.TweenFunctions.SineEaseIn(t, from, to, 1f);
		case SRMath.EaseType.SineEaseInOut:
			return SRMath.TweenFunctions.SineEaseInOut(t, from, to, 1f);
		case SRMath.EaseType.SineEaseOutIn:
			return SRMath.TweenFunctions.SineEaseOutIn(t, from, to, 1f);
		case SRMath.EaseType.ElasticEaseOut:
			return SRMath.TweenFunctions.ElasticEaseOut(t, from, to, 1f);
		case SRMath.EaseType.ElasticEaseIn:
			return SRMath.TweenFunctions.ElasticEaseIn(t, from, to, 1f);
		case SRMath.EaseType.ElasticEaseInOut:
			return SRMath.TweenFunctions.ElasticEaseInOut(t, from, to, 1f);
		case SRMath.EaseType.ElasticEaseOutIn:
			return SRMath.TweenFunctions.ElasticEaseOutIn(t, from, to, 1f);
		case SRMath.EaseType.BounceEaseOut:
			return SRMath.TweenFunctions.BounceEaseOut(t, from, to, 1f);
		case SRMath.EaseType.BounceEaseIn:
			return SRMath.TweenFunctions.BounceEaseIn(t, from, to, 1f);
		case SRMath.EaseType.BounceEaseInOut:
			return SRMath.TweenFunctions.BounceEaseInOut(t, from, to, 1f);
		case SRMath.EaseType.BounceEaseOutIn:
			return SRMath.TweenFunctions.BounceEaseOutIn(t, from, to, 1f);
		case SRMath.EaseType.BackEaseOut:
			return SRMath.TweenFunctions.BackEaseOut(t, from, to, 1f);
		case SRMath.EaseType.BackEaseIn:
			return SRMath.TweenFunctions.BackEaseIn(t, from, to, 1f);
		case SRMath.EaseType.BackEaseInOut:
			return SRMath.TweenFunctions.BackEaseInOut(t, from, to, 1f);
		case SRMath.EaseType.BackEaseOutIn:
			return SRMath.TweenFunctions.BackEaseOutIn(t, from, to, 1f);
		default:
			throw new ArgumentOutOfRangeException("type");
		}
	}

	public static float SpringLerp(float strength, float deltaTime)
	{
		int num = Mathf.RoundToInt(deltaTime * 1000f);
		float t = 0.001f * strength;
		float num2 = 0f;
		float b = 1f;
		for (int i = 0; i < num; i++)
		{
			num2 = Mathf.Lerp(num2, b, t);
		}
		return num2;
	}

	public static float SpringLerp(float from, float to, float strength, float deltaTime)
	{
		return Mathf.Lerp(from, to, SRMath.SpringLerp(strength, deltaTime));
	}

	public static Vector3 SpringLerp(Vector3 from, Vector3 to, float strength, float deltaTime)
	{
		return Vector3.Lerp(from, to, SRMath.SpringLerp(strength, deltaTime));
	}

	public static Quaternion SpringLerp(Quaternion from, Quaternion to, float strength, float deltaTime)
	{
		return Quaternion.Slerp(from, to, SRMath.SpringLerp(strength, deltaTime));
	}

	public static float SmoothClamp(float value, float min, float max, float scrollMax, SRMath.EaseType easeType = SRMath.EaseType.ExpoEaseOut)
	{
		if (value < min)
		{
			return value;
		}
		float num = Mathf.Clamp01((value - min) / (scrollMax - min));
		UnityEngine.Debug.Log(num);
		return Mathf.Clamp(min + Mathf.Lerp(value - min, max, SRMath.Ease(0f, 1f, num, easeType)), 0f, max);
	}

	public static float LerpUnclamped(float from, float to, float t)
	{
		return (1f - t) * from + t * to;
	}

	public static Vector3 LerpUnclamped(Vector3 from, Vector3 to, float t)
	{
		return new Vector3(SRMath.LerpUnclamped(from.x, to.x, t), SRMath.LerpUnclamped(from.y, to.y, t), SRMath.LerpUnclamped(from.z, to.z, t));
	}

	public static float FacingNormalized(Vector3 dir1, Vector3 dir2)
	{
		dir1.Normalize();
		dir2.Normalize();
		return Mathf.InverseLerp(-1f, 1f, Vector3.Dot(dir1, dir2));
	}

	public static float WrapAngle(float angle)
	{
		if (angle <= -180f)
		{
			angle += 360f;
		}
		else if (angle > 180f)
		{
			angle -= 360f;
		}
		return angle;
	}

	public static float NearestAngle(float to, float angle1, float angle2)
	{
		if (Mathf.Abs(Mathf.DeltaAngle(to, angle1)) > Mathf.Abs(Mathf.DeltaAngle(to, angle2)))
		{
			return angle2;
		}
		return angle1;
	}

	public static int Wrap(int max, int value)
	{
		if (max < 0)
		{
			throw new ArgumentOutOfRangeException("max", "max must be greater than 0");
		}
		while (value < 0)
		{
			value += max;
		}
		while (value >= max)
		{
			value -= max;
		}
		return value;
	}

	public static float Wrap(float max, float value)
	{
		while (value < 0f)
		{
			value += max;
		}
		while (value >= max)
		{
			value -= max;
		}
		return value;
	}

	public static float Average(float v1, float v2)
	{
		return (v1 + v2) * 0.5f;
	}

	public static float Angle(Vector2 direction)
	{
		float num = Vector3.Angle(Vector3.up, direction);
		if (Vector3.Cross(direction, Vector3.up).z > 0f)
		{
			num *= -1f;
		}
		return num;
	}

	private static class TweenFunctions
	{
		public static float Linear(float t, float b, float c, float d)
		{
			return c * t / d + b;
		}

		public static float ExpoEaseOut(float t, float b, float c, float d)
		{
			return (t != d) ? (c * (-Mathf.Pow(2f, -10f * t / d) + 1f) + b) : (b + c);
		}

		public static float ExpoEaseIn(float t, float b, float c, float d)
		{
			return (t != 0f) ? (c * Mathf.Pow(2f, 10f * (t / d - 1f)) + b) : b;
		}

		public static float ExpoEaseInOut(float t, float b, float c, float d)
		{
			if (t == 0f)
			{
				return b;
			}
			if (t == d)
			{
				return b + c;
			}
			if ((t /= d / 2f) < 1f)
			{
				return c / 2f * Mathf.Pow(2f, 10f * (t - 1f)) + b;
			}
			return c / 2f * (-Mathf.Pow(2f, -10f * (t -= 1f)) + 2f) + b;
		}

		public static float ExpoEaseOutIn(float t, float b, float c, float d)
		{
			if (t < d / 2f)
			{
				return SRMath.TweenFunctions.ExpoEaseOut(t * 2f, b, c / 2f, d);
			}
			return SRMath.TweenFunctions.ExpoEaseIn(t * 2f - d, b + c / 2f, c / 2f, d);
		}

		public static float CircEaseOut(float t, float b, float c, float d)
		{
			return c * Mathf.Sqrt(1f - (t = t / d - 1f) * t) + b;
		}

		public static float CircEaseIn(float t, float b, float c, float d)
		{
			return -c * (Mathf.Sqrt(1f - (t /= d) * t) - 1f) + b;
		}

		public static float CircEaseInOut(float t, float b, float c, float d)
		{
			if ((t /= d / 2f) < 1f)
			{
				return -c / 2f * (Mathf.Sqrt(1f - t * t) - 1f) + b;
			}
			return c / 2f * (Mathf.Sqrt(1f - (t -= 2f) * t) + 1f) + b;
		}

		public static float CircEaseOutIn(float t, float b, float c, float d)
		{
			if (t < d / 2f)
			{
				return SRMath.TweenFunctions.CircEaseOut(t * 2f, b, c / 2f, d);
			}
			return SRMath.TweenFunctions.CircEaseIn(t * 2f - d, b + c / 2f, c / 2f, d);
		}

		public static float QuadEaseOut(float t, float b, float c, float d)
		{
			return -c * (t /= d) * (t - 2f) + b;
		}

		public static float QuadEaseIn(float t, float b, float c, float d)
		{
			return c * (t /= d) * t + b;
		}

		public static float QuadEaseInOut(float t, float b, float c, float d)
		{
			if ((t /= d / 2f) < 1f)
			{
				return c / 2f * t * t + b;
			}
			return -c / 2f * ((t -= 1f) * (t - 2f) - 1f) + b;
		}

		public static float QuadEaseOutIn(float t, float b, float c, float d)
		{
			if (t < d / 2f)
			{
				return SRMath.TweenFunctions.QuadEaseOut(t * 2f, b, c / 2f, d);
			}
			return SRMath.TweenFunctions.QuadEaseIn(t * 2f - d, b + c / 2f, c / 2f, d);
		}

		public static float SineEaseOut(float t, float b, float c, float d)
		{
			return c * Mathf.Sin(t / d * 1.57079637f) + b;
		}

		public static float SineEaseIn(float t, float b, float c, float d)
		{
			return -c * Mathf.Cos(t / d * 1.57079637f) + c + b;
		}

		public static float SineEaseInOut(float t, float b, float c, float d)
		{
			if ((t /= d / 2f) < 1f)
			{
				return c / 2f * Mathf.Sin(3.14159274f * t / 2f) + b;
			}
			return -c / 2f * (Mathf.Cos(3.14159274f * (t -= 1f) / 2f) - 2f) + b;
		}

		public static float SineEaseOutIn(float t, float b, float c, float d)
		{
			if (t < d / 2f)
			{
				return SRMath.TweenFunctions.SineEaseOut(t * 2f, b, c / 2f, d);
			}
			return SRMath.TweenFunctions.SineEaseIn(t * 2f - d, b + c / 2f, c / 2f, d);
		}

		public static float CubicEaseOut(float t, float b, float c, float d)
		{
			return c * ((t = t / d - 1f) * t * t + 1f) + b;
		}

		public static float CubicEaseIn(float t, float b, float c, float d)
		{
			return c * (t /= d) * t * t + b;
		}

		public static float CubicEaseInOut(float t, float b, float c, float d)
		{
			if ((t /= d / 2f) < 1f)
			{
				return c / 2f * t * t * t + b;
			}
			return c / 2f * ((t -= 2f) * t * t + 2f) + b;
		}

		public static float CubicEaseOutIn(float t, float b, float c, float d)
		{
			if (t < d / 2f)
			{
				return SRMath.TweenFunctions.CubicEaseOut(t * 2f, b, c / 2f, d);
			}
			return SRMath.TweenFunctions.CubicEaseIn(t * 2f - d, b + c / 2f, c / 2f, d);
		}

		public static float QuartEaseOut(float t, float b, float c, float d)
		{
			return -c * ((t = t / d - 1f) * t * t * t - 1f) + b;
		}

		public static float QuartEaseIn(float t, float b, float c, float d)
		{
			return c * (t /= d) * t * t * t + b;
		}

		public static float QuartEaseInOut(float t, float b, float c, float d)
		{
			if ((t /= d / 2f) < 1f)
			{
				return c / 2f * t * t * t * t + b;
			}
			return -c / 2f * ((t -= 2f) * t * t * t - 2f) + b;
		}

		public static float QuartEaseOutIn(float t, float b, float c, float d)
		{
			if (t < d / 2f)
			{
				return SRMath.TweenFunctions.QuartEaseOut(t * 2f, b, c / 2f, d);
			}
			return SRMath.TweenFunctions.QuartEaseIn(t * 2f - d, b + c / 2f, c / 2f, d);
		}

		public static float QuintEaseOut(float t, float b, float c, float d)
		{
			return c * ((t = t / d - 1f) * t * t * t * t + 1f) + b;
		}

		public static float QuintEaseIn(float t, float b, float c, float d)
		{
			return c * (t /= d) * t * t * t * t + b;
		}

		public static float QuintEaseInOut(float t, float b, float c, float d)
		{
			if ((t /= d / 2f) < 1f)
			{
				return c / 2f * t * t * t * t * t + b;
			}
			return c / 2f * ((t -= 2f) * t * t * t * t + 2f) + b;
		}

		public static float QuintEaseOutIn(float t, float b, float c, float d)
		{
			if (t < d / 2f)
			{
				return SRMath.TweenFunctions.QuintEaseOut(t * 2f, b, c / 2f, d);
			}
			return SRMath.TweenFunctions.QuintEaseIn(t * 2f - d, b + c / 2f, c / 2f, d);
		}

		public static float ElasticEaseOut(float t, float b, float c, float d)
		{
			if ((t /= d) == 1f)
			{
				return b + c;
			}
			float num = d * 0.3f;
			float num2 = num / 4f;
			return c * Mathf.Pow(2f, -10f * t) * Mathf.Sin((t * d - num2) * 6.28318548f / num) + c + b;
		}

		public static float ElasticEaseIn(float t, float b, float c, float d)
		{
			if ((t /= d) == 1f)
			{
				return b + c;
			}
			float num = d * 0.3f;
			float num2 = num / 4f;
			return -(c * Mathf.Pow(2f, 10f * (t -= 1f)) * Mathf.Sin((t * d - num2) * 6.28318548f / num)) + b;
		}

		public static float ElasticEaseInOut(float t, float b, float c, float d)
		{
			if ((t /= d / 2f) == 2f)
			{
				return b + c;
			}
			float num = d * 0.450000018f;
			float num2 = num / 4f;
			if (t < 1f)
			{
				return -0.5f * (c * Mathf.Pow(2f, 10f * (t -= 1f)) * Mathf.Sin((t * d - num2) * 6.28318548f / num)) + b;
			}
			return c * Mathf.Pow(2f, -10f * (t -= 1f)) * Mathf.Sin((t * d - num2) * 6.28318548f / num) * 0.5f + c + b;
		}

		public static float ElasticEaseOutIn(float t, float b, float c, float d)
		{
			if (t < d / 2f)
			{
				return SRMath.TweenFunctions.ElasticEaseOut(t * 2f, b, c / 2f, d);
			}
			return SRMath.TweenFunctions.ElasticEaseIn(t * 2f - d, b + c / 2f, c / 2f, d);
		}

		public static float BounceEaseOut(float t, float b, float c, float d)
		{
			if ((t /= d) < 0.363636374f)
			{
				return c * (7.5625f * t * t) + b;
			}
			if ((double)t < 0.72727272727272729)
			{
				return c * (7.5625f * (t -= 0.545454562f) * t + 0.75f) + b;
			}
			if ((double)t < 0.90909090909090906)
			{
				return c * (7.5625f * (t -= 0.8181818f) * t + 0.9375f) + b;
			}
			return c * (7.5625f * (t -= 0.954545438f) * t + 0.984375f) + b;
		}

		public static float BounceEaseIn(float t, float b, float c, float d)
		{
			return c - SRMath.TweenFunctions.BounceEaseOut(d - t, 0f, c, d) + b;
		}

		public static float BounceEaseInOut(float t, float b, float c, float d)
		{
			if (t < d / 2f)
			{
				return SRMath.TweenFunctions.BounceEaseIn(t * 2f, 0f, c, d) * 0.5f + b;
			}
			return SRMath.TweenFunctions.BounceEaseOut(t * 2f - d, 0f, c, d) * 0.5f + c * 0.5f + b;
		}

		public static float BounceEaseOutIn(float t, float b, float c, float d)
		{
			if (t < d / 2f)
			{
				return SRMath.TweenFunctions.BounceEaseOut(t * 2f, b, c / 2f, d);
			}
			return SRMath.TweenFunctions.BounceEaseIn(t * 2f - d, b + c / 2f, c / 2f, d);
		}

		public static float BackEaseOut(float t, float b, float c, float d)
		{
			return c * ((t = t / d - 1f) * t * (2.70158f * t + 1.70158f) + 1f) + b;
		}

		public static float BackEaseIn(float t, float b, float c, float d)
		{
			return c * (t /= d) * t * (2.70158f * t - 1.70158f) + b;
		}

		public static float BackEaseInOut(float t, float b, float c, float d)
		{
			float num = 1.70158f;
			if ((t /= d / 2f) < 1f)
			{
				return c / 2f * (t * t * (((num *= 1.525f) + 1f) * t - num)) + b;
			}
			return c / 2f * ((t -= 2f) * t * (((num *= 1.525f) + 1f) * t + num) + 2f) + b;
		}

		public static float BackEaseOutIn(float t, float b, float c, float d)
		{
			if (t < d / 2f)
			{
				return SRMath.TweenFunctions.BackEaseOut(t * 2f, b, c / 2f, d);
			}
			return SRMath.TweenFunctions.BackEaseIn(t * 2f - d, b + c / 2f, c / 2f, d);
		}
	}

	public enum EaseType
	{
		Linear,
		QuadEaseOut,
		QuadEaseIn,
		QuadEaseInOut,
		QuadEaseOutIn,
		ExpoEaseOut,
		ExpoEaseIn,
		ExpoEaseInOut,
		ExpoEaseOutIn,
		CubicEaseOut,
		CubicEaseIn,
		CubicEaseInOut,
		CubicEaseOutIn,
		QuartEaseOut,
		QuartEaseIn,
		QuartEaseInOut,
		QuartEaseOutIn,
		QuintEaseOut,
		QuintEaseIn,
		QuintEaseInOut,
		QuintEaseOutIn,
		CircEaseOut,
		CircEaseIn,
		CircEaseInOut,
		CircEaseOutIn,
		SineEaseOut,
		SineEaseIn,
		SineEaseInOut,
		SineEaseOutIn,
		ElasticEaseOut,
		ElasticEaseIn,
		ElasticEaseInOut,
		ElasticEaseOutIn,
		BounceEaseOut,
		BounceEaseIn,
		BounceEaseInOut,
		BounceEaseOutIn,
		BackEaseOut,
		BackEaseIn,
		BackEaseInOut,
		BackEaseOutIn
	}
}
