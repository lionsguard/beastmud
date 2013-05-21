using Beast.IO;
using Beast.Items;
using Beast.Skills;
using System.Collections.Generic;

namespace Beast.Mobiles
{
    public interface IMobile : IGameObject
    {
        BoundProperty<int> Health { get; set; }
        BoundProperty<int> Mana { get; set; }

        Unit Position { get; set; }

        int Level { get; set; }

        SkillValueCollection Skills { get; set; }

        IMobile GetTarget();
        void SetTarget(IMobile mobile);

        IWeapon GetWeapon();
        IArmor GetArmor();

        bool IsDead();

        /// <summary>
        /// Queues up any output intended for the connection associated with the current mobile.
        /// </summary>
        /// <param name="output"></param>
        void EnqueueOutput(IOutput output);

        /// <summary>
        /// Dequeues any output queued up for the mobile
        /// </summary>
        /// <returns></returns>
        IEnumerable<IOutput> DequeueOutput();

        /// <summary>
        /// Raises the specified event for the current mobile, allowing the mobile to react.
        /// </summary>
        /// <param name="eventName">The name of the event to raise.</param>
        /// <param name="args">An object representing the arguments for the event.</param>
        void Raise(string eventName, object args);
    }
}
