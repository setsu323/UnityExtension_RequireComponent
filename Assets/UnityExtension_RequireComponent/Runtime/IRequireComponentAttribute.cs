namespace UnityEngine
{
    public interface IRequireComponentAttribute
    {
        string Note { get; }
        bool IsValid(Component[] components);
    }
}
