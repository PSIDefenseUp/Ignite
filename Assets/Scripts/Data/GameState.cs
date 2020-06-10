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
	private readonly MapToWorldPositionSystem mapToWorldPositionSystem;

	public InputCommand InputCommand;

	private GameState()
	{
		this.inputSystem = new InputSystem();
		this.turnSystem = new TurnSystem(.2f);
		this.mapToWorldPositionSystem = new MapToWorldPositionSystem();
	}

	public void Tick()
	{
		inputSystem.Tick();
		turnSystem.Tick();
		mapToWorldPositionSystem.Tick();
	}
}