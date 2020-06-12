using System.Linq;
using UnityEngine;

public class TurnSystem
{
	public void Tick()
	{
		if (GameState.Instance.SecondsUntilNextTurn <= 0)
		{
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
				// everyone else go
				foreach (var actor in actors)
				{
					if (actor.RemainingActions > 0)
					{
						var thinker = actor.GetComponent<Thinker>();
						if (thinker != null)
						{
							thinker.Think();
						}

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

			if (!Object.FindObjectsOfType<Actor>().Any(actor => actor.RemainingActions > 0))
			{
				foreach(var actor in actors)
				{
					actor.Refresh();
				}
			}

			if (anyoneActed)
			{
				GameState.Instance.SecondsUntilNextTurn = GameState.Instance.SecondsBetweenTurns;
			}
		}
		else
		{
			GameState.Instance.SecondsUntilNextTurn -= Time.deltaTime;
		}
	}
}