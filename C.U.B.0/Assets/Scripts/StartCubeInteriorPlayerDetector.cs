using UnityEngine;

public class StartCubeInteriorPlayerDetector : MonoBehaviour
{
	[SerializeField]
	private StartCube startCube;

	private void OnTriggerEnter(Collider other)
	{
		if (GameController.sharedGameController.AllUnlocked() && other.CompareTag("Player"))
		{
			startCube.CloseCapsule();
			GameController.sharedGameController.GameOver(restartScene: false, 2f);
		}
	}
}
