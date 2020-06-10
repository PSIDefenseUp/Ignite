using Unity.Mathematics;
using UnityEngine;

public class MoveAction : IAction
{
	private readonly int2 delta;

	public MoveAction(int dx, int dy)
	{
		this.delta = new int2(dx, dy);
	}

	public int GetCost()
	{
		return 1;
	}

	public void Perform(GameObject actor)
	{
		var position = actor.GetComponent<Position>();
		position.Value += delta;
	}
}