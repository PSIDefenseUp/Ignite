using System.Linq;
using UnityEngine;

public class TurnSystem
{
	private readonly float secondsBetweenTurns;
	private float secondsUntilNextTurn;

	private readonly ThinkSystem thinkSystem;

	public TurnSystem(float secondsBetweenTurns)
	{
		this.secondsBetweenTurns = secondsBetweenTurns;
		this.secondsUntilNextTurn = 0;

		this.thinkSystem = new ThinkSystem();
	}

	public void Tick()
	{
		if (secondsUntilNextTurn <= 0)
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
						thinker.Think();
						actor.Act();
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
				secondsUntilNextTurn = secondsBetweenTurns;
			}
		}
		else
		{
			secondsUntilNextTurn -= Time.deltaTime;
		}
	}
}