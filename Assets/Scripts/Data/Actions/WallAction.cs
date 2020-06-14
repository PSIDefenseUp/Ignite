using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class WallAction : IAction
{
	private readonly int2 wallPosition;
	private readonly int wallHealth;

	public WallAction(int2 position, int health)
	{
		this.wallPosition = position;
		this.wallHealth = health;
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

			return !solids.Any(solid => solid.GetComponent<Position>().Value.Equals(wallPosition));
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

		Object.Instantiate(wall);
	}
}
