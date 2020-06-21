using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

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

	// data
	public Map Map;
	public readonly float SecondsBetweenTurns;
	public float SecondsUntilNextTurn;
	public bool GameOver;
	public bool StageWon;

	// systems
	private readonly FaceMouseSystem faceMouseSystem;
	private readonly TurnSystem turnSystem;
	private readonly MapToWorldPositionSystem mapToWorldPositionSystem;
	private readonly CameraFollowSystem cameraFollowSystem;
	private readonly MapToWorldRotationSystem mapToWorldRotationSystem;
	private readonly AnimationDirectionSystem animationDirectionSystem;

	public InputCommand InputCommand;

	private GameState()
	{
		this.faceMouseSystem = new FaceMouseSystem();
		this.turnSystem = new TurnSystem();
		this.mapToWorldPositionSystem = new MapToWorldPositionSystem();
		this.cameraFollowSystem = new CameraFollowSystem();
		this.mapToWorldRotationSystem = new MapToWorldRotationSystem();
		this.animationDirectionSystem = new AnimationDirectionSystem();

		this.SecondsBetweenTurns = .1f;
		this.SecondsUntilNextTurn = SecondsBetweenTurns;

		this.Map = Map.FromScene();
		this.StageWon = false;
		this.GameOver = false;
	}

	public void Tick()
	{
		InputSystem.Instance.Tick();

		if (!StageWon)
		{
			faceMouseSystem.Tick();
			turnSystem.Tick();
			mapToWorldPositionSystem.Tick();
			mapToWorldRotationSystem.Tick();
			animationDirectionSystem.Tick();
			cameraFollowSystem.Tick();
		}

		if (GameOver)
		{
			if (InputCommand != InputCommand.NONE)
			{
				LoadScene(SceneManager.GetActiveScene().name);
			}
		}

		if (StageWon)
		{
			if (InputCommand != InputCommand.NONE)
			{
				LoadScene(Object.FindObjectOfType<LevelEndPoint>().NextSceneName);
			}
		}

		if (InputCommand == InputCommand.EXIT)
		{
			Application.Quit();
		}
	}

	private void LoadScene(string name)
	{
		SceneManager.LoadScene(name);
		instance = null;
	}
}