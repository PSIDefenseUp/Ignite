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
