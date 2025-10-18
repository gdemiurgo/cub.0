using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockCube : MonoBehaviour
{
    [SerializeField] private int lockNumber = 1;

    [Header("SCREEN UI")]
    [SerializeField] private GameObject screenLocked;
    [SerializeField] private GameObject screenUnlocked;
    [SerializeField] private TextMesh lockNumberText;

    [Header("AUDIO")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip errorSound;
    [SerializeField] private AudioClip unlockedSound;

    private bool canUnlock, unlocked;

    // Start is called before the first frame update
    void Start()
    {
        IniLock();
    }

    private void IniLock()
    {
        lockNumberText.text = lockNumber.ToString();
        unlocked = false;
        screenLocked.SetActive(true);
        screenUnlocked.SetActive(false);
    }

    private void OnMouseDown()
    {
        if (canUnlock)
        {
            if (!unlocked)
            {
                GameController.sharedGameController.Unlock(lockNumber);
                unlocked = true;
                screenLocked.SetActive(false);
                screenUnlocked.SetActive(true);

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
        if(other.CompareTag("Player"))
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
