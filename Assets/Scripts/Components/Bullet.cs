using UnityEngine;

public class Bullet : MonoBehaviour
{
	public int Damage;
	public Team Team;

	public void Init(Bullet other)
	{
		this.Damage = other.Damage;
		this.Team = other.Team;
	}

	public void Init(int damage)
	{
		this.Damage = damage;
		this.Team = Team.NEUTRAL;
	}

	public void Init(int damage, Team team)
	{
		this.Damage = damage;
		this.Team = team;
	}
}