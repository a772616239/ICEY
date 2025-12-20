using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameWorld
{
	public class EventManager
	{
		public static bool RegisterEvent<TEventArgs>(string eventName, EventManager.FBEventHandler<TEventArgs> handler, EventManager.ListenerQueue queue = EventManager.ListenerQueue.Game) where TEventArgs : EventArgs
		{
			Dictionary<string, List<object>> dictionary = (queue != EventManager.ListenerQueue.Game) ? EventManager.ToolsEventDelegates : EventManager.GameEventDelegates;
			if (!dictionary.ContainsKey(eventName))
			{
				dictionary[eventName] = new List<object>();
			}
			dictionary[eventName].Add(handler);
			return true;
		}

		public static bool UnregisterEvent<TEventArgs>(string eventName, EventManager.FBEventHandler<TEventArgs> handler, EventManager.ListenerQueue queue = EventManager.ListenerQueue.Game) where TEventArgs : EventArgs
		{
			Dictionary<string, List<object>> dictionary = (queue != EventManager.ListenerQueue.Game) ? EventManager.ToolsEventDelegates : EventManager.GameEventDelegates;
			if (!dictionary.ContainsKey(eventName))
			{
				return false;
			}
			dictionary[eventName].Remove(handler);
			return true;
		}

		public static void PostEvent<TSender, TEventArgs>(string eventDefine, TSender sender, TEventArgs args) where TSender : class where TEventArgs : EventArgs
		{
			if (EventManager.HandlePostEvent<TEventArgs>(EventManager.ToolsEventDelegates, eventDefine, sender, args))
			{
				EventManager.HandlePostEvent<TEventArgs>(EventManager.GameEventDelegates, eventDefine, sender, args);
			}
		}

		public static void PostEvent<TSender>(string eventDefine, TSender sender) where TSender : class
		{
			EventManager.PostEvent<TSender, EventArgs>(eventDefine, sender, EventArgs.Empty);
		}

		public static void PostEvent(string eventDefine)
		{
			EventManager.PostEvent<object>(eventDefine, null);
		}

		private static bool HandlePostEvent<TEventArgs>(Dictionary<string, List<object>> eventDelegates, string eventDefine, object sender, TEventArgs args) where TEventArgs : EventArgs
		{
			bool flag = true;
			if (eventDelegates.ContainsKey(eventDefine))
			{
				List<object> list = eventDelegates[eventDefine];
				for (int i = 0; i < list.Count; i++)
				{
					object obj = list[i];
					if (obj is EventManager.FBEventHandler<TEventArgs>)
					{
						flag &= (obj as EventManager.FBEventHandler<TEventArgs>)(eventDefine, sender, args);
					}
					else if (obj is EventManager.FBEventHandler<EventArgs>)
					{
						flag &= (obj as EventManager.FBEventHandler<EventArgs>)(eventDefine, sender, args);
					}
					else
					{
						UnityEngine.Debug.LogError(string.Concat(new object[]
						{
							eventDefine,
							"事件Args类型注册错误，应为",
							typeof(TEventArgs),
							"请检查是否写成了object\n技术支持麻烦联系凤翔，谢谢"
						}));
					}
				}
			}
			return flag;
		}

		private static readonly Dictionary<string, List<object>> GameEventDelegates = new Dictionary<string, List<object>>();

		private static readonly Dictionary<string, List<object>> ToolsEventDelegates = new Dictionary<string, List<object>>();

		public delegate bool FBEventHandler<TEventArgs>(string eventDefine, object sender, TEventArgs msg) where TEventArgs : EventArgs;

		public enum ListenerQueue
		{
			Game = 3,
			Tools = 6
		}
	}
}
