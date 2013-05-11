using System;
using System.ComponentModel.Composition;

namespace Beast
{
    /// <summary>
    /// Defines an export of a Module.
    /// </summary>
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ExportModuleAttribute : ExportAttribute
    {
        /// <summary>
        /// Gets or sets the name of the module.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Defines a module export and specifies the name of the module.
        /// </summary>
        /// <param name="name">The name of the module.</param>
        public ExportModuleAttribute(string name)
        {
            Name = name;
        }
    }
}
