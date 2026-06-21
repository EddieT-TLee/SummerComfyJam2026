using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneController : MonoBehaviour
{
    public RectTransform filmStrip;
    public TMP_Text buttonText;
    public float slideTransitionDuration = 0.3f;

    private Vector2 filmStripStart;
    
    private int currentFilmCellIndex = 0;
    private GameObject currentFilmCell;

    private void Start()
    {
        currentFilmCell = filmStrip.transform.GetChild(currentFilmCellIndex).gameObject;
        filmStripStart = filmStrip.anchoredPosition;
    }

    private IEnumerator NextSlide()
    {
        if (currentFilmCellIndex+1 == filmStrip.transform.childCount-1)
        {
            buttonText.text = "Start Game";
        }

        currentFilmCellIndex++;
        currentFilmCell = filmStrip.transform.GetChild(currentFilmCellIndex).gameObject;
        float offset = currentFilmCell.GetComponent<RectTransform>().rect.size.y;

        Vector2 startPosition = filmStrip.anchoredPosition;

        Vector2 targetPosition = new Vector2(
            filmStripStart.x,
            filmStripStart.y + offset * currentFilmCellIndex
        );

        float elapsed = 0f;
        while (elapsed < slideTransitionDuration)
        {
            elapsed += Time.deltaTime;
            filmStrip.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, elapsed/slideTransitionDuration);
            yield return null;
        }

        filmStrip.anchoredPosition = targetPosition;
    }

    public void OnSlideButtonClick()
    {
        if (currentFilmCellIndex == filmStrip.transform.childCount-1)
        {
            StartCoroutine(SwitchToMainGame());

        } else
        {
            StartCoroutine(NextSlide());
        }
    }

    private IEnumerator SwitchToMainGame()
    {
        yield return StartCoroutine(ScreenFader.instance.FadeOut());

        AsyncOperation load = SceneManager.LoadSceneAsync("Beach");

        while (!load.isDone) yield return null;

        yield return StartCoroutine(ScreenFader.instance.FadeIn());
    }
}
