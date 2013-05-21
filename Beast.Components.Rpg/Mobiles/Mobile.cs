using Beast.IO;
using Beast.Items;
using Beast.Skills;
using System.Collections.Generic;

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

        /// <summary>
        /// Queues up any output intended for the connection associated with the current mobile.
        /// </summary>
        /// <param name="output"></param>
        public abstract void EnqueueOutput(IOutput output);

        /// <summary>
        /// Dequeues any output queued up for the mobile
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerable<IOutput> DequeueOutput();

        /// <summary>
        /// Raises the specified event for the current mobile, allowing the mobile to react.
        /// </summary>
        /// <param name="eventName">The name of the event to raise.</param>
        /// <param name="args">An object representing the arguments for the event.</param>
        public virtual void Raise(string eventName, object args)
        {
        }
    }
}
