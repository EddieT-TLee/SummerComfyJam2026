using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class WhackAMoleManager : MonoBehaviour
{
    // Singleton
    public static WhackAMoleManager instance { get; private set; }

    [SerializeField]
    private GameObject holesParent;
    private List<GameObject> holes;
    private List<bool> holeAvailability;
    private readonly object _holeLock = new object();

    [SerializeField]
    private Mole molePrefab;
    private string moleTag;
    [SerializeField]
    private int moleCount;

    private List<Mole> moles;

    [SerializeField]
    private GameObject scoreText;
    private TMP_Text scoreTextMesh;

    [SerializeField]
    private GameObject timeText;
    private TMP_Text timeTextMesh;


    [SerializeField]
    private GameObject instructionsScreen;
    [SerializeField]
    private Button startGameButton;

    [SerializeField]
    private GameObject endScreen;
    [SerializeField]
    private TMP_Text endScreenText;
    [SerializeField]
    private Button returnButton;

    private Camera cam;
    private Mouse mouse;

    private int score = 0;
    private const int SCORE_PER_HIT = 100;
    private const int GOAL_SCORE = 1000;

    public float totalTimeRemaining = 0f;
    private const float TIME_LIMIT = 30f;

    private bool paused = true;


    private void Awake()
    {
        // Setup singleton
        if (instance == null)
        {
            instance = this;
        }

        cam = Camera.main;
    }

    private void Start()
    {
        scoreTextMesh = scoreText.GetComponent<TMP_Text>();
        timeTextMesh = timeText.GetComponent<TMP_Text>();
        startGameButton.onClick.AddListener(StartGame);

        if (SceneLoader.instance != null)
        {
            returnButton.onClick.AddListener(SceneLoader.instance.ReturnToPreviousScene);
        }
        HideEndScreen();
        UpdateScore(0);
        UpdateTime(TIME_LIMIT);
    }

    private void Update()
    {
        if (paused) return;

        UpdateTime(totalTimeRemaining - Time.deltaTime);

        // Reached time limit, stop minigame
        if (totalTimeRemaining <= 0)
        {
            MinigameFinished();
        };

        // Check if player hit the mole
        mouse = Mouse.current;

        if (mouse.leftButton.wasPressedThisFrame)
        {
            Ray ray = cam.ScreenPointToRay(mouse.position.ReadValue());

            RaycastHit2D hit = Physics2D.GetRayIntersection(ray);
            
            if (hit.collider && hit.collider.CompareTag(moleTag))
            {
                Mole mole = hit.collider.gameObject.GetComponent<Mole>();
                if (!mole.hit && mole.isJumping)
                {
                    mole.hit = true;
                    OnMoleHit();
                }
            }
        }
    }

    private void StartGame()
    {
        paused = false;
        instructionsScreen.SetActive(false);

        // Setup moles
        moleTag = molePrefab.tag;
        moles = new List<Mole>();

        for (int i = 0; i < moleCount; i++)
        {
            Mole m = Instantiate(molePrefab);
            moles.Add(m);
        }

        holes = new List<GameObject>();

        foreach (Transform hole in holesParent.transform)
        {
            holes.Add(hole.gameObject);
        }

        holeAvailability = new List<bool>(Enumerable.Repeat(true, holes.Count));
    }

    public GameObject GetRandomHole()
    {
        lock (_holeLock)
        {
            List<int> availableIndices = new List<int>();

            for (int i = 0; i < holes.Count; i++)
            {
                if (holeAvailability[i])
                {
                    availableIndices.Add(i);
                }
            }

            if (availableIndices.Count == 0) return null;

            int index = Random.Range(0, availableIndices.Count);
            int holeIndex = availableIndices[index];

            holeAvailability[holeIndex] = false;

            return holes[holeIndex];
        }
    }

    public void MarkHoleAvailable(GameObject hole)
    {
        holeAvailability[holes.IndexOf(hole)] = true;
    }

    public void OnMoleHit()
    {
        UpdateScore(score + SCORE_PER_HIT);
    }

    private void MinigameFinished()
    {
        foreach (Mole mole in moles)
        {
            mole.gameObject.SetActive(false);
        }

        UpdateTime(0);
        paused = true;

        if (score >= GOAL_SCORE)
        {
            if (QuestController.instance != null)
            {
                QuestController.instance.CompleteQuest("Whack-A-Mole");
            }
            ShowEndScreen("YOU WIN!");
        } else
        {
            ShowEndScreen("Our boardwalk will now run rampant with moles. I blame you");
        }
    }

    private void ShowEndScreen(string message)
    {
        endScreen.SetActive(true);
        endScreenText.text = message;
    }
    
    private void HideEndScreen()
    {
        endScreen.SetActive(false);
    }

    private void UpdateScore(int newScore)
    {
        score = newScore;
        scoreTextMesh.SetText(score.ToString());
    }

    private void UpdateTime(float newTime)
    {
        totalTimeRemaining = newTime;
        int minutes = Mathf.FloorToInt(totalTimeRemaining / 60f);
        int seconds = Mathf.FloorToInt(totalTimeRemaining % 60f);
        timeTextMesh.SetText(string.Format("{0:00}:{1:00}", minutes, seconds));
    }
}
