using System.Collections;
using TMPro;
using UnityEngine;

public class BallGameSingleton : MonoBehaviour
{
    public static BallGameSingleton instance;

    [Header("Ball Game UI")]
    public GameObject WinPanel;
    public TMP_Text WinText;
    public GameObject InstructionsPanel;
    
    [Header("Bottles for Game")]
    public Bottle[] bottles;
 
    [Header("Ball Manager")]
    public TMP_Text ballsLeftText;
    public GameObject ballPrefab;
    public Vector3 spawnPosition = new Vector3(-5.5f, -2.2f, 0);
    public float respawnDelay;
    public int balls;


    public bool gameWon { get; private set; }

    private int bottlesOut;
    private GameObject currentBall;
  

    void Awake()
    {
        instance = this;
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
        if (gameWon) return;
        bottlesOut++;

        if (bottlesOut >= bottles.Length)
        {
            gameWon = true;
            GameOver("You Win!!!");
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
           GameOver("YOU LOSE!!!");
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
    
    
    
}
