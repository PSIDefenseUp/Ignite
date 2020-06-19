using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using System.Collections.Generic;

public class BroomThinker : Thinker
{
	private Sprite bulletSprite;
	public void Start()
	{
		bulletSprite = Resources.Load<Sprite>("Dev/Sprites/EnemyBullet");
	}

	private bool AreWithinDistance(int2 a, int2 b, int distance)
	{
		return (Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y)) <= distance;
	}

	public override void Think()
	{
		var actor = GetComponent<Actor>();
		var position = GetComponent<Position>();

		var destinations = new List<int2>
		{
			position.Value
			, position.Value + new int2(1, 0)
			, position.Value + new int2(-1, 0)
			, position.Value + new int2(0, 1)
			, position.Value + new int2(0, -1)
		};

		var bullets = FindObjectsOfType<Bullet>()
			.Where(bullet => bullet.Team != Team.ENEMY)
			.Select(bullet => bullet.GetComponent<Position>())
			.Where(bullet => AreWithinDistance(bullet.Value, position.Value, 2));

		// if there are any bullets within 2 spaces of us, we want to move (or not) and reflect them
		if(bullets.Count() != 0)
		{
			var solids = FindObjectsOfType<Solid>();
			var filteredDestinations = destinations.Where(dest =>
			{
				if (position.Value.Equals(dest))
				{
					return true;
				}

				if (!GameState.Instance.Map.Contains(dest))
				{
					return false;
				}

				return !solids.Any(solid => solid.GetComponent<Position>().Value.Equals(dest));
			}).ToArray();

			var potentialDestinationBullets = new List<Position>[filteredDestinations.Length];
			for (int i = 0; i < potentialDestinationBullets.Length; i++)
			{
				potentialDestinationBullets[i] = new List<Position>();
			}

			foreach (var bullet in bullets)
			{
				for (int i = 0; i < filteredDestinations.Length; i++)
				{
					if (!bullet.Value.Equals(filteredDestinations[i]) && AreWithinDistance(bullet.Value, filteredDestinations[i], 1))
					{
						potentialDestinationBullets[i].Add(bullet);
					}
				}
			}

			// find the potential destination with the most adjacent bullets
			var destination = filteredDestinations[0];
			var destinationBullets = potentialDestinationBullets[0];

			for(int i = 0; i < filteredDestinations.Length; i++)
			{
				if (potentialDestinationBullets[i].Count > destinationBullets.Count)
				{
					destination = filteredDestinations[i];
					destinationBullets = potentialDestinationBullets[i];
				}
			}

			// do we want to reflect bullets on the x or y axis?
			var xBullets = destinationBullets.Where(bullet => bullet.Value.x == destination.x);
			var yBullets = destinationBullets.Where(bullet => bullet.Value.y == destination.y);

			IEnumerable<Position> bulletsToReflect = xBullets;
			if(yBullets.Count() > xBullets.Count())
			{
				bulletsToReflect = yBullets;
			}

			// reflect bullets
			foreach(var bullet in bulletsToReflect)
			{
				bullet.Rotation = bullet.GetClosestAbsoluteDirection(destination, 4) + 180;

				var bulletOfTheBullet = bullet.GetComponent<Bullet>();
				bulletOfTheBullet.Team = Team.ENEMY;
				bulletOfTheBullet.GetComponent<SpriteRenderer>().sprite = bulletSprite;
			}

			actor.SetAction(new MoveAction(destination - position.Value));
		}

		if (!actor.HasAction())
		{
			var player = FindObjectOfType<Player>();

			if(player != null)
			{
				position.Rotation = position.GetClosestAbsoluteDirection(player.GetComponent<Position>().Value, 4);
				actor.SetAction(new MoveRelativeAction(new int2(0, 1)));
			}
			else
			{
				actor.SetAction(new PassTurnAction());
			}
		}
	}
}