using Unity.Mathematics;
using UnityEngine;

public class BulletData
{
	public int2 Position;
	public float Rotation;
	public int Damage;
	public Team Team;
	public Sprite[] Sprites;

	public BulletData(int2 position, float rotation, int damage, Team team, params Sprite[] sprites)
	{
		this.Position = position;
		this.Rotation = rotation;
		this.Damage = damage;
		this.Team = team;
		this.Sprites = sprites;
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

		var bulletSprite = bullet.GetComponent<SpriteAnimator>();
		bulletSprite.Frames = Sprites;

		Object.Instantiate(bullet);

		return bullet;
	}
}