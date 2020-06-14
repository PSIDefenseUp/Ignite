using UnityEngine;

public interface IAction
{
	bool CanPerform(GameObject actor);
	void Perform(GameObject actor);
	int GetCost();
}