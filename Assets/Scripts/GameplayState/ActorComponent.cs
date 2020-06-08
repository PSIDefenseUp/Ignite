using System;
using Unity.Entities;

public struct ActorComponent : IComponentData
{
	public int MaxActions { get; set; }
	public int RemainingActions { get; set; }
	public UnitAction Action { get; set; }

	public ActorComponent(int maxActions)
	{
		this.MaxActions = maxActions;
		this.RemainingActions = maxActions;
		this.Action = UnitAction.NONE;
	}

	public ActorComponent(ActorComponent other)
	{
		this.MaxActions = other.MaxActions;
		this.RemainingActions = other.RemainingActions;
		this.Action = other.Action;
	}

	public ActorComponent SetAction(UnitAction action)
	{
		return new ActorComponent(this) { Action = action };
	}

	public ActorComponent ConsumeAction(int cost)
	{
		return new ActorComponent(this) { Action = UnitAction.NONE, RemainingActions = RemainingActions - cost };
	}

	public ActorComponent Refresh()
	{
		return new ActorComponent(this) { RemainingActions = MaxActions };
	}
}