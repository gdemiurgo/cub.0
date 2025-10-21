using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class GameController : MonoBehaviour
{
	public static GameController sharedGameController;

	[SerializeField]
	private int locksNumber;

	public bool unlocked1;

	public bool unlocked2;

	public bool unlocked3;

	[SerializeField]
	private bool enableGravityMode = true;

	[SerializeField]
	private bool enableFire = true;

    [SerializeField]
    private float letterPause = 0.03f;

	[SerializeField]
	private Color[] crossHairColors;

	private GameObject crossHair;

	private GameObject playerObj;

	private GameObject startCube;

	private GameObject blackBack;

	private GameObject sceneTittleObj;

	private GameObject capsuleSceneTextObj;

	private GameObject playerIniTextObj;

	private Text sceneTittleText;

	private TextMesh capsuleSceneText;

	private Text playerIniText;

	private string iniMessage;

	public bool gameOver;

	public bool mainMenu;

	private bool startedGame;

	private bool tittleIni;

	private bool startCapsuleClosed;

	private bool allUnlocked;

	private StartCube startCubeController;

	private PlayerController playerController;

	private CanvasGroup blackCanvasGroup;

	private Image crossHairImage;

	private int unlockedCount;

	private void Awake()
	{
		sharedGameController = this;
	}

	private void Start()
	{
		playerObj = GameObject.FindGameObjectWithTag("Player");
		playerController = playerObj.GetComponent<PlayerController>();
		crossHair = GameObject.FindGameObjectWithTag("CrossHair");
		crossHairImage = crossHair.GetComponent<Image>();
		playerController.Deactivate();
		startCube = GameObject.FindGameObjectWithTag("StartCube");
		startCubeController = startCube.GetComponent<StartCube>();
		Cursor.visible = false;
		SetStartTittle();
	}

	private void Update()
	{
		if (startedGame && !gameOver && blackCanvasGroup.alpha > 0f)
		{
			blackCanvasGroup.alpha -= 0.1f;
		}
		if (gameOver && blackCanvasGroup.alpha < 1f)
		{
			if (!mainMenu)
			{
				blackCanvasGroup.alpha += 0.1f;
			}
			else
			{
				blackCanvasGroup.alpha += 0.005f;
			}
		}
		if (!tittleIni)
		{
			if (Input.anyKeyDown)
			{
				tittleIni = true;
			}
		}
		else if (Input.anyKeyDown)
		{
			StartedGame();
		}
	}

	private void SetStartTittle()
	{
		AudioController.instance.StartSound();
		blackBack = GameObject.FindGameObjectWithTag("BlackBack");
		blackCanvasGroup = blackBack.GetComponent<CanvasGroup>();
		blackCanvasGroup.alpha = 1f;
		sceneTittleObj = GameObject.FindGameObjectWithTag("SceneText");
		sceneTittleObj.SetActive(value: true);
		sceneTittleText = sceneTittleObj.GetComponent<Text>();
		capsuleSceneTextObj = GameObject.FindGameObjectWithTag("CapsuleSceneText");
		capsuleSceneText = capsuleSceneTextObj.GetComponent<TextMesh>();
		sceneTittleText.text = "CUB." + SceneManager.GetActiveScene().buildIndex;
		capsuleSceneText.text = sceneTittleText.text;
		playerIniTextObj = GameObject.FindGameObjectWithTag("PlayerIniText");
		playerIniText = playerIniTextObj.GetComponent<Text>();
		iniMessage = playerIniText.text;
		playerIniText.text = "";
		StartCoroutine(TypeIniText());
	}

	public void StartedGame()
	{
		if (!startedGame && tittleIni)
		{
			startCubeController.OpenCapsule();
			blackCanvasGroup.alpha = 1f;
			sceneTittleObj.SetActive(value: false);
			playerIniTextObj.SetActive(value: false);
			playerController.Activate();
			startedGame = true;
		}
	}

	public void Restart()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void GameOver(bool restartScene, float restartTime)
	{
		if (gameOver)
		{
			return;
		}
		if (!mainMenu)
		{
			playerController.Deactivate();
			if (restartScene)
			{
				Invoke("RestartScene", restartTime);
				GameManager.instance.NewClone();
			}
			else
			{
				Invoke("NextScene", restartTime);
			}
		}
		else
		{
			Invoke("GoToMainMenu", restartTime);
		}
		gameOver = true;
	}

	public void MainMenu()
	{
		RigidbodyFirstPersonController component = playerObj.GetComponent<RigidbodyFirstPersonController>();
		component.movementSettings.ForwardSpeed = 0f;
		component.movementSettings.BackwardSpeed = 0f;
		component.movementSettings.StrafeSpeed = 0f;
		component.movementSettings.JumpForce = 0f;
		mainMenu = true;
	}

	public void GoToMainMenu()
	{
		Object.Destroy(GameManager.instance);
		SceneManager.LoadScene(0);
	}

	public bool IsGameOver()
	{
		return gameOver;
	}

	public int LocksNumber()
	{
		return locksNumber;
	}

	public void EnableCrossHair()
	{
		crossHairImage.color = crossHairColors[0];
	}

	public void DisableCrossHair()
	{
		crossHairImage.color = crossHairColors[1];
	}

	public void SetStartCapsuleClosed(bool startCapsuleClosed)
	{
		this.startCapsuleClosed = startCapsuleClosed;
		if (enableGravityMode)
		{
			playerController.GravityControlActivate();
		}
		if (enableFire)
		{
			playerController.FireActivate();
		}
	}

	public bool GetStartCapsuleClosed()
	{
		return startCapsuleClosed;
	}

	public void Unlock(int lockNumber)
	{
		switch (lockNumber)
		{
		case 1:
			unlocked1 = true;
			break;
		case 2:
			unlocked2 = true;
			break;
		case 3:
			unlocked3 = true;
			break;
		}
		startCubeController.SetScreenUI(unlocked1, unlocked2, unlocked3);
		unlockedCount++;
		CheckAllUnlocked();
	}

	public void CheckAllUnlocked()
	{
		if (unlockedCount == locksNumber)
		{
			allUnlocked = true;
		}
	}

	public bool AllUnlocked()
	{
		return allUnlocked;
	}

	public bool GravityEnabled()
	{
		return enableGravityMode;
	}

	public void TheManShooted()
	{
		playerIniText.text = "[ mouse . 2 ]   gun . disabled";
		RectTransform component = playerIniTextObj.GetComponent<RectTransform>();
		component.anchoredPosition = new Vector2(component.anchoredPosition.x, 50f);
		playerIniTextObj.SetActive(value: true);
		Invoke("DisableIniText", 5f);
	}

	private void DisableIniText()
	{
		playerIniTextObj.SetActive(value: false);
	}

	private void RestartScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	private void NextScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

	private IEnumerator TypeIniText()
	{
		char[] array = iniMessage.ToCharArray();
		foreach (char c in array)
		{
			if (!tittleIni)
			{
				AudioController.instance.TypeSound();
				playerIniText.text += c;
				yield return 0;
				yield return new WaitForSeconds(letterPause);
				continue;
			}
			playerIniText.text = iniMessage;
			break;
		}
		if (iniMessage != "")
		{
			AudioController.instance.EndTypeSound();
		}
		tittleIni = true;
	}
}
