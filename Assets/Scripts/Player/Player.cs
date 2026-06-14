using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    
    public float moveSpeed = 5f; // Move speed of player
    
    public InputActionAsset actionAsset; // Input actions of player

    private InputAction moveAction;
    private Rigidbody2D rb; // Players rigidbody used for movement
    // private Camera playerCamera; // Camera from Scene
    [SerializeField]
    private GameObject DialoguePanel; // Im Lazy to code up where to find this
    

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
        
        if(DialoguePanel == null) DialoguePanel = GameObject.FindGameObjectWithTag("DialoguePanel");
        //playerCamera = Camera.main;
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
    
    // void LateUpdate()
    // {
    //     playerCamera.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
    // }
    
    
    // Input action stuff
    private void OnEnable()
    {
        moveAction.Enable();
       // SceneManager.sceneLoaded += OnSceneLoaded;
       
    }

    private void OnDisable()
    {
        if (moveAction == null) return; 
        moveAction.Disable();
       // SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    // private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    // {
    //     playerCamera = Camera.main;
    // }
    
    
}

