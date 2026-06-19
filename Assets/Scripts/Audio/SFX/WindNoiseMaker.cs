using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindNoiseMaker : MonoBehaviour
{
    [SerializeField]
    private List<AudioClip> windNoises;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        StartCoroutine(PlayWindSounds());
    }

    private IEnumerator PlayWindSounds()
    {
        while (true)
        {
            if (windNoises.Count == 0)
            {
                yield return null;
                continue;
            }

            AudioClip clip = windNoises[Random.Range(0, windNoises.Count)];

            audioSource.clip = clip;
            audioSource.Play();

            yield return new WaitForSeconds(clip.length);
        }
    }
}