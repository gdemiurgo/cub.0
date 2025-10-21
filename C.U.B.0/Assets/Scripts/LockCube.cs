using UnityEngine;

public class LockCube : MonoBehaviour
{
	[SerializeField]
	private int lockNumber = 1;

	[Header("SCREEN UI")]
	[SerializeField]
	private GameObject screenLocked;

	[SerializeField]
	private GameObject screenUnlocked;

	[SerializeField]
	private TextMesh lockNumberText;

	[Header("AUDIO")]
	[SerializeField]
	private AudioSource audioSource;

	[SerializeField]
	private AudioClip errorSound;

	[SerializeField]
	private AudioClip unlockedSound;

	private bool canUnlock;

	private bool unlocked;

	private void Start()
	{
		IniLock();
	}

	private void IniLock()
	{
		lockNumberText.text = lockNumber.ToString();
		unlocked = false;
		screenLocked.SetActive(value: true);
		screenUnlocked.SetActive(value: false);
	}

	private void OnMouseDown()
	{
		if (canUnlock)
		{
			if (!unlocked && (lockNumber == 1 || (lockNumber == 2 && GameController.sharedGameController.unlocked1) || (lockNumber == 3 && GameController.sharedGameController.unlocked1 && GameController.sharedGameController.unlocked2)))
			{
				GameController.sharedGameController.Unlock(lockNumber);
				unlocked = true;
				screenLocked.SetActive(value: false);
				screenUnlocked.SetActive(value: true);
				audioSource.PlayOneShot(unlockedSound, 0.2f);
			}
			else
			{
				audioSource.PlayOneShot(errorSound, 0.2f);
			}
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			canUnlock = true;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			canUnlock = false;
		}
	}
}
