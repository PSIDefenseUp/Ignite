using System.Linq;
using UnityEngine;

public class TurnSystem
{
	private bool haveTickedTurnEndSystems;
	private readonly ThreatSystem threatSystem = new ThreatSystem();
	private readonly BulletCollisionSystem bulletCollisionSystem = new BulletCollisionSystem();
	private readonly BulletReflectionSystem bulletReflectionSystem = new BulletReflectionSystem();
	private readonly HealthSystem healthSystem = new HealthSystem();
	private readonly GameOverSystem gameOverSystem = new GameOverSystem();
	private readonly LevelVictorySystem levelVictorySystem = new LevelVictorySystem();
	private readonly ExpirationDateSystem expirationDateSystem = new ExpirationDateSystem();

	public void Tick()
	{
		if (GameState.Instance.SecondsUntilNextTurn <= 0)
		{
			// yes, this is called at the beginning of the loop
			// run after the animations for the previous turn have resolved
			if (!haveTickedTurnEndSystems)
			{
				TickTurnEndSystems();
			}

			// MAKE ACTORS TAKE ACTIONS
			var players = Object.FindObjectsOfType<Player>();
			var playerActors = players.Select(player => player.GetComponent<Actor>()).Where(actor => actor != null);
			var actors = Object.FindObjectsOfType<Actor>();

			bool allPlayersHaveGone = true;
			foreach (var playerActor in playerActors)
			{
				if (playerActor.RemainingActions > 0)
				{
					allPlayersHaveGone = false;
				}

				if (!allPlayersHaveGone)
				{
					break;
				}
			}

			bool anyoneActed = false;
			if (!allPlayersHaveGone)
			{
				// players go
				foreach (var playerActor in playerActors)
				{
					var thinker = playerActor.GetComponent<Thinker>();
					thinker.Think();
					if (playerActor.RemainingActions > 0 && playerActor.HasAction())
					{
						playerActor.Act();
						anyoneActed = true;
					}
				}
			}
			else
			{
				var bullets = Object.FindObjectsOfType<Bullet>();
				var bulletActors = bullets.Select(bullet => bullet.GetComponent<Actor>()).Where(actor => actor != null);

				bool allBulletsHaveGone = true;
				foreach (var bulletActor in bulletActors)
				{
					if (bulletActor.RemainingActions > 0)
					{
						allBulletsHaveGone = false;
					}

					if (!allBulletsHaveGone)
					{
						break;
					}
				}

				if (!allBulletsHaveGone)
				{
					foreach (var bulletActor in bulletActors)
					{
						var thinker = bulletActor.GetComponent<Thinker>();
						thinker.Think();

						if (bulletActor.HasAction())
						{
							bulletActor.Act();
						}
						else
						{
							bulletActor.RemainingActions = 0;
						}

						anyoneActed = true;
					}
				}
				else
				{
					// everyone else go
					foreach (var actor in actors)
					{
						if (actor.RemainingActions > 0 && actor.GetComponent<Player>() == null && actor.GetComponent<Bullet>() == null)
						{
							var thinker = actor.GetComponent<Thinker>();
							thinker.Think();

							if (actor.HasAction())
							{
								actor.Act();
							}
							else
							{
								actor.RemainingActions = 0;
							}

							anyoneActed = true;
						}
					}
				}
			}

			if (!actors.Any(actor => actor.RemainingActions > 0)) // (!Object.FindObjectsOfType<Actor>().Any(actor => actor.RemainingActions > 0))
			{
				TickRoundEndSystems();

				foreach(var actor in actors)
				{
					actor.Refresh();
				}
			}

			if (anyoneActed)
			{
				bulletReflectionSystem.Tick();

				haveTickedTurnEndSystems = false;
				GameState.Instance.SecondsUntilNextTurn = GameState.Instance.SecondsBetweenTurns;
			}
		}
		else
		{
			GameState.Instance.SecondsUntilNextTurn -= Time.deltaTime;
		}
	}

	// where a round is EVERY ACTOR USES ALL THEIR ACTIONS
	private void TickRoundEndSystems()
	{
		expirationDateSystem.Tick();
	}

	// where a turn is WHATEVER SET OF ACTORS ARE ACTING THIS TIME TAKE AN ACTION
	private void TickTurnEndSystems()
	{
		bulletCollisionSystem.Tick();
		healthSystem.Tick();
		threatSystem.Tick();
		gameOverSystem.Tick();
		levelVictorySystem.Tick();

		haveTickedTurnEndSystems = true;
	}
}