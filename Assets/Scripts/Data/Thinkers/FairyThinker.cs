using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class FairyThinker : Thinker
{
	private bool hasTurned = false;
	private bool hasMoved = false;
	private bool hasFired = false;
	private readonly float[] turnDirections = new float[] { 0, 90, 180, 270 };
	private readonly int2[] moveDirections = new int2[] { new int2(1, 0), new int2(-1, 0), new int2(0, -1), new int2(0, 1) };

	public override void Think()
	{
		IAction action = null;

		var actor = GetComponent<Actor>();
		var position = GetComponent<Position>();

		// move in an open direction (where there is not a solid)
		// choose between valid options at random
		var solids = Object.FindObjectsOfType<Solid>();
		var randomlyOrderedDirections = moveDirections.Shuffle();

		foreach(var direction in randomlyOrderedDirections)
		{
			var positionContainsSolid = solids.Any(solid => {
				var solidPosition = solid.GetComponent<Position>();

				if (solidPosition.Value.Equals(position.Value + direction))
				{
					return true;
				}

				return false;
			});

			if (!positionContainsSolid)
			{
				action = new MoveAction(direction.x, direction.y);
			}
		}

		actor.SetAction(action);
	}
}