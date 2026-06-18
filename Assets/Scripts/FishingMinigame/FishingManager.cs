using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FishingManager : MonoBehaviour
{
    [SerializeField]
    private FishingMeter fishingMeter;

    [SerializeField]
    private ProgressBar progressBar;
    
    [SerializeField]
    private BaitCounter baitCounter;
    
    [SerializeField]
    private GameObject fishCounter;
    [SerializeField]
    private Animator fishAnimator;
    [SerializeField]
    private List<Sprite> fishSprites;

    [SerializeField]
    private Button castLineButton;

    [SerializeField]
    private GameObject instructionsScreen;
    [SerializeField]
    private Button startButton;

    [SerializeField]
    private GameObject endScreen;
    [SerializeField]
    private TMP_Text endScreenText;    
    
    private Gradient colors;

    private float fishCaughtProgress = 0.5f;
    private const float CATCH_TIME = 10f;
    private const float DECAY_TIME = 15f;

    private bool paused = false;

    private int fishCaught = 0;
    private const int FISH_GOAL = 3;

    private void Awake()
    {
        colors = new Gradient();

        colors.SetKeys(
            new GradientColorKey[]
            {
                new GradientColorKey(Color.red, 0.25f),
                new GradientColorKey(Color.white, 0.5f),
                new GradientColorKey(Color.green, 1f)
            },
            new GradientAlphaKey[]
            {
                new GradientAlphaKey(1f, 0f),
                new GradientAlphaKey(1f, 0f),
                new GradientAlphaKey(1f, 0f)
            }
        );
    }

    private void Start()
    {
        HideFishingUI();
        HideButtonUI();
        HideBaitCounter();
        HideFishCounter();
        HideFishDancer();
        HideEndScreenText();
        paused = true;

        startButton.onClick.AddListener(StartGame);
        castLineButton.onClick.AddListener(CastLine);
    }

    private void Update()
    {
        if (paused) return;

        if (fishingMeter.zonesOverlapping)
        {
            fishCaughtProgress += Time.deltaTime / CATCH_TIME;
        } else
        {
            fishCaughtProgress -= Time.deltaTime / DECAY_TIME;
        }

        // On fail or win, do something
        if (fishCaughtProgress <= 0f)
        {
            fishCaughtProgress = 0f;
            OnFishLost();
        } else if (fishCaughtProgress >= 1f)
        {
            fishCaughtProgress = 1f;
            OnFishCaught();
        }

        progressBar.UpdateColor(colors.Evaluate(fishCaughtProgress));
        progressBar.UpdateProgress(fishCaughtProgress);
    }

    private void StartGame()
    {
        HideInstructions();
        ShowBaitCounter();
        ShowFishCounter();
        ShowButtonUI();
    }

    private void CastLine()
    {
        ShowFishingUI();
        HideButtonUI();
        baitCounter.UseBait();
        paused = false;
    }

    private void OnFishLost()
    {
        if (baitCounter.bait == 0)
        {
            OnMinigameEnd();
            return;
        }

        HideFishingUI();
        ShowButtonUI();

        ResetProgress();
        fishingMeter.ResetFishingMeter();
        paused = true;
    }

    private void OnFishCaught()
    {
        fishCaught++;

        StartCoroutine(PlayFishCaughtAnimation());

        if (baitCounter.bait == 0)
        {
            OnMinigameEnd();
            return;
        }

        HideFishingUI();

        ResetProgress();
        fishingMeter.ResetFishingMeter();
        paused = true;
    }

    private IEnumerator PlayFishCaughtAnimation()
    {
        Sprite fish = fishSprites[Random.Range(0, fishSprites.Count)];

        fishAnimator.gameObject.GetComponent<Image>().sprite = fish;
        fishAnimator.SetTrigger("PlayAnimation");
        ShowFishDancer();

        yield return new WaitUntil(() => fishAnimator.GetCurrentAnimatorStateInfo(0).IsName("FishDance"));
        yield return new WaitUntil(() => !fishAnimator.GetCurrentAnimatorStateInfo(0).IsName("FishDance"));

        HideFishDancer();
        AddFishToCounter(fish);
        ShowButtonUI();

        if (fishCaught == FISH_GOAL)
        {
            OnMinigameEnd();
        }
    }

    private void AddFishToCounter(Sprite fish)
    {
        GameObject fishObject = new GameObject($"Fish_{fishCaught}");

        fishObject.transform.SetParent(fishCounter.transform, false);

        Image img = fishObject.AddComponent<Image>();
        img.sprite = fish;
        img.rectTransform.sizeDelta = new Vector2(75f, 75f);
        img.preserveAspect = true;
    }

    private void OnMinigameEnd()
    {
        if (fishCaught == 3)
        {
            endScreenText.text = "YOU WIN!";
        } else
        {
            endScreenText.text = "I shall starve for one thousand nights. I must kill a child to sustain myself tonight, and the next night, and so on...";
        }

        HideButtonUI();
        HideFishingUI();
        ShowEndScreenText();
        paused = true;
    }

    private void ResetProgress()
    {
        fishCaughtProgress = 0.5f;
        progressBar.UpdateProgress(fishCaughtProgress);
    }

    private void ShowFishingUI()
    {
        fishingMeter.gameObject.SetActive(true);
        progressBar.gameObject.SetActive(true);
    }

    private void HideFishingUI()
    {
        fishingMeter.gameObject.SetActive(false);
        progressBar.gameObject.SetActive(false);
    }

    private void ShowBaitCounter()
    {
        baitCounter.gameObject.SetActive(true);
    }

    private void HideBaitCounter()
    {
        baitCounter.gameObject.SetActive(false);
    }

    private void ShowFishCounter()
    {
        fishCounter.SetActive(true);
    }

    private void HideFishCounter()
    {
        fishCounter.SetActive(false);
    }

    private void HideInstructions()
    {
        instructionsScreen.SetActive(false);
    }

    private void ShowFishDancer()
    {
        fishAnimator.gameObject.GetComponent<Image>().enabled = true;
    }

    private void HideFishDancer()
    {
        fishAnimator.gameObject.GetComponent<Image>().enabled = false;
    }

    private void ShowButtonUI()
    {
        castLineButton.gameObject.SetActive(true);
    }

    private void HideButtonUI()
    {
        castLineButton.gameObject.SetActive(false);
    }

    private void ShowEndScreenText()
    {
        endScreen.SetActive(true);
    }

    private void HideEndScreenText()
    {
        endScreen.SetActive(false);
    }


}
