using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class WhirlwindWeaselThinker : EnemyThinker
{
    // Start is called before the first frame update
	private static GameObject whirlwindPrefab;
	private static GameObject deployedWeaselPrefab;
    private static Sprite bulletSprite;
	public void Start()
	{	
		bulletSprite = bulletSprite ?? Resources.Load<Sprite>("Dev/Sprites/EnemyBullet");
		whirlwindPrefab = whirlwindPrefab ?? Resources.Load<GameObject>("Prefabs/Enemies/Whirlwind");
		deployedWeaselPrefab = deployedWeaselPrefab ?? Resources.Load<GameObject>("Prefabs/Enemies/DeployedWeasel");

	}

	private bool AreWithinDistance(int2 a, int2 b, int distance)
	{
		return (Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y)) <= distance;
	}

	public override void Think()
	{
		var actor = GetComponent<Actor>();
		var position = GetComponent<Position>();
		var bullets = FindObjectsOfType<Bullet>()
			.Where(bullet => bullet.Team != Team.ENEMY)
			.Select(bullet => bullet.GetComponent<Position>())
			.Where(bullet => AreWithinDistance(bullet.Value, position.Value, 1));
		var up = new List<Position>();
		var down = new List<Position>();
		var left = new List<Position>();
		var right = new List<Position>();
		var standing = new List<Position>();
		int2 leftPos = position.Value + new int2 (-1, 0);
		int2 rightPos = position.Value + new int2 (1, 0);
		int2 upPos = position.Value + new int2 (0, 1);
		int2 downPos = position.Value + new int2 (0, -1);
		if(bullets.Count() != 0)
		{
		
			foreach (var bullet in bullets)
			{
				if(rightPos.Equals(bullet.Value))
				{
					right.Add(bullet);
				}
				if(leftPos.Equals(bullet.Value))
				{
					left.Add(bullet);
				}
				if(upPos.Equals(bullet.Value))
				{
					up.Add(bullet);
				}
				if(downPos.Equals(bullet.Value))
				{
					down.Add(bullet);
				}
				if(position.Value.Equals(bullet.Value))
				{
					standing.Add(bullet);
				}
			}
			int2 destination = position.Value;
			var curr = standing;
			if(up.Count >= standing.Count && up.Count >= left.Count && up.Count >= right.Count && up.Count >= down.Count)
			{
				destination = upPos;
				curr = up;
			}
			else if(down.Count >= standing.Count && down.Count >= left.Count && down.Count >= right.Count)
			{
				destination = downPos;
				curr = down;
			}
			else if(right.Count >= standing.Count && right.Count >= left.Count)
			{
				destination = rightPos;
				curr = right;
			}
			else if (left.Count >= standing.Count)
			{
				destination = leftPos;
				curr = left;
			}
            var reflectDirections = new float[]{0, 90, 180, 270};
            foreach(var bullet in curr)
				{
					bullet.Rotation = reflectDirections.RandomValue();
					var bulletOfTheBullet = bullet.GetComponent<Bullet>();
					bulletOfTheBullet.Team = Team.ENEMY;
					bulletOfTheBullet.GetComponent<SpriteRenderer>().sprite = bulletSprite;

				}
			actor.SetAction(new TurnMoveAction(destination-position.Value));
		}
		
		if (!actor.HasAction())
		{
			var player = FindObjectOfType<Player>();

			if(player != null)
			{
				var playerPosition = player.GetComponent<Position>();
				var behindPlayer = playerPosition.GetRelativePosition(new int2 (0, -1));
				
				if(AreWithinDistance(playerPosition.Value, position.Value, 4) && CanCreateWeasel(behindPlayer))
				{
					//create whirlwind here
					var whirlwind = Instantiate(whirlwindPrefab);
					var whirlwindPosition = whirlwind.GetComponent<Position>();
					whirlwindPosition.Value = position.Value;

					//create deployedweasel behind player
					var weasel = Instantiate(deployedWeaselPrefab);
					var weaselPosition = weasel.GetComponent<Position>();
					weaselPosition.Value = behindPlayer;

					Destroy(this.gameObject);
				}
				else
				{
					position.Rotation = position.GetClosestAbsoluteDirection(playerPosition.Value, 4);
					actor.SetAction(new MoveRelativeAction(new int2(0, 1)));
				}
			}
			else
			{
				actor.SetAction(new PassTurnAction());	
			}	
		}
	}
	private bool CanCreateWeasel(int2 destination)
	{
		if (!GameState.Instance.Map.Contains(destination))
		{
			return false;
		}
		var solids = Object.FindObjectsOfType<Solid>();
		var isDestinationOpen = !solids.Any(solid => solid.GetComponent<Position>().Value.Equals(destination));
		return isDestinationOpen;
		
	}
}
