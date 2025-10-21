using UnityEngine;

public class EndScreen : MonoBehaviour
{
	[SerializeField]
	private EndCube endCube;

	private bool canOpen;

	private bool opened;

	private void OnMouseDown()
	{
		if (canOpen && !opened)
		{
			endCube.OpenCapsule();
			opened = true;
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			canOpen = true;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player")
		{
			canOpen = false;
		}
	}
}
