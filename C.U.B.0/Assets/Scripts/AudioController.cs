using UnityEngine;

public class AudioController : MonoBehaviour
{
	public static AudioController instance;

	[SerializeField]
	private AudioSource audioSource;

	[Header("UI AUDIO")]
	[SerializeField]
	private AudioClip iniTypeSound;

	[SerializeField]
	private AudioClip endTypeSound;

	[SerializeField]
	private AudioClip startSound;

	private void Awake()
	{
		instance = this;
	}

	public void TypeSound()
	{
		audioSource.pitch = Random.Range(0.99f, 1.01f);
		audioSource.PlayOneShot(iniTypeSound, 0.04f);
	}

	public void EndTypeSound()
	{
		audioSource.PlayOneShot(endTypeSound, 0.04f);
	}

	public void StartSound()
	{
		audioSource.PlayOneShot(startSound, 0.5f);
	}
}
