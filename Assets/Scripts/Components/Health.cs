using UnityEngine;

public class Health : MonoBehaviour
{
	[SerializeField]
	private int max;
	public int Value;

	public void Init(int maxHealth)
	{
		this.max = maxHealth;
		this.Value = maxHealth;
	}
}