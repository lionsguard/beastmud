using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beast.Items
{
    public interface IItem : IGameObject
    {
        double Cost { get; set; }
        int RequiredLevel { get; set; }
        int RequiredExperience { get; set; }
        int RequiredSkill { get; set; }
        int RequiredSkillValue { get; set; }
    }
}
