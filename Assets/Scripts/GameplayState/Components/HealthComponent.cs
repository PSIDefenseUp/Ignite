using System;
using Unity.Entities;

public struct HealthComponent : IComponentData
{
	public int CurrentHealth;
	public int MaxHealth;

	public HealthComponent(int maxHealth)
	{
		this.CurrentHealth = maxHealth;
		this.MaxHealth = maxHealth;
	}

	public HealthComponent(HealthComponent other)
	{
		this.CurrentHealth = other.CurrentHealth;
		this.MaxHealth = other.MaxHealth;
	}

	public HealthComponent ModifyHealth(int change)
	{
		return new HealthComponent(this) { CurrentHealth = Math.Min(CurrentHealth + change, MaxHealth) };
	}
}