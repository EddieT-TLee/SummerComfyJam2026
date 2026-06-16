using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f; // Move speed of player

    public InputActionAsset actionAsset; // Input actions of player

    private InputAction moveAction;
    private Rigidbody2D rb; // Players rigidbody used for movement

    void Awake()
    {
        if (FindObjectsByType<Player>(FindObjectsInactive.Exclude).Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        InputActionMap playerMap = actionAsset.FindActionMap("Player");
        moveAction = playerMap.FindAction("Move");
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (PauseManager.IsPaused)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 input = moveAction.ReadValue<Vector2>();
        rb.linearVelocity = new Vector2(input.x * moveSpeed, input.y * moveSpeed);
    }

    // Input action stuff
    private void OnEnable()
    {
        moveAction.Enable();
    }

    private void OnDisable()
    {
        if (moveAction == null) return;
        moveAction.Disable();
    }
}