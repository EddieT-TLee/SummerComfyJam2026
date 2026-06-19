using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class IceCreamStack : MonoBehaviour {
    [Header("Ice Cream Game Object Stuff")]
    [SerializeField] private Transform iceCreamScoopPrefab;
    [SerializeField] private List<Sprite> iceCreamScoopSprites;
    [SerializeField] private Sprite cherrySprite;
    private List<Transform> iceCreamScoops = new List<Transform>();
    private int iceCreamScoopSpriteIndex = 0;

    [Header("Ice Cream Cone")]
    [SerializeField] private Transform iceCreamCone;
    private int coneSortingOrder;

    [Header("Ice Cream Cone Game Variables")]
    [SerializeField] private int startingLives = 3;
    [SerializeField] private RectTransform livesCounter;
    [SerializeField] private Sprite lifeIconSprite;

    [Header("UI Stuff")]
    [SerializeField] private GameObject instructionPanel;
    [SerializeField] private GameObject returnPanel;
    [SerializeField] private TMP_Text returnText;
    [SerializeField] private Button ReturnButton;

    
    private Vector2 lifeIconSize = new Vector2(64f, 64f);

    private readonly List<Image> lifeIcons = new List<Image>();
    private int lives;
    private bool gameFailed;
    private bool gameWon;

    private Transform currentScoop = null;
    private Rigidbody2D currentRigidBody;
    private Camera mainCamera;

    private Vector2 scoopStartPosition = new Vector2(0f, 4f);

    private float scoopSpeed = 6f;
    private float scoopSpeedIncrement = 0.5f;
    private float scoopDirection = 1;
    private float xLimit = 5;

    private const int WinningScoop = 11;
    private int scoopsStacked = 0;


    private void Awake()
    {
        ReturnButton.onClick.AddListener(ReturnToMainGame);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        mainCamera = Camera.main;
        startingLives = Mathf.Max(0, startingLives);
        lives = startingLives;
        RandomizeIceCreamOrder();
        coneSortingOrder = iceCreamCone.GetComponent<SpriteRenderer>().sortingOrder;
        CreateLifeIcons();
        UpdateLivesCounter();
        
        
    }

    void Update() {
        if (gameFailed || gameWon) return;
      
        if (currentScoop) {
            float moveAmount = Time.deltaTime * scoopSpeed * scoopDirection;
            currentScoop.position += new Vector3(moveAmount, 0, 0);
            if (Mathf.Abs(currentScoop.position.x) > xLimit) {
                currentScoop.position = new Vector3(scoopDirection * xLimit, currentScoop.position.y, 0);
                scoopDirection = -scoopDirection;
            }

            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                currentScoop = null;
                currentRigidBody.simulated = true;
                SpawnNewScoop();
            }
        }

        CheckForFallenScoops();
        UpdateScoopDepths();
    }

    public void ScoopFell()
    {
        if (lives <= 0) return;

        lives = Mathf.Max(0, lives - 1);
        UpdateLivesCounter();

        if (lives <= 0)
        {
            FailGame();
        }
    }
    
    public void WinGame()
    {
        gameWon = true;
        currentScoop = null;

        returnText.text = "I Guess this is good enough of a stack";
        returnPanel.SetActive(true);

        if (QuestController.instance != null)
        {
            QuestController.instance.CompleteQuest("Ice Cream Stack");
        }
    }

    public void FailGame()
    {
        gameFailed = true;
        currentScoop = null;
        
        returnText.text = "How?!?!?! Literally just put the Ice cream in the cone bro";
        returnPanel.SetActive(true);
    }

    private void RandomizeIceCreamOrder()
    {
        for (int i = iceCreamScoopSprites.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);

            (iceCreamScoopSprites[i], iceCreamScoopSprites[j]) = (iceCreamScoopSprites[j], iceCreamScoopSprites[i]);
        }
    }

    private void SpawnNewScoop()
    {
        
        currentScoop = Instantiate(iceCreamScoopPrefab);
        iceCreamScoops.Add(currentScoop);

        bool isCherry = scoopsStacked + 1 >= WinningScoop;
        SpriteRenderer sr = currentScoop.GetComponent<SpriteRenderer>();
        sr.sprite = isCherry ? cherrySprite : iceCreamScoopSprites[iceCreamScoopSpriteIndex];

        if (!isCherry)
            iceCreamScoopSpriteIndex = (iceCreamScoopSpriteIndex + 1) % iceCreamScoopSprites.Count;

        IceCream iceCream = currentScoop.GetComponent<IceCream>();
        iceCream?.Initialize(iceCreamCone, () =>
        {
            scoopsStacked++;
            if (iceCream.IsCherry)
                WinGame();
        });
        if (iceCream != null) iceCream.IsCherry = isCherry; // ← set the flag

        currentScoop.position = scoopStartPosition;
        currentRigidBody = currentScoop.GetComponent<Rigidbody2D>();

        scoopSpeed += scoopSpeedIncrement;
    }
    
    private void UpdateScoopDepths()
    {
        foreach (Transform scoop in iceCreamScoops)
        {
            if (!scoop) continue;

            int sortingOrder = Mathf.RoundToInt(scoop.transform.localPosition.y * 100);
            scoop.GetComponent<SpriteRenderer>().sortingOrder = Mathf.Max(coneSortingOrder, sortingOrder);
        }
    }

    private void CheckForFallenScoops()
    {
        float fallLimit = mainCamera.ViewportToWorldPoint(new Vector3(0f, 0f, Mathf.Abs(mainCamera.transform.position.z))).y - 1;

        for (int i = iceCreamScoops.Count - 1; i >= 0; i--)
        {
            Transform scoop = iceCreamScoops[i];

            if (!scoop)
            {
                iceCreamScoops.RemoveAt(i);
                continue;
            }

            if (scoop.position.y < fallLimit)
            {
                iceCreamScoops.RemoveAt(i);
                Destroy(scoop.gameObject);
                ScoopFell();
            }
        }
    }

    private void CreateLifeIcons()
    {
        for (int i = 0; i < startingLives; i++)
        {
            GameObject iconObject = new GameObject($"Life Icon {i + 1}", typeof(RectTransform), typeof(Image));
            iconObject.transform.SetParent(livesCounter, false);

            RectTransform iconTransform = iconObject.GetComponent<RectTransform>();
            iconTransform.sizeDelta = lifeIconSize;

            Image iconImage = iconObject.GetComponent<Image>();
            iconImage.sprite = lifeIconSprite;
            iconImage.preserveAspect = true;

            lifeIcons.Add(iconImage);
        }
    }

    private void UpdateLivesCounter()
    {
        for (int i = 0; i < lifeIcons.Count; i++)
        {
            lifeIcons[i].enabled = i < lives;
        }
    }
    
    
    public void StartGame()
    {
        instructionPanel.SetActive(false);
        SpawnNewScoop();
    }
    
    void ReturnToMainGame()
    {
        if (SceneLoader.instance != null)
        {
            SceneLoader.instance.ReturnToPreviousScene();
        }
    }

}

    
