using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class NeetoriThinker : Thinker
{
	private static Sprite bulletSprite;
	private static GameObject turretPrefab;
	private static GameObject wallPrefab;

	private const int TURNS_BETWEEN_WAVES = 5;
	private bool isRepairing = false;
	private int turnsUntilNextWave = TURNS_BETWEEN_WAVES;

	public void Start()
	{
		bulletSprite = bulletSprite ?? Resources.Load<Sprite>("Dev/Sprites/EnemyBullet");
		turretPrefab = turretPrefab ?? Resources.Load<GameObject>("Prefabs/Enemies/Nitori/NitoriTurret");
		wallPrefab = wallPrefab ?? Resources.Load<GameObject>("Prefabs/Enemies/Nitori/NitoriWall");
	}

	public void OnDestroy()
	{
		GameState.Instance.StageWon = true;
	}

	public override void Think()
	{
		// IF ALL TURRETS ARE DEAD WALK TO REPAIR THEM ONE BY ONE
		var turrets = FindObjectsOfType<NitoriTurret>();
		var turretMounts = FindObjectsOfType<NitoriTurretMount>();
		var nitoriHome = FindObjectOfType<NitoriHome>().GetComponent<Position>();
		var actor = GetComponent<Actor>();
		var position = GetComponent<Position>();

		if (!isRepairing)
		{
			if (turrets.Length == 0)
			{
				isRepairing = true;
				foreach(var mount in turretMounts)
				{
					mount.NeedsRepair = true;
				}

				var walls = FindObjectsOfType<NitoriWall>();
				foreach(var wall in walls)
				{
					Destroy(wall.gameObject);
				}
			}
		}

		if (isRepairing)
		{
			var mountToRepair = turretMounts
				.Where(mount => mount.NeedsRepair)
				.OrderBy(mount => mount.Index)
				.FirstOrDefault();

			if (mountToRepair != null)
			{
				var mountPosition = mountToRepair.GetComponent<Position>();
				position.Rotation = position.GetClosestAbsoluteDirection(mountPosition.Value, 4);

				// repair mount if adjacent, otherwise move closer
				if (position.DistanceTo(mountPosition.Value) == 1)
				{
					// to repair, create a turret on top of the mount
					var turret = Instantiate(turretPrefab);
					var turretPosition = turret.GetComponent<Position>();
					var turretComponent = turret.GetComponent<NitoriTurret>();

					turretComponent.Index = mountToRepair.Index;
					turretPosition.Value = mountPosition.Value;
					mountToRepair.NeedsRepair = false;

					actor.SetAction(new MoveRelativeAction(0, 1));
					return;
				}
				else
				{
					actor.SetAction(new MoveRelativeAction(0, 1));
					return;
				}
			}
			else
			{
				isRepairing = false;

				var wallBases = FindObjectsOfType<NitoriWallBase>()
					.Select(wallBase => wallBase.GetComponent<Position>());

				foreach (var wallBase in wallBases)
				{
					var wall = Instantiate(wallPrefab);
					var wallPosition = wall.GetComponent<Position>();
					wallPosition.Value = wallBase.Value;
				}
			}
		}

		if (nitoriHome.Value.Equals(position.Value))
		{
			// IF IN CENTER, FIRE WAVE TOWARDS PLAYER AT 90 DEGREE TICKS EVERY THIRD TURN
			turnsUntilNextWave--;

			if (turnsUntilNextWave == 0)
			{
				// fire wave of bullets towards player
				turnsUntilNextWave = TURNS_BETWEEN_WAVES;

				var playerPosition = FindObjectOfType<Player>()?.GetComponent<Position>();
				if (playerPosition != null)
				{
					position.Rotation = position.GetClosestAbsoluteDirection(playerPosition.Value, 4);
				}

				BulletData bullet1 = new BulletData(position.GetRelativePosition(new int2(-1, 1)), position.Rotation, 1, Team.ENEMY, bulletSprite);
				BulletData bullet2 = new BulletData(position.GetRelativePosition(new int2(0, 1)), position.Rotation, 1, Team.ENEMY, bulletSprite);
				BulletData bullet3 = new BulletData(position.GetRelativePosition(new int2(1, 1)), position.Rotation, 1, Team.ENEMY, bulletSprite);

				actor.SetAction(new FireAction(bullet1, bullet2, bullet3));
				return;
			}
			else
			{
				// pass turn
				actor.SetAction(new PassTurnAction());
				return;
			}
		}
		else
		{
			// OTHERWISE WALK BACK TO CENTER
			position.Rotation = position.GetClosestAbsoluteDirection(nitoriHome.Value, 4);
			actor.SetAction(new MoveRelativeAction(0, 1));
			return;
		}
	}
}