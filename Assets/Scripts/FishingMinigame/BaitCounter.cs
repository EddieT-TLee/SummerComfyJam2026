using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaitCounter : MonoBehaviour
{
    [SerializeField]
    private GameObject baitPrefab;

    public float spacing = 25f;

    private readonly List<Image> baitImages = new List<Image>();
    public int bait = 5;
    
    private void Awake()
    {
        BuildBaitImages();
    }

    private void BuildBaitImages()
    {
        bait = Mathf.Max(0, bait);
        baitImages.Clear();

        List<Image> existingImages = new List<Image>();

        for (int i = 0; i < bait; i++)
        {
            Image baitImage = Instantiate(baitPrefab, transform).GetComponent<Image>();

            baitImage.name = $"Bait {i + 1}";
            baitImages.Add(baitImage);
        }

        PositionBaitImages();
    }

    private void PositionBaitImages()
    {
        if (baitImages.Count == 0) return;

        float baitWidth = baitImages[0].rectTransform.rect.width;
        float step = baitWidth + spacing;

        for (int i = 0; i < baitImages.Count; i++)
        {
            RectTransform rect = baitImages[i].rectTransform;
            rect.anchoredPosition = new Vector2(step * i, rect.anchoredPosition.y);
        }
    }

    public void UseBait()
    {
        if (bait == 0) return;
        bait--;

        baitImages[bait].enabled = false;
    }
}
