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
	private readonly InputSystem inputSystem;
	private readonly FaceMouseSystem faceMouseSystem;
	private readonly TurnSystem turnSystem;
	private readonly HealthSystem healthSystem; // tick with turn instead of every frame?
	private readonly BulletReflectionSystem bulletReflectionSystem; // tick with turn instead of every frame?
	private readonly BulletCollisionSystem bulletCollisionSystem; // tick with turn instead of every frame?
	private readonly MapToWorldPositionSystem mapToWorldPositionSystem;
	private readonly CameraFollowSystem cameraFollowSystem;
	private readonly MapToWorldRotationSystem mapToWorldRotationSystem;
	private readonly AnimationDirectionSystem animationDirectionSystem;

	public InputCommand InputCommand;

	private GameState()
	{
		this.inputSystem = new InputSystem();
		this.faceMouseSystem = new FaceMouseSystem();
		this.turnSystem = new TurnSystem();
		this.healthSystem = new HealthSystem();
		this.bulletReflectionSystem = new BulletReflectionSystem();
		this.bulletCollisionSystem = new BulletCollisionSystem();
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
		inputSystem.Tick();
		faceMouseSystem.Tick();
		turnSystem.Tick();
		mapToWorldPositionSystem.Tick();
		mapToWorldRotationSystem.Tick();
		animationDirectionSystem.Tick();
		cameraFollowSystem.Tick();

		// DEBUGSHIT
		debugTick();
	}

	private void debugTick()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}

		if (Input.GetKeyDown(KeyCode.R))
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
			instance = null;
		}
	}
}