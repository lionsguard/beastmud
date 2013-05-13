using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beast.Items
{
    public abstract class Item : GameObject, IItem
    {
        public double Cost { get; set; }
        public int RequiredLevel { get; set; }
        public int RequiredExperience { get; set; }
        public int RequiredSkill { get; set; }
        public int RequiredSkillValue { get; set; }
    }
}
