using UnityEngine;

public class StartCubeDoorClosedControl : MonoBehaviour
{
	[SerializeField]
	private StartCube startCube;

	public void DoorClosed()
	{
		startCube.DoorClosed();
	}
}
