using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SRF
{
	public class Hierarchy
	{
		[Obsolete("Use static Get() instead")]
		public Transform this[string key]
		{
			get
			{
				return Hierarchy.Get(key);
			}
		}

		public static Transform Get(string key)
		{
			Transform transform;
			if (Hierarchy.Cache.TryGetValue(key, out transform))
			{
				return transform;
			}
			GameObject gameObject = GameObject.Find(key);
			if (gameObject)
			{
				transform = gameObject.transform;
				Hierarchy.Cache.Add(key, transform);
				return transform;
			}
			string[] array = key.Split(Hierarchy.Seperator, StringSplitOptions.RemoveEmptyEntries);
			transform = new GameObject(array.Last<string>()).transform;
			Hierarchy.Cache.Add(key, transform);
			if (array.Length == 1)
			{
				return transform;
			}
			transform.parent = Hierarchy.Get(string.Join("/", array, 0, array.Length - 1));
			return transform;
		}

		private static readonly char[] Seperator = new char[]
		{
			'/'
		};

		private static readonly Dictionary<string, Transform> Cache = new Dictionary<string, Transform>();
	}
}
