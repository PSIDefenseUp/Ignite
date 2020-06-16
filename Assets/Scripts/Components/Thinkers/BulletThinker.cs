public class BulletThinker : Thinker
{
	public override void Think()
	{
		var actor = gameObject.GetComponent<Actor>();
		actor.SetAction(new MoveRelativeAction(0, 1));
	}
}