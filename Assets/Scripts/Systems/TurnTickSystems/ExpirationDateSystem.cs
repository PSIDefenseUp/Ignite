using UnityEngine;

public class ExpirationDateSystem
{
	public void Tick()
	{
		var expirationDates = Object.FindObjectsOfType<ExpirationDate>();

		foreach(var expirationDate in expirationDates)
		{
			if (expirationDate.Value > 0)
			{
				expirationDate.Value--;

				if (expirationDate.Value == 0)
				{
					Object.Destroy(expirationDate.gameObject);
				}
			}
		}
	}
}