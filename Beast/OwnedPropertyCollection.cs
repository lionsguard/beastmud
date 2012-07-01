namespace Beast
{
	public abstract class OwnedPropertyCollection<T>
	{
		protected IGameObject Owner { get; private set; }
		protected string Prefix { get; private set; }

		protected OwnedPropertyCollection(IGameObject owner, string prefix)
		{
			Owner = owner;
			Prefix = prefix;
		}

		protected string GetPrefixKey(string key)
		{
			if (key.StartsWith(Prefix))
				key = key.Replace(Prefix, string.Empty);
			return string.Concat(Prefix, key);
		}

		protected string GetKey(string prefixedKey)
		{
			return prefixedKey.Replace(Prefix, string.Empty);
		}

		public T Get(string key)
		{
			object value;
			if (!Owner.TryGetValue(key, out value))
				return default(T);
			return (T) value;
		}

		public void Set(string key, T value)
		{
			Owner[key] = value;
		}
	}
}