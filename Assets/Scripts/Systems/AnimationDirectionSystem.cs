using UnityEngine;

public class AnimationDirectionSystem
{
	public void Tick()
	{
		var directionalSprites = Object.FindObjectsOfType<DirectionalSprite>();

		foreach (var directionalSpriteSet in directionalSprites)
		{
			var position = directionalSpriteSet.GetComponent<Position>();

			if (position == null)
			{
				continue;
			}

			Sprite[] sprites = null;
			var actionSpriteSet = directionalSpriteSet.GetComponent<ActionSprite>();

			switch ((int)position.Rotation)
			{
				case 0:
					directionalSpriteSet.transform.localScale = directionalSpriteSet.FlipUpSprite ? new Vector3(-1, 1, 1) : new Vector3(1, 1, 1);
					sprites = directionalSpriteSet.Up;
					break;
				case 90:
					directionalSpriteSet.transform.localScale = directionalSpriteSet.FlipLeftSprite ? new Vector3(-1, 1, 1) : new Vector3(1, 1, 1);
					sprites = directionalSpriteSet.Left;
					break;
				case 180:
					directionalSpriteSet.transform.localScale = directionalSpriteSet.FlipDownSprite ? new Vector3(-1, 1, 1) : new Vector3(1, 1, 1);
					sprites = directionalSpriteSet.Down;
					break;
				case 270:
					directionalSpriteSet.transform.localScale = directionalSpriteSet.FlipRightSprite ? new Vector3(-1, 1, 1) : new Vector3(1, 1, 1);
					sprites = directionalSpriteSet.Right;
					break;
			}

			var animator = directionalSpriteSet.GetComponent<SpriteAnimator>();

			if (animator != null && sprites != null)
			{
				if (animator.Frames != sprites)
				{
					animator.Frames = sprites;
					// animator.CurrentFrame = 0;
				}
			}
		}
	}
}
