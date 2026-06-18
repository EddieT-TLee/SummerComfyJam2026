using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(HorizontalLayoutGroup))]
public class BaitCounter : MonoBehaviour
{
    [SerializeField]
    private GameObject baitPrefab;

    [SerializeField]
    private float spacing = 0f;

    private readonly List<Image> baitImages = new List<Image>();
    public int bait = 5;
    
    private void Awake()
    {
        ApplyLayoutSettings();
        BuildBaitImages();
    }

    private void OnValidate()
    {
        ApplyLayoutSettings();
    }

    private void BuildBaitImages()
    {
        bait = Mathf.Max(0, bait);
        baitImages.Clear();

        if (baitPrefab == null) return;

        for (int i = 0; i < bait; i++)
        {
            Image baitImage = Instantiate(baitPrefab, transform).GetComponent<Image>();

            baitImage.name = $"Bait {i + 1}";
            baitImages.Add(baitImage);
        }

        if (transform is RectTransform rectTransform)
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
        }
    }

    private void ApplyLayoutSettings()
    {
        HorizontalLayoutGroup layoutGroup = GetComponent<HorizontalLayoutGroup>();

        if (layoutGroup == null) return;

        layoutGroup.spacing = spacing;
    }

    public void UseBait()
    {
        if (bait == 0) return;
        bait--;

        baitImages[bait].enabled = false;
    }
}
