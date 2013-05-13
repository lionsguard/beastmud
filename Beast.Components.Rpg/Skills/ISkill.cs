using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beast.Skills
{
    public interface ISkill
    {
        int Id { get; set; }
        string Name { get; set; }
        string Description { get; set; }
    }
}
