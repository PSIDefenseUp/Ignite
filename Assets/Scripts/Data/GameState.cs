using UnityEngine;

public class GameState
{
	private static readonly object instanceLock = new object();
	private static GameState instance;
	public static GameState Instance
	{
		get
		{
			if (instance == null)
			{
				lock(instanceLock)
				{
					if (instance == null)
					{
						instance = new GameState();
					}
				}
			}

			return instance;
		}
	}

	private readonly InputSystem inputSystem;
	private readonly TurnSystem turnSystem;
	private readonly HealthSystem healthSystem; // tick with turn instead of every frame?
	private readonly BulletReflectionSystem bulletReflectionSystem; // tick with turn instead of every frame?
	private readonly BulletCollisionSystem bulletCollisionSystem; // tick with turn instead of every frame?
	private readonly MapToWorldPositionSystem mapToWorldPositionSystem;

	public InputCommand InputCommand;

	private GameState()
	{
		this.inputSystem = new InputSystem();
		this.turnSystem = new TurnSystem(.2f);
		this.healthSystem = new HealthSystem();
		this.bulletReflectionSystem = new BulletReflectionSystem();
		this.bulletCollisionSystem = new BulletCollisionSystem();
		this.mapToWorldPositionSystem = new MapToWorldPositionSystem();
	}

	public void Tick()
	{
		inputSystem.Tick();
		turnSystem.Tick();
		bulletReflectionSystem.Tick();
		bulletCollisionSystem.Tick();
		healthSystem.Tick();
		mapToWorldPositionSystem.Tick();
	}
}