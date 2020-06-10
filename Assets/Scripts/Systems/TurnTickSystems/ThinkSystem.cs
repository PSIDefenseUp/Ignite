using UnityEngine;

public class ThinkSystem
{
	public void Tick()
	{
		var thinkers = Object.FindObjectsOfType<Thinker>();

		foreach(var thinker in thinkers)
		{
			thinker.Think();
		}
	}
}