using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using System.Collections.Generic;

public class BroomThinker : Thinker
{
	private bool AreWithinDistance(int2 a, int2 b, int distance)
	{
		return (Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y)) <= distance;
	}

	public override void Think()
	{
		var actor = GetComponent<Actor>();
		var position = GetComponent<Position>();
		var bullets = FindObjectsOfType<Bullet>()
			.Where(bullet => bullet.Team != Team.ENEMY)
			.Select(bullet => bullet.GetComponent<Position>())
			.Where(bullet => AreWithinDistance(bullet.Value, position.Value, 2));
		var up = new List<Position>();
		var down = new List<Position>();
		var left = new List<Position>();
		var right = new List<Position>();
		var standing = new List<Position>();
		int2 leftPos = position.Value + new int2 (-1, 0);
		int2 rightPos = position.Value + new int2 (1, 0);
		int2 upPos = position.Value + new int2 (0, 1);
		int2 downPos = position.Value + new int2 (0, -1);
		
		
		foreach (var bullet in bullets)
		{
			if(AreWithinDistance(bullet.Value, rightPos, 1))
			{
				right.Add(bullet);
			}
			if(AreWithinDistance(bullet.Value, leftPos, 1))
			{
				left.Add(bullet);
			}
			if(AreWithinDistance(bullet.Value, upPos, 1))
			{
				up.Add(bullet);
			}
			if(AreWithinDistance(bullet.Value, downPos, 1))
			{
				down.Add(bullet);
			}
			if(AreWithinDistance(bullet.Value, position.Value, 1))
			{
				standing.Add(bullet);
			}
		}
		int2 destination = position.Value;
		var curr = standing;
		if(up.Count >= standing.Count && up.Count >= left.Count && up.Count >= right.Count && up.Count >= down.Count)
		{
			destination = upPos;
			curr = up;
		}
		else if(down.Count >= standing.Count && down.Count >= left.Count && down.Count >= right.Count)
		{
			destination = downPos;
			curr = down;
		}
		else if(right.Count >= standing.Count && right.Count >= left.Count)
		{
			destination = rightPos;
			curr = right;
		}
		else if (left.Count >= standing.Count)
		{
			destination = leftPos;
			curr = left;
		}

		var xBullets = curr.Where(bullet => bullet.Value.x == destination.x);
		var yBullets = curr.Where(bullet => bullet.Value.y == destination.y);
		if(yBullets.Count() > xBullets.Count())
		{
			foreach(var bullet in yBullets)
			{
				bullet.Rotation = bullet.GetClosestAbsoluteDirection(destination, 4) + 180;
				var bulletOfTheBullet = bullet.GetComponent<Bullet>();
				bulletOfTheBullet.Team = Team.ENEMY;
			}
		}
		else
		{
			foreach(var bullet in xBullets)
			{
				bullet.Rotation = bullet.GetClosestAbsoluteDirection(destination, 4) + 180;
				var bulletOfTheBullet = bullet.GetComponent<Bullet>();
				bulletOfTheBullet.Team = Team.ENEMY;
			}
		}
		actor.SetAction(new MoveAction(destination-position.Value));
		
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