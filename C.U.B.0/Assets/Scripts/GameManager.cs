using UnityEngine;

using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	[SerializeField] private bool testing = false;

	public static GameManager instance;

	private int v1;

	private int v2;

	private void Awake()
	{
		if (instance != null)
		{
			Object.Destroy(base.gameObject);
		}
		else
		{
			instance = this;
			Object.DontDestroyOnLoad(base.gameObject);
		}
		IniGame();
	}

	private void Start()
	{
	}

	private void Update()
	{
		if (testing)
		{
            int currentIndex = SceneManager.GetActiveScene().buildIndex;
            int totalScenes = SceneManager.sceneCountInBuildSettings;

            if (Input.GetKeyDown(KeyCode.N))
            {
                if (currentIndex < totalScenes - 1)
                {
                    SceneManager.LoadScene(currentIndex + 1);
                }
                else
                {
                    Debug.Log("No hay escena siguiente (ya estás en la última).");
                }
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                if (currentIndex > 0)
                {
                    SceneManager.LoadScene(currentIndex - 1);
                }
                else
                {
                    Debug.Log("No hay escena anterior (ya estás en la primera).");
                }
            }
        }
	}

	private void IniGame()
	{
		v2 = 1;
	}

	public void NewClone()
	{
		v2++;
		if (v2 > 9)
		{
			if (v1 != 99)
			{
				v2 = 0;
				v1++;
			}
			else
			{
				v1 = 99;
				v2 = 9;
			}
		}
	}

	public int GetV1()
	{
		return v1;
	}

	public int GetV2()
	{
		return v2;
	}
}
