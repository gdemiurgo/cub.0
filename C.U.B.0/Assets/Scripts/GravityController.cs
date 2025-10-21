using UnityEngine;

public class GravityController : MonoBehaviour
{
	[SerializeField]
	private float speed;

	[SerializeField]
	private GameObject spawnPosCam;

	[SerializeField]
	private bool activated;

	[SerializeField]
	private AudioSource weaponAudioSource;

	[SerializeField]
	private AudioClip gGunSound;

	private GameObject sceneObj;

	private GameObject sceneRotator;

	private GameObject gravitySelectorObj;

	private Transform target;

	private Vector3 rotTarget;

	private bool canRotate = true;

	private bool rotating;

	private bool hitCeiling;

	private int rotUpDir;

	private bool gravityMode;

	private int gravityLayer;

	private Camera camera;

	private float camLerpTime;

	private float iniFov;

	private bool changeFov;

	private bool fovDown;

	private Vector3 velocity = Vector3.zero;

	private void Start()
	{
		sceneObj = GameObject.FindGameObjectWithTag("SceneObj");
		sceneRotator = GameObject.FindGameObjectWithTag("SceneRotator");
		gravityLayer = LayerMask.GetMask("GravityLayer");
		target = GameObject.FindGameObjectWithTag("GameController").transform;
		gravitySelectorObj = GameObject.FindGameObjectWithTag("GravitySelector");
		gravitySelectorObj.SetActive(value: false);
		camera = spawnPosCam.GetComponent<Camera>();
		iniFov = camera.fieldOfView;
	}

	private void Update()
	{
		if (activated && GameController.sharedGameController.GetStartCapsuleClosed())
		{
			if (Physics.Raycast(spawnPosCam.transform.position, spawnPosCam.transform.forward, out var hitInfo, 100f) && canRotate)
			{
				if (hitInfo.transform.tag == "GWall")
				{
					if (Vector3.up != hitInfo.normal && Input.GetKey(KeyCode.Mouse0))
					{
						weaponAudioSource.pitch = 0.9f;
						weaponAudioSource.PlayOneShot(gGunSound, 0.15f);
						rotTarget = hitInfo.normal;
						if (Vector3.Angle(base.transform.up, rotTarget) > 100f)
						{
							if (Vector3.Angle(base.transform.forward, GameController.sharedGameController.transform.forward) > 91f)
							{
								target.eulerAngles = new Vector3(0f, 0f, 0f);
							}
							else
							{
								target.eulerAngles = new Vector3(0f, 360f, 0f);
							}
						}
						SetRot();
						canRotate = false;
						changeFov = true;
					}
					gravitySelectorObj.SetActive(value: true);
				}
				else
				{
					gravitySelectorObj.SetActive(value: false);
				}
			}
			else
			{
				gravitySelectorObj.SetActive(value: false);
			}
		}
		else
		{
			gravitySelectorObj.SetActive(value: false);
		}
		if (rotating)
		{
			Rotate();
		}
		if (changeFov)
		{
			ChangeFov();
		}
	}

	private void ChangeFov()
	{
		if (!fovDown)
		{
			camLerpTime += 0.08f;
			if (camLerpTime >= 1f)
			{
				fovDown = true;
			}
		}
		else
		{
			camLerpTime -= 0.08f;
			if (camLerpTime <= 0f)
			{
				changeFov = false;
				fovDown = false;
			}
		}
		camera.fieldOfView = Mathf.Lerp(iniFov, iniFov + 2f, camLerpTime);
		Time.timeScale = Mathf.Lerp(1f, 0.8f, camLerpTime);
	}

	private void Rotate()
	{
		_ = speed;
		_ = Time.deltaTime;
		sceneRotator.transform.rotation = Quaternion.Slerp(sceneRotator.transform.rotation, target.rotation, 0.12f);
		if (Vector3.Angle(sceneRotator.transform.up, target.transform.up) < 0.1f)
		{
			sceneRotator.transform.up = target.transform.up;
		}
		if (sceneRotator.transform.up == target.transform.up)
		{
			ResetRot();
			rotating = false;
			Debug.Log("MISMA ROTACION");
		}
	}

	private void SetRot()
	{
		sceneRotator.transform.position = base.transform.position;
		sceneRotator.transform.rotation = Quaternion.FromToRotation(Vector3.up, rotTarget);
		sceneObj.transform.SetParent(sceneRotator.transform);
		rotating = true;
	}

	private void ResetRot()
	{
		sceneObj.transform.parent = null;
		canRotate = true;
	}

	public void SetActivated(bool activated)
	{
		this.activated = activated;
	}
}
