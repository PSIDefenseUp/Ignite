using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class NitoriTurretThinker : Thinker
{
	private static Sprite bulletSprite;

	public void Start()
	{
		bulletSprite = bulletSprite ?? Resources.Load<Sprite>("Dev/Sprites/EnemyBullet");
	}

	public override void Think()
	{
		var playerPosition = FindObjectOfType<Player>()?.GetComponent<Position>();
		var actor = GetComponent<Actor>();
		var position = GetComponent<Position>();

		// if we are one of the two turrets closest to the player, FIRE TOWARDS PLAYER THIS TURN (at 90 degree angles)
		if (playerPosition != null)
		{
			var firingTurrets = FindObjectsOfType<NitoriTurret>()
				.Select(turret => turret.GetComponent<Position>())
				.OrderBy(turret => turret.DistanceTo(playerPosition.Value))
				.Take(2);

			foreach (var turret in firingTurrets)
			{
				if (turret.Value.Equals(position.Value))
				{
					// FIRE
					position.Rotation = position.GetClosestAbsoluteDirection(playerPosition.Value, 4);
					var bulletData = new BulletData(position.GetRelativePosition(new int2(0, 1)), position.Rotation, 1, Team.ENEMY, bulletSprite);
					actor.SetAction(new FireAction(bulletData));
					return;
				}
			}
		}
		else
		{
			actor.SetAction(new PassTurnAction());
		}
	}
}