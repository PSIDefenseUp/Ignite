public class Map
{
	public readonly int Width;
	public readonly int Height;
	// private readonly bool[] staticCollisionGrid;

	public Map()
	{
		this.Width = 20;
		this.Height = 15;

		// this.staticCollisionGrid = new bool[Width * Height];

		// for (int y = 0; y < Height; y++)
		// {
		// 	for (int x = 0; x < Width; x++)
		// 	{
		// 		// cover map bounds with static collision
		// 		this.staticCollisionGrid[y * Width + x] = (x == 0 || y == 0 || x == Width - 1 || y == Height - 1);
		// 	}
		// }
	}

	public bool Contains(int x, int y)
	{
		return x >= 0 && y >= 0 && x < Width && y < Height;
	}

	// public bool CannotBeOccupied(int x, int y)
	// {
	// 	return this.staticCollisionGrid[y * Width + x];
	// }
}