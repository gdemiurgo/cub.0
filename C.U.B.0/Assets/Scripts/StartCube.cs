using UnityEngine;

public class StartCube : MonoBehaviour
{
	[SerializeField]
	private GameObject capsuleObj;

	[Header("SCREEN UI")]
	[SerializeField]
	private GameObject screenLocks1;

	[SerializeField]
	private GameObject screenLocks2;

	[SerializeField]
	private GameObject screenLocks3;

	[Header("1 LOCK")]
	[SerializeField]
	private GameObject locks1Locked1;

	[SerializeField]
	private GameObject locks1Unlocked1;

	[Header("2 LOCKS")]
	[SerializeField]
	private GameObject locks2Locked1;

	[SerializeField]
	private GameObject locks2Locked2;

	[SerializeField]
	private GameObject locks2Unlocked1;

	[SerializeField]
	private GameObject locks2Unlocked2;

	[Header("3 LOCKS")]
	[SerializeField]
	private GameObject locks3Locked1;

	[SerializeField]
	private GameObject locks3Locked2;

	[SerializeField]
	private GameObject locks3Locked3;

	[SerializeField]
	private GameObject locks3Unlocked1;

	[SerializeField]
	private GameObject locks3Unlocked2;

	[SerializeField]
	private GameObject locks3Unlocked3;

	[Header("AUDIO")]
	[SerializeField]
	private AudioSource audioSource;

	[SerializeField]
	private AudioClip openDoorSound;

	[SerializeField]
	private AudioClip closedDoorSound;

	[Header("ANIMATION")]
	[SerializeField]
	private Animator capsuleAnim;

	private bool opened;

	public bool doorClosed;

	private void Start()
	{
		IniStartCube();
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
			if (GameController.sharedGameController.AllUnlocked())
			{
				doorClosed = false;
			}
		}

		Debug.Log("Open Capsule");
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

	public bool Opened()
	{
		return opened;
	}

	public void SetScreenUI(bool unlocked1, bool unlocked2, bool unlocked3)
	{
		locks1Locked1.SetActive(!unlocked1);
		locks1Unlocked1.SetActive(unlocked1);
		locks2Locked1.SetActive(!unlocked1);
		locks2Locked2.SetActive(!unlocked2);
		locks2Unlocked1.SetActive(unlocked1);
		locks2Unlocked2.SetActive(unlocked2);
		locks3Locked1.SetActive(!unlocked1);
		locks3Locked2.SetActive(!unlocked2);
		locks3Locked3.SetActive(!unlocked3);
		locks3Unlocked1.SetActive(unlocked1);
		locks3Unlocked2.SetActive(unlocked2);
		locks3Unlocked3.SetActive(unlocked3);
	}

	private void IniStartCube()
	{
		capsuleAnim = capsuleObj.GetComponent<Animator>();
		ResetScreen();
		SetScreenUI(unlocked1: false, unlocked2: false, unlocked3: false);
		doorClosed = true;
	}

	private void ResetScreen()
	{
		switch (GameController.sharedGameController.LocksNumber())
		{
		case 1:
			screenLocks1.SetActive(value: true);
			screenLocks2.SetActive(value: false);
			screenLocks3.SetActive(value: false);
			break;
		case 2:
			screenLocks1.SetActive(value: false);
			screenLocks2.SetActive(value: true);
			screenLocks3.SetActive(value: false);
			break;
		case 3:
			screenLocks1.SetActive(value: false);
			screenLocks2.SetActive(value: false);
			screenLocks3.SetActive(value: true);
			break;
		}
	}

	public void DoorClosed()
	{
		if (!GameController.sharedGameController.IsGameOver())
		{
			audioSource.PlayOneShot(closedDoorSound, 0.04f);
		}
		doorClosed = true;
	}

	public bool IsDoorClosed()
	{
		return doorClosed;
	}

	private void OnTriggerExit(Collider other)
	{
		if (opened && other.tag == "Player")
		{
			CloseCapsule();
			GameController.sharedGameController.SetStartCapsuleClosed(startCapsuleClosed: true);
		}
	}
}
