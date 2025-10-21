using UnityEngine;

public class StartCubeDoorPlayerDetector : MonoBehaviour
{
	[SerializeField]
	private StartCube startCube;

	private bool checkIfOpen;

	private GameObject checkObj;

	private void Update()
	{
		if (checkIfOpen && checkObj.transform.up == base.transform.up && !startCube.Opened())
		{
			startCube.OpenCapsule();
			checkObj.GetComponent<PlayerController>().GravityControlDeactivate();
			checkIfOpen = false;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (GameController.sharedGameController.AllUnlocked() && other.CompareTag("Player"))
		{
			checkObj = other.gameObject;
			checkIfOpen = true;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (GameController.sharedGameController.AllUnlocked() && other.CompareTag("Player") && startCube.Opened())
		{
			startCube.CloseCapsule();
			if (GameController.sharedGameController.GravityEnabled())
			{
				other.GetComponent<PlayerController>().GravityControlActivate();
			}
			checkIfOpen = false;
		}
	}
}
