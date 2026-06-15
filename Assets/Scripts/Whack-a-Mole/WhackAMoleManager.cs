using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

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

    private Camera cam;
    private Mouse mouse;

    private int score = 0;
    private const int SCORE_PER_HIT = 100;
    private const int SCORE_PER_MISS = 100;
    private const int GOAL_SCORE = 1000;

    public float totalTimeRemaining = 0f;
    private const float TIME_LIMIT = 30f;

    private bool gameFinished = false;


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
        moleTag = molePrefab.tag;
        moles = new List<Mole>();

        for (int i = 0; i < moleCount; i++)
        {
            Mole m = Instantiate(molePrefab);
            moles.Add(m);
        }
        
        scoreTextMesh = scoreText.GetComponent<TMP_Text>();
        timeTextMesh = timeText.GetComponent<TMP_Text>();

        UpdateScore(0);
        UpdateTime(TIME_LIMIT);

        holes = new List<GameObject>();

        foreach (Transform hole in holesParent.transform)
        {
            holes.Add(hole.gameObject);
        }

        holeAvailability = new List<bool>(Enumerable.Repeat(true, holes.Count));
    }

    private void Update()
    {
        if (gameFinished) return;

        UpdateTime(totalTimeRemaining - Time.deltaTime);

        // Reached time limit, stop minigame
        if (totalTimeRemaining <= 0)
        {
            foreach(Mole mole in moles)
            {
                mole.gameObject.SetActive(false);
            }

            UpdateTime(0);
            gameFinished = true;
        };

        // Check if player hit the mole
        mouse = Mouse.current;

        if (mouse.leftButton.wasPressedThisFrame)
        {
            Ray ray = cam.ScreenPointToRay(mouse.position.ReadValue());

            RaycastHit2D hit = Physics2D.GetRayIntersection(ray);
            
            if (hit.collider && hit.collider.CompareTag(moleTag))
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

    public GameObject GetRandomHole()
    {
        lock (_holeLock)
        {
            List<GameObject> holeBag = new List<GameObject>();

            for (int i = 0; i < holes.Count; i++)
            {
                if (holeAvailability[i])
                {
                    holeBag.Add(holes[i]);
                }
            }

            if (holeBag.Count == 0) return null;

            int index = Random.Range(0, holeBag.Count);
            holeAvailability[index] = false;

            return holeBag[index];
        }
    }

    public void MarkHoleAvailable(GameObject hole)
    {
        int index = holes.IndexOf(hole);
        if (index != -1)
        {
            holeAvailability[index] = true;
        }
    }

    public void OnMoleHit()
    {
        UpdateScore(score + SCORE_PER_HIT);
    }

    public void OnMoleMiss()
    {
        UpdateScore(score - SCORE_PER_MISS);
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
