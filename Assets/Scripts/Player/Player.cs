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
    

    void Awake()
    {
        InputActionMap playerMap = actionAsset.FindActionMap("Player");
        moveAction = playerMap.FindAction("Move");
        rb = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        Vector2 input = moveAction.ReadValue<Vector2>();
        rb.linearVelocity = new Vector2(input.x * moveSpeed, input.y * moveSpeed);
       
        // If a scene is bigger it will have have a camera that follows
        // If its a scene like a room it has a fixed camera
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

