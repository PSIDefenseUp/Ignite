using UnityEngine;

public interface IAction
{
	void Perform(GameObject actor);
	int GetCost();
}