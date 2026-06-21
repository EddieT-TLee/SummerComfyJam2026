using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    private Image borderBackgroundImage;

    private GameObject fill;
    private Image fillImage;

    private RectTransform borderRect;
    private RectTransform fillRect;

    private float backgroundWidth;
    private float backgroundHeight;

    private Vector4 borders;

    public float progressValue = 0f;

    private void Start()
    {
        borderBackgroundImage = GetComponent<Image>();
        borderRect = GetComponent<RectTransform>();

        fill = transform.GetChild(0).gameObject;
        fillImage = fill.GetComponent<Image>();
        fillRect = fill.GetComponent<RectTransform>();

        Canvas.ForceUpdateCanvases();

        float borderWidth = borderRect.rect.width;
        float borderHeight = borderRect.rect.height;

        Vector4 backgroundBorders = borderBackgroundImage.sprite != null ? borderBackgroundImage.sprite.border : Vector4.zero;
        float borderPixelsPerUnit = Mathf.Max(borderBackgroundImage.pixelsPerUnit, Mathf.Epsilon);
        borders = backgroundBorders / borderPixelsPerUnit;

        backgroundWidth = borderWidth - borders.x - borders.z;

        backgroundHeight = borderHeight - borders.y - borders.w;

        fillRect.anchorMin = new Vector2(0f, 0.5f);
        fillRect.anchorMax = new Vector2(0f, 0.5f);
        fillRect.pivot = new Vector2(0f, 0.5f);

        fillRect.anchoredPosition = new Vector2(borders.x, 0f);

        fillRect.sizeDelta = new Vector2(0f, backgroundHeight);
    }

    public void UpdateColor(Color c)
    {
        fillImage.color = c;
    }

    public void UpdateProgress(float progress)
    {
        progressValue = Mathf.Clamp01(progress);
        ResizeFillBar(progressValue);
    }

    private void ResizeFillBar(float progress)
    {
        fillRect.sizeDelta = new Vector2(
            backgroundWidth * progress,
            backgroundHeight
        );
    }
}
