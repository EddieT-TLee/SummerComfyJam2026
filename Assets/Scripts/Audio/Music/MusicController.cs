using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicController : MonoBehaviour
{
    public static MusicController instance;

    [SerializeField]
    private List<SceneMusic> sceneMusics;
    private AudioSource audioSource;
    private SceneMusic? currentSceneMusic;
    private Dictionary<string, SceneMusic> sceneMusicPairs = new Dictionary<string, SceneMusic>();
    private Coroutine transitionCoroutine;
    
    public float transitionDuration = 1f;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(instance);

        audioSource = GetComponent<AudioSource>();
        foreach (SceneMusic sm in sceneMusics)
        {
            sceneMusicPairs.Add(sm.sceneName, sm);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

   private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        bool hasNewMusic = sceneMusicPairs.TryGetValue(scene.name, out SceneMusic newSceneMusic);

        bool shouldPersistCurrentMusic =
            currentSceneMusic.HasValue &&
            currentSceneMusic.Value.persistThroughSceneChange;

        // If current music persists and there's no music defined for the new scene,
        // keep playing the current track.
        if (shouldPersistCurrentMusic && !hasNewMusic)
            return;

        // No music for this scene and current music shouldn't persist.
        if (!hasNewMusic)
        {
            audioSource.Stop();
            currentSceneMusic = null;
            audioSource.clip = null;
            return;
        }

        // Play or transition to the new scene's music.
        if (audioSource.clip == null)
        {
            PlayMusic(newSceneMusic.music);
        }
        else
        {
            TransitionToMusic(newSceneMusic.music);
        }

        currentSceneMusic = newSceneMusic;
    }

    private void TransitionToMusic(AudioClip clip)
    {
        if (audioSource.clip == clip) return;

        if (transitionCoroutine != null)
        {
            StopCoroutine(transitionCoroutine);
        }
        transitionCoroutine = StartCoroutine(FadeToMusic(clip, transitionDuration));
    }

    private void PlayMusic(AudioClip clip)
    {
        if (audioSource.clip == clip) return;

        audioSource.clip = clip;
        audioSource.Play();
    }

    private IEnumerator FadeToMusic(AudioClip newMusic, float fadeDuration)
    {
        float startVolume = audioSource.volume;

        // Fade out
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, timer / fadeDuration);
            yield return null;
        }

        audioSource.volume = 0f;

        // Switch tracks
        audioSource.Stop();
        audioSource.clip = newMusic;
        audioSource.Play();

        // Fade in
        timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0f, startVolume, timer / fadeDuration);
            yield return null;
        }

        audioSource.volume = startVolume;

        transitionCoroutine = null;
    }
}
