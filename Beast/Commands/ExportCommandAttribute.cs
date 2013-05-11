﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beast.Commands
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ExportCommandAttribute : ExportAttribute
    {
        public string[] Aliases { get; set; }

        public ExportCommandAttribute(params string[] aliases)
            : base(typeof(ICommand))
        {
            Aliases = aliases;
        }
    }
}