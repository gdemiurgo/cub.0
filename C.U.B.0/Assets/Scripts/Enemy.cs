using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("ENEMY PARAMETERS")]
    [SerializeField] private float fireRate;
    [SerializeField] private float bulletSpeed;

    [Header("COMPONENTS")]
    [SerializeField] private GameObject cannon;
    [SerializeField] private LineRenderer laser;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject deathFX;

    [Header("MESHES")]
    [SerializeField] private GameObject redSphere;
    [SerializeField] private GameObject body;

    [Header("AUDIO")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip fireSound;


    private bool dead, canFire;
    private GameObject player;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        IniEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        if(!dead)
        {
            SightControl();
            FireRateControl();
        }
    }

    private void SightControl()
    {
        cannon.transform.LookAt(player.transform);

        RaycastHit playerHit;
        if(canFire && Physics.Raycast(cannon.transform.position, cannon.transform.forward, out playerHit, 200))
        {
            if(playerHit.transform.CompareTag("Player"))
            {
                Fire();
            }
        }
    }

    private void IniEnemy()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Fire()
    {
        GameObject newBullet = Instantiate(bullet, cannon.transform.position, Quaternion.identity);
        newBullet.GetComponent<Rigidbody>().AddForce(cannon.transform.forward * bulletSpeed);

        audioSource.PlayOneShot(fireSound, 0.2f);

        canFire = false;
    }

    private void FireRateControl()
    {
        if (!canFire)
        {
            timer += Time.deltaTime;
            if(timer > fireRate)
            {
                timer = 0;
                canFire = true;
            }
        }
    }

    public void Death()
    {
        redSphere.SetActive(false);
        deathFX.SetActive(true);
    }
}
