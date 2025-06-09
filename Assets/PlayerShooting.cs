using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 0.2f;
    
    private float nextFireTime;
    private Camera mainCamera;
    private bool isShooting;
    
    private void Start()
    {
        mainCamera = Camera.main;
        
        // Create fire point if it doesn't exist
        if (firePoint == null)
        {
            GameObject firePointObj = new GameObject("FirePoint");
            firePointObj.transform.SetParent(transform);
            firePointObj.transform.localPosition = new Vector3(0, 0.5f, 0); // Adjust this position as needed
            firePoint = firePointObj.transform;
        }
    }
    
    private void Update()
    {
        if (isShooting && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }
    
    // Called by the Input System when mouse button is pressed
    public void OnAttack(UnityEngine.InputSystem.InputValue value)
    {
        isShooting = value.isPressed;
    }
    
    private void Shoot()
    {
        if (BulletPool.Instance == null)
        {
            Debug.LogError("BulletPool instance not found!");
            return;
        }
        
        // Get bullet from pool
        GameObject bullet = BulletPool.Instance.GetBullet();
        
        // Set bullet position and rotation
        bullet.transform.position = firePoint.position;
        bullet.transform.rotation = firePoint.rotation;
    }
} 