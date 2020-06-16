using Unity.Mathematics;
using UnityEngine;

public class BulletData
{
	public int2 Position;
	public float Rotation;
	public int Damage;
	public Team Team;
	public Sprite Sprite;

	public BulletData(int2 position, float rotation, int damage, Team team, Sprite sprite)
	{
		this.Position = position;
		this.Rotation = rotation;
		this.Damage = damage;
		this.Team = team;
		this.Sprite = sprite;
	}

	public GameObject Instantiate()
	{
		var bullet = Resources.Load<GameObject>("Prefabs/Bullet");

		var bulletPositionComponent = bullet.GetComponent<Position>();
		bulletPositionComponent.Init(Position, Rotation);

		var bulletActor = bullet.GetComponent<Actor>();
		bulletActor.Init(1);

		var bulletComponent = bullet.GetComponent<Bullet>();
		bulletComponent.Init(Damage, Team);

		var bulletSprite = bullet.GetComponent<SpriteRenderer>();
		bulletSprite.sprite = Sprite;

		Object.Instantiate(bullet);

		return bullet;
	}
}