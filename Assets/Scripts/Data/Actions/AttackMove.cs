using Unity.Mathematics;
using UnityEngine;

public class AttackMove : IAction
{
	private int2 destination;

	public AttackMove(int2 destination)
	{
		this.destination = destination;
	}

	public bool CanPerform(GameObject actor)
	{
		throw new System.NotImplementedException();
	}

	public int GetCost()
	{
		throw new System.NotImplementedException();
	}

	public void Perform(GameObject actor)
	{
		throw new System.NotImplementedException();
	}
}