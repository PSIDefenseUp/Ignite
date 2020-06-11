using System;

public static class ArrayExtensions
{
	private static readonly Random random = new Random();

	public static T[] Shuffle<T>(this T[] array)
	{
		var newArray = new T[array.Length];

		System.Array.Copy(array, newArray, array.Length);

		for (int i = 0; i < newArray.Length; i++)
		{
			var temp = newArray[i];

			int randomIndex = random.Next(i, newArray.Length);
			newArray[i] = newArray[randomIndex];
			newArray[randomIndex] = temp;
		}

		return newArray;
	}

	public static T RandomValue<T>(this T[] array)
	{
		return array[random.Next(array.Length)];
	}
}