using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private bool useInputSystem = true; // Toggle between Input System and traditional Input
    
    private Camera mainCamera;
    private bool isShooting;
    private Mouse mouse;
    private WeaponSystem weaponSystem;
    
    private void Start()
    {
        mainCamera = Camera.main;
        mouse = Mouse.current;
        
        // Get the weapon system component
        weaponSystem = GetComponent<WeaponSystem>();
        if (weaponSystem == null)
        {
            Debug.LogError("WeaponSystem component not found on player!");
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
        
        if (isShooting && weaponSystem != null && weaponSystem.CanFire())
        {
            weaponSystem.Fire();
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
    
    // Method to get current weapon info (useful for UI)
    public string GetCurrentWeaponInfo()
    {
        if (weaponSystem != null)
        {
            return weaponSystem.GetCurrentWeaponInfo();
        }
        return "No Weapon System";
    }
} 