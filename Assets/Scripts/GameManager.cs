using System;
using UnityEngine;
using UnityEngine.EventSystems;


// Singleton to keep track of what not to destroy when Switching Scenes
public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton
   
    // Objects to not destroy
    public GameObject[] persistentObjects;
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            MarkObjectAsPersistent();
            
        }
        else
        {
           ClearUpAndDestroyObjects(); // Make sure only one instance
        }
    }

    private void MarkObjectAsPersistent()
    {
        foreach (GameObject go in persistentObjects)
        {
            if(go != null){
                DontDestroyOnLoad(go);
            }
        }
    }
    
    private void ClearUpAndDestroyObjects()
    {
        foreach (GameObject go in persistentObjects)
        {
            Destroy(go);
        }
        Destroy(gameObject);
    }
    
    
}
