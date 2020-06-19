using UnityEngine;

public class BulletReflector : MonoBehaviour
{
	public Team[] ReflectedBulletTeams;
	public float[] reflectDirections; // rotations relative to incoming bullet
	public bool PushesBulletBack;
	public bool ChangesBulletTeam;
	public Team ReflectedBulletTeam;
}