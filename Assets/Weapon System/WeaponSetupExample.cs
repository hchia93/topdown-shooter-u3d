using UnityEngine;

// This script demonstrates how to set up different weapon types
// Attach this to a GameObject to automatically create weapon examples
public class WeaponSetupExample : MonoBehaviour
{
    [Header("Bullet Prefab")]
    [SerializeField] private GameObject bulletPrefab; // Assign your bullet prefab here
    
    [Header("Auto Setup")]
    [SerializeField] private bool createExampleWeapons = true;
    
    private void Start()
    {
        if (createExampleWeapons && bulletPrefab != null)
        {
            CreateExampleWeapons();
        }
    }
    
    private void CreateExampleWeapons()
    {
        WeaponSystem weaponSystem = GetComponent<WeaponSystem>();
        if (weaponSystem == null)
        {
            weaponSystem = gameObject.AddComponent<WeaponSystem>();
        }
        
        // Create Basic Weapon
        CreateWeapon("Basic Blaster", WeaponPresets.BasicBulletStats, 0.3f, 1, 0f, weaponSystem);
        
        // Create Rapid Fire Weapon
        CreateWeapon("Rapid Blaster", WeaponPresets.RapidFireStats, 0.1f, 1, 0f, weaponSystem);
        
        // Create Heavy Weapon
        CreateWeapon("Heavy Cannon", WeaponPresets.HeavyBulletStats, 0.8f, 1, 0f, weaponSystem);
        
        // Create Shotgun Weapon
        CreateWeapon("Shotgun", WeaponPresets.ShotgunStats, 0.6f, 5, 30f, weaponSystem);
        
        // Create Laser Weapon
        CreateWeapon("Laser Rifle", WeaponPresets.LaserStats, 0.15f, 1, 0f, weaponSystem);
    }
    
    private void CreateWeapon(string weaponName, BulletStats stats, float fireRate, int bulletsPerShot, float spreadAngle, WeaponSystem weaponSystem)
    {
        // Create weapon GameObject
        GameObject weaponObj = new GameObject(weaponName);
        weaponObj.transform.SetParent(transform);
        weaponObj.transform.localPosition = Vector3.zero;
        
        // Add BulletWeapon component
        BulletWeapon weapon = weaponObj.AddComponent<BulletWeapon>();
        
        // Initialize weapon with configuration
        weapon.InitializeWeapon(weaponName, bulletPrefab, stats, fireRate, bulletsPerShot, spreadAngle);
        
        // Add weapon to the weapon system
        weaponSystem.AddWeapon(weapon);
        
        Debug.Log($"Created weapon: {weaponName}");
    }
}

// Alternative approach: Create a weapon factory
public static class WeaponFactory
{
    public static GameObject CreateBasicWeapon(GameObject bulletPrefab, Transform parent = null)
    {
        return CreateWeapon("Basic Blaster", WeaponPresets.BasicBulletStats, 0.3f, 1, 0f, bulletPrefab, parent);
    }
    
    public static GameObject CreateRapidFireWeapon(GameObject bulletPrefab, Transform parent = null)
    {
        return CreateWeapon("Rapid Blaster", WeaponPresets.RapidFireStats, 0.1f, 1, 0f, bulletPrefab, parent);
    }
    
    public static GameObject CreateHeavyWeapon(GameObject bulletPrefab, Transform parent = null)
    {
        return CreateWeapon("Heavy Cannon", WeaponPresets.HeavyBulletStats, 0.8f, 1, 0f, bulletPrefab, parent);
    }
    
    public static GameObject CreateShotgunWeapon(GameObject bulletPrefab, Transform parent = null)
    {
        return CreateWeapon("Shotgun", WeaponPresets.ShotgunStats, 0.6f, 5, 30f, bulletPrefab, parent);
    }
    
    public static GameObject CreateLaserWeapon(GameObject bulletPrefab, Transform parent = null)
    {
        return CreateWeapon("Laser Rifle", WeaponPresets.LaserStats, 0.15f, 1, 0f, bulletPrefab, parent);
    }
    
    private static GameObject CreateWeapon(string weaponName, BulletStats stats, float fireRate, int bulletsPerShot, float spreadAngle, GameObject bulletPrefab, Transform parent)
    {
        GameObject weaponObj = new GameObject(weaponName);
        if (parent != null)
        {
            weaponObj.transform.SetParent(parent);
            weaponObj.transform.localPosition = Vector3.zero;
        }
        
        BulletWeapon weapon = weaponObj.AddComponent<BulletWeapon>();
        
        // Initialize weapon with configuration
        weapon.InitializeWeapon(weaponName, bulletPrefab, stats, fireRate, bulletsPerShot, spreadAngle);
        
        return weaponObj;
    }
} 