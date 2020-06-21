using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class FireFoxThinker : Thinker
{
	private const int DASH_COOLDOWN = 3;
	private const int DASH_COUNT = 3;
	private int remainingDashCooldown = 0;
	private int remainingDashes = DASH_COUNT;
	public override void Think()
	{
		var actor = GetComponent<Actor>();
		var position = GetComponent<Position>();
		var playerPosition = FindObjectOfType<Player>()?.GetComponent<Position>();

		if (playerPosition != null)
		{
			position.Rotation = position.GetClosestAbsoluteDirection(playerPosition.Value, 8);
		}

		if (remainingDashes == 0 && remainingDashCooldown == 0)
		{
			remainingDashes = DASH_COUNT;
			remainingDashCooldown = DASH_COOLDOWN;
		}

		if (remainingDashes > 0)
		{
			var step = position.GetRelativePosition(new int2(0, 1)) - position.Value;

			var reflectingWalls = FindObjectsOfType<BulletReflector>()
				.Select(reflector => reflector.GetComponent<Position>());

			var hit = FindObjectsOfType<BulletCollider>()
				.Where(collider => collider.Team != Team.ENEMY)
				.Select(collider => collider.GetComponent<Position>());

			int2 endPos = position.Value;
			while(GameState.Instance.Map.Contains(endPos + step))
			{
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

			// TODO: hurt everything on the way

			var moveFrom = GetComponent<SlipNSlide>();
			moveFrom.Init(position.Value);
			position.Value = endPos;

			remainingDashes--;
		}
		else if (remainingDashCooldown > 0)
		{
			remainingDashCooldown--;
		}

		actor.SetAction(new PassTurnAction());
	}
}