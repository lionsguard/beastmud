using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.ComponentModel.Composition.ReflectionModel;
using System.Linq;
using System.Reflection;

namespace Beast.Configuration
{
	public class ConfigurationExportProvider : ExportProvider
	{
		private readonly IConfigurationSource _source;

		public ConfigurationExportProvider(IConfigurationSource source)
		{
			_source = source;
		}

		#region Overrides of ExportProvider

		protected override IEnumerable<Export> GetExportsCore(ImportDefinition definition, AtomicComposition atomicComposition)
		{
			var contractName = definition.ContractName;

			if (string.IsNullOrEmpty(contractName))
			{
				// no contract, nothing we can do
				yield break;
			}

			if (definition.Cardinality != ImportCardinality.ZeroOrOne && definition.Cardinality != ImportCardinality.ExactlyOne)
			{
				// we only support single value cardinalities
				yield break;
			}

			if (_source.Contains(contractName))
			{
				// import was found to be an app setting - may need to convert it to an appropriate type for the importer
				Type targetType = null;
				string stringValue = null;

				if (ReflectionModelServices.IsImportingParameter(definition))
				{
					// import appears on a parameter
					var importingParameter = ReflectionModelServices.GetImportingParameter(definition);
					targetType = importingParameter.Value.ParameterType;
					stringValue = _source.GetValue(contractName);
				}
				else
				{
					// import appears on a member (property)
					var getAccessor = ReflectionModelServices
						.GetImportingMember(definition)
						.GetAccessors()
						.Where(x => x is MethodInfo)
						.Select(x => x as MethodInfo)
						.FirstOrDefault(x => (x.Attributes & MethodAttributes.SpecialName) == MethodAttributes.SpecialName && x.Name.StartsWith("get_", StringComparison.Ordinal));

					if (getAccessor == null)
					{
						// cannot determine type of importing property
						yield break;
					}

					targetType = getAccessor.ReturnType;
					stringValue = _source.GetValue(contractName);
				}

				var export = new Export(contractName, () => Convert.ChangeType(stringValue, targetType));
				yield return export;
			}
		}

		#endregion
	}
}
