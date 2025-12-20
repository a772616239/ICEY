using System;

public class Incrementor
{
	public static int GetNextId()
	{
		return ++Incrementor._i;
	}

	private static int _i;
}
