using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beast
{
    public interface IGameObject : IUpdatable
    {
        string Id { get; set; }
        string Name { get; set; }
        string Description { get; set; }
    }
}
