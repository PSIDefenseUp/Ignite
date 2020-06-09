using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public class MapRenderSystem : ComponentSystem
{
	protected override void OnUpdate()
	{
		Entities.ForEach((ref MapPositionComponent mapPosition, ref Translation translation) =>
		{
			translation.Value = new float3(1 * mapPosition.X, -1 * mapPosition.Y, 0);
		});

		Entities.ForEach((ref MapPositionComponent mapPosition, ref Rotation rotation) =>
		{
			// TODO: probably get this right
			rotation.Value = quaternion.RotateZ((float)mapPosition.Rotation);
		});
	}
}