using UnityEngine;

public class Enemy : MonoBehaviour
{
	[Header("ENEMY PARAMETERS")]
	[SerializeField]
	private float fireRate;

	[SerializeField]
	private float bulletSpeed;

	[Header("COMPONENTS")]
	[SerializeField]
	private GameObject cannon;

	[SerializeField]
	private GameObject cannonEnd;

	[SerializeField]
	private LineRenderer laser;

	[SerializeField]
	private GameObject bullet;

	[SerializeField]
	private GameObject deathFX;

	[SerializeField]
	private Collider collider;

	[Header("MESHES")]
	[SerializeField]
	private GameObject redSphere;

	[SerializeField]
	private GameObject body;

	[Header("AUDIO")]
	[SerializeField]
	private AudioSource audioSource;

	[SerializeField]
	private AudioClip fireSound;

	private bool dead;

	private bool canFire;

	private GameObject player;

	private float timer;

	private void Start()
	{
		IniEnemy();
	}

	private void Update()
	{
		if (!dead && !GameController.sharedGameController.IsGameOver())
		{
			SightControl();
			FireRateControl();
		}
	}

	private void SightControl()
	{
		cannon.transform.LookAt(player.transform);
		Vector3 direction = Camera.main.transform.position - cannonEnd.transform.position;
		if (canFire && Physics.Raycast(cannonEnd.transform.position, direction, out var hitInfo, 200f) && hitInfo.transform.CompareTag("Player"))
		{
			Fire();
		}
	}

	private void IniEnemy()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		canFire = true;
	}

	private void Fire()
	{
		Object.Instantiate(bullet, cannonEnd.transform.position, Quaternion.identity).GetComponent<Rigidbody>().AddForce(cannon.transform.forward * bulletSpeed);
		audioSource.PlayOneShot(fireSound, 0.8f);
		canFire = false;
	}

	private void FireRateControl()
	{
		if (!canFire)
		{
			timer += Time.deltaTime;
			if (timer > fireRate)
			{
				timer = 0f;
				canFire = true;
			}
		}
	}

	public void Death()
	{
		redSphere.SetActive(value: false);
		deathFX.SetActive(value: true);
		cannon.SetActive(value: false);
		dead = true;
		collider.enabled = false;
	}
}
