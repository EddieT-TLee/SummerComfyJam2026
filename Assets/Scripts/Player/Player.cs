using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f; // Move speed of player

    public InputActionAsset actionAsset; // Input actions of player

    private InputAction moveAction;
    private Rigidbody2D rb; // Players rigidbody used for movement
    private Animator animator;

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
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (PauseManager.IsPaused)
        {
            UpdateAnimator(Vector2.zero); // Literally just to make it stop playing the walking animation
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 input = moveAction.ReadValue<Vector2>();
        rb.linearVelocity = new Vector2(input.x * moveSpeed, input.y * moveSpeed);
        UpdateAnimator(input);
    }

    private void UpdateAnimator(Vector2 input)
    {
        bool isMoving = input != Vector2.zero;
        animator.SetBool("IsWalking", isMoving);
        animator.SetFloat("InputX", input.x);
        animator.SetFloat("InputY", input.y);

        if (isMoving)
        {
            animator.SetFloat("LastInputX", input.x);
            animator.SetFloat("LastInputY", input.y);
        }
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