
namespace Beast.Items
{
    public interface IItem : IGameObject
    {
        double Cost { get; set; }
        int RequiredLevel { get; set; }
        int RequiredExperience { get; set; }
        int RequiredSkill { get; set; }
        int RequiredSkillValue { get; set; }
        EquipLocation EquipLocation { get; set; }

        bool Equals(string alias);
    }
}
