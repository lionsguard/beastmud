using System;
using System.Collections.Generic;

namespace Beast.Commands
{
	/// <summary>
	/// Represents a command issued by a character, specifically a player character.
	/// </summary>
	public class Command : Dictionary<string,object>
	{
		public const string ParameterKeyCommand = "cmd";

		/// <summary>
		/// Gets or sets a unique identifier for the command.
		/// </summary>
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the name of the command.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Initializes a new instance of the Command class.
		/// </summary>
		public Command()
			: base(StringComparer.InvariantCultureIgnoreCase)
		{
			Id = Guid.NewGuid().ToString();
		}

		/// <summary>
		/// Initializes a new instance of the Command class, sets the name and optionally sets any command arguments.
		/// </summary>
		/// <param name="name">The name of the command.</param>
		/// <param name="args">An array of arguments for the command.</param>
		public Command(IDictionary<string, object> args)
			: base(args, StringComparer.InvariantCultureIgnoreCase)
		{
			Id = Guid.NewGuid().ToString();
			Name = Get<string>(ParameterKeyCommand);
		}

		/// <summary>
		/// Initializes a new instance of the Command class, sets the name and optionally sets any command arguments.
		/// </summary>
		/// <param name="name">The name of the command.</param>
		/// <param name="args">An array of arguments for the command.</param>
		public Command(string name, params KeyValuePair<string,object>[] args)
			: this()
		{
			Name = name;
			if (args == null || args.Length <= 0) 
				return;
			foreach (var arg in args)
			{
				Add(arg.Key, arg.Value);
			}
		}

		/// <summary>
		/// Gets a value from the command of the specified type for the specified key.
		/// </summary>
		/// <typeparam name="T">The type of the value to return.</typeparam>
		/// <param name="key">The key used to locate the value.</param>
		/// <returns>The value of the specified key cast as T.</returns>
		public T Get<T>(string key)
		{
			object value;
			if (TryGetValue(key, out value))
			{
				return ValueConverter.Convert<T>(value);
			}
			return default(T);
		}

		/// <summary>
		/// Attempts to retrieve the specified value using the specified key.
		/// </summary>
		/// <typeparam name="T">The type of the value to return.</typeparam>
		/// <param name="key">The key used to locate the value.</param>
		/// <param name="value">The variable to hold the returned value.</param>
		/// <returns>True if a value was found for the specified key; otherwise false.</returns>
		public bool TryGetValue<T>(string key, out T value)
		{
			object objValue;
			if (!TryGetValue(key, out objValue))
			{
				value = default(T);
				return false;
			}
			value = ValueConverter.Convert<T>(objValue);
			return true;
		}
	}
}