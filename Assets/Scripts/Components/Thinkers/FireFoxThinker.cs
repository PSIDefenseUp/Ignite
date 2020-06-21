using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class FireFoxThinker : Thinker
{
	private static Sprite[] bulletSprites;

	private const int DASH_COOLDOWN = 5;
	private const int DASH_COUNT = 3;
	private int remainingDashCooldown = 0;
	private int remainingDashes = DASH_COUNT;

	public void Start()
	{
		bulletSprites = bulletSprites ?? Resources.LoadAll<Sprite>("Final/FoxFire");
	}

	public void OnDestroy()
	{
		if (GetComponent<Health>().Value <= 0)
		{
			GameState.Instance.StageWon = true;
		}
	}

	public override void Think()
	{
		var actor = GetComponent<Actor>();
		var position = GetComponent<Position>();
		var playerPosition = FindObjectOfType<Player>()?.GetComponent<Position>();
		var health = GetComponent<Health>();

		if (playerPosition != null)
		{
			position.Rotation = position.GetClosestAbsoluteDirection(playerPosition.Value, 8);
		}

		if (remainingDashes == 0 && remainingDashCooldown == 0)
		{
			remainingDashes = DASH_COUNT;
			remainingDashCooldown = DASH_COOLDOWN;

			var home = FindObjectOfType<FirefoxHome>().GetComponent<Position>().Value;
			position.Value = home;

			if (playerPosition != null)
			{
				position.Rotation = position.GetClosestAbsoluteDirection(playerPosition.Value, 8);
			}

			var bulletData1 = new BulletData(position.GetRelativePosition(new int2(0, 1)), position.Rotation, 1, Team.ENEMY, bulletSprites);
			var bulletData2 = new BulletData(position.GetRelativePosition(new int2(1, 1)), position.Rotation, 1, Team.ENEMY, bulletSprites);
			var bulletData3 = new BulletData(position.GetRelativePosition(new int2(-1, 1)), position.Rotation, 1, Team.ENEMY, bulletSprites);

			actor.SetAction(new FireAction(bulletData1, bulletData2, bulletData3));
			return;
		}

		if (remainingDashes > 0 && remainingDashCooldown == 0)
		{
			var step = position.GetRelativePosition(new int2(0, 1)) - position.Value;

			var reflectingWalls = FindObjectsOfType<BulletReflector>()
				.Select(reflector => reflector.GetComponent<Position>());

			var hit = FindObjectsOfType<BulletCollider>()
				.Where(collider => collider.Team != Team.ENEMY)
				.Select(collider => collider.GetComponent<Position>());

			var bullets = FindObjectsOfType<Bullet>()
				.Where(bullet => bullet.Team != Team.ENEMY)
				.Select(bullet => bullet.GetComponent<Position>());

			int2 endPos = position.Value;
			while(GameState.Instance.Map.Contains(endPos + step))
			{
				var hitBullets = bullets.Where(bulletPosition => bulletPosition.Value.Equals(endPos + step));
				foreach (var bullet in hitBullets)
				{
					var bulletComponent = bullet.GetComponent<Bullet>();
					if (bulletComponent != null)
					{
						health.Value -= bulletComponent.Damage;
						Destroy(bulletComponent.gameObject);
					}
				}

				var hitColliders = hit.Where(colliderPosition => colliderPosition.Value.Equals(endPos + step));
				foreach (var collider in hitColliders)
				{
					var colliderHealth = collider.GetComponent<Health>();
					if (colliderHealth != null)
					{
						colliderHealth.Value--;
					}
				}

				if (reflectingWalls.Any(reflectorPosition => reflectorPosition.Value.Equals(endPos + step)))
				{
					break;
				}

				endPos += step;
			}

			var moveFrom = GetComponent<SlipNSlide>();
			moveFrom.Init(position.Value);
			position.Value = endPos;

			// create bullets
			var bulletData1 = new BulletData(position.GetRelativePosition(new int2(0, -1)), position.Rotation + 180, 1, Team.ENEMY, bulletSprites);
			var bulletData2 = new BulletData(position.GetRelativePosition(new int2(1, -1)), position.Rotation + 180, 1, Team.ENEMY, bulletSprites);
			var bulletData3 = new BulletData(position.GetRelativePosition(new int2(-1, -1)), position.Rotation + 180, 1, Team.ENEMY, bulletSprites);

			remainingDashes--;

			actor.SetAction(new FireAction(bulletData1, bulletData2, bulletData3));
		}
		else if (remainingDashCooldown > 0)
		{
			remainingDashCooldown--;
			actor.SetAction(new PassTurnAction());
		}
		else
		{
			actor.SetAction(new PassTurnAction());
		}
	}
}