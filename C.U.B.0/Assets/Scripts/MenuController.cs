using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("VOICES SOUND EFFECT")]
    [SerializeField] private GameObject voicesObj;
    [SerializeField] private float xMovePosLimit;
    [SerializeField] private float yMovePosLimit;
    [SerializeField] private float smoothTime;
    [SerializeField] private float moveRate;
    [SerializeField] private AudioSource voicesAudioSource;
    [SerializeField] private AudioClip voicesSound;

    [Header("MUSIC")]
    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private AudioClip startSound;
    [SerializeField] private AudioClip musicSound;

    private Vector3 startVoicesPos, newPos;
    private Vector3 refVel = Vector3.zero;
    private float timer;
    private bool gameStarted;

    // Start is called before the first frame update
    void Start()
    {
        IniGame();
    }

    // Update is called once per frame
    void Update()
    {
        VoicesMovement();

        if (gameStarted) GameStartControl();
    }

    private void VoicesMovement()
    {
        voicesObj.transform.position = Vector3.SmoothDamp(voicesObj.transform.position, newPos, ref refVel, smoothTime);
        MoveRateControl();
    }

    private void RandomizePos()
    {
        float randX = Random.Range(startVoicesPos.x - xMovePosLimit, startVoicesPos.x + xMovePosLimit);
        float randY = Random.Range(startVoicesPos.y - yMovePosLimit, startVoicesPos.y + yMovePosLimit);

        newPos = new Vector3(randX, randY, 0);
    }

    private void MoveRateControl()
    {
        timer += Time.deltaTime;
        if(timer > moveRate)
        {
            RandomizePos();
            timer = 0;
        }
    }

    private void IniGame()
    {
        startVoicesPos = voicesObj.transform.position;

        voicesAudioSource.clip = voicesSound;
        voicesAudioSource.Play();

        musicAudioSource.PlayOneShot(startSound, 0.2f);
        musicAudioSource.clip = musicSound;
        musicAudioSource.Play();
    }

    public void StartGame()
    {
        Invoke("GoToNextScene", 2f);

        gameStarted = true;
    }

    private void GameStartControl()
    {
        if (voicesAudioSource.volume > 0) voicesAudioSource.volume -= 0.1f;
        if (musicAudioSource.volume > 0) musicAudioSource.volume -= 0.1f;

        if (canvasGroup.alpha > 0) canvasGroup.alpha -= 0.1f;
    }

    private void GoToNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
