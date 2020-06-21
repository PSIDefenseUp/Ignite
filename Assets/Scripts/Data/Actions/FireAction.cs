using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class FireAction : IAction
{
	private readonly BulletData[] bulletData;

	public FireAction(params BulletData[] bulletData)
	{
		this.bulletData = bulletData;
	}

	public bool CanPerform(GameObject actor)
	{
		// can fire so long as at least one bullet isn't being spawned in a reflector that pushes
		var reflectors = Object.FindObjectsOfType<BulletReflector>();

		return bulletData.Any(bullet =>
		{
			foreach (var reflector in reflectors)
			{
				if (reflector.GetComponent<Position>().Value.Equals(bullet.Position) && reflector.PushesBulletBack)
				{
					return false;
				}
			}

			return true;
		});
	}

	public int GetCost()
	{
		return 1;
	}

	public void Perform(GameObject actor)
	{
		foreach (var bullet in bulletData)
		{
			// TODO: animation?

			bullet.Instantiate();
		}
	}
}