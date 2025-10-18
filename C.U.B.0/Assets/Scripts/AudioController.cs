using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController instance;

    [SerializeField] private AudioSource audioSource;

    [Header("UI AUDIO")]
    [SerializeField] private AudioClip iniTypeSound;


    private void Awake()
    {
        instance = this;
    }

    public void TypeSound()
    {
        audioSource.pitch = Random.Range(0.95f, 1.05f);
        audioSource.PlayOneShot(iniTypeSound, 0.1f);
    }
}
