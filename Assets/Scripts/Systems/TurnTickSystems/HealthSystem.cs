using UnityEngine;

public class HealthSystem
{
	public void Tick()
	{
		var healths = Object.FindObjectsOfType<Health>();

		foreach (var health in healths)
		{
			if (health.Value <= 0)
			{
				Object.Destroy(health.gameObject);
			}
		}
	}
}
