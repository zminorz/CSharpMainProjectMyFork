namespace Model.Runtime
{
    public class MainBase : IReadOnlyBase
    {
        public int Health { get; private set; }
        
        public MainBase(int health)
        {
            Health = health;
        }
    }
}