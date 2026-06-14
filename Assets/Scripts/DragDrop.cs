 using UnityEngine;
 using UnityEngine.InputSystem;
public class NewMonoBehaviourScript : MonoBehaviour
{
    public float power = 10f;

    public float maxDragDistance = 5f;

    public int trajectoryResolution = 30;

    private Rigidbody2D rb;

    private Camera cam;

    private Vector3 startPoint;

    private LineRenderer lineRenderer; 


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            startPoint = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            startPoint.z = 0;
        }

        if (Mouse.current.leftButton.isPressed)
        {
            Vector3 currentPoint = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            currentPoint.z = 0;

            Vector2 dragVector = startPoint - currentPoint;
            dragVector = Vector2.ClampMagnitude(dragVector, maxDragDistance);

            ShowTrajectory(dragVector * power);
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            Vector3 endPoint = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            endPoint.z = 0;

            Vector2 force = (startPoint - endPoint) * power;
            rb.AddForce(force, ForceMode2D.Impulse);

            lineRenderer.enabled = false;
        }
    }

    void ShowTrajectory(Vector2 initialForce)
    {
        lineRenderer.enabled = true;
        lineRenderer.positionCount = trajectoryResolution;

        Vector3[] points = new Vector3[trajectoryResolution];
        Vector2 velocity = initialForce / rb.mass;
        Vector2 startPos = transform.position;

        for (int i = 0; i < trajectoryResolution; i++)
        {
            float t = i * Time.fixedDeltaTime;
            Vector2 pos = startPos + velocity * t + 0.5f * Physics2D.gravity * t * t;
            points[i] = pos;
        }

        lineRenderer.SetPositions(points);
    }
}
