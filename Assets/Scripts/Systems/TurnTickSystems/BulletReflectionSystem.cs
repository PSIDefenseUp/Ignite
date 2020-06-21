using Unity.Mathematics;
using UnityEngine;
using System.Linq;

public class BulletReflectionSystem
{
	private Sprite[] enemyBulletSprites;
	private Sprite[] playerBulletSprites;
	private Sprite[] neutralBulletSprite;
	public BulletReflectionSystem()
	{
		enemyBulletSprites = Resources.LoadAll<Sprite>("Final/Enemy_Bullet");
		playerBulletSprites = Resources.LoadAll<Sprite>("Final/Marisa_Projectile");
		neutralBulletSprite = Resources.LoadAll<Sprite>("Dev/Sprites/NeutralBullet");
	}

	public void Tick()
	{
		var bullets = Object.FindObjectsOfType<Bullet>();
		var bulletReflectors = Object.FindObjectsOfType<BulletReflector>();

		foreach (var bullet in bullets)
		{
			var bulletPosition = bullet.GetComponent<Position>();

			foreach (var reflector in bulletReflectors)
			{
				var reflectorPosition = reflector.GetComponent<Position>();
				var reflectBullet = reflector.ReflectedBulletTeams.Any(team => team == bullet.Team);

				if (reflectBullet && bulletPosition.Value.Equals(reflectorPosition.Value))
				{
					if(reflector.PushesBulletBack)
					{
						var bulletBackoutOffset = bulletPosition.GetAbsoluteOffset(new int2(0, -1));
						bulletPosition.Value += bulletBackoutOffset;
					}

					if(reflector.ChangesBulletTeam)
					{
						bullet.Team = reflector.ReflectedBulletTeam;
						switch(reflector.ReflectedBulletTeam)
						{
							case Team.ENEMY:
								bullet.Team = Team.ENEMY;
								bullet.GetComponent<SpriteAnimator>().Frames = enemyBulletSprites;
								break;
							case Team.PLAYER:
								bullet.Team = Team.PLAYER;
								bullet.GetComponent<SpriteAnimator>().Frames = playerBulletSprites;
								break;
							case Team.NEUTRAL:
								bullet.Team = Team.NEUTRAL;
								bullet.GetComponent<SpriteAnimator>().Frames = neutralBulletSprite;
								break;
						}
					}
					bulletPosition.Rotation += reflector.reflectDirections.RandomValue();
				}
			}
		}
	}
}