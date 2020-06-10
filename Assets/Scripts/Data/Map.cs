using Unity.Mathematics;

public class Map
{
	public readonly int2 Dimensions;

	public Map()
	{
		this.Dimensions = new int2(20, 15);
	}

	public bool Contains(int x, int y)
	{
		return x >= 0 && y >= 0 && x < Dimensions.x && y < Dimensions.y;
	}
}