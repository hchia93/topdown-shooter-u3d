using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class WeaponSystem : MonoBehaviour
{
    [Header("Weapon Management")]
    [SerializeField] private List<BulletWeapon> availableWeapons = new List<BulletWeapon>();
    [SerializeField] private int currentWeaponIndex = 0;
    [SerializeField] private bool allowWeaponSwitching = true;
    
    private Keyboard keyboard;
    
    private BulletWeapon currentWeapon;
    private bool isInitialized = false;
    
    public BulletWeapon CurrentWeapon => currentWeapon;
    public int WeaponCount => availableWeapons.Count;
    public int CurrentWeaponIndex => currentWeaponIndex;
    
    // Events for UI updates
    public System.Action<BulletWeapon> OnWeaponChanged;
    
    private void Start()
    {
        keyboard = Keyboard.current;
        InitializeWeapons();
        
        // Delay weapon switching to ensure everything is properly initialized
        Invoke(nameof(DelayedWeaponSwitch), 0.1f);
    }
    
    private void DelayedWeaponSwitch()
    {
        SwitchToWeapon(currentWeaponIndex);
    }
    
    private void Update()
    {
        if (!allowWeaponSwitching) return;
        
        HandleWeaponSwitching();
    }
    
    private void InitializeWeapons()
    {
        if (isInitialized)
        {
            Debug.LogWarning("WeaponSystem: Already initialized, skipping...");
            return;
        }
        
        Debug.Log($"WeaponSystem: Starting initialization with {availableWeapons.Count} weapons in list");
        
        // Ensure all weapons are properly set up
        for (int i = 0; i < availableWeapons.Count; i++)
        {
            if (availableWeapons[i] == null)
            {
                Debug.LogWarning($"WeaponSystem: Weapon at index {i} is null!");
                continue;
            }
            
            Debug.Log($"WeaponSystem: Found weapon {i}: {availableWeapons[i].WeaponName}");
            
            // Don't deactivate weapons initially - let them stay as they are
            // availableWeapons[i].gameObject.SetActive(false);
        }
        
        // Remove null entries
        int nullCount = availableWeapons.RemoveAll(weapon => weapon == null);
        if (nullCount > 0)
        {
            Debug.LogWarning($"WeaponSystem: Removed {nullCount} null weapon references");
        }
        
        if (availableWeapons.Count == 0)
        {
            Debug.LogError("WeaponSystem: No weapons available! Make sure to assign weapon prefabs in the Available Weapons array.");
        }
        else
        {
            Debug.Log($"WeaponSystem: Successfully initialized {availableWeapons.Count} weapons");
            isInitialized = true;
        }
    }
    
    private void HandleWeaponSwitching()
    {
        if (keyboard == null) return;
        
        // Cycle through weapons with Tab key
        if (keyboard.tabKey.wasPressedThisFrame)
        {
            SwitchToNextWeapon();
        }
        
        // Direct weapon selection with number keys
        if (keyboard.digit1Key.wasPressedThisFrame && availableWeapons.Count > 0)
            SwitchToWeapon(0);
        else if (keyboard.digit2Key.wasPressedThisFrame && availableWeapons.Count > 1)
            SwitchToWeapon(1);
        else if (keyboard.digit3Key.wasPressedThisFrame && availableWeapons.Count > 2)
            SwitchToWeapon(2);
        else if (keyboard.digit4Key.wasPressedThisFrame && availableWeapons.Count > 3)
            SwitchToWeapon(3);
    }
    
    public void SwitchToNextWeapon()
    {
        if (availableWeapons.Count <= 1) return;
        
        int nextIndex = (currentWeaponIndex + 1) % availableWeapons.Count;
        SwitchToWeapon(nextIndex);
    }
    
    public void SwitchToPreviousWeapon()
    {
        if (availableWeapons.Count <= 1) return;
        
        int prevIndex = (currentWeaponIndex - 1 + availableWeapons.Count) % availableWeapons.Count;
        SwitchToWeapon(prevIndex);
    }
    
    public void SwitchToWeapon(int weaponIndex)
    {
        if (weaponIndex < 0 || weaponIndex >= availableWeapons.Count)
        {
            Debug.LogWarning($"Invalid weapon index: {weaponIndex}");
            return;
        }
        
        // Check if weapon still exists
        if (availableWeapons[weaponIndex] == null)
        {
            Debug.LogError($"WeaponSystem: Weapon at index {weaponIndex} has become null! Something destroyed it.");
            return;
        }
        
        // Deactivate current weapon
        if (currentWeapon != null && currentWeapon != availableWeapons[weaponIndex])
        {
            currentWeapon.gameObject.SetActive(false);
        }
        
        // Switch to new weapon
        currentWeaponIndex = weaponIndex;
        currentWeapon = availableWeapons[currentWeaponIndex];
        
        if (currentWeapon == null)
        {
            Debug.LogError("WeaponSystem: Current weapon is null after assignment!");
            return;
        }
        
        // Only activate if not already active
        if (!currentWeapon.gameObject.activeInHierarchy)
        {
            currentWeapon.gameObject.SetActive(true);
        }
        
        // Notify listeners
        OnWeaponChanged?.Invoke(currentWeapon);
        
        Debug.Log($"Switched to weapon: {currentWeapon.WeaponName}");
    }
    
    public void Fire()
    {
        if (currentWeapon == null)
        {
            Debug.LogWarning("WeaponSystem: Cannot fire - current weapon is null!");
            return;
        }
        
        Debug.Log($"WeaponSystem: Firing weapon: {currentWeapon.WeaponName}");
        currentWeapon.Fire();
    }
    
    public bool CanFire()
    {
        return currentWeapon != null && currentWeapon.CanFire();
    }
    
    // Method to add a new weapon (useful for pickups)
    public void AddWeapon(BulletWeapon newWeapon)
    {
        if (newWeapon == null) return;
        
        // Set the weapon as a child of this transform
        newWeapon.transform.SetParent(transform);
        newWeapon.transform.localPosition = Vector3.zero;
        newWeapon.transform.localRotation = Quaternion.identity;
        
        // Add to available weapons
        availableWeapons.Add(newWeapon);
        newWeapon.gameObject.SetActive(false);
        
        Debug.Log($"Added weapon: {newWeapon.WeaponName}");
    }
    
    // Method to remove a weapon
    public void RemoveWeapon(int weaponIndex)
    {
        if (weaponIndex < 0 || weaponIndex >= availableWeapons.Count) return;
        
        BulletWeapon weaponToRemove = availableWeapons[weaponIndex];
        availableWeapons.RemoveAt(weaponIndex);
        
        // If we removed the current weapon, switch to another one
        if (weaponIndex == currentWeaponIndex)
        {
            if (availableWeapons.Count > 0)
            {
                int newIndex = Mathf.Min(currentWeaponIndex, availableWeapons.Count - 1);
                SwitchToWeapon(newIndex);
            }
            else
            {
                currentWeapon = null;
                currentWeaponIndex = 0;
            }
        }
        else if (weaponIndex < currentWeaponIndex)
        {
            // Adjust current weapon index if we removed a weapon before it
            currentWeaponIndex--;
        }
        
        // Destroy the weapon
        if (weaponToRemove != null)
        {
            Destroy(weaponToRemove.gameObject);
        }
    }
    
    // Method to replace current weapon (useful for weapon upgrades)
    public void ReplaceCurrentWeapon(BulletWeapon newWeapon)
    {
        if (newWeapon == null || availableWeapons.Count == 0) return;
        
        int currentIndex = currentWeaponIndex;
        RemoveWeapon(currentIndex);
        
        // Insert new weapon at the same position
        newWeapon.transform.SetParent(transform);
        newWeapon.transform.localPosition = Vector3.zero;
        newWeapon.transform.localRotation = Quaternion.identity;
        
        availableWeapons.Insert(currentIndex, newWeapon);
        SwitchToWeapon(currentIndex);
        
        Debug.Log($"Replaced weapon with: {newWeapon.WeaponName}");
    }
    
    // Get weapon info for UI
    public string GetCurrentWeaponInfo()
    {
        if (currentWeapon == null) return "No Weapon";
        
        return $"{currentWeapon.WeaponName} ({currentWeaponIndex + 1}/{availableWeapons.Count})";
    }
} 