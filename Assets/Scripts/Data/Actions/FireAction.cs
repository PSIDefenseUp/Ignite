using Unity.Mathematics;
using UnityEngine;

public class FireAction : IAction
{
	private int2 bulletPosition;
	private float bulletRotation;
	private int bulletDamage;

	public FireAction(int2 position, float rotation, int bulletDamage)
	{
		this.bulletPosition = position;
		this.bulletRotation = rotation;
		this.bulletDamage = bulletDamage;
	}

	public int GetCost()
	{
		return 1;
	}

	public void Perform(GameObject actor)
	{
		var bulletObject = new GameObject();
		bulletObject.AddComponent<Position>();
		bulletObject.AddComponent<Actor>();
		bulletObject.AddComponent<SpriteRenderer>();
		bulletObject.AddComponent<Bullet>();
		bulletObject.AddComponent<BulletThinker>();

		var bulletPositionComponent = bulletObject.GetComponent<Position>();
		bulletPositionComponent.Init(bulletPosition, bulletRotation);

		var bulletActor = bulletObject.GetComponent<Actor>();
		bulletActor.Init(1);

		var bulletComponent = bulletObject.GetComponent<Bullet>();
		bulletComponent.Init(bulletDamage);

		var bulletSprite = bulletObject.GetComponent<SpriteRenderer>();
		bulletSprite.sprite = actor.GetComponent<SpriteRenderer>().sprite;
	}
}
