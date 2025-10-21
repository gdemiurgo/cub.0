using UnityEngine;

public class GameManager : MonoBehaviour
{
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
