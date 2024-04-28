namespace Units.Components
{
    internal class HealthModule
    {
        public void ChangeHealth(Entity entity, float delta)
        {
            entity.health += delta;
            if (entity.health <= 0) entity.died = true;
        }
    }
}