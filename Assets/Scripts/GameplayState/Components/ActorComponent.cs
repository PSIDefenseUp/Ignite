using System;
using Unity.Entities;

public struct ActorComponent : IComponentData
{
	public int MaxActions { get; set; }
	public int RemainingActions { get; private set; }
	public bool ActedThisTurn { get; set; }

	public ActorComponent(int maxActions)
	{
		this.MaxActions = maxActions;
		this.RemainingActions = maxActions;
		this.ActedThisTurn = false;
	}

	public ActorComponent(ActorComponent other)
	{
		this.MaxActions = other.MaxActions;
		this.RemainingActions = other.RemainingActions;
		this.ActedThisTurn = other.ActedThisTurn;
	}

	public ActorComponent ConsumeAction(int cost)
	{
		return new ActorComponent(this) { ActedThisTurn = true, RemainingActions = RemainingActions - cost };
	}

	public ActorComponent Refresh()
	{
		return new ActorComponent(this) { RemainingActions = MaxActions };
	}
}