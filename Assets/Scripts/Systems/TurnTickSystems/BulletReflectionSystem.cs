using Unity.Mathematics;
using UnityEngine;

public class BulletReflectionSystem
{
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

				if (bulletPosition.Value.Equals(reflectorPosition.Value))
				{
					var bulletBackoutOffset = bulletPosition.GetAbsoluteOffset(new int2(0, -1));
					bulletPosition.Value += bulletBackoutOffset;
					bulletPosition.Rotation += reflector.reflectDirections.RandomValue();
				}
			}
		}
	}
}