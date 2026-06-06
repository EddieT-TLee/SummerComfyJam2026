using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    
    public float moveSpeed = 5f;
    
    [SerializeField]
    private Camera playerCamera;
   
    [SerializeField]
    private InputActionAsset actionAsset;

    private InputAction moveAction;
    private InputAction interactAction;

    void Awake()
    {
        InputActionMap playerMap = actionAsset.FindActionMap("Player");
        moveAction = playerMap.FindAction("Move");
        interactAction = playerMap.FindAction("Interact");
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input = moveAction.ReadValue<Vector2>();
        transform.Translate(input.x * moveSpeed * Time.deltaTime, input.y * moveSpeed * Time.deltaTime, 0f);
        playerCamera.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
    }

    private void OnEnable()
    {
        moveAction.Enable();
        interactAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Enable();
        interactAction.Enable();
    }
}
