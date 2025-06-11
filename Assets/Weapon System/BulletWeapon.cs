using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class BulletStats
{
    [Header("Bullet Properties")]
    public float speed = 10f;
    public float lifetime = 3f;
    public float damage = 1f;
    public LayerMask targetLayers = -1;
    
    [Header("Visual")]
    public Color bulletColor = Color.white;
    public Vector3 bulletScale = Vector3.one;
}

public class BulletWeapon : MonoBehaviour
{
    [Header("Weapon Configuration")]
    [SerializeField] private string weaponName = "Basic Weapon";
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private BulletStats bulletStats;
    [SerializeField] private int poolSize = 20;
    [SerializeField] private Transform firePoint;
    
    [Header("Firing")]
    [SerializeField] private float fireRate = 0.2f;
    [SerializeField] private int bulletsPerShot = 1;
    [SerializeField] private float spreadAngle = 0f; // For multi-bullet weapons
    
    private Queue<GameObject> bulletPool;
    private float nextFireTime;
    
    public string WeaponName => weaponName;
    public float FireRate => fireRate;
    public BulletStats Stats => bulletStats;
    
    private void Awake()
    {
        InitializePool();
        
        // Create fire point if it doesn't exist
        if (firePoint == null)
        {
            GameObject firePointObj = new GameObject("FirePoint");
            firePointObj.transform.SetParent(transform);
            firePointObj.transform.localPosition = new Vector3(0, 0.5f, 0);
            firePoint = firePointObj.transform;
        }
    }
    
    private void InitializePool()
    {
        bulletPool = new Queue<GameObject>();
        
        for (int i = 0; i < poolSize; i++)
        {
            CreateNewBullet();
        }
    }
    
    private void CreateNewBullet()
    {
        if (bulletPrefab == null)
        {
            Debug.LogError($"Bullet prefab is not assigned for weapon: {weaponName}");
            return;
        }
        
        GameObject bullet = Instantiate(bulletPrefab, transform);
        bullet.SetActive(false);
        
        // Configure bullet with weapon stats
        ModularBullet bulletScript = bullet.GetComponent<ModularBullet>();
        if (bulletScript != null)
        {
            bulletScript.Initialize(this, bulletStats);
        }
        
        bulletPool.Enqueue(bullet);
    }
    
    public bool CanFire()
    {
        return Time.time >= nextFireTime;
    }
    
    public void Fire()
    {
        if (!CanFire()) return;
        
        nextFireTime = Time.time + fireRate;
        
        for (int i = 0; i < bulletsPerShot; i++)
        {
            FireSingleBullet(i);
        }
    }
    
    private void FireSingleBullet(int bulletIndex)
    {
        GameObject bullet = GetBullet();
        if (bullet == null) return;
        
        // Calculate spread angle for multi-bullet weapons
        float angle = 0f;
        if (bulletsPerShot > 1)
        {
            float totalSpread = spreadAngle;
            float angleStep = totalSpread / (bulletsPerShot - 1);
            angle = -totalSpread / 2f + angleStep * bulletIndex;
        }
        
        // Unparent bullet so it moves independently
        bullet.transform.SetParent(null);
        
        // Set bullet position and rotation
        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = firePoint.rotation * Quaternion.Euler(0, 0, angle);
        
        // Activate bullet
        bullet.SetActive(true);
    }
    
    public GameObject GetBullet()
    {
        if (bulletPool.Count == 0)
        {
            CreateNewBullet();
        }
        
        if (bulletPool.Count == 0) return null; // Safety check
        
        return bulletPool.Dequeue();
    }
    
    public void ReturnBullet(GameObject bullet)
    {
        if (bullet == null) return;
        
        bullet.SetActive(false);
        bullet.transform.SetParent(transform);
        bulletPool.Enqueue(bullet);
    }
    
    // Method to upgrade weapon stats (useful for power-ups)
    public void UpgradeStats(BulletStats newStats)
    {
        bulletStats = newStats;
        
        // Update all bullets in pool with new stats
        List<GameObject> tempList = new List<GameObject>(bulletPool);
        foreach (GameObject bullet in tempList)
        {
            ModularBullet bulletScript = bullet.GetComponent<ModularBullet>();
            if (bulletScript != null)
            {
                bulletScript.Initialize(this, bulletStats);
            }
        }
    }
    
    // Public initialization method for programmatic setup
    public void InitializeWeapon(string name, GameObject bulletPrefabRef, BulletStats stats, float rate, int bullets, float spread, int poolSizeOverride = 20)
    {
        weaponName = name;
        bulletPrefab = bulletPrefabRef;
        bulletStats = stats;
        fireRate = rate;
        bulletsPerShot = bullets;
        spreadAngle = spread;
        poolSize = poolSizeOverride;
        
        // Reinitialize pool if it was already created
        if (bulletPool != null)
        {
            // Clear existing pool
            while (bulletPool.Count > 0)
            {
                GameObject bullet = bulletPool.Dequeue();
                if (bullet != null)
                {
                    DestroyImmediate(bullet);
                }
            }
            
            // Recreate pool with new settings
            InitializePool();
        }
    }
} 