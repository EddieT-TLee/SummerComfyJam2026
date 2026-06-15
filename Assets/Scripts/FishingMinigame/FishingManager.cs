using UnityEngine;

public class FishingManager : MonoBehaviour
{
    [SerializeField]
    private FishingMeter fishingMeter;
    [SerializeField]
    private ProgressBar progressBar;

    private Gradient colors;

    private float fishCaughtProgress = 0.5f;
    private const float CATCH_TIME = 10f;
    private const float DECAY_TIME = 15f;

    private bool gameEnded = false;

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

    private void Update()
    {
        if (gameEnded) return;

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
            OnFail();
            gameEnded = true;
        } else if (fishCaughtProgress >= 1f)
        {
            fishCaughtProgress = 1f;
            OnWin();
            gameEnded = true;
        }


        progressBar.UpdateColor(colors.Evaluate(fishCaughtProgress));
        progressBar.UpdateProgress(fishCaughtProgress);
    }

    private void OnFail()
    {

    }

    private void OnWin()
    {

    }
}
