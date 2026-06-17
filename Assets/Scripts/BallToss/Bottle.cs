using UnityEngine;

public class Bottle : MonoBehaviour
{
    private bool hasLeft;

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("ContainmentArea"))
        {
            if (hasLeft) return;
            hasLeft = true;
            BallGameSingleton.instance.OnBottleLeft();
        }
    }
}
