using System;
using System.ComponentModel.Composition;
using Beast.Items;
using Beast.Mobiles;
using Beast.Net;

namespace Beast.Commands
{
	[Export(typeof(ICommand))]
	[ExportMetadata(KeyName, "Create")]
	[ExportMetadata(KeyDescription, "Creates objects in the game.")]
	[ExportMetadata(KeySynopsis, "CREATE <type> <name> <args...>")]
	[ExportMetadata(KeyAliases, new[] { "create" })]
	[ExportMetadata(KeyArguments, new[] { "type" })]
	public class CreateCommand : Command
	{
		public override bool RequiresUser
		{
			get { return true; }
		}

		protected override void ExecuteOverride(IInput input, IConnection connection, ResponseMessage response)
		{
			CreateType createType;
			if (!Enum.TryParse(input.Get<string>("type"), true, out createType))
			{
				response.Invalidate(CommonResources.CreateCommandInvalidTypeArg);
				return;
			}

			var type = typeof(GameObject);
			switch (createType)
			{
				case CreateType.Item:
					type = typeof(Item);
					break;
				case CreateType.Place:
					type = typeof(Place);
					break;
				case CreateType.Mobile:
					type = typeof(Mobile);
					break;
				case CreateType.Character:
					type = typeof(Character);
					break;
			}

			IGameObject obj;
			string errorMessage;
			if (!Game.Current.TryCreateObject(type, input, out errorMessage, out obj))
			{
				response.Invalidate(errorMessage);
				return;
			}

			response.Data = obj;
		}

		public enum CreateType
		{
			Object,
			Place,
			Mobile,
			Character,
			Item,
		}
	}
}
