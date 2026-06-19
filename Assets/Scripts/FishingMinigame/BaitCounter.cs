using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(HorizontalLayoutGroup))]
public class BaitCounter : MonoBehaviour
{
    [SerializeField]
    private GameObject baitPrefab;

    private readonly List<Image> baitImages = new List<Image>();
    public int bait = 5;
    
    private void Awake()
    {
        HorizontalLayoutGroup layoutGroup = GetComponent<HorizontalLayoutGroup>();
        RectTransform rect = GetComponent<RectTransform>();
        layoutGroup.childControlWidth = true;
        layoutGroup.childControlHeight = true;

        bait = Mathf.Max(0, bait);
        baitImages.Clear();

        if (baitPrefab == null) return;

        for (int i = 0; i < bait; i++)
        {
            Image baitImage = Instantiate(baitPrefab, transform).GetComponent<Image>();

            baitImage.name = $"Bait {i + 1}";
            baitImage.rectTransform.sizeDelta = new Vector2(64, 64);
            baitImage.preserveAspect = true;
            baitImages.Add(baitImage);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(rect);

        layoutGroup.childControlWidth = false;
        layoutGroup.childControlHeight = false;
    }

    public void UseBait()
    {
        if (bait == 0) return;
        bait--;

        baitImages[bait].enabled = false;
    }
}
