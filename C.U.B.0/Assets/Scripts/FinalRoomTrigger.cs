using UnityEngine;

public class FinalRoomTrigger : MonoBehaviour
{
	[SerializeField]
	private GameObject frontDeathPlane;

	[SerializeField]
	private AudioSource audioSource;

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			frontDeathPlane.SetActive(value: false);
			other.GetComponent<PlayerController>().FireDeactivate();
			other.GetComponent<GravityController>().enabled = false;
			audioSource.Play();
		}
	}
}
