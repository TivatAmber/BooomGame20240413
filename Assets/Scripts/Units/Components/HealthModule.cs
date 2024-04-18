namespace Units.Components
{
    internal class HealthModule
    {
        public void ChangeHealth(Entity entity, float delta)
        {
            entity.health += delta;
        }
        public void ChangeHealth(Entity entity, int delta)
        {
            entity.health += delta;
        }
    }
}