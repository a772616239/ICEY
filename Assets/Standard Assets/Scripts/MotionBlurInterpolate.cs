using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MotionBlurInterpolate
{
	public static IEnumerator NewEase(MotionBlurInterpolate.EaseFunction ease, Vector3 start, Vector3 end, float duration)
	{
		IEnumerator driver = MotionBlurInterpolate.NewTimer(duration);
		return MotionBlurInterpolate.NewEase(ease, start, end, duration, driver);
	}

	public static IEnumerator NewEase(MotionBlurInterpolate.EaseFunction ease, Vector3 start, Vector3 end, int slices)
	{
		IEnumerator driver = MotionBlurInterpolate.NewCounter(0, slices + 1, 1);
		return MotionBlurInterpolate.NewEase(ease, start, end, (float)(slices + 1), driver);
	}

	public static IEnumerator NewEase(MotionBlurInterpolate.EaseFunction ease, Vector3 start, Vector3 end, float total, IEnumerator driver)
	{
		Vector3 distance = end - start;
		while (driver.MoveNext())
		{
			object obj = driver.Current;
			yield return MotionBlurInterpolate.Ease(ease, start, distance, (float)obj, total);
		}
		yield break;
	}

	public static Vector3 Ease(MotionBlurInterpolate.EaseFunction ease, Vector3 start, Vector3 distance, float elapsedTime, float duration)
	{
		start.x = ease(start.x, distance.x, elapsedTime, duration);
		start.y = ease(start.y, distance.y, elapsedTime, duration);
		start.z = ease(start.z, distance.z, elapsedTime, duration);
		return start;
	}

	public static MotionBlurInterpolate.EaseFunction Ease(MotionBlurInterpolate.EaseType type)
	{
		MotionBlurInterpolate.EaseFunction result;
		switch (type)
		{
		case MotionBlurInterpolate.EaseType.Linear:
			if (MotionBlurInterpolate._003C_003Ef__mg_0024cache0 == null)
			{
				MotionBlurInterpolate._003C_003Ef__mg_0024cache0 = new MotionBlurInterpolate.EaseFunction(MotionBlurInterpolate.Linear);
			}
			result = MotionBlurInterpolate._003C_003Ef__mg_0024cache0;
			break;
		case MotionBlurInterpolate.EaseType.EaseInQuad:
			if (MotionBlurInterpolate._003C_003Ef__mg_0024cache1 == null)
			{
				MotionBlurInterpolate._003C_003Ef__mg_0024cache1 = new MotionBlurInterpolate.EaseFunction(MotionBlurInterpolate.EaseInQuad);
			}
			result = MotionBlurInterpolate._003C_003Ef__mg_0024cache1;
			break;
		case MotionBlurInterpolate.EaseType.EaseOutQuad:
			if (MotionBlurInterpolate._003C_003Ef__mg_0024cache2 == null)
			{
				MotionBlurInterpolate._003C_003Ef__mg_0024cache2 = new MotionBlurInterpolate.EaseFunction(MotionBlurInterpolate.EaseOutQuad);
			}
			result = MotionBlurInterpolate._003C_003Ef__mg_0024cache2;
			break;
		case MotionBlurInterpolate.EaseType.EaseInOutQuad:
			if (MotionBlurInterpolate._003C_003Ef__mg_0024cache3 == null)
			{
				MotionBlurInterpolate._003C_003Ef__mg_0024cache3 = new MotionBlurInterpolate.EaseFunction(MotionBlurInterpolate.EaseInOutQuad);
			}
			result = MotionBlurInterpolate._003C_003Ef__mg_0024cache3;
			break;
		case MotionBlurInterpolate.EaseType.EaseInCubic:
			if (MotionBlurInterpolate._003C_003Ef__mg_0024cache4 == null)
			{
				MotionBlurInterpolate._003C_003Ef__mg_0024cache4 = new MotionBlurInterpolate.EaseFunction(MotionBlurInterpolate.EaseInCubic);
			}
			result = MotionBlurInterpolate._003C_003Ef__mg_0024cache4;
			break;
		case MotionBlurInterpolate.EaseType.EaseOutCubic:
			if (MotionBlurInterpolate._003C_003Ef__mg_0024cache5 == null)
			{
				MotionBlurInterpolate._003C_003Ef__mg_0024cache5 = new MotionBlurInterpolate.EaseFunction(MotionBlurInterpolate.EaseOutCubic);
			}
			result = MotionBlurInterpolate._003C_003Ef__mg_0024cache5;
			break;
		case MotionBlurInterpolate.EaseType.EaseInOutCubic:
			if (MotionBlurInterpolate._003C_003Ef__mg_0024cache6 == null)
			{
				MotionBlurInterpolate._003C_003Ef__mg_0024cache6 = new MotionBlurInterpolate.EaseFunction(MotionBlurInterpolate.EaseInOutCubic);
			}
			result = MotionBlurInterpolate._003C_003Ef__mg_0024cache6;
			break;
		case MotionBlurInterpolate.EaseType.EaseInQuart:
			if (MotionBlurInterpolate._003C_003Ef__mg_0024cache7 == null)
			{
				MotionBlurInterpolate._003C_003Ef__mg_0024cache7 = new MotionBlurInterpolate.EaseFunction(MotionBlurInterpolate.EaseInQuart);
			}
			result = MotionBlurInterpolate._003C_003Ef__mg_0024cache7;
			break;
		case MotionBlurInterpolate.EaseType.EaseOutQuart:
			if (MotionBlurInterpolate._003C_003Ef__mg_0024cache8 == null)
			{
				MotionBlurInterpolate._003C_003Ef__mg_0024cache8 = new MotionBlurInterpolate.EaseFunction(MotionBlurInterpolate.EaseOutQuart);
			}
			result = MotionBlurInterpolate._003C_003Ef__mg_0024cache8;
			break;
		case MotionBlurInterpolate.EaseType.EaseInOutQuart:
			if (MotionBlurInterpolate._003C_003Ef__mg_0024cache9 == null)
			{
				MotionBlurInterpolate._003C_003Ef__mg_0024cache9 = new MotionBlurInterpolate.EaseFunction(MotionBlurInterpolate.EaseInOutQuart);
			}
			result = MotionBlurInterpolate._003C_003Ef__mg_0024cache9;
			break;
		case MotionBlurInterpolate.EaseType.EaseInQuint:
			if (MotionBlurInterpolate._003C_003Ef__mg_0024cacheA == null)
			{
				MotionBlurInterpolate._003C_003Ef__mg_0024cacheA = new MotionBlurInterpolate.EaseFunction(MotionBlurInterpolate.EaseInQuint);
			}
			result = MotionBlurInterpolate._003C_003Ef__mg_0024cacheA;
			break;
		case MotionBlurInterpolate.EaseType.EaseOutQuint:
			if (MotionBlurInterpolate._003C_003Ef__mg_0024cacheB == null)
			{
				MotionBlurInterpolate._003C_003Ef__mg_0024cacheB = new MotionBlurInterpolate.EaseFunction(MotionBlurInterpolate.EaseOutQuint);
			}
			result = MotionBlurInterpolate._003C_003Ef__mg_0024cacheB;
			break;
		case MotionBlurInterpolate.EaseType.EaseInOutQuint:
			if (MotionBlurInterpolate._003C_003Ef__mg_0024cacheC == null)
			{
				MotionBlurInterpolate._003C_003Ef__mg_0024cacheC = new MotionBlurInterpolate.EaseFunction(MotionBlurInterpolate.EaseInOutQuint);
			}
			result = MotionBlurInterpolate._003C_003Ef__mg_0024cacheC;
			break;
		case MotionBlurInterpolate.EaseType.EaseInSine:
			if (MotionBlurInterpolate._003C_003Ef__mg_0024cacheD == null)
			{
				MotionBlurInterpolate._003C_003Ef__mg_0024cacheD = new MotionBlurInterpolate.EaseFunction(MotionBlurInterpolate.EaseInSine);
			}
			result = MotionBlurInterpolate._003C_003Ef__mg_0024cacheD;
			break;
		case MotionBlurInterpolate.EaseType.EaseOutSine:
			if (MotionBlurInterpolate._003C_003Ef__mg_0024cacheE == null)
			{
				MotionBlurInterpolate._003C_003Ef__mg_0024cacheE = new MotionBlurInterpolate.EaseFunction(MotionBlurInterpolate.EaseOutSine);
			}
			result = MotionBlurInterpolate._003C_003Ef__mg_0024cacheE;
			break;
		case MotionBlurInterpolate.EaseType.EaseInOutSine:
			if (MotionBlurInterpolate._003C_003Ef__mg_0024cacheF == null)
			{
				MotionBlurInterpolate._003C_003Ef__mg_0024cacheF = new MotionBlurInterpolate.EaseFunction(MotionBlurInterpolate.EaseInOutSine);
			}
			result = MotionBlurInterpolate._003C_003Ef__mg_0024cacheF;
			break;
		case MotionBlurInterpolate.EaseType.EaseInExpo:
			if (MotionBlurInterpolate._003C_003Ef__mg_0024cache10 == null)
			{
				MotionBlurInterpolate._003C_003Ef__mg_0024cache10 = new MotionBlurInterpolate.EaseFunction(MotionBlurInterpolate.EaseInExpo);
			}
			result = MotionBlurInterpolate._003C_003Ef__mg_0024cache10;
			break;
		case MotionBlurInterpolate.EaseType.EaseOutExpo:
			if (MotionBlurInterpolate._003C_003Ef__mg_0024cache11 == null)
			{
				MotionBlurInterpolate._003C_003Ef__mg_0024cache11 = new MotionBlurInterpolate.EaseFunction(MotionBlurInterpolate.EaseOutExpo);
			}
			result = MotionBlurInterpolate._003C_003Ef__mg_0024cache11;
			break;
		case MotionBlurInterpolate.EaseType.EaseInOutExpo:
			if (MotionBlurInterpolate._003C_003Ef__mg_0024cache12 == null)
			{
				MotionBlurInterpolate._003C_003Ef__mg_0024cache12 = new MotionBlurInterpolate.EaseFunction(MotionBlurInterpolate.EaseInOutExpo);
			}
			result = MotionBlurInterpolate._003C_003Ef__mg_0024cache12;
			break;
		case MotionBlurInterpolate.EaseType.EaseInCirc:
			if (MotionBlurInterpolate._003C_003Ef__mg_0024cache13 == null)
			{
				MotionBlurInterpolate._003C_003Ef__mg_0024cache13 = new MotionBlurInterpolate.EaseFunction(MotionBlurInterpolate.EaseInCirc);
			}
			result = MotionBlurInterpolate._003C_003Ef__mg_0024cache13;
			break;
		case MotionBlurInterpolate.EaseType.EaseOutCirc:
			if (MotionBlurInterpolate._003C_003Ef__mg_0024cache14 == null)
			{
				MotionBlurInterpolate._003C_003Ef__mg_0024cache14 = new MotionBlurInterpolate.EaseFunction(MotionBlurInterpolate.EaseOutCirc);
			}
			result = MotionBlurInterpolate._003C_003Ef__mg_0024cache14;
			break;
		case MotionBlurInterpolate.EaseType.EaseInOutCirc:
			if (MotionBlurInterpolate._003C_003Ef__mg_0024cache15 == null)
			{
				MotionBlurInterpolate._003C_003Ef__mg_0024cache15 = new MotionBlurInterpolate.EaseFunction(MotionBlurInterpolate.EaseInOutCirc);
			}
			result = MotionBlurInterpolate._003C_003Ef__mg_0024cache15;
			break;
		default:
			if (MotionBlurInterpolate._003C_003Ef__mg_0024cache16 == null)
			{
				MotionBlurInterpolate._003C_003Ef__mg_0024cache16 = new MotionBlurInterpolate.EaseFunction(MotionBlurInterpolate.Linear);
			}
			result = MotionBlurInterpolate._003C_003Ef__mg_0024cache16;
			break;
		}
		return result;
	}

	public static IEnumerator NewBezier(MotionBlurInterpolate.EaseFunction ease, Transform[] nodes, float duration)
	{
		IEnumerator steps = MotionBlurInterpolate.NewTimer(duration);
		if (MotionBlurInterpolate._003C_003Ef__mg_0024cache17 == null)
		{
			MotionBlurInterpolate._003C_003Ef__mg_0024cache17 = new MotionBlurInterpolate.TransformToPositionFunction(MotionBlurInterpolate.TransformDotPosition);
		}
		return MotionBlurInterpolate.NewBezier(ease, nodes, MotionBlurInterpolate._003C_003Ef__mg_0024cache17, duration, steps);
	}

	public static IEnumerator NewBezier(MotionBlurInterpolate.EaseFunction ease, Transform[] nodes, int slices)
	{
		IEnumerator steps = MotionBlurInterpolate.NewCounter(0, slices + 1, 1);
		if (MotionBlurInterpolate._003C_003Ef__mg_0024cache18 == null)
		{
			MotionBlurInterpolate._003C_003Ef__mg_0024cache18 = new MotionBlurInterpolate.TransformToPositionFunction(MotionBlurInterpolate.TransformDotPosition);
		}
		return MotionBlurInterpolate.NewBezier(ease, nodes, MotionBlurInterpolate._003C_003Ef__mg_0024cache18, (float)(slices + 1), steps);
	}

	public static IEnumerator NewBezier(MotionBlurInterpolate.EaseFunction ease, Vector3[] points, float duration)
	{
		IEnumerator steps = MotionBlurInterpolate.NewTimer(duration);
		if (MotionBlurInterpolate._003C_003Ef__mg_0024cache19 == null)
		{
			MotionBlurInterpolate._003C_003Ef__mg_0024cache19 = new MotionBlurInterpolate.PositionToPositionFunction(MotionBlurInterpolate.Identity);
		}
		return MotionBlurInterpolate.NewBezier(ease, points, MotionBlurInterpolate._003C_003Ef__mg_0024cache19, duration, steps);
	}

	public static IEnumerator NewBezier(MotionBlurInterpolate.EaseFunction ease, Vector3[] points, int slices)
	{
		IEnumerator steps = MotionBlurInterpolate.NewCounter(0, slices + 1, 1);
		if (MotionBlurInterpolate._003C_003Ef__mg_0024cache1A == null)
		{
			MotionBlurInterpolate._003C_003Ef__mg_0024cache1A = new MotionBlurInterpolate.PositionToPositionFunction(MotionBlurInterpolate.Identity);
		}
		return MotionBlurInterpolate.NewBezier(ease, points, MotionBlurInterpolate._003C_003Ef__mg_0024cache1A, (float)(slices + 1), steps);
	}

	public static IEnumerator NewBezier(MotionBlurInterpolate.EaseFunction ease, Transform[] nodes, MotionBlurInterpolate.TransformToPositionFunction toVector3, float maxStep, IEnumerator steps)
	{
		if (nodes.Length >= 2)
		{
			Vector3[] points = new Vector3[nodes.Length];
			while (steps.MoveNext())
			{
				for (int i = 0; i < nodes.Length; i++)
				{
					points[i] = toVector3(nodes[i]);
				}
				yield return MotionBlurInterpolate.Bezier(ease, points, (float)steps.Current, maxStep);
			}
		}
		yield break;
	}

	public static IEnumerator NewBezier(MotionBlurInterpolate.EaseFunction ease, Vector3[] nodes, MotionBlurInterpolate.PositionToPositionFunction toVector3, float maxStep, IEnumerator steps)
	{
		if (nodes.Length >= 2)
		{
			Vector3[] points = new Vector3[nodes.Length];
			while (steps.MoveNext())
			{
				for (int i = 0; i < nodes.Length; i++)
				{
					points[i] = toVector3(nodes[i]);
				}
				yield return MotionBlurInterpolate.Bezier(ease, points, (float)steps.Current, maxStep);
			}
		}
		yield break;
	}

	public static Vector3 Bezier(MotionBlurInterpolate.EaseFunction ease, Vector3[] points, float elapsedTime, float duration)
	{
		for (int i = points.Length - 1; i > 0; i--)
		{
			for (int j = 0; j < i; j++)
			{
				points[j].x = ease(points[j].x, points[j + 1].x - points[j].x, elapsedTime, duration);
				points[j].y = ease(points[j].y, points[j + 1].y - points[j].y, elapsedTime, duration);
				points[j].z = ease(points[j].z, points[j + 1].z - points[j].z, elapsedTime, duration);
			}
		}
		return points[0];
	}

	public static IEnumerator NewCatmullRom(Transform[] nodes, int slices, bool loop)
	{
		if (MotionBlurInterpolate._003C_003Ef__mg_0024cache1B == null)
		{
			MotionBlurInterpolate._003C_003Ef__mg_0024cache1B = new MotionBlurInterpolate.TransformToPositionFunction(MotionBlurInterpolate.TransformDotPosition);
		}
		return MotionBlurInterpolate.NewCatmullRom(nodes, MotionBlurInterpolate._003C_003Ef__mg_0024cache1B, slices, loop);
	}

	public static IEnumerator NewCatmullRom(Vector3[] points, int slices, bool loop)
	{
		if (MotionBlurInterpolate._003C_003Ef__mg_0024cache1C == null)
		{
			MotionBlurInterpolate._003C_003Ef__mg_0024cache1C = new MotionBlurInterpolate.PositionToPositionFunction(MotionBlurInterpolate.Identity);
		}
		return MotionBlurInterpolate.NewCatmullRom(points, MotionBlurInterpolate._003C_003Ef__mg_0024cache1C, slices, loop);
	}

	public static IEnumerator NewCatmullRom(Transform[] nodes, MotionBlurInterpolate.TransformToPositionFunction toVector3, int slices, bool loop)
	{
		if (nodes.Length >= 2)
		{
			yield return toVector3(nodes[0]);
			int last = nodes.Length - 1;
			int current = 0;
			while (loop || current < last)
			{
				if (loop && current > last)
				{
					current = 0;
				}
				int previous = (current != 0) ? (current - 1) : ((!loop) ? current : last);
				int start = current;
				int end = (current != last) ? (current + 1) : ((!loop) ? current : 0);
				int next = (end != last) ? (end + 1) : ((!loop) ? end : 0);
				int stepCount = slices + 1;
				for (int step = 1; step <= stepCount; step++)
				{
					yield return MotionBlurInterpolate.CatmullRom(toVector3(nodes[previous]), toVector3(nodes[start]), toVector3(nodes[end]), toVector3(nodes[next]), (float)step, (float)stepCount);
				}
				current++;
			}
		}
		yield break;
	}

	public static IEnumerator NewCatmullRom(Vector3[] nodes, MotionBlurInterpolate.PositionToPositionFunction toVector3, int slices, bool loop)
	{
		if (nodes.Length >= 2)
		{
			yield return toVector3(nodes[0]);
			int last = nodes.Length - 1;
			int current = 0;
			while (loop || current < last)
			{
				if (loop && current > last)
				{
					current = 0;
				}
				int previous = (current != 0) ? (current - 1) : ((!loop) ? current : last);
				int start = current;
				int end = (current != last) ? (current + 1) : ((!loop) ? current : 0);
				int next = (end != last) ? (end + 1) : ((!loop) ? end : 0);
				int stepCount = slices + 1;
				for (int step = 1; step <= stepCount; step++)
				{
					yield return MotionBlurInterpolate.CatmullRom(toVector3(nodes[previous]), toVector3(nodes[start]), toVector3(nodes[end]), toVector3(nodes[next]), (float)step, (float)stepCount);
				}
				current++;
			}
		}
		yield break;
	}

	public static Vector3 CatmullRom(Vector3 previous, Vector3 start, Vector3 end, Vector3 next, float elapsedTime, float duration)
	{
		float num = elapsedTime / duration;
		float num2 = num * num;
		float num3 = num2 * num;
		return previous * (-0.5f * num3 + num2 - 0.5f * num) + start * (1.5f * num3 + -2.5f * num2 + 1f) + end * (-1.5f * num3 + 2f * num2 + 0.5f * num) + next * (0.5f * num3 - 0.5f * num2);
	}

	public static IEnumerator NewTimer(float duration)
	{
		float elapsedTime = 0f;
		while (elapsedTime < duration)
		{
			yield return elapsedTime;
			elapsedTime += Time.deltaTime;
			if (elapsedTime >= duration)
			{
				yield return elapsedTime;
			}
		}
		yield break;
	}

	public static IEnumerator NewCounter(int start, int end, int step)
	{
		for (int i = start; i <= end; i += step)
		{
			yield return i;
		}
		yield break;
	}

	public static Vector3 Identity(Vector3 v)
	{
		return v;
	}

	public static Vector3 TransformDotPosition(Transform t)
	{
		return t.position;
	}

	public static float Linear(float start, float distance, float elapsedTime, float duration)
	{
		if (elapsedTime > duration)
		{
			elapsedTime = duration;
		}
		return distance * (elapsedTime / duration) + start;
	}

	public static float EaseInQuad(float start, float distance, float elapsedTime, float duration)
	{
		elapsedTime = ((elapsedTime <= duration) ? (elapsedTime / duration) : 1f);
		return distance * elapsedTime * elapsedTime + start;
	}

	public static float EaseOutQuad(float start, float distance, float elapsedTime, float duration)
	{
		elapsedTime = ((elapsedTime <= duration) ? (elapsedTime / duration) : 1f);
		return -distance * elapsedTime * (elapsedTime - 2f) + start;
	}

	public static float EaseInOutQuad(float start, float distance, float elapsedTime, float duration)
	{
		elapsedTime = ((elapsedTime <= duration) ? (elapsedTime / (duration / 2f)) : 2f);
		if (elapsedTime < 1f)
		{
			return distance / 2f * elapsedTime * elapsedTime + start;
		}
		elapsedTime -= 1f;
		return -distance / 2f * (elapsedTime * (elapsedTime - 2f) - 1f) + start;
	}

	public static float EaseInCubic(float start, float distance, float elapsedTime, float duration)
	{
		elapsedTime = ((elapsedTime <= duration) ? (elapsedTime / duration) : 1f);
		return distance * elapsedTime * elapsedTime * elapsedTime + start;
	}

	public static float EaseOutCubic(float start, float distance, float elapsedTime, float duration)
	{
		elapsedTime = ((elapsedTime <= duration) ? (elapsedTime / duration) : 1f);
		elapsedTime -= 1f;
		return distance * (elapsedTime * elapsedTime * elapsedTime + 1f) + start;
	}

	public static float EaseInOutCubic(float start, float distance, float elapsedTime, float duration)
	{
		elapsedTime = ((elapsedTime <= duration) ? (elapsedTime / (duration / 2f)) : 2f);
		if (elapsedTime < 1f)
		{
			return distance / 2f * elapsedTime * elapsedTime * elapsedTime + start;
		}
		elapsedTime -= 2f;
		return distance / 2f * (elapsedTime * elapsedTime * elapsedTime + 2f) + start;
	}

	public static float EaseInQuart(float start, float distance, float elapsedTime, float duration)
	{
		elapsedTime = ((elapsedTime <= duration) ? (elapsedTime / duration) : 1f);
		return distance * elapsedTime * elapsedTime * elapsedTime * elapsedTime + start;
	}

	public static float EaseOutQuart(float start, float distance, float elapsedTime, float duration)
	{
		elapsedTime = ((elapsedTime <= duration) ? (elapsedTime / duration) : 1f);
		elapsedTime -= 1f;
		return -distance * (elapsedTime * elapsedTime * elapsedTime * elapsedTime - 1f) + start;
	}

	public static float EaseInOutQuart(float start, float distance, float elapsedTime, float duration)
	{
		elapsedTime = ((elapsedTime <= duration) ? (elapsedTime / (duration / 2f)) : 2f);
		if (elapsedTime < 1f)
		{
			return distance / 2f * elapsedTime * elapsedTime * elapsedTime * elapsedTime + start;
		}
		elapsedTime -= 2f;
		return -distance / 2f * (elapsedTime * elapsedTime * elapsedTime * elapsedTime - 2f) + start;
	}

	public static float EaseInQuint(float start, float distance, float elapsedTime, float duration)
	{
		elapsedTime = ((elapsedTime <= duration) ? (elapsedTime / duration) : 1f);
		return distance * elapsedTime * elapsedTime * elapsedTime * elapsedTime * elapsedTime + start;
	}

	public static float EaseOutQuint(float start, float distance, float elapsedTime, float duration)
	{
		elapsedTime = ((elapsedTime <= duration) ? (elapsedTime / duration) : 1f);
		elapsedTime -= 1f;
		return distance * (elapsedTime * elapsedTime * elapsedTime * elapsedTime * elapsedTime + 1f) + start;
	}

	public static float EaseInOutQuint(float start, float distance, float elapsedTime, float duration)
	{
		elapsedTime = ((elapsedTime <= duration) ? (elapsedTime / (duration / 2f)) : 2f);
		if (elapsedTime < 1f)
		{
			return distance / 2f * elapsedTime * elapsedTime * elapsedTime * elapsedTime * elapsedTime + start;
		}
		elapsedTime -= 2f;
		return distance / 2f * (elapsedTime * elapsedTime * elapsedTime * elapsedTime * elapsedTime + 2f) + start;
	}

	public static float EaseInSine(float start, float distance, float elapsedTime, float duration)
	{
		if (elapsedTime > duration)
		{
			elapsedTime = duration;
		}
		return -distance * Mathf.Cos(elapsedTime / duration * 1.57079637f) + distance + start;
	}

	public static float EaseOutSine(float start, float distance, float elapsedTime, float duration)
	{
		if (elapsedTime > duration)
		{
			elapsedTime = duration;
		}
		return distance * Mathf.Sin(elapsedTime / duration * 1.57079637f) + start;
	}

	public static float EaseInOutSine(float start, float distance, float elapsedTime, float duration)
	{
		if (elapsedTime > duration)
		{
			elapsedTime = duration;
		}
		return -distance / 2f * (Mathf.Cos(3.14159274f * elapsedTime / duration) - 1f) + start;
	}

	public static float EaseInExpo(float start, float distance, float elapsedTime, float duration)
	{
		if (elapsedTime > duration)
		{
			elapsedTime = duration;
		}
		return distance * Mathf.Pow(2f, 10f * (elapsedTime / duration - 1f)) + start;
	}

	public static float EaseOutExpo(float start, float distance, float elapsedTime, float duration)
	{
		if (elapsedTime > duration)
		{
			elapsedTime = duration;
		}
		return distance * (-Mathf.Pow(2f, -10f * elapsedTime / duration) + 1f) + start;
	}

	public static float EaseInOutExpo(float start, float distance, float elapsedTime, float duration)
	{
		elapsedTime = ((elapsedTime <= duration) ? (elapsedTime / (duration / 2f)) : 2f);
		if (elapsedTime < 1f)
		{
			return distance / 2f * Mathf.Pow(2f, 10f * (elapsedTime - 1f)) + start;
		}
		elapsedTime -= 1f;
		return distance / 2f * (-Mathf.Pow(2f, -10f * elapsedTime) + 2f) + start;
	}

	public static float EaseInCirc(float start, float distance, float elapsedTime, float duration)
	{
		elapsedTime = ((elapsedTime <= duration) ? (elapsedTime / duration) : 1f);
		return -distance * (Mathf.Sqrt(1f - elapsedTime * elapsedTime) - 1f) + start;
	}

	public static float EaseOutCirc(float start, float distance, float elapsedTime, float duration)
	{
		elapsedTime = ((elapsedTime <= duration) ? (elapsedTime / duration) : 1f);
		elapsedTime -= 1f;
		return distance * Mathf.Sqrt(1f - elapsedTime * elapsedTime) + start;
	}

	public static float EaseInOutCirc(float start, float distance, float elapsedTime, float duration)
	{
		elapsedTime = ((elapsedTime <= duration) ? (elapsedTime / (duration / 2f)) : 2f);
		if (elapsedTime < 1f)
		{
			return -distance / 2f * (Mathf.Sqrt(1f - elapsedTime * elapsedTime) - 1f) + start;
		}
		elapsedTime -= 2f;
		return distance / 2f * (Mathf.Sqrt(1f - elapsedTime * elapsedTime) + 1f) + start;
	}

	[CompilerGenerated]
	private static MotionBlurInterpolate.EaseFunction _003C_003Ef__mg_0024cache0;

	[CompilerGenerated]
	private static MotionBlurInterpolate.EaseFunction _003C_003Ef__mg_0024cache1;

	[CompilerGenerated]
	private static MotionBlurInterpolate.EaseFunction _003C_003Ef__mg_0024cache2;

	[CompilerGenerated]
	private static MotionBlurInterpolate.EaseFunction _003C_003Ef__mg_0024cache3;

	[CompilerGenerated]
	private static MotionBlurInterpolate.EaseFunction _003C_003Ef__mg_0024cache4;

	[CompilerGenerated]
	private static MotionBlurInterpolate.EaseFunction _003C_003Ef__mg_0024cache5;

	[CompilerGenerated]
	private static MotionBlurInterpolate.EaseFunction _003C_003Ef__mg_0024cache6;

	[CompilerGenerated]
	private static MotionBlurInterpolate.EaseFunction _003C_003Ef__mg_0024cache7;

	[CompilerGenerated]
	private static MotionBlurInterpolate.EaseFunction _003C_003Ef__mg_0024cache8;

	[CompilerGenerated]
	private static MotionBlurInterpolate.EaseFunction _003C_003Ef__mg_0024cache9;

	[CompilerGenerated]
	private static MotionBlurInterpolate.EaseFunction _003C_003Ef__mg_0024cacheA;

	[CompilerGenerated]
	private static MotionBlurInterpolate.EaseFunction _003C_003Ef__mg_0024cacheB;

	[CompilerGenerated]
	private static MotionBlurInterpolate.EaseFunction _003C_003Ef__mg_0024cacheC;

	[CompilerGenerated]
	private static MotionBlurInterpolate.EaseFunction _003C_003Ef__mg_0024cacheD;

	[CompilerGenerated]
	private static MotionBlurInterpolate.EaseFunction _003C_003Ef__mg_0024cacheE;

	[CompilerGenerated]
	private static MotionBlurInterpolate.EaseFunction _003C_003Ef__mg_0024cacheF;

	[CompilerGenerated]
	private static MotionBlurInterpolate.EaseFunction _003C_003Ef__mg_0024cache10;

	[CompilerGenerated]
	private static MotionBlurInterpolate.EaseFunction _003C_003Ef__mg_0024cache11;

	[CompilerGenerated]
	private static MotionBlurInterpolate.EaseFunction _003C_003Ef__mg_0024cache12;

	[CompilerGenerated]
	private static MotionBlurInterpolate.EaseFunction _003C_003Ef__mg_0024cache13;

	[CompilerGenerated]
	private static MotionBlurInterpolate.EaseFunction _003C_003Ef__mg_0024cache14;

	[CompilerGenerated]
	private static MotionBlurInterpolate.EaseFunction _003C_003Ef__mg_0024cache15;

	[CompilerGenerated]
	private static MotionBlurInterpolate.EaseFunction _003C_003Ef__mg_0024cache16;

	[CompilerGenerated]
	private static MotionBlurInterpolate.TransformToPositionFunction _003C_003Ef__mg_0024cache17;

	[CompilerGenerated]
	private static MotionBlurInterpolate.TransformToPositionFunction _003C_003Ef__mg_0024cache18;

	[CompilerGenerated]
	private static MotionBlurInterpolate.PositionToPositionFunction _003C_003Ef__mg_0024cache19;

	[CompilerGenerated]
	private static MotionBlurInterpolate.PositionToPositionFunction _003C_003Ef__mg_0024cache1A;

	[CompilerGenerated]
	private static MotionBlurInterpolate.TransformToPositionFunction _003C_003Ef__mg_0024cache1B;

	[CompilerGenerated]
	private static MotionBlurInterpolate.PositionToPositionFunction _003C_003Ef__mg_0024cache1C;

	public delegate float EaseFunction(float start, float distance, float elapsedTime, float duration);

	public delegate Vector3 TransformToPositionFunction(Transform t);

	public delegate Vector3 PositionToPositionFunction(Vector3 v3);

	public enum EaseType
	{
		Linear,
		EaseInQuad,
		EaseOutQuad,
		EaseInOutQuad,
		EaseInCubic,
		EaseOutCubic,
		EaseInOutCubic,
		EaseInQuart,
		EaseOutQuart,
		EaseInOutQuart,
		EaseInQuint,
		EaseOutQuint,
		EaseInOutQuint,
		EaseInSine,
		EaseOutSine,
		EaseInOutSine,
		EaseInExpo,
		EaseOutExpo,
		EaseInOutExpo,
		EaseInCirc,
		EaseOutCirc,
		EaseInOutCirc
	}
}
