using UnityEngine;

public class Mole : MonoBehaviour
{
    private GameObject holeToAppearAt;
    private GameObject appearPoint;

    private SpriteRenderer spriteRenderer;
    private GameObject spriteMask;

    private bool readyToJump = false;
    
    public bool isJumping = false;
    public bool hit = false;

    private float timeVisibleRemaining = 0f;
    private float visibleTime = 0f;
    private float timeTillAppearance = 0f;

    private const float MIN_WAIT_TIME = 0.4f;
    private const float MAX_WAIT_TIME = 4.0f;

    private const float MIN_VISIBLE_TIME = 0.3f;
    private const float MAX_VISIBLE_TIME = 0.7f;

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
                visibleTime = Random.Range(MIN_VISIBLE_TIME, MAX_VISIBLE_TIME);
                timeVisibleRemaining = visibleTime;
            }
        }

        if (isJumping)
        {
            timeVisibleRemaining -= Time.deltaTime;
            if (timeVisibleRemaining <= 0f)
            {
                isJumping = false;
                WhackAMoleManager.instance.MarkHoleAvailable(holeToAppearAt);
                holeToAppearAt = null;
                spriteRenderer.enabled = false;
                hit = false;
                return;
            }

            float spriteHeight = spriteRenderer.bounds.size.y;
            
            Vector3 startPosition = appearPoint.transform.position - new Vector3(0, spriteHeight/2, 0);
            float num = -timeVisibleRemaining * (timeVisibleRemaining - visibleTime);
            float den = (visibleTime / 2) * (visibleTime / 2);
            float offset = spriteHeight * (num / den);

            transform.position = startPosition + new Vector3(0, offset, 0);
            spriteMask.transform.position = appearPoint.transform.position + new Vector3(0, spriteHeight/2, 0);
        }
    }
}
