using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beast.MapMaker.Services
{
    public interface IFileService
    {
        string OpenFile(string windowTitle, string filter);
        string SaveFile(string windowTitle, string filter, string extension);
    }
}
