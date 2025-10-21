using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
	[Header("UI")]
	[SerializeField]
	private CanvasGroup canvasGroup;

	[SerializeField]
	private string iniMessage;

	[SerializeField]
	private Text playerIniText;

	[SerializeField]
	private float letterPause;

	[Header("VOICES SOUND EFFECT")]
	[SerializeField]
	private GameObject voicesObj;

	[SerializeField]
	private float xMovePosLimit;

	[SerializeField]
	private float yMovePosLimit;

	[SerializeField]
	private float smoothTime;

	[SerializeField]
	private float moveRate;

	[SerializeField]
	private float voicesSoundChangeRate;

	[SerializeField]
	private AudioSource voicesAudioSource;

	[SerializeField]
	private AudioClip[] voicesSounds;

	[Header("MUSIC")]
	[SerializeField]
	private AudioSource musicAudioSource;

	[SerializeField]
	private AudioClip startSound;

	[SerializeField]
	private AudioClip musicSound;

	[SerializeField]
	private AudioClip startButtonSound;

	private Vector3 startVoicesPos;

	private Vector3 newPos;

	private Vector3 refVel = Vector3.zero;

	private float timer;

	private float voicesTimer;

	private bool gameStarted;

	private bool tittleIni;

	private void Start()
	{
		IniGame();
	}

	private void Update()
	{
		VoicesMovement();
		VoicesSound();
		if (Input.GetKeyDown(KeyCode.S))
		{
			StartGame();
		}
		if (gameStarted)
		{
			GameStartControl();
		}
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
	}

	private void VoicesMovement()
	{
		voicesObj.transform.position = Vector3.SmoothDamp(voicesObj.transform.position, newPos, ref refVel, smoothTime);
		MoveRateControl();
	}

	private void VoicesSound()
	{
		voicesTimer += Time.deltaTime;
		if (voicesTimer > voicesSoundChangeRate)
		{
			if (!voicesAudioSource.isPlaying)
			{
				int num = Random.Range(0, voicesSounds.Length);
				voicesAudioSource.volume = ((num < 5) ? 0.8f : 0.4f);
				voicesAudioSource.PlayOneShot(voicesSounds[num]);
				voicesSoundChangeRate = Random.Range(4, 8);
				voicesTimer = 0f;
			}
			else
			{
				voicesTimer = 0f;
			}
		}
	}

	private void RandomizePos()
	{
		float x = Random.Range(startVoicesPos.x - xMovePosLimit, startVoicesPos.x + xMovePosLimit);
		float y = Random.Range(startVoicesPos.y - yMovePosLimit, startVoicesPos.y + yMovePosLimit);
		newPos = new Vector3(x, y, 0f);
	}

	private void MoveRateControl()
	{
		timer += Time.deltaTime;
		if (timer > moveRate)
		{
			RandomizePos();
			timer = 0f;
		}
	}

	private void IniGame()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		StartCoroutine(TypeIniText());
		startVoicesPos = voicesObj.transform.position;
		voicesAudioSource.PlayOneShot(startSound, 0.1f);
		musicAudioSource.clip = musicSound;
		musicAudioSource.Play();
	}

	public void StartGame()
	{
		musicAudioSource.clip = startButtonSound;
		musicAudioSource.volume = 0.1f;
		musicAudioSource.loop = false;
		musicAudioSource.Play();
		Invoke("GoToNextScene", 2f);
		gameStarted = true;
	}

	private void GameStartControl()
	{
		if (voicesAudioSource.volume > 0f)
		{
			voicesAudioSource.volume -= 0.05f;
		}
		if (canvasGroup.alpha > 0f)
		{
			canvasGroup.alpha -= 0.05f;
		}
	}

	private void GoToNextScene()
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
		tittleIni = true;
	}
}
