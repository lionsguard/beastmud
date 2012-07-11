using System;
using System.Collections.Generic;
using System.Linq;

namespace Beast.Security
{
	public class User : DataObject
	{
		#region AccessLevel
		public UserAccessLevel AccessLevel
		{	
			get { return (UserAccessLevel)Get<int>(PropertyAccessLevel); }
			set { Set(PropertyAccessLevel, (int)value); }
		}
		public static readonly string PropertyAccessLevel = "AccessLevel";
		#endregion

		#region DateCreated
		public DateTime DateCreated
		{	
			get { return Get<DateTime>(PropertyDateCreated); }
			set { Set(PropertyDateCreated, value); }
		}
		public static readonly string PropertyDateCreated = "DateCreated";
		#endregion

		#region Logins
		public List<Login> Logins
		{
			get { return Get<List<Login>>(PropertyLogins); }
			set { Set(PropertyLogins, value); }
		}
		public static readonly string PropertyLogins = "Logins";
		#endregion	
			
		public User()
		{
			DateCreated = DateTime.UtcNow;
			Logins = new List<Login>();
			AccessLevel = UserAccessLevel.Player;
		}

		public T Login<T>() where T : Login
		{
			return Logins.FirstOrDefault(l => l is T) as T;
		}
	}
}
