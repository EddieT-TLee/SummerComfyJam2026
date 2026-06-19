using UnityEngine;

public class AmbientSFX : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;   
    private BoxCollider2D boxCollider;
    private GameObject player;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        player = GameObject.FindWithTag("Player");

        audioSource.loop = true;
        audioSource.spatialBlend = 1f;
        audioSource.Play();
    }

    void Update()
    {
        
        Vector3 closestPoint = boxCollider.ClosestPoint(player.transform.position);
        audioSource.transform.position = closestPoint;
    
    }
}
