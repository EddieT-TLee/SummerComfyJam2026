
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class FishingMeter : MonoBehaviour
{
    private Image backgroundImage;

    [SerializeField]
    private GameObject safeZone;
    private RectTransform safeZoneRect;

    [SerializeField]
    private GameObject indicator;
    private RectTransform indicatorRect;

    private const float EPSILON = 0.001f;

    private float safeZoneLowerBound;
    private float safeZoneUpperBound;
    private float indicatorLowerBound;
    private float indicatorUpperBound;

    private float halfPoint;

    private Vector2 indicatorVelocity = Vector2.zero;
    private Vector2 gravity = new Vector3(0, -400.0f);

    private const float FORCE = 800f;
    private const float BOUNCE_COEFFICIENT = 0.6f;

    private const float MIN_WAIT_TIME = 1f;
    private const float MAX_WAIT_TIME = 3f;
    private float timeTillMove;

    private const float SAFE_ZONE_SPEED = 500f;
    private Vector2 moveTo;
    private bool moving = false;

    public bool zonesOverlapping = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        backgroundImage = GetComponent<Image>();
        safeZoneRect = safeZone.GetComponent<RectTransform>();
        indicatorRect = indicator.GetComponent<RectTransform>();

        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)transform);

        float backgroundWidth = backgroundImage.rectTransform.rect.width;
        float backgroundHeight = backgroundImage.rectTransform.rect.height;
        
        Vector4 borders = backgroundImage.sprite.border;
        float borderPixelsPerUnit = Mathf.Max(backgroundImage.pixelsPerUnit * backgroundImage.pixelsPerUnitMultiplier, Mathf.Epsilon);

        borders /= borderPixelsPerUnit;
        
        float safeZoneHeight = safeZoneRect.rect.height;
        float indicatorHeight = indicatorRect.rect.height;

        indicatorLowerBound = -backgroundHeight / 2f + borders.y + indicatorHeight / 2f;
        indicatorUpperBound = backgroundHeight / 2f - borders.w - indicatorHeight / 2f;
        safeZoneLowerBound = -backgroundHeight / 2f + borders.y + safeZoneHeight / 2f;
        safeZoneUpperBound = backgroundHeight / 2f - borders.w - safeZoneHeight / 2f;

        indicatorRect.sizeDelta = new Vector2(backgroundWidth - borders.x - borders.z, indicatorRect.sizeDelta.y);
        safeZoneRect.sizeDelta = new Vector2(backgroundWidth - borders.x - borders.z, safeZoneRect.sizeDelta.y);

        halfPoint = (safeZoneUpperBound + safeZoneLowerBound) / 2f;

        ResetFishingMeter();
    }

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current != null)
        {
            if (Mouse.current.leftButton.isPressed)
            {
                indicatorVelocity += new Vector2(0, FORCE) * Time.deltaTime;
            }
        }

        indicatorVelocity += gravity * Time.deltaTime;
        indicatorRect.anchoredPosition += (indicatorVelocity) * Time.deltaTime;

        CheckIndicatorCollisions();

        // Move the safe zone around to mess with the player
        if (timeTillMove <= 0)
        {
            if (!moving)
            {
                UpdateMoveTo();
                moving = true;
            }

            safeZoneRect.anchoredPosition = Vector2.MoveTowards(safeZoneRect.anchoredPosition, moveTo, SAFE_ZONE_SPEED * Time.deltaTime);
            
            if (Vector2.Distance(safeZoneRect.anchoredPosition, moveTo) <= EPSILON)
            {
                timeTillMove = Random.Range(MIN_WAIT_TIME, MAX_WAIT_TIME);
                moving = false;
            }
        } else
        {
            timeTillMove -= Time.deltaTime;
        }

        UpdateOverlapping();
    }

    private void CheckIndicatorCollisions()
    {
        if (indicatorRect.anchoredPosition.y <= indicatorLowerBound)
        {
            indicatorRect.anchoredPosition = new Vector2(indicatorRect.anchoredPosition.x, indicatorLowerBound);
            indicatorVelocity.y *= -BOUNCE_COEFFICIENT;
        }
        else if (indicatorRect.anchoredPosition.y >= indicatorUpperBound)
        {
            indicatorRect.anchoredPosition = new Vector2(indicatorRect.anchoredPosition.x, indicatorUpperBound);
            indicatorVelocity.y *= -BOUNCE_COEFFICIENT;
        }
    }

    private void UpdateMoveTo()
    {
        if (safeZoneRect.anchoredPosition.y >= halfPoint)
        {
            moveTo = new Vector2
            (
                safeZoneRect.anchoredPosition.x,
                Random.Range(safeZoneLowerBound, halfPoint)
            );
        }
        else
        {
            moveTo = new Vector2
            (
                safeZoneRect.anchoredPosition.x,
                Random.Range(halfPoint, safeZoneUpperBound)
            );
        }
    }

    private void UpdateOverlapping()
    {
        float safeMin = safeZoneRect.anchoredPosition.y - safeZoneRect.rect.height / 2f;
        float safeMax = safeZoneRect.anchoredPosition.y + safeZoneRect.rect.height / 2f;

        float indicatorMin = indicatorRect.anchoredPosition.y - indicatorRect.rect.height / 2f;
        float indicatorMax = indicatorRect.anchoredPosition.y + indicatorRect.rect.height / 2f;

        zonesOverlapping = indicatorMax > safeMin && indicatorMin < safeMax;
    }

    public void ResetFishingMeter()
    {
        safeZoneRect.anchoredPosition = new Vector2(safeZoneRect.anchoredPosition.x, Random.Range(safeZoneLowerBound, safeZoneUpperBound));
        indicatorRect.anchoredPosition = new Vector2(indicatorRect.anchoredPosition.x, 0);
        timeTillMove = Random.Range(MIN_WAIT_TIME, MAX_WAIT_TIME);
        moving = false;
    }
}
