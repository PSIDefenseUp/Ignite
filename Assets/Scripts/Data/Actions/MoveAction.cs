using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class MoveAction : IAction
{
	private readonly int2 delta;

	public MoveAction(int dx, int dy)
	{
		this.delta = new int2(dx, dy);
	}

	public MoveAction(int2 delta)
	{
		this.delta = delta;
	}

	public int GetCost()
	{
		return 1;
	}

	public bool CanPerform(GameObject actor)
	{
		// no, you can't move one solid object into another
		var actorIsSolid = actor.GetComponent<Solid>();
		var destination = actor.GetComponent<Position>().Value + delta;

		if (!GameState.Instance.Map.Contains(destination))
		{
			return false;
		}

		if (actorIsSolid)
		{
			var solids = Object.FindObjectsOfType<Solid>();
			var canMove = !solids.Any(solid => solid.GetComponent<Position>().Value.Equals(destination));

			return canMove;
		}

		return true;
	}

	public void Perform(GameObject actor)
	{
		var position = actor.GetComponent<Position>();
		var actorIsSolid = actor.GetComponent<Solid>() != null;

		// don't allow solids to move into other solid objects
		if (actorIsSolid)
		{
			var solids = Object.FindObjectsOfType<Solid>();
			foreach (var solid in solids)
			{
				var solidPosition = solid.GetComponent<Position>();
				if (solidPosition.Value.Equals(position.Value + delta))
				{
					return;
				}
			}
		}

		actor.AddComponent<SlipNSlide>();
		var move = actor.GetComponent<SlipNSlide>();
		move.Init(position.Value);
		position.Value += delta;
	}
}