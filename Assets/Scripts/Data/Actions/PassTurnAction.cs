using UnityEngine;

public class PassTurnAction : IAction
{
	public int GetCost()
	{
		return 0;
	}

	public void Perform(GameObject actor)
	{
		actor.GetComponent<Actor>().RemainingActions = 0;
	}
}