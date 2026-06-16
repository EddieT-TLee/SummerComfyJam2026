using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CamManager : MonoBehaviour
{
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (Camera.main) return; 
        Camera cam = GameObject.FindGameObjectWithTag("PlayerCam").GetComponent<Camera>();
        // Add cinemachine brain to main cam if it is an overworld camera
        if(cam != null){
            if (!cam.TryGetComponent<CinemachineBrain>(out CinemachineBrain brain))
            {
                cam.gameObject.AddComponent<CinemachineBrain>();
            }
        }
        CinemachineConfiner2D confiner = GetComponent<CinemachineConfiner2D>();
        confiner.BoundingShape2D = GameObject.FindGameObjectWithTag("Confiner").GetComponent<BoxCollider2D>();
    }
}

