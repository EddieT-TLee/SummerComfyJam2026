using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class IceCreamStack : MonoBehaviour {
    [SerializeField] private Transform iceCreamScoopPrefab;
    [SerializeField] private List<Sprite> iceCreamScoopSprites;
    private List<Transform> iceCreamScoops = new List<Transform>();
    private int iceCreamScoopIndex = 0;

    [SerializeField] private Transform iceCreamCone;
    private int coneSortingOrder;

    [SerializeField] private int startingLives = 3;
    [SerializeField] private RectTransform livesCounter;
    [SerializeField] private Sprite lifeIconSprite;
    private Vector2 lifeIconSize = new Vector2(64f, 64f);

    private readonly List<Image> lifeIcons = new List<Image>();
    private int lives;
    private bool gameFailed;

    private Transform currentScoop = null;
    private Rigidbody2D currentRigidBody;
    private Camera mainCamera;

    private Vector2 scoopStartPosition = new Vector2(0f, 4f);

    private float scoopSpeed = 6f;
    private float scoopSpeedIncrement = 0.5f;
    private float scoopDirection = 1;
    private float xLimit = 5;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        mainCamera = Camera.main;
        startingLives = Mathf.Max(0, startingLives);
        lives = startingLives;
        RandomizeIceCreamOrder();
        coneSortingOrder = iceCreamCone.GetComponent<SpriteRenderer>().sortingOrder;
        CreateLifeIcons();
        UpdateLivesCounter();
        SpawnNewScoop();
    }

    void Update() {
        if (gameFailed) return;
      
        if (currentScoop) {
            float moveAmount = Time.deltaTime * scoopSpeed * scoopDirection;
            currentScoop.position += new Vector3(moveAmount, 0, 0);
            if (Mathf.Abs(currentScoop.position.x) > xLimit) {
                currentScoop.position = new Vector3(scoopDirection * xLimit, currentScoop.position.y, 0);
                scoopDirection = -scoopDirection;
            }

            if (Keyboard.current.spaceKey.wasPressedThisFrame)
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

    public void FailGame()
    {
        gameFailed = true;
        currentScoop = null;
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

        currentScoop.GetComponent<SpriteRenderer>().sprite = iceCreamScoopSprites[iceCreamScoopIndex];
        currentScoop.GetComponent<IceCream>()?.Initialize(iceCreamCone);
        iceCreamScoopIndex = (iceCreamScoopIndex + 1) % iceCreamScoopSprites.Count;

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

}

    
