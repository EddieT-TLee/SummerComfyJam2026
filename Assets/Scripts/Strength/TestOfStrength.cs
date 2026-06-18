using System.Net.Security;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestOfStrength : MonoBehaviour
{
    public GameObject powerBar;
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

        centerPosition = powerBar.transform.position + new Vector3(spriteRenderer.bounds.size.x/2 + -0.1f, 0, 0) + new Vector3(powerBarRenderer.bounds.size.x/2, 0, 0);
        powerBarHeight = powerBarRenderer.bounds.size.y;
        powerBarCenter = powerBar.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
       if (moving)
        {
            offsetPosition = new Vector3(0, Mathf.Sin(Time.time * 3f), 0) * powerBarRenderer.bounds.size.y/2;
            transform.position = centerPosition + offsetPosition;
        
            if (Mouse.current.leftButton.isPressed)
            {
                moving = false;

                powerValue = (1 - (Mathf.Abs(powerBarCenter - transform.position.y) / (powerBarHeight/2))) * 100;

                Debug.Log(powerValue);
            }
        }
        

    }
}
