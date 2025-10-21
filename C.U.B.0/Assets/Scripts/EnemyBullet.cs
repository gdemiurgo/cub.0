using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
	[SerializeField]
	private AudioSource audioSource;

	[SerializeField]
	private AudioClip moveSound;

	[SerializeField]
	private AudioClip impactSound;

	[SerializeField]
	private GameObject impact;

	[SerializeField]
	private GameObject body;

	private void OnEnable()
	{
		audioSource.clip = moveSound;
		audioSource.Play();
		Object.Destroy(base.gameObject, 3f);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			GameController.sharedGameController.GameOver(restartScene: true, 2f);
		}
		Debug.Log("ME DESTRUYE: " + other.name);
		Object.Instantiate(impact, base.transform.position, Quaternion.identity);
		audioSource.Stop();
		audioSource.PlayOneShot(impactSound, 0.5f);
		body.SetActive(value: false);
	}
}
