using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beast.Commands
{
    public interface ICommandMetadata
    {
        string[] Aliases { get; }
    }
}
