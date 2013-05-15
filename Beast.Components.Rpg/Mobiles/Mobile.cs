using Beast.Items;
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

        public Unit Position { get; set; }

        public int Level { get; set; }

        public SkillValueCollection Skills { get; set; }

        private IMobile _target;

        protected Mobile()
        {
            Level = 1;
            Skills = new SkillValueCollection();
        }

        public virtual IMobile GetTarget()
        {
            return _target;
        }

        public virtual void SetTarget(IMobile mobile)
        {
            _target = mobile;
        }

        public virtual IWeapon GetWeapon()
        {
            return null;
        }
        public virtual IArmor GetArmor()
        {
            return null;
        }

        public bool IsDead()
        {
            return Health.Value <= Health.Minimum;
        }
    }
}
