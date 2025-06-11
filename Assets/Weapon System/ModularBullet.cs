using UnityEngine;

public class ModularBullet : MonoBehaviour
{
    private BulletWeapon parentWeapon;
    private BulletStats stats;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private bool isInitialized = false;
    
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        
        // Ensure we have a Rigidbody2D for physics-based movement (optional)
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0f;
        }
    }
    
    public void Initialize(BulletWeapon weapon, BulletStats bulletStats)
    {
        parentWeapon = weapon;
        stats = bulletStats;
        isInitialized = true;
        
        // Apply visual settings
        if (spriteRenderer != null)
        {
            spriteRenderer.color = stats.bulletColor;
        }
        
        transform.localScale = stats.bulletScale;
    }
    
    private void OnEnable()
    {
        if (!isInitialized) return;
        
        // Reset physics
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
        
        // Schedule return to pool after lifetime
        Invoke(nameof(ReturnToPool), stats.lifetime);
    }
    
    private void Update()
    {
        if (!isInitialized) return;
        
        // Move bullet forward based on its rotation
        Vector2 direction = transform.up;
        transform.Translate(direction * stats.speed * Time.deltaTime, Space.World);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isInitialized) return;
        
        // Check if the collided object is on a target layer
        if (IsTargetLayer(other.gameObject.layer))
        {
            // Handle collision (damage, effects, etc.)
            HandleCollision(other);
            ReturnToPool();
        }
    }
    
    private bool IsTargetLayer(int layer)
    {
        return (stats.targetLayers.value & (1 << layer)) != 0;
    }
    
    private void HandleCollision(Collider2D other)
    {
        // Try to deal damage to the target
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(stats.damage);
        }
        
        // Add other collision effects here (particles, sound, etc.)
        // For example:
        // - Spawn hit effect
        // - Play sound
        // - Apply knockback
    }
    
    private void ReturnToPool()
    {
        if (parentWeapon != null)
        {
            parentWeapon.ReturnBullet(gameObject);
        }
        else
        {
            // Fallback: destroy if no parent weapon
            Destroy(gameObject);
        }
    }
    
    private void OnDisable()
    {
        CancelInvoke(nameof(ReturnToPool));
    }
    
    // Optional: Method to get bullet stats (useful for UI or debugging)
    public BulletStats GetStats()
    {
        return stats;
    }
    
    // Optional: Method to get parent weapon reference
    public BulletWeapon GetParentWeapon()
    {
        return parentWeapon;
    }
}

// Interface for objects that can take damage
public interface IDamageable
{
    void TakeDamage(float damage);
} 