using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beast.Skills
{
    public interface ISkillProvider : IInitializable
    {
        T GetSkill<T>(Func<T, bool> predicate) where T : ISkill;
        IEnumerable<T> GetSkills<T>(Func<T, bool> predicate) where T : ISkill;
        void SaveSkill<T>(T skill) where T : ISkill;
    }
}
