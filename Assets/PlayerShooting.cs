using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 0.2f;
    [SerializeField] private bool useInputSystem = true; // Toggle between Input System and traditional Input
    
    private float nextFireTime;
    private Camera mainCamera;
    private bool isShooting;
    private Mouse mouse;
    
    private void Start()
    {
        mainCamera = Camera.main;
        mouse = Mouse.current;
        
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
        // Handle input based on the selected method
        if (useInputSystem)
        {
            // Double-check mouse state to ensure we stop shooting when button is released
            if (mouse != null && !mouse.leftButton.isPressed)
            {
                isShooting = false;
            }
        }
        else
        {
            // Traditional Input method as fallback
            isShooting = Input.GetMouseButton(0);
        }
        
        if (isShooting && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }
    
    // Called by the Input System when mouse button is pressed/released
    public void OnAttack(UnityEngine.InputSystem.InputValue value)
    {
        if (!useInputSystem)
        {
            return; // Skip if using traditional input
        }

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