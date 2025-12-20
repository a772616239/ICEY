using System;
using System.Collections.Generic;

public static class PoolController
{
	public static readonly Dictionary<string, ObjectPool> EffectDict = new Dictionary<string, ObjectPool>();
}
