namespace Units.Components
{
    internal class HealthModule
    {
        public void ChangeHealth(Entity entity, float delta)
        {
            if (!entity.canBeHurt) return;
            entity.health += delta;
            if (entity.health > entity.healthLimit) entity.health = entity.healthLimit;
            if (entity.health <= 0) entity.died = true;
        }
    }
}