using Beast.Skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beast.Mobiles
{
    public abstract class Mobile : GameObject, IMobile
    {
        public BoundProperty<int> Health { get; set; }
        public BoundProperty<int> Mana { get; set; }

        public SkillValueCollection Skills { get; set; }

        private IMobile _target;

        protected Mobile()
        {
            Skills = new SkillValueCollection();
        }

        public IMobile GetTarget()
        {
            return _target;
        }

        public void SetTarget(IMobile mobile)
        {
            _target = mobile;
        }
    }
}
