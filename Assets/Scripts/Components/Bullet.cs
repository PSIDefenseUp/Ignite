using UnityEngine;

public class Bullet : MonoBehaviour
{
	public int Damage;

	public void Init(Bullet other)
	{
		this.Damage = other.Damage;
	}

	public void Init(int damage)
	{
		this.Damage = damage;
	}
}