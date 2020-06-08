using Unity.Entities;

public interface IActingStrategy
{
	bool HasActionReady();
	IAction GetAction(Entity entity);
}