using Unity.Entities;

public class BulletCollisionSystem : ComponentSystem
{
	protected override void OnUpdate()
	{
		// get all bullets
		// check if they are colliding with anything with health
		// if so, remove health from the thing they hit and destroy bullet
		// if not, check if they are colliding with another bullet
		// if so, either choose 1 to keep or combine bullets in same direction into super bullet (or choose a super bullet to keep, etc.)
	}
}