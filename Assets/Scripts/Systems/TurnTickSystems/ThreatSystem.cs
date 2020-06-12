using Unity.Mathematics;
using UnityEngine;

public class ThreatSystem
{
	public void Tick()
	{
		var threats = Object.FindObjectsOfType<ThreatIndicator>();
		foreach (var threat in threats)
		{
			Object.Destroy(threat.gameObject);
		}

		// check if the player is in check
		var player = Object.FindObjectOfType<Player>();
		if (player == null)
		{
			return;
		}

		var playerPos = player.GetComponent<Position>();
		var bullets = Object.FindObjectsOfType<Bullet>();
		foreach (var bullet in bullets)
		{
			var bulletPos = bullet.GetComponent<Position>();
			var nextBulletPos = bulletPos.GetAbsoluteOffset(new int2(0, 1)) + bulletPos.Value;

			if (playerPos.Value.Equals(nextBulletPos))
			{
				var threat = Resources.Load<GameObject>("Prefabs/ThreatIndicator");
				threat.name = "threat";
				var threatPosition = threat.GetComponent<Position>();
				threatPosition.Init(bulletPos);

				Object.Instantiate(threat);
			}
		}
	}
}