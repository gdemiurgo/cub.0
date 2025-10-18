using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerController : MonoBehaviour
{
    public int lifePoints;
    public int totalLifePoints;
    public int healthPoints;
    public GameObject spawnPos;
    public GameObject shotPos;
    public int bullets;
    public int totalBullets;
    public float shootRate;
    public GameObject shotEffect;
    public GameObject bulletColEffect;
    public int damagePoints;
    public float deathFallSpeedLimit;

    [Header("AUDIO")]
    public AudioSource stepsAudioSource;
    public AudioClip rightStepSound;
    public AudioClip leftStepSound;
    public AudioSource weaponAudioSource;
    public AudioClip fireSound;
    public int stepOrder;
    public float stepRate;
    

    bool canFire = true;
    bool fireActivated;
    float timer, stepsTimer;
    GameObject gameControllerObj;
    GameController gameController;
    Rigidbody myRB;
    float lastFallSpeed;
    RigidbodyFirstPersonController firstPersonController;
    GravityController grabityController;

    
    // Start is called before the first frame update
    void Start()
    {
        gameControllerObj = GameObject.FindGameObjectWithTag("GameController");
        gameController = gameControllerObj.GetComponent<GameController>();
        myRB = GetComponent<Rigidbody>();

        firstPersonController = GetComponent<RigidbodyFirstPersonController>();
        grabityController = GetComponent<GravityController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!canFire)
        {
            FireRateControl();
        }

        if (canFire && Input.GetKeyDown(KeyCode.Mouse0) && bullets > 0 && fireActivated)
        {
            Fire();
        }

        //Fall death check
        FallDeathControl(); 

        if(firstPersonController.Grounded && firstPersonController.Velocity.magnitude > 0)
        {
            StepsControl();
        }
    }

    void Fire()
    {
        Instantiate(shotEffect, shotPos.transform.position, spawnPos.transform.rotation);

        RaycastHit hit;
        if (Physics.Raycast(spawnPos.transform.position, spawnPos.transform.forward, out hit, 100))
        {
            if (hit.transform.tag == "Wall" || hit.transform.tag == "EnemyBody" || hit.transform.tag == "GWall")
            {
                Instantiate(bulletColEffect, hit.point, Quaternion.LookRotation(hit.normal));
            }

            if (hit.transform.tag == "Enemy")
            {
                Instantiate(bulletColEffect, hit.point, Quaternion.LookRotation(hit.normal));
                GameObject enemy = hit.transform.gameObject;
                //enemy.GetComponent<EnemyController>().GetDamage(damagePoints);
            }
        }

        weaponAudioSource.PlayOneShot(fireSound, 0.2f);
        canFire = false;

        //bullets -= 1;
        //gameController.UpdatePlayerInfo();
    }

    // Activar/Desactivar movimiento
    public void Activate()
    {
        firstPersonController = GetComponent<RigidbodyFirstPersonController>();
        firstPersonController.enabled = true;
    }

    public void Deactivate()
    {
        firstPersonController = GetComponent<RigidbodyFirstPersonController>();
        firstPersonController.enabled = false;
        GravityControlDeactivate();
        FireDeactivate();
    }

    // Activar/Desactivar disparo
    public void FireActivate()
    {
        gameControllerObj = GameObject.FindGameObjectWithTag("GameController");
        gameController = gameControllerObj.GetComponent<GameController>();
        gameController.EnableCrossHair();
        fireActivated = true;
    }

    public void FireDeactivate()
    {
        gameControllerObj = GameObject.FindGameObjectWithTag("GameController");
        gameController = gameControllerObj.GetComponent<GameController>();
        gameController.DisableCrossHair();
        fireActivated = false;
    }

    // Activar/Desactivar modo gravedad
    public void GravityControlActivate()
    {
        grabityController = GetComponent<GravityController>();
        grabityController.SetActivated(true);
    }

    public void GravityControlDeactivate()
    {
        grabityController = GetComponent<GravityController>();
        grabityController.SetActivated(false);
    }

    // Metodo para quitar puntos de vida
    public void GetDamage(int damagePoints)
    {
        if (lifePoints - damagePoints > 0)
        {
            lifePoints -= damagePoints;
        }
        else
        {
            lifePoints = 0;
            gameController.GameOver(true, 3);
        }

        //gameController.UpdatePlayerInfo();
    }

    // Metodo para subir los puntos de vida hasta el tope
    public void GetHealth()
    {
        if (lifePoints + healthPoints < totalLifePoints)
        {
            lifePoints += healthPoints;
        }
        else
        {
            lifePoints = totalLifePoints;
        }

        //gameController.UpdatePlayerInfo();
    }

    // Metodo para subir la municion hasta el tope
    public void GetAmmo()
    {
        bullets = totalBullets;

        //gameController.UpdatePlayerInfo();
    }

    private void FireRateControl()
    {
        timer += Time.deltaTime;
        if (timer >= shootRate)
        {
            timer = 0;
            canFire = true;
        }
    }

    private void FallDeathControl()
    {
        if (myRB.velocity.magnitude > lastFallSpeed)
        {
            lastFallSpeed = myRB.velocity.magnitude;
        }

        RaycastHit floorHit;
        if (Physics.Raycast(transform.position, -transform.up, out floorHit, 1.65f))
        {
            if (floorHit.transform.tag == "Wall" || floorHit.transform.tag == "GWall")
            {
                if (lastFallSpeed > deathFallSpeedLimit)
                {
                    Debug.Log("HAS MUERTO!!");
                    gameController.GameOver(true, 3);
                    lastFallSpeed = 0;
                }
                else
                {
                    lastFallSpeed = 0;
                }
            }
        }
    }

    private void StepsControl()
    {
        stepsTimer += Time.deltaTime;
        if(stepsTimer > stepRate)
        {
            AudioClip stepSound = stepOrder == -1 ? leftStepSound : rightStepSound;
            stepsAudioSource.pitch = Random.Range(0.98f, 1.02f);
            stepsAudioSource.PlayOneShot(stepSound, 0.1f);

            stepOrder *= -1;
            stepsTimer = 0;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "EnemyBullet")
        {
            //int damagePoints = collision.gameObject.GetComponent<EnemyBullet>().damagePoints;
            GetDamage(damagePoints);

            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "EnemyBody")
        {
            int damagePoints = 1;
            GetDamage(damagePoints);
        }


        if (collision.gameObject.tag == "FirstAid")
        {
            GetHealth();
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "AmmoBox")
        {
            GetAmmo();
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "DeathPlane")
        {
            Debug.Log("HAS MUERTO!!");
            gameController.GameOver(true, 3);
        }
    }
}
