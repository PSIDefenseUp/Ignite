using Unity.Entities;

public class HealthSystem : ComponentSystem
{
	protected override void OnUpdate()
	{
		Entities.ForEach((Entity entity, ref HealthComponent health) => {
			if (health.CurrentHealth <= 0)
			{
				EntityManager.DestroyEntity(entity);
			}
		});
	}
}