using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BallGameSingleton : MonoBehaviour
{
    public static BallGameSingleton instance;

    [Header("Ball Game UI")] public GameObject WinPanel;
    public TMP_Text WinText;
    public Button ReturnButton;
    public GameObject InstructionsPanel;

    [Header("Bottles for Game")] public Bottle[] bottles;

    [Header("Ball Manager")] public TMP_Text ballsLeftText;
    public GameObject ballPrefab;
    public Vector3 spawnPosition = new Vector3(-5.5f, -2.2f, 0);
    public float respawnDelay;
    public int balls;
    private bool gameLost;


    public bool gameWon { get; private set; }

    private int bottlesOut;
    private GameObject currentBall;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject); // Make sure only one instance
        }

        ReturnButton.onClick.AddListener(ReturnToMainGame);
    }

    void Start()
    {
        UpdateBallUI();
        WinPanel.SetActive(false);
    }

    public void GameOver(string text)
    {
        WinText.text = text;
        WinPanel.SetActive(true);
    }

    public void StartGame()
    {
        InstructionsPanel.SetActive(false);
        SpawnBall();
    }

    //Bottle Manager
    public void OnBottleLeft()
    {
        if (gameWon || gameLost) return;
        bottlesOut++;

        if (bottlesOut >= bottles.Length)
        {
            gameWon = true;

            if (QuestController.instance != null)
            {
                QuestController.instance.CompleteQuest("Ball Toss");
            }

            GameOver("You Win... \n but at what cost. We no longer have bottles to store water, now " +
                     "our children are dehydrated and our plants don't have enough water to sustain themselves. " +
                     "We can longer survive the long summer");
        }
    }

    // Ball Manager
    public void OnBallThrown()
    {
        balls--;
        UpdateBallUI();
        StartCoroutine(RespawnAfterDelay());
    }

    IEnumerator RespawnAfterDelay()
    {
        yield return new WaitForSeconds(respawnDelay);

        if (currentBall != null)
            Destroy(currentBall);

        if (balls > 0)
        {
            SpawnBall();
        }
        else
        {
            gameLost = true;
            GameOver("YOU LOSE!!! Idiot couldn't even knock down 3 bottles");
        }
    }

    void SpawnBall()
    {
        currentBall = Instantiate(ballPrefab, spawnPosition, Quaternion.identity);
    }

    void UpdateBallUI()
    {
        ballsLeftText.text = "Balls: " + balls;
    }

    void ReturnToMainGame()
    {
        if (SceneLoader.instance != null)
        {
            SceneLoader.instance.ReturnToPreviousScene();
        }
    }
}