using UnityEngine;

public class EndCube : MonoBehaviour
{
	[Header("AUDIO")]
	[SerializeField]
	private AudioSource audioSource;

	[SerializeField]
	private AudioClip openDoorSound;

	[Header("ANIMATION")]
	[SerializeField]
	private Animator capsuleAnim;

	private bool opened;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void OpenCapsule()
	{
		if (!opened)
		{
			capsuleAnim.SetBool("Opened", value: true);
			audioSource.PlayOneShot(openDoorSound, 0.02f);
			opened = true;
		}
	}

	public void CloseCapsule()
	{
		if (opened)
		{
			capsuleAnim.SetBool("Opened", value: false);
			audioSource.PlayOneShot(openDoorSound, 0.03f);
			opened = false;
		}
	}
}
