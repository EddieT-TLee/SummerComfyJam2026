using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class WhackAMoleManager : MonoBehaviour
{
    [SerializeField]
    private GameObject holesParent;
    private List<Transform> holes;
    
    [SerializeField]
    private GameObject mole;
    private Collider2D moleCollider;

    [SerializeField]
    private GameObject scoreText;
    private TextMeshProUGUI scoreTextMesh;
    [SerializeField]
    private GameObject timeText;
    private TextMeshProUGUI timeTextMesh;

    private Transform holeToAppear;

    private Camera cam;
    private Mouse mouse;

    private int score = 0;
    private const int SCORE_PER_HIT = 100;
    private const int SCORE_PER_MISS = 100;
    private const int GOAL_SCORE = 1000;

    private float totalTimeRemaining = 0f;
    private float timeVisible = 0f;
    private float timeTillAppearance;

    private bool isVisible;

    private const float TIME_LIMIT = 30f;
    private const float MIN_WAIT_TIME = 0.4f;
    private const float MAX_WAIT_TIME = 1.2f;
    private const float VISIBLE_TIME = 0.5f;

    private const float MOLE_JUMP_HEIGHT = 3f;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Start()
    {
        moleCollider = mole.GetComponent<Collider2D>();
        
        scoreTextMesh = scoreText.GetComponent<TextMeshProUGUI>();
        timeTextMesh = timeText.GetComponent<TextMeshProUGUI>();

        UpdateScore(0);
        UpdateTime(TIME_LIMIT);

        holes = new List<Transform>();

        foreach (Transform hole in holesParent.transform)
        {
            holes.Add(hole);
        }

        mole.SetActive(false);
        isVisible = false;
        ChooseNewHole();
    }

    private void Update()
    {
        UpdateTime(totalTimeRemaining - Time.deltaTime);

        // Reached time limit, stop minigame
        if (totalTimeRemaining <= 0)
        {
            UpdateTime(0);
        };
        
        timeTillAppearance -= Time.deltaTime;

        // Mole should appear, make mole visible and begin jumping from hole
        if (timeTillAppearance <= 0 && !isVisible)
        {
            mole.SetActive(true);
            isVisible = true;
        }

        // Move the mole so it jumps from the hole and back
        if (isVisible)
        {
            timeVisible += Time.deltaTime;
            if (timeVisible >= VISIBLE_TIME)
            {
                mole.SetActive(false);
                isVisible = false;
                timeVisible = 0f;
                ChooseNewHole();
                return;
            }

            mole.transform.position = holeToAppear.position + new Vector3(0, -timeVisible * (timeVisible - VISIBLE_TIME) * MOLE_JUMP_HEIGHT, 0);
        }

        // Check if player hit the mole
        mouse = Mouse.current;

        if (mouse.leftButton.wasPressedThisFrame)
        {
            Ray ray = cam.ScreenPointToRay(mouse.position.ReadValue());

            RaycastHit2D hit = Physics2D.GetRayIntersection(ray);
            if (hit.collider && hit.collider == moleCollider)
            {
                Debug.Log("HIT");
                OnMoleHit();
            } else
            {
                Debug.Log("MISS");
                OnMoleMiss();
            }
            
        }
    }

    private void ChooseNewHole()
    {
        int index = Random.Range(0, holes.Count);
        holeToAppear = holes[index];
        timeTillAppearance = Random.Range(MIN_WAIT_TIME, MAX_WAIT_TIME);
    }

    private void OnMoleHit()
    {
        UpdateScore(score + SCORE_PER_HIT);
    }

    private void OnMoleMiss()
    {
        UpdateScore(score - SCORE_PER_MISS);
    }

    private void UpdateScore(int newScore)
    {
        score = newScore;
        scoreTextMesh.SetText("SCORE: " + score.ToString());
    }

    private void UpdateTime(float newTime)
    {
        totalTimeRemaining = newTime;
        timeTextMesh.SetText("TIME REMAINING: " + totalTimeRemaining.ToString());
    }
}
