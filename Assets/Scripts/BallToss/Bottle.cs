using UnityEngine;

public class Bottle : MonoBehaviour
{
    private bool hasLeft;

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("ContainmentArea"))
            NotifyLeft();
    }

    public void NotifyLeft()
    {
        if (hasLeft) return;
        hasLeft = true;
        BottleManager.instance.OnBottleLeft();
    }
}
