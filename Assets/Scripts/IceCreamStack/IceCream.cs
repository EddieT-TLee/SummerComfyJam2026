using System;
using UnityEngine;

public class IceCream : MonoBehaviour
{
    private const float LandingNormalThreshold = 0.5f;

    private Transform cone;
    private Rigidbody2D rb;

    public bool IsFrozen { get; private set; }
    
    public bool IsCherry { get; set; }
    
    private Action onFrozen;

    public void Initialize(Transform iceCreamCone, Action onFrozenCallback = null)
    {
        cone = iceCreamCone;
        onFrozen = onFrozenCallback;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!IsFrozen && IsLandingSurface(collision))
        {
            FreezeInPlace();
        }

    }

    private bool IsLandingSurface(Collision2D collision)
    {
        if (!HasLandingContact(collision)) return false;

        Collider2D other = collision.collider;

        if (cone && other.transform == cone)
        {
            return true;
        }

        IceCream otherScoop = other.GetComponent<IceCream>();
        return otherScoop && otherScoop.IsFrozen;
    }

    private bool HasLandingContact(Collision2D collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            if (collision.GetContact(i).normal.y >= LandingNormalThreshold)
            {
                return true;
            }
        }

        return false;
    }

    private void FreezeInPlace()
    {
        IsFrozen = true;

        if (!rb) return;

        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.bodyType = RigidbodyType2D.Static;
        
        onFrozen?.Invoke();
    }
}
