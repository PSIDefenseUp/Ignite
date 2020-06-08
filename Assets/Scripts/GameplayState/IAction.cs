using Unity.Entities;

public interface IAction
{
	void CanPerform(Entity actor);
	void Perform(Entity actor);
}