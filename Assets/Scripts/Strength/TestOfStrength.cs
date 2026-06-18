using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestOfStrength : MonoBehaviour
{
    public GameObject powerBar;
    public Animator hammerAnimator;
    public Animator bellAnimator;
    public TMP_Text strengthText;

    private SpriteRenderer powerBarRenderer;
    private SpriteRenderer spriteRenderer;
    private Vector3 centerPosition;
    private Vector3 offsetPosition;

    private bool moving = true;

    private float powerBarCenter;
    private float powerBarHeight;

    private float powerValue;

    void Start()
    {
        powerBarRenderer = powerBar.GetComponent<SpriteRenderer>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        centerPosition = powerBar.transform.position + new Vector3(spriteRenderer.bounds.size.x / 2 + -0.1f, 0, 0) +
                         new Vector3(powerBarRenderer.bounds.size.x / 2, 0, 0);
        powerBarHeight = powerBarRenderer.bounds.size.y;
        powerBarCenter = powerBar.transform.position.y;
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

                hammerAnimator.Play("Hammer");
                bellAnimator.Play("Ringing");

                SetStrengthText(powerValue);
                
            }
        }
    }
    
    void SetStrengthText(float power)
    {
        if (power > 90)
        {
            strengthText.text = "WOW YOURE PRETTY STRONG";
        }
        else if (power > 70)
        {
            strengthText.text = "PrEtTY GOoD But COUld be BEtteR";
        }
        else if (power > 50)
        {
            strengthText.text = "C'MON ARE YOU EVEN TRYING";
        }
        else if (power > 30)
        {
            strengthText.text = "BL:EG. bet you wont know what this means";
        }
        else if (power > 10)
        {
            strengthText.text = "you should honest be dead";
        }
        else if (power > 1)
        {
            strengthText.text = "HOW???? BRO LOKWEY MIGHT BE THE WEAKEST PERSON EVER";
        }

        strengthText.text += "\n Power: " + Mathf.RoundToInt(powerValue);
        strengthText.enabled = true;
    }
    
    
    
}