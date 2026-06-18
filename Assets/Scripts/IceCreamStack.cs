using UnityEngine;

public class IceCreamStack : MonoBehaviour {
    [SerializeField] private Transform blockPrefab;
    [SerializeField] private Transform blockHolder;

    private Transform currentBlock = null;
    private Rigidbody2D currentRigidBody;

    private Vector2 blockStartPosition = new Vector2(0f, 4f);

    private float blockSpeed = 8f;
    private float blockSpeedIncrement = 0.5f;
    private float blockDirection = 1;
    private float xLimit = 5;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        SpawnNewBlock();
    }

    private void SpawnNewBlock() {

        currentBlock = Instantiate(blockPrefab, blockHolder);
        currentBlock.position = blockStartPosition;
        currentBlock.GetComponent<SpriteRenderer>().color = Random.ColorHSV();
        currentRigidBody = currentBlock.GetComponent<Rigidbody2D>();

        blockSpeed += blockSpeedIncrement;
     }  
    void Update() {
      
        if (currentBlock) {
            float moveAmount = Time.deltaTime * blockSpeed * blockDirection;
            currentBlock.position += new Vector3(moveAmount, 0, 0);
            if (Mathf.Abs(currentBlock.position.x) > xLimit) {

                currentBlock.position = new Vector3(blockDirection * xLimit, currentBlock.position.y, 0);
                blockDirection = -blockDirection;
            }

            if (Input.GetKeyDown(KeyCode.Space))
                Debug.Log("SpaceBar is being held down");
            {
                currentBlock = null;
                currentRigidBody.simulated = true;
                SpawnNewBlock();
            }
          }
        }
     }
