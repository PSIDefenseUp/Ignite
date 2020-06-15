using UnityEngine;

public class BulletCollisionSystem
{
	public void Tick()
	{
		var bullets = Object.FindObjectsOfType<Bullet>();
		var bulletColliders = Object.FindObjectsOfType<BulletCollider>();

		foreach (var bullet in bullets)
		{
			var bulletPosition = bullet.GetComponent<Position>();

			foreach (var bulletCollider in bulletColliders)
			{
				if (bullet.Team == bulletCollider.Team)
				{
					continue;
				}

				var colliderPosition = bulletCollider.GetComponent<Position>();

				if (bulletPosition.Value.Equals(colliderPosition.Value))
				{
					var colliderHealth = bulletCollider.GetComponent<Health>();
					if (colliderHealth != null)
					{
						colliderHealth.Value -= bullet.Damage;
					}

					Object.Destroy(bullet.gameObject);
				}
			}
		}
	}
}