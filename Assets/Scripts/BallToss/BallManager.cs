using System.Collections;
using TMPro;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    public static BallManager instance;
    
    public TMP_Text ballsLeftText;
    public GameObject ballPrefab;
    public Vector3 spawnPosition = new Vector3(-5.5f, -2.2f, 0);
    public float respawnDelay;
    public int balls;
    
    private GameObject currentBall;
    private bool isWaitingToRespawn;
 
    void Awake()
    {
        instance = this;
    }
 
    void Start()
    {
        UpdateBallUI();
        SpawnBall();
    }
 
    public void OnBallThrown()
    {
        if (isWaitingToRespawn) return;
        balls--;
        UpdateBallUI();
        StartCoroutine(RespawnAfterDelay());
    }
 
    IEnumerator RespawnAfterDelay()
    {
        isWaitingToRespawn = true;
        yield return new WaitForSeconds(respawnDelay);
 
        if (currentBall != null)
            Destroy(currentBall);
 
        if (balls > 0)
        {
            SpawnBall();
        }
 
        isWaitingToRespawn = false;
    }
 
    void SpawnBall()
    {
        currentBall = Instantiate(ballPrefab, spawnPosition, Quaternion.identity);
        UpdateBallUI();
    }
 
    void UpdateBallUI()
    {
        ballsLeftText.text = "Balls: " + balls;
    }
}
