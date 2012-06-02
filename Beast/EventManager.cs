using System;
using System.Collections.Generic;

namespace Beast
{
	public static class EventManager
	{
		private static readonly Dictionary<string, List<Action<GameObject, EventArgs>>> Handlers = new Dictionary<string, List<Action<GameObject, EventArgs>>>(StringComparer.InvariantCultureIgnoreCase);

		public static void Register(string eventName, Action<GameObject, EventArgs> handler)
		{
			if (!Handlers.ContainsKey(eventName))
				Handlers[eventName] = new List<Action<GameObject, EventArgs>>();

			Handlers[eventName].Add(handler);
		}

		public static void Unregister(string eventName, Action<GameObject, EventArgs> handler)
		{
			if (!Handlers.ContainsKey(eventName))
			{
				Log.Warning("A handler for the event '{0}' was not found while attempting to unregister.", eventName);
				return;
			}
			Handlers[eventName].Remove(handler);
		}

		public static void Raise(string eventName, GameObject sender, EventArgs args)
		{
			if(!Handlers.ContainsKey(eventName))
			{
				Log.Warning("A handler for the event '{0}' was not found while attempting to raise the event.", eventName);
				return;
			}

			var list = Handlers[eventName];
			foreach (var action in list)
			{
				action(sender, args);
			}
		}
	}
}