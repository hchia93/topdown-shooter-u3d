using UnityEngine;

[CreateAssetMenu(fileName = "WeaponConfig", menuName = "Weapons/Weapon Configuration")]
public class WeaponConfiguration : ScriptableObject
{
    [Header("Weapon Identity")]
    public string weaponName = "Basic Weapon";
    public Sprite weaponIcon;
    public string description = "A basic weapon";
    
    [Header("Bullet Configuration")]
    public BulletStats bulletStats;
    
    [Header("Firing Configuration")]
    public float fireRate = 0.2f;
    public int bulletsPerShot = 1;
    public float spreadAngle = 0f;
    
    [Header("Pool Configuration")]
    public int poolSize = 20;
    
    [Header("Prefab References")]
    public GameObject bulletPrefab;
    
    // Method to apply this configuration to a BulletWeapon
    public void ApplyToWeapon(BulletWeapon weapon)
    {
        if (weapon == null) return;
        
        // Use reflection or direct field access to set private fields
        // This would require making some fields in BulletWeapon public or adding setter methods
        // For now, this serves as a template for weapon configuration
    }
}

// Static class with predefined weapon configurations
public static class WeaponPresets
{
    public static BulletStats BasicBulletStats => new BulletStats
    {
        speed = 10f,
        lifetime = 3f,
        damage = 1f,
        targetLayers = -1,
        bulletColor = Color.white,
        bulletScale = Vector3.one
    };
    
    public static BulletStats RapidFireStats => new BulletStats
    {
        speed = 12f,
        lifetime = 2.5f,
        damage = 0.7f,
        targetLayers = -1,
        bulletColor = Color.yellow,
        bulletScale = new Vector3(0.8f, 0.8f, 1f)
    };
    
    public static BulletStats HeavyBulletStats => new BulletStats
    {
        speed = 6f,
        lifetime = 4f,
        damage = 2.5f,
        targetLayers = -1,
        bulletColor = Color.red,
        bulletScale = new Vector3(1.3f, 1.3f, 1f)
    };
    
    public static BulletStats ShotgunStats => new BulletStats
    {
        speed = 8f,
        lifetime = 2f,
        damage = 0.8f,
        targetLayers = -1,
        bulletColor = Color.orange,
        bulletScale = new Vector3(0.7f, 0.7f, 1f)
    };
    
    public static BulletStats LaserStats => new BulletStats
    {
        speed = 20f,
        lifetime = 1.5f,
        damage = 1.2f,
        targetLayers = -1,
        bulletColor = Color.cyan,
        bulletScale = new Vector3(0.5f, 2f, 1f)
    };
} 