using UnityEngine;

public class MenuExit : MonoBehaviour
{
	[SerializeField]
	private GameObject exitMenu;

	private bool opened;

	private void Start()
	{
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			OpenMenu();
		}
		if (opened)
		{
			if (Input.GetKeyDown(KeyCode.Y))
			{
				Time.timeScale = 1f;
				GameController.sharedGameController.GoToMainMenu();
			}
			if (Input.GetKeyDown(KeyCode.N))
			{
				CloseMenu();
			}
		}
	}

	private void OpenMenu()
	{
		Time.timeScale = 0f;
		exitMenu.SetActive(value: true);
		opened = true;
	}

	private void CloseMenu()
	{
		Time.timeScale = 1f;
		exitMenu.SetActive(value: false);
		opened = false;
	}
}
