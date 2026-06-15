using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    private SpriteRenderer borderBackgroundRenderer;
    private GameObject fill;
    private SpriteRenderer fillRenderer;

    private float borderBackgroundWidth;
    private float borderBackgroundHeight;

    private float backgroundWidth;
    private float backgroundHeight;

    private Vector4 borders;
    public float progressValue = 0f;

    private void Start()
    {
        borderBackgroundRenderer = GetComponent<SpriteRenderer>();
        fill = transform.GetChild(0).gameObject;
        fillRenderer = fill.GetComponent<SpriteRenderer>();

        borderBackgroundWidth = borderBackgroundRenderer.bounds.size.x;
        borderBackgroundHeight = borderBackgroundRenderer.bounds.size.y;

        borders = borderBackgroundRenderer.sprite.border / borderBackgroundRenderer.sprite.pixelsPerUnit;

        backgroundWidth = borderBackgroundWidth - borders.x - borders.z;
        backgroundHeight = borderBackgroundHeight - borders.y - borders.w;

        fill.transform.localPosition = new Vector3(borders.x - borderBackgroundWidth/2, 0, fill.transform.localPosition.z);
        fill.transform.localScale = new Vector3
        (
             0f,
             fill.transform.localScale.y,
             fill.transform.localScale.z
        );
    }

    public void UpdateColor(Color c)
    {
        fillRenderer.color = c;
    }

    public void UpdateProgress(float progress)
    {
        progressValue = progress;
        ResizeFillBar(progress);
    }

    private void ResizeFillBar(float progress)
    {
        fill.transform.localScale = new Vector3
        (
            progress * backgroundWidth,
            backgroundHeight,
            fill.transform.localScale.z
        );
    }
}
