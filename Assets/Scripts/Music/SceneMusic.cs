
using System;
using UnityEngine;

[Serializable]
public struct SceneMusic
{
    public string sceneName;
    public AudioClip music;
    public bool persistThroughSceneChange;
}
