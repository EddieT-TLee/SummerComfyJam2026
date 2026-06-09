using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    
    public float moveSpeed = 5f; // Move speed of player
    public Camera playerCamera; // Camera from Scene
    public InputActionAsset actionAsset; // Input actions of player

    private InputAction moveAction;
    private Rigidbody2D rb; // Players rigidbody used for movement
    [SerializeField]
    private GameObject DialoguePanel; // Im Lazy to code up where to find this
    

    void Awake()
    {
        InputActionMap playerMap = actionAsset.FindActionMap("Player");
        moveAction = playerMap.FindAction("Move");
        rb = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        if(DialoguePanel.activeSelf) // Disable movement if player is talking to someone
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }
        Vector2 input = moveAction.ReadValue<Vector2>();

        
        rb.linearVelocity = new Vector2(input.x * moveSpeed, input.y * moveSpeed);
    }
    
    void LateUpdate() // Stops the Camera from lagging behind player movement
    {
        // If a scene is bigger it will have a camera that follows
        // If it's a scene like a room it has a fixed camera
        if(playerCamera.CompareTag("Follow"))
            playerCamera.transform.position = new Vector3(transform.position.x, transform.position.y, -10); 
    }
    
    
    // Input action stuff
    private void OnEnable()
    {
        moveAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
    }
    
}

