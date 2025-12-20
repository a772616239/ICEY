using System;
using System.Collections;
using UnityEngine;

public class CoroutineManager
{
	public Coroutine Start(IEnumerator routine)
	{
		return SingletonMono<CoroutineProxy>.Instance.StartCoroutine(routine);
	}

	public Coroutine Start_Auto(IEnumerator routine)
	{
		return SingletonMono<CoroutineProxy>.Instance.StartCoroutine(routine);
	}

	public Coroutine Start(string methodName, object obj = null)
	{
		return SingletonMono<CoroutineProxy>.Instance.StartCoroutine(methodName, obj);
	}

	public void Stop(string methodName)
	{
		SingletonMono<CoroutineProxy>.Instance.StopCoroutine(methodName);
	}

	public void Stop(IEnumerator routine)
	{
		SingletonMono<CoroutineProxy>.Instance.StopCoroutine(routine);
	}

	public void Stop(Coroutine routine)
	{
		SingletonMono<CoroutineProxy>.Instance.StopCoroutine(routine);
	}

	public void StopAll()
	{
		SingletonMono<CoroutineProxy>.Instance.StopAllCoroutines();
	}
}
