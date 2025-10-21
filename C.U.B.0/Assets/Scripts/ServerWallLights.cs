using UnityEngine;

public class ServerWallLights : MonoBehaviour
{
	[SerializeField]
	private SpriteRenderer spriteRenderer;

	[SerializeField]
	private float lightRate;

	private bool changeLight;

	private void Start()
	{
		ChangeLight();
		lightRate = Random.Range(1, 3);
	}

	private void ChangeLight()
	{
		spriteRenderer.flipX = ((Random.Range(0, 11) <= 5) ? true : false);
		spriteRenderer.flipY = ((Random.Range(0, 11) <= 5) ? true : false);
		Invoke("ChangeLight", lightRate);
	}
}
