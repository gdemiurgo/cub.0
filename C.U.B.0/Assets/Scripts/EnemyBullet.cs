using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip moveSound;
    [SerializeField] private AudioClip impactSound;
    [SerializeField] private GameObject impact;

    private void OnEnable()
    {
        audioSource.clip = moveSound;
        audioSource.Play();

        Destroy(gameObject, 3);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            GameController.sharedGameController.GameOver(true, 3);
        }

        Instantiate(impact, transform.position, Quaternion.identity);

        audioSource.Stop();
        audioSource.PlayOneShot(impactSound, 0.2f);
    }
}
