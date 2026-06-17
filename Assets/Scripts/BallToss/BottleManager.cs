using UnityEngine;

// Attach to any persistent GameObject (e.g. a "GameManager" empty object)
public class BottleManager : MonoBehaviour
{
    public static BottleManager instance;
    
    public Bottle[] bottles;
 
    private int bottlesOut;
    private bool gameWon;
 
    void Awake()
    {
        instance = this;
    }
 
    public void OnBottleLeft()
    {
        if (gameWon) return;
        bottlesOut++;
 
        if (bottlesOut >= bottles.Length)
            TriggerWin();
    }
 
    void TriggerWin()
    {
        gameWon = true;
        Debug.Log("YOU WIN! All bottles knocked out!");
    }
}