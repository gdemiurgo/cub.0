using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController sharedGameController;

    [SerializeField] private int locksNumber;
    [SerializeField] private bool unlocked1, unlocked2, unlocked3;

    [SerializeField] private bool enableGravityMode = true;
    [SerializeField] private bool enableFire = true;
    [SerializeField] private float letterPause = 0.2f;
    [SerializeField] private Color[] crossHairColors;

    GameObject crossHair;
    GameObject playerObj;
    GameObject startCube;
    GameObject blackBack;
    GameObject sceneTittleObj;
    GameObject capsuleSceneTextObj;
    GameObject playerIniTextObj;
    Text sceneTittleText;
    TextMesh capsuleSceneText;
    Text playerIniText;
    string iniMessage;
    bool gameOver;
    bool startedGame;
    bool tittleIni;
    bool startCapsuleClosed;
    bool allUnlocked;
    StartCube startCubeController;
    PlayerController playerController;
    CanvasGroup blackCanvasGroup;
    Image crossHairImage;
    private int unlockedCount;

    // Start is called before the first frame update
    void Start()
    {
        sharedGameController = this;

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

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }

        if(startedGame && !gameOver)
        {
            if (blackCanvasGroup.alpha > 0)
            {
                blackCanvasGroup.alpha -= 0.1f;
            }
        }

        if (gameOver)
        {
            if (blackCanvasGroup.alpha < 1)
            {
                blackCanvasGroup.alpha += 0.1f;
            }
        }

        if(!tittleIni)
        {
            if (Input.anyKeyDown)
            {
                tittleIni = true;
            }
        }
        else
        {
            if(Input.anyKeyDown)
            {
                StartedGame();
            }
        }
    }

    void SetStartTittle()
    {
        blackBack = GameObject.FindGameObjectWithTag("BlackBack");
        blackCanvasGroup = blackBack.GetComponent<CanvasGroup>();
        blackCanvasGroup.alpha = 1;

        sceneTittleObj = GameObject.FindGameObjectWithTag("SceneText");
        sceneTittleObj.SetActive(true);
        sceneTittleText = sceneTittleObj.GetComponent<Text>();

        capsuleSceneTextObj = GameObject.FindGameObjectWithTag("CapsuleSceneText");
        capsuleSceneText = capsuleSceneTextObj.GetComponent<TextMesh>();

        sceneTittleText.text = "C.U.B." + SceneManager.GetActiveScene().buildIndex;
        capsuleSceneText.text = sceneTittleText.text;

        playerIniTextObj = GameObject.FindGameObjectWithTag("PlayerIniText");
        playerIniText = playerIniTextObj.GetComponent<Text>();

        iniMessage = playerIniText.text;
        playerIniText.text = "";

        StartCoroutine(TypeIniText());
    }

    public void StartedGame()
    {
        if(!startedGame && tittleIni)
        {
            startCubeController.OpenCapsule();

            blackCanvasGroup.alpha = 1;
            sceneTittleObj.SetActive(false);
            playerIniTextObj.SetActive(false);

            playerController.Activate();

            /*if (enableGravityMode)
            {
                playerController.GravityControlActivate();
            }

            if (enableFire)
            {
                playerController.FireActivate();
            }*/

            startedGame = true;
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GameOver(bool restartScene, float restartTime)
    {
        gameOver = true;
        playerController.Deactivate();

        if (restartScene) Invoke("RestartScene", restartTime);
    }

    public int LocksNumber()
    {
        return locksNumber;
    }

    public void EnableCrossHair()
    {
        crossHairImage.color = crossHairColors[0];
        //crossHair.SetActive(true);
    }

    public void DisableCrossHair()
    {
        crossHairImage.color = crossHairColors[1];
        //crossHair.SetActive(false);
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

    //LOCKS

    public void Unlock(int lockNumber)
    {
        switch(lockNumber)
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
        if(unlockedCount == locksNumber)
        {
            allUnlocked = true;
        }
    }

    public bool AllUnlocked()
    {
        return allUnlocked;
    }

    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private IEnumerator TypeIniText()
    {
        foreach (char letter in iniMessage.ToCharArray())
        {
            if(!tittleIni)
            {
                playerIniText.text += letter;
                yield return 0;
                yield return new WaitForSeconds(letterPause);
            }
            else
            {
                playerIniText.text = iniMessage;
                break;
            }
        }
        tittleIni = true;
    }
}
