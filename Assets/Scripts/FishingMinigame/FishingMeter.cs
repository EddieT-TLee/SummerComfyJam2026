
using System;
using UnityEngine;
using UnityEngine.InputSystem;

using Random = UnityEngine.Random;

public class FishingMeter : MonoBehaviour
{
    private SpriteRenderer backgroundRenderer;

    [SerializeField]
    private GameObject safeZone;
    private Collider2D safeZoneCollider;

    [SerializeField]
    private GameObject indicator;
    private Collider2D indicatorCollider;

    private const float EPSILON = 0.001f;

    private float safeZoneLowerBound;
    private float safeZoneUpperBound;
    private float indicatorLowerBound;
    private float indicatorUpperBound;

    private Vector3 indicatorVelocity;
    private Vector3 gravity = new Vector3(0, -4.0f, 0);

    private const float FORCE = 20f;
    private const float BOUNCE_COEFFICIENT = 0.6f;

    private const float MIN_WAIT_TIME = 1f;
    private const float MAX_WAIT_TIME = 3f;
    private float timeTillMove;

    [SerializeField]
    private const float SAFE_ZONE_SPEED = 5f;
    private Vector3 moveTo;
    private bool moving = false;

    public bool zonesOverlapping = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        backgroundRenderer = GetComponent<SpriteRenderer>();
        safeZoneCollider = safeZone.GetComponent<Collider2D>();
        indicatorCollider = indicator.GetComponent<Collider2D>();

        float backgroundHeight = backgroundRenderer.bounds.size.y;
        float backgroundBorderTop = backgroundRenderer.sprite.border.w / backgroundRenderer.sprite.pixelsPerUnit;
        float backgroundBorderBottom= backgroundRenderer.sprite.border.w / backgroundRenderer.sprite.pixelsPerUnit;
        float safeZoneHeight = safeZoneCollider.bounds.size.y;
        float indicatorHeight = indicatorCollider.bounds.size.y;

        indicatorLowerBound = transform.position.y - backgroundHeight/2 + backgroundBorderBottom + indicatorHeight/2;
        indicatorUpperBound = transform.position.y + backgroundHeight/2 - backgroundBorderTop - indicatorHeight/2;
        safeZoneLowerBound = transform.position.y - backgroundHeight/2 + backgroundBorderBottom + safeZoneHeight/2;
        safeZoneUpperBound = transform.position.y + backgroundHeight/2 - backgroundBorderTop - safeZoneHeight/2;

        timeTillMove = Random.Range(MIN_WAIT_TIME, MAX_WAIT_TIME);
    }

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current != null)
        {
            if (Mouse.current.leftButton.isPressed)
            {
                indicatorVelocity += new Vector3(0, FORCE, 0) * Time.deltaTime;
            }
        }

        indicatorVelocity += gravity * Time.deltaTime;
        indicator.transform.position += (indicatorVelocity) * Time.deltaTime;
        
        if (indicator.transform.position.y <= indicatorLowerBound)
        {
            indicator.transform.position = new Vector3(indicator.transform.position.x, indicatorLowerBound, indicator.transform.position.z);
            indicatorVelocity *= -BOUNCE_COEFFICIENT;
        } else if (indicator.transform.position.y >= indicatorUpperBound)
        {
            indicator.transform.position = new Vector3(indicator.transform.position.x, indicatorUpperBound, indicator.transform.position.z);
            indicatorVelocity *= -BOUNCE_COEFFICIENT;
        }

        // Move the safe zone around to mess with the player
        if (timeTillMove <= 0)
        {
            if (!moving)
            {
                float halfPoint = (safeZoneUpperBound + safeZoneLowerBound)/2f;
                if (safeZone.transform.position.y >= halfPoint)
                {
                    moveTo = new Vector3
                    (
                        safeZone.transform.position.x,
                        Random.Range(safeZoneLowerBound, halfPoint),
                        safeZone.transform.position.z
                    );
                } else
                {
                    moveTo = new Vector3
                    (
                        safeZone.transform.position.x,
                        Random.Range(halfPoint, safeZoneUpperBound),
                        safeZone.transform.position.z
                    );
                }
                    
                moving = true;
            }

            safeZone.transform.position = Vector3.Lerp(safeZone.transform.position, moveTo, SAFE_ZONE_SPEED * Time.deltaTime);

            if ((safeZone.transform.position - moveTo).magnitude <= EPSILON)
            {
                timeTillMove = Random.Range(MIN_WAIT_TIME, MAX_WAIT_TIME);
                moving = false;
            }
        } else
        {
            timeTillMove -= Time.deltaTime;
        }

        // Check if safe zone contains the indicator
        if (safeZoneCollider.bounds.Contains(indicatorCollider.bounds.min) && safeZoneCollider.bounds.Contains(indicatorCollider.bounds.max))
        {
            zonesOverlapping = true;
        } else
        {
            zonesOverlapping = false;
        }
    }
}
