using System;
using System.Collections.Generic;
using System.Linq;

namespace Beast
{
	public static class EventManager
	{
		#region Nested Classes
		private class EventEntry
		{
			public WeakReference Owner { get; set; }
			public string Name { get; set; }

			private readonly string _comparison;
			private readonly int _hashCode;

			public EventEntry(IGameObject owner, string name)
			{
				Owner = new WeakReference(owner);
				Name = name;
				_comparison = owner.Id;
				_hashCode = Name.GetHashCode() ^ Owner.GetHashCode();
			}

			public override bool Equals(object obj)
			{
				return obj is EventEntry && (obj as EventEntry)._comparison.Equals(_comparison);
			}

			public override int GetHashCode()
			{
				return _hashCode;
			}
		}
		#endregion

		private static readonly Dictionary<EventEntry, List<WeakReference>> Entries = new Dictionary<EventEntry, List<WeakReference>>();

		public static void Register(string eventName, IGameObject owner, Action<IGameObject, EventArgs> handler)
		{
			var key = new EventEntry(owner, eventName);
			if (!Entries.ContainsKey(key))
				Entries[key] = new List<WeakReference>();

			Entries[key].Add(new WeakReference(handler));
		}

		public static void Unregister(string eventName, IGameObject owner, Action<IGameObject, EventArgs> handler)
		{
			var key = new EventEntry(owner, eventName);
			if (!Entries.ContainsKey(key))
			{
				Log.Warning("A handler for the event '{0}' was not found while attempting to unregister.", eventName);
				return;
			}
			var item = Entries[key].FirstOrDefault(wr => wr.IsAlive && wr.Target != null && (Action<IGameObject, EventArgs>)wr.Target == handler);
			Entries[key].Remove(item);
		}

		public static void Raise(string eventName, IGameObject sender, EventArgs args)
		{
			var key = new EventEntry(sender, eventName);
			if(!Entries.ContainsKey(key))
			{
				Log.Warning("A handler for the event '{0}' was not found while attempting to raise the event.", eventName);
				return;
			}

			var list = Entries[key];
			var removes = new List<WeakReference>();
			foreach (var weakRef in list)
			{
				if (!weakRef.IsAlive || weakRef.Target != null)
				{
					removes.Add(weakRef);
					continue;
				}
				var action = (Action<IGameObject, EventArgs>) weakRef.Target;
				if (action == null)
					continue;
				action(sender, args);
			}

			foreach (var weakReference in removes)
			{
				Entries[key].Remove(weakReference);
			}
		}
	}
}