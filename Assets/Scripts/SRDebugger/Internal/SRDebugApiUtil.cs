using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using SRF;

namespace SRDebugger.Internal
{
	public static class SRDebugApiUtil
	{
		public static string ParseErrorException(WebException ex)
		{
			if (ex.Response == null)
			{
				return ex.Message;
			}
			string result;
			try
			{
				string response = SRDebugApiUtil.ReadResponseStream(ex.Response);
				result = SRDebugApiUtil.ParseErrorResponse(response, "Unexpected Response");
			}
			catch
			{
				result = ex.Message;
			}
			return result;
		}

		public static string ParseErrorResponse(string response, string fallback = "Unexpected Response")
		{
			string result;
			try
			{
				Dictionary<string, object> dictionary = (Dictionary<string, object>)Json.Deserialize(response);
				string text = string.Empty;
				text += dictionary["errorMessage"];
				object obj;
				if (dictionary.TryGetValue("errors", out obj) && obj is IList<object>)
				{
					IList<object> list = obj as IList<object>;
					foreach (object arg in list)
					{
						text += "\n";
						text += arg;
					}
				}
				result = text;
			}
			catch
			{
				if (response.Contains("<html>"))
				{
					result = fallback;
				}
				else
				{
					result = response;
				}
			}
			return result;
		}

		public static bool ReadResponse(HttpWebRequest request, out string result)
		{
			bool result2;
			try
			{
				WebResponse response = request.GetResponse();
				result = SRDebugApiUtil.ReadResponseStream(response);
				result2 = true;
			}
			catch (WebException ex)
			{
				result = SRDebugApiUtil.ParseErrorException(ex);
				result2 = false;
			}
			return result2;
		}

		public static string ReadResponseStream(WebResponse stream)
		{
			string result;
			using (Stream responseStream = stream.GetResponseStream())
			{
				using (StreamReader streamReader = new StreamReader(responseStream))
				{
					result = streamReader.ReadToEnd();
				}
			}
			return result;
		}
	}
}
