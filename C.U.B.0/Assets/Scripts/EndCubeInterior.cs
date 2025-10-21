using UnityEngine;

public class EndCubeInterior : MonoBehaviour
{
	[SerializeField]
	private EndCube endCube;

	[SerializeField]
	private AudioSource audioSource;

	[SerializeField]
	private AudioClip endClip;

	private bool audioFade;

	private void Update()
	{
		if (audioFade && audioSource.volume > 0f)
		{
			audioSource.volume -= 0.0035f;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			GameController.sharedGameController.MainMenu();
			GameController.sharedGameController.GameOver(restartScene: false, 5f);
			audioSource.PlayOneShot(endClip, 0.2f);
			audioFade = true;
		}
	}
}
