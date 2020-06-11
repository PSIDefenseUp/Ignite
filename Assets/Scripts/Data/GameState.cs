using Unity.Mathematics;
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

	// data
	public Map Map;

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
		this.turnSystem = new TurnSystem(.1f);
		this.healthSystem = new HealthSystem();
		this.bulletReflectionSystem = new BulletReflectionSystem();
		this.bulletCollisionSystem = new BulletCollisionSystem();
		this.mapToWorldPositionSystem = new MapToWorldPositionSystem();
		this.cameraFollowSystem = new CameraFollowSystem();
		this.mapToWorldRotationSystem = new MapToWorldRotationSystem();
		this.animationDirectionSystem = new AnimationDirectionSystem();

		this.LoadMap(new Map());
	}

	private void LoadMap(Map map)
	{
		this.Map = map;

		for (var y = 0; y < Map.Dimensions.y; y++)
		{
			for (var x = 0; x < Map.Dimensions.x; x++)
			{
				if (x == 0 || y == 0 || x == Map.Dimensions.x - 1 || y == Map.Dimensions.y - 1)
				{
					// create boundary walls
					var wall = Resources.Load<GameObject>("Prefabs/IndestructibleReflectingWall");
					var wallPosition = wall.GetComponent<Position>();
					wallPosition.Value = new int2(x, y);

					Object.Instantiate(wall);
				}
				else
				{
					var floorTile = Resources.Load<GameObject>("Prefabs/FloorTile");
					var tilePosition = floorTile.GetComponent<Position>();
					tilePosition.Value = new int2(x, y);

					Object.Instantiate(floorTile);
				}
			}
		}
	}

	public void Tick()
	{
		inputSystem.Tick();
		faceMouseSystem.Tick();
		turnSystem.Tick();
		bulletReflectionSystem.Tick();
		bulletCollisionSystem.Tick();
		healthSystem.Tick();
		mapToWorldPositionSystem.Tick();
		cameraFollowSystem.Tick();
		mapToWorldRotationSystem.Tick();
		animationDirectionSystem.Tick();
	}
}