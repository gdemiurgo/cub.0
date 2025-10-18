using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCube : MonoBehaviour
{
    [SerializeField] private GameObject capsuleObj;

    [Header("SCREEN UI")]
    [SerializeField] private GameObject screenLocks1;
    [SerializeField] private GameObject screenLocks2;
    [SerializeField] private GameObject screenLocks3;

    [Header("1 LOCK")]
    [SerializeField] private GameObject locks1Locked1;
    [SerializeField] private GameObject locks1Unlocked1;
    [Header("2 LOCKS")]
    [SerializeField] private GameObject locks2Locked1;
    [SerializeField] private GameObject locks2Locked2;
    [SerializeField] private GameObject locks2Unlocked1;
    [SerializeField] private GameObject locks2Unlocked2;
    [Header("3 LOCKS")]
    [SerializeField] private GameObject locks3Locked1;
    [SerializeField] private GameObject locks3Locked2;
    [SerializeField] private GameObject locks3Locked3;
    [SerializeField] private GameObject locks3Unlocked1;
    [SerializeField] private GameObject locks3Unlocked2;
    [SerializeField] private GameObject locks3Unlocked3;

    [Header("AUDIO")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip openDoorSound;
    [SerializeField] private AudioClip closedDoorSound;

    Animator capsuleAnim;
    bool opened;

    void Start()
    {
        capsuleAnim = capsuleObj.GetComponent<Animator>();
    }

    void Update()
    {
        
    }

    public void OpenCapsule()
    {
        capsuleAnim.SetBool("Opened", true);

        audioSource.PlayOneShot(openDoorSound, 0.2f);
    }

    public void CloseCapsule()
    {
        capsuleAnim.SetBool("Opened", false);

        audioSource.PlayOneShot(openDoorSound, 0.2f);
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
        SetScreenUI(false, false, false);
    }

    private void ResetScreen()
    {
        switch (GameController.sharedGameController.LocksNumber())
        {
            case 1:
                screenLocks1.SetActive(true);
                screenLocks2.SetActive(false);
                screenLocks3.SetActive(false);
                break;
            case 2:
                screenLocks1.SetActive(false);
                screenLocks2.SetActive(true);
                screenLocks3.SetActive(false);
                break;
            case 3:
                screenLocks1.SetActive(false);
                screenLocks2.SetActive(false);
                screenLocks3.SetActive(true);
                break;
        }
    }

    //CALLED FROM ANIMATION EVENT
    private void DoorClosed()
    {
        audioSource.PlayOneShot(closedDoorSound, 0.2f);
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            CloseCapsule();
            GameController.sharedGameController.SetStartCapsuleClosed(true);
        }
    }
}
