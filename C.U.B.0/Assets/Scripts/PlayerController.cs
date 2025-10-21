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

	public AudioClip fallDeathSound;

	public AudioClip theManSound;

	public int stepOrder;

	public float stepRate;

	private bool canFire = true;

	private bool fireActivated;

	private bool fallDeadSound;

	private float timer;

	private float stepsTimer;

	private GameObject gameControllerObj;

	private GameController gameController;

	private Rigidbody myRB;

	private float lastFallSpeed;

	private RigidbodyFirstPersonController firstPersonController;

	private GravityController grabityController;

	private void Start()
	{
		gameControllerObj = GameObject.FindGameObjectWithTag("GameController");
		gameController = gameControllerObj.GetComponent<GameController>();
		myRB = GetComponent<Rigidbody>();
		firstPersonController = GetComponent<RigidbodyFirstPersonController>();
		grabityController = GetComponent<GravityController>();
	}

	private void Update()
	{
		if (!canFire)
		{
			FireRateControl();
		}
		if (canFire && Input.GetKeyDown(KeyCode.Mouse1) && fireActivated)
		{
			Fire();
		}
		FallDeathControl();
		if (firstPersonController.Grounded)
		{
			if (firstPersonController.Velocity.magnitude > 0.1f)
			{
				StepsControl();
			}
		}
		else
		{
			stepsTimer = 0f;
		}
	}

	private void Fire()
	{
		if (Physics.Raycast(spawnPos.transform.position, spawnPos.transform.forward, out var hitInfo, 100f))
		{
			if (hitInfo.transform.tag == "TheMan")
			{
				weaponAudioSource.PlayOneShot(theManSound, 0.2f);
				FireDeactivate();
				GameController.sharedGameController.TheManShooted();
				return;
			}
			if (hitInfo.transform.tag == "Wall" || hitInfo.transform.tag == "EnemyBody" || hitInfo.transform.tag == "GWall")
			{
				Object.Instantiate(bulletColEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
			}
			if (hitInfo.transform.tag == "Enemy")
			{
				Enemy component = hitInfo.transform.gameObject.GetComponent<Enemy>();
				if ((bool)component)
				{
					component.Death();
				}
			}
			Object.Instantiate(bulletColEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
		}
		Object.Instantiate(shotEffect, shotPos.transform.position, spawnPos.transform.rotation);
		weaponAudioSource.PlayOneShot(fireSound, 0.4f);
		canFire = false;
	}

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

	public void GravityControlActivate()
	{
		grabityController = GetComponent<GravityController>();
		grabityController.SetActivated(activated: true);
	}

	public void GravityControlDeactivate()
	{
		grabityController = GetComponent<GravityController>();
		grabityController.SetActivated(activated: false);
	}

	public void GetDamage(int damagePoints)
	{
		if (lifePoints - damagePoints > 0)
		{
			lifePoints -= damagePoints;
			return;
		}
		lifePoints = 0;
		gameController.GameOver(restartScene: true, 3f);
	}

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
	}

	public void GetAmmo()
	{
		bullets = totalBullets;
	}

	private void FireRateControl()
	{
		timer += Time.deltaTime;
		if (timer >= shootRate)
		{
			timer = 0f;
			canFire = true;
		}
	}

	private void FallDeathControl()
	{
		if (myRB.velocity.magnitude > lastFallSpeed)
		{
			lastFallSpeed = myRB.velocity.magnitude;
		}
		if (Physics.Raycast(base.transform.position, -base.transform.up, out var hitInfo, 1.65f) && (hitInfo.transform.tag == "Wall" || hitInfo.transform.tag == "GWall" || hitInfo.transform.tag == "StartCube"))
		{
			if (lastFallSpeed > deathFallSpeedLimit)
			{
				FallDeadSound();
				gameController.GameOver(restartScene: true, 2f);
				lastFallSpeed = 0f;
			}
			else
			{
				lastFallSpeed = 0f;
			}
		}
	}

	private void FallDeadSound()
	{
		if (!fallDeadSound)
		{
			stepsAudioSource.pitch = Random.Range(1.9f, 2f);
			stepsAudioSource.PlayOneShot(fallDeathSound, 0.2f);
			fallDeadSound = true;
		}
	}

	private void StepsControl()
	{
		stepsTimer += Time.deltaTime;
		if (stepsTimer > stepRate)
		{
			AudioClip clip = ((stepOrder == -1) ? leftStepSound : rightStepSound);
			stepsAudioSource.pitch = Random.Range(0.97f, 1.03f);
			stepsAudioSource.PlayOneShot(clip, 0.4f);
			stepOrder *= -1;
			stepsTimer = 0f;
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "EnemyBullet")
		{
			GetDamage(damagePoints);
			Object.Destroy(collision.gameObject);
		}
		if (collision.gameObject.tag == "EnemyBody")
		{
			int num = 1;
			GetDamage(num);
		}
		if (collision.gameObject.tag == "FirstAid")
		{
			GetHealth();
			Object.Destroy(collision.gameObject);
		}
		if (collision.gameObject.tag == "AmmoBox")
		{
			GetAmmo();
			Object.Destroy(collision.gameObject);
		}
		if (collision.gameObject.tag == "DeathPlane")
		{
			FallDeadSound();
			gameController.GameOver(restartScene: true, 2f);
		}
		if (collision.gameObject.tag == "Floor" && collision.transform.position.y < base.transform.position.y && myRB.velocity.y < -1f)
		{
			stepsAudioSource.PlayOneShot(leftStepSound, 0.4f);
			stepsAudioSource.PlayOneShot(rightStepSound, 0.4f);
		}
	}
}
