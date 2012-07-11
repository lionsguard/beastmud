using System.Collections.Generic;

namespace Beast
{
	/// <summary>
	/// Provides an abstract base class to handle active world data including places, objects and mobiles.
	/// </summary>
	public abstract class WorldBase : DataObject, IWorld
	{
		/// <summary>
		/// Gets a list of the current terrain loaded into the active world.
		/// </summary>
		public TerrainCollection Terrain { get; private set; }

		/// <summary>
		/// Gets a collection of current loaded places.
		/// </summary>
		public PlaceCollection Places { get; private set; }

		protected WorldBase()
		{
			Places = new PlaceCollection();
			Terrain = new TerrainCollection(this);
		}

		/// <summary>
		/// Initializes the game world.
		/// </summary>
		public virtual void Initialize()
		{
		}

		#region Place Methods
		public Place GetPlace(string id)
		{
			if (!Places.ContainsKey(id))
			{
				var place = Game.Current.Repository.GetPlace(id);
				if (place != null)
					Places[place.Id] = place;
			}
			return Places[id];
		}
		public void SavePlace(Place place)
		{
			Game.Current.Repository.SavePlace(place);
			Places[place.Id] = place;
		}
		public IEnumerable<Place> GetPlaceMap(Place center, int radius)
		{
			var max = radius*2;

			var stack = new Stack<Place>();
			stack.Push(center);

			var list = new List<Place>();

			while (stack.Count > 0 && max > 0)
			{
				var place = stack.Pop();
				list.Add(place);

				place.Exits.ForEach(stack.Push);

				max--;
			}

			return list;
		}
		#endregion
	}

	internal class DefaultWorld : WorldBase
	{
	}
}