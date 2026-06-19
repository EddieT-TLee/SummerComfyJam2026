using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TestOfStrength : MonoBehaviour
{
    [Header("Object References")]
    public GameObject powerBar;
    public Animator hammerAnimator;
    public Animator bellAnimator;
    public TMP_Text strengthText;
    public Transform bellPosition;
    public Transform sliderPosition;
    public GameObject instructionScreen;
    public GameObject winPanel;
    public Button returnButton;

    private SpriteRenderer powerBarRenderer;
    private SpriteRenderer spriteRenderer;
    private Vector3 centerPosition;
    private Vector3 offsetPosition;

    private bool moving = false;
    private bool sliderMoving = false;
    private bool startSliderAnimation = false;
    private float sliderVelocity;
    private float gravity;

    [Header("Time of Slider Animation")]
    public float sliderAnimationDuration = 0.5f;

    private float powerBarCenter;
    private float powerBarHeight;

    private float powerValue;
    
    private const float BOUNCE_COEFFICIENT = 0.6f;

    // Used for strength thingie top and bottom
    private float upperBound;
    private float lowerBound;

    void Start()
    {
        if (SceneLoader.instance != null)
        {
            returnButton.onClick.AddListener(SceneLoader.instance.ReturnToPreviousScene);
        }
        powerBarRenderer = powerBar.GetComponent<SpriteRenderer>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        centerPosition = powerBar.transform.position + new Vector3(spriteRenderer.bounds.size.x / 2 + -0.1f, 0, 0) +
                         new Vector3(powerBarRenderer.bounds.size.x / 2, 0, 0);
        powerBarHeight = powerBarRenderer.bounds.size.y;
        powerBarCenter = powerBar.transform.position.y;
        
        upperBound = bellPosition.position.y;  
        lowerBound = sliderPosition.position.y; 
    }

    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            offsetPosition = new Vector3(0, Mathf.Sin(Time.time * 3f), 0) * powerBarRenderer.bounds.size.y / 2;
            transform.position = centerPosition + offsetPosition;

            if (Mouse.current.leftButton.isPressed)
            {
                moving = false;

                powerValue = (1 - (Mathf.Abs(powerBarCenter - transform.position.y) / (powerBarHeight / 2))) * 100;

                StartCoroutine(HitHammer(powerValue));
            }
        }

        if (sliderMoving)
        {
            if (!startSliderAnimation)
            {
                startSliderAnimation = true;
                float height = (upperBound - lowerBound) * (powerValue / 100f);

                gravity = (8 * height)/(sliderAnimationDuration*sliderAnimationDuration);
                sliderVelocity = 4 * height/sliderAnimationDuration;
            }

            sliderVelocity -= gravity * Time.deltaTime;
            sliderPosition.position += new Vector3(0, sliderVelocity, 0) * Time.deltaTime;

            if (sliderPosition.position.y <= lowerBound)
            {
                sliderPosition.position = new Vector3
                (
                    sliderPosition.position.x,
                    lowerBound,
                    sliderPosition.position.z
                );
                sliderMoving = false;
                startSliderAnimation = false;
            }
        }
    }

    public void StartGame()
    {
        instructionScreen.SetActive(false);
        moving = true;
    }
    
    private IEnumerator HitHammer(float powerValue)
    {
        hammerAnimator.Play("Hammer");

        yield return null;

        while (hammerAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1f)
        {
            yield return null;
        }

        sliderMoving = true;

        while (sliderMoving)
        {
            yield return null;
        }

        SetStrengthText(powerValue);
    }

    void SetStrengthText(float power)
    {
        if (power > 90)
        {
            strengthText.text = "WOW YOURE PRETTY STRONG";
            if (QuestController.instance != null)
            {
                QuestController.instance.CompleteQuest("Test of Strength");
            }
        } else if (power > 70)
            strengthText.text = "PrEtTY GOoD But COUld be BEtteR";
        else if (power > 50)
            strengthText.text = "C'MON ARE YOU EVEN TRYING";
        else if (power > 30)
            strengthText.text = "BL:EG. bet you wont know what this means";
        else if (power > 10)
            strengthText.text = "you should honest be dead";
        else if (power > 1)
            strengthText.text = "HOW???? BRO LOKWEY MIGHT BE THE WEAKEST PERSON EVER";

        strengthText.text += "\n Power: " + Mathf.RoundToInt(powerValue);
        winPanel.SetActive(true);
    }
    
}