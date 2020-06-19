using Unity.Mathematics;
using UnityEngine;
using System.Linq;

public class BulletReflectionSystem
{
	private Sprite enemyBulletSprite;
	private Sprite playerBulletSprite;
	private Sprite neutralBulletSprite;
	public BulletReflectionSystem()
	{
		enemyBulletSprite = Resources.Load<Sprite>("Dev/Sprites/EnemyBullet");
		playerBulletSprite = Resources.Load<Sprite>("Dev/Sprites/PlayerBullet");
		neutralBulletSprite = Resources.Load<Sprite>("Dev/Sprites/NeutralBullet");


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
								bullet.GetComponent<SpriteRenderer>().sprite = enemyBulletSprite;
								break;
							case Team.PLAYER:
								bullet.Team = Team.PLAYER;
								bullet.GetComponent<SpriteRenderer>().sprite = playerBulletSprite;
								break;
							case Team.NEUTRAL:
								bullet.Team = Team.NEUTRAL;
								bullet.GetComponent<SpriteRenderer>().sprite = neutralBulletSprite;
								break;
						}
					}
					bulletPosition.Rotation += reflector.reflectDirections.RandomValue();
				}
			}
		}
	}
}