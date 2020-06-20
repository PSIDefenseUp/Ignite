using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class WallAction : IAction
{
	private readonly int2 wallPosition;
	private readonly int wallHealth;
	private readonly Team wallTeam;

	public WallAction(int2 position, int health, Team wallTeam = Team.NEUTRAL)
	{
		this.wallPosition = position;
		this.wallHealth = health;
		this.wallTeam = wallTeam;
	}

	public int GetCost()
	{
		return 1;
	}

	public bool CanPerform(GameObject actor)
	{
		// no, you can't create a wall in a solid object
		var actorIsSolid = actor.GetComponent<Solid>();

		if (!GameState.Instance.Map.Contains(wallPosition))
		{
			return false;
		}

		if (actorIsSolid)
		{
			var solids = Object.FindObjectsOfType<Solid>();

			if (solids.Any(solid => solid.GetComponent<Position>().Value.Equals(wallPosition)))
			{
				return false;
			}
		}

		// you also can't create a wall inside of a bullet that will hit it
		var bulletPositions = Object.FindObjectsOfType<Bullet>()
			.Where(bullet => wallTeam == Team.NEUTRAL || bullet.Team != wallTeam)
			.Select(bullet => bullet.GetComponent<Position>());

		if (bulletPositions.Any(bulletPos => bulletPos.Value.Equals(wallPosition)))
		{
			return false;
		}

		return true;
	}

	public void Perform(GameObject actor)
	{
		var wall = Resources.Load<GameObject>("Prefabs/DestructibleWall");
		var position = wall.GetComponent<Position>();
		position.Init(wallPosition, 0);

		var health = wall.GetComponent<Health>();
		health.Init(wallHealth);

		var bulletCollider = wall.GetComponent<BulletCollider>();
		bulletCollider.Team = wallTeam;

		Object.Instantiate(wall);
	}
}
