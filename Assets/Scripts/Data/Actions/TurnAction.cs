using Unity.Mathematics;
using UnityEngine;

public class TurnAction : IAction
{
	private readonly float rotateBy;

	public TurnAction(float rotateBy)
	{
		this.rotateBy = rotateBy;
	}

	public int GetCost()
	{
		return 0;
	}

	public void Perform(GameObject actor)
	{
		var position = actor.GetComponent<Position>();
		position.Rotation += rotateBy;
	}
}