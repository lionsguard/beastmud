
namespace Beast.Mobiles
{
    public interface ICharacter : IMobile
    {
        double Currency { get; set; }
        BoundProperty<int> Experience { get; set; }
    }
}
