using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class FireAction : IAction
{
	private readonly int2 bulletPosition;
	private readonly float bulletRotation;
	private readonly int bulletDamage;

	public FireAction(int2 position, float rotation, int bulletDamage)
	{
		this.bulletPosition = position;
		this.bulletRotation = rotation;
		this.bulletDamage = bulletDamage;
	}

	public bool CanPerform(GameObject actor)
	{
		// don't let anyone fire a bullet that starts inside a wall (and will immediately bounce back and kill them)
		var reflectors = Object.FindObjectsOfType<BulletReflector>();

		return !reflectors.Any(reflector => reflector.GetComponent<Position>().Value.Equals(bulletPosition));
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
		bulletObject.AddComponent<RotateWithPosition>();

		var bulletPositionComponent = bulletObject.GetComponent<Position>();
		bulletPositionComponent.Init(bulletPosition, bulletRotation);

		var bulletActor = bulletObject.GetComponent<Actor>();
		bulletActor.Init(1);

		var bulletComponent = bulletObject.GetComponent<Bullet>();
		bulletComponent.Init(bulletDamage);

		var bulletSprite = bulletObject.GetComponent<SpriteRenderer>();
		bulletSprite.sprite = Resources.Load<Sprite>("Dev/Sprites/bullet");
	}
}
