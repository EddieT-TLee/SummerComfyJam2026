using UnityEngine;

public class Mole : MonoBehaviour
{
    private GameObject holeToAppearAt;
    private GameObject appearPoint;

    private SpriteRenderer spriteRenderer;
    private GameObject spriteMask;

    private bool readyToJump = false;
    private bool isJumping = false;

    private float timeVisible = 0f;
    private float timeTillAppearance = 0f;

    private const float MIN_WAIT_TIME = 0.4f;
    private const float MAX_WAIT_TIME = 4.0f;

    private const float VISIBLE_TIME = 0.7f;

    void Start()
    {
        spriteMask = transform.GetChild(0).gameObject;
        spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Try to find a hole to jump
        if (!readyToJump && !isJumping)
        {
            GameObject hole = WhackAMoleManager.instance.GetRandomHole();

            if (hole != null)
            {
                holeToAppearAt = hole;
                appearPoint = holeToAppearAt.transform.GetChild(0).gameObject;
                readyToJump = true;
                timeTillAppearance = Random.Range(MIN_WAIT_TIME, MAX_WAIT_TIME);
            }
        }

        if (readyToJump && !isJumping)
        {
            timeTillAppearance -= Time.deltaTime;
            if (timeTillAppearance <= 0f)
            {
                readyToJump = false;
                isJumping = true;
                spriteRenderer.enabled = true;
                timeVisible = VISIBLE_TIME;
            }
        }

        if (isJumping)
        {
            timeVisible -= Time.deltaTime;
            if (timeVisible <= 0f)
            {
                isJumping = false;
                WhackAMoleManager.instance.MarkHoleAvailable(holeToAppearAt);
                holeToAppearAt = null;
                spriteRenderer.enabled = false;
                return;
            }

            float spriteHeight = spriteRenderer.bounds.size.y;
            Debug.Log(spriteHeight);
            
            Vector3 startPosition = appearPoint.transform.position - new Vector3(0, spriteHeight/2, 0);
            float num = -timeVisible * (timeVisible - VISIBLE_TIME);
            float den = (VISIBLE_TIME / 2) * (VISIBLE_TIME / 2);
            float offset = spriteHeight * (num / den);

            transform.position = startPosition + new Vector3(0, offset, 0);
            spriteMask.transform.position = appearPoint.transform.position + new Vector3(0, spriteHeight/2, 0);
        }
    }
}
