using UnityEngine;

public class Actor : MonoBehaviour
{
	[SerializeField]
	private int maxActions;
	private IAction Action;
	public int RemainingActions;

	public void Init(int maxActions)
	{
		this.maxActions = maxActions;
		this.RemainingActions = maxActions;
	}

	public void Act()
	{
		if (RemainingActions >= Action.GetCost())
		{
			Action.Perform(gameObject);
			RemainingActions -= Action.GetCost();
			Action = null;
		}
	}

	public void SetAction(IAction action)
	{
		this.Action = action;
	}

	public bool HasAction()
	{
		return Action != null;
	}

	public void Refresh()
	{
		this.RemainingActions = maxActions;
	}
}