using System;
using System.IO;
using System.Threading;

public static class SRFileUtil
{
	public static void DeleteDirectory(string path)
	{
		try
		{
			Directory.Delete(path, true);
		}
		catch (IOException)
		{
			Thread.Sleep(0);
			Directory.Delete(path, true);
		}
	}

	public static string GetBytesReadable(long i)
	{
		string str = (i >= 0L) ? string.Empty : "-";
		double num = (double)((i >= 0L) ? i : (-(double)i));
		string str2;
		if (i >= 1152921504606846976L)
		{
			str2 = "EB";
			num = (double)(i >> 50);
		}
		else if (i >= 1125899906842624L)
		{
			str2 = "PB";
			num = (double)(i >> 40);
		}
		else if (i >= 1099511627776L)
		{
			str2 = "TB";
			num = (double)(i >> 30);
		}
		else if (i >= 1073741824L)
		{
			str2 = "GB";
			num = (double)(i >> 20);
		}
		else if (i >= 1048576L)
		{
			str2 = "MB";
			num = (double)(i >> 10);
		}
		else
		{
			if (i < 1024L)
			{
				return i.ToString(str + "0 B");
			}
			str2 = "KB";
			num = (double)i;
		}
		return str + (num / 1024.0).ToString("0.### ") + str2;
	}
}
