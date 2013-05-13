using Beast.Skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beast.Mobiles
{
    public interface IMobile : IGameObject
    {
        BoundProperty<int> Health { get; set; }
        BoundProperty<int> Mana { get; set; }

        SkillValueCollection Skills { get; set; }

        IMobile GetTarget();
        void SetTarget(IMobile mobile);
    }
}
