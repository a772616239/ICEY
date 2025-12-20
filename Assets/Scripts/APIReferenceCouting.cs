using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class APIReferenceCouting : MonoBehaviour
{
	private void Awake()
	{
		APIReferenceCouting._referenceCountingDict.Add("Effect", new Dictionary<string, int>());
		APIReferenceCouting._referenceCountingDict.Add("Audio", new Dictionary<string, int>());
	}

	[Conditional("UNITY_EDITOR")]
	public static void OnUse(string flag, string id)
	{
		Dictionary<string, int> dictionary = APIReferenceCouting._referenceCountingDict[flag];
		if (!dictionary.ContainsKey(id))
		{
			dictionary.Add(id, 1);
		}
		else
		{
			Dictionary<string, int> dictionary2;
			(dictionary2 = dictionary)[id] = dictionary2[id] + 1;
		}
	}

	public const string Effect = "Effect";

	public const string Audio = "Audio";

	private static readonly Dictionary<string, Dictionary<string, int>> _referenceCountingDict = new Dictionary<string, Dictionary<string, int>>();
}
