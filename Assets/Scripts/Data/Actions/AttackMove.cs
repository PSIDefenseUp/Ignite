using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class DirectAttack : IAction
{
	private int2 destination;
	private Team team;
	private int damage;

	public DirectAttack(int2 destination, Team team, int damage)
	{
		this.destination = destination;
		this.team = team;
		this.damage = damage;
	}

	public bool CanPerform(GameObject actor)
	{
		if (!GameState.Instance.Map.Contains(destination))
		{
			return false;
		}

		var validTargetPositions = Object.FindObjectsOfType<BulletCollider>()
			.Where(collider => collider.Team != team)
			.Select(collider => collider.GetComponent<Position>())
			.Where(position => position != null);

		return validTargetPositions.Any(position => position.Value.Equals(destination));
	}

	public int GetCost()
	{
		return 1;
	}

	public void Perform(GameObject actor)
	{
		var target = Object.FindObjectsOfType<BulletCollider>()
			.Where(collider => collider.Team != team)
			.Select(collider => collider.GetComponent<Position>())
			.Where(position => position != null && position.Value.Equals(destination))
			.Select(position => position.GetComponent<Health>())
			.FirstOrDefault();

		if (target != null)
		{
			// TODO: animation?

			target.Value -= damage;
		}
	}
}