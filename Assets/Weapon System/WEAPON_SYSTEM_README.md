# Modular Weapon System

This system replaces the old generic bullet pool with a modular weapon system where each weapon has its own bullet pool and can be easily swapped on the player.

## Key Components

### 1. ModularBullet.cs
- Replaces the old `Bullet.cs`
- Can be configured with different stats per weapon
- Handles collision detection and damage dealing
- Returns to its parent weapon's pool when done

### 2. BulletWeapon.cs
- Contains its own bullet pool
- Manages bullet spawning and configuration
- Can be configured with different firing patterns (single shot, burst, spread)
- Handles weapon-specific stats like fire rate, damage, bullet speed, etc.

### 3. WeaponSystem.cs
- Manages multiple weapons on the player
- Handles weapon switching (Tab key or number keys 1-4)
- Allows adding/removing weapons dynamically
- Provides events for UI updates

### 4. BulletStats (Serializable Class)
- Contains all bullet properties: speed, lifetime, damage, color, scale
- Can be easily configured in the inspector or via code

## Setup Instructions

### Basic Setup:
1. **Create a bullet prefab** with `ModularBullet` component and a `Collider2D` (set as trigger)
2. **Add `WeaponSystem` and `PlayerShooting` to your player**
3. **Create weapon prefabs** with `BulletWeapon` component
4. **Assign the weapon prefabs** to the WeaponSystem's available weapons list

### Programmatic Setup:
```csharp
// Create a weapon programmatically
GameObject weaponObj = new GameObject("My Weapon");
BulletWeapon weapon = weaponObj.AddComponent<BulletWeapon>();

// Configure the weapon
weapon.InitializeWeapon(
    "Rapid Blaster",           // Weapon name
    bulletPrefab,              // Bullet prefab reference
    WeaponPresets.RapidFireStats, // Bullet stats
    0.1f,                      // Fire rate
    1,                         // Bullets per shot
    0f                         // Spread angle
);

// Add to weapon system
weaponSystem.AddWeapon(weapon);
```

### Using Presets:
```csharp
// Use predefined weapon configurations
BulletStats rapidStats = WeaponPresets.RapidFireStats;
BulletStats heavyStats = WeaponPresets.HeavyBulletStats;
BulletStats shotgunStats = WeaponPresets.ShotgunStats;
```

## Weapon Types Examples

### Basic Blaster
- Single shot
- Medium speed and damage
- White bullets

### Rapid Blaster
- Fast fire rate
- Lower damage per shot
- Yellow bullets

### Heavy Cannon
- Slow fire rate
- High damage
- Large red bullets

### Shotgun
- Multiple bullets per shot (5)
- Spread pattern (30° spread)
- Orange bullets

### Laser Rifle
- Very fast bullets
- Thin, long bullets
- Cyan color

## Controls

- **Mouse/Touch**: Fire weapon
- **Tab**: Cycle through weapons
- **1-4 Keys**: Direct weapon selection

## Weapon Swapping

Weapons can be easily swapped by:
1. **Replacing the entire weapon**: `weaponSystem.ReplaceCurrentWeapon(newWeapon)`
2. **Adding new weapons**: `weaponSystem.AddWeapon(newWeapon)`
3. **Upgrading stats**: `weapon.UpgradeStats(newStats)`

## Damage System

The system includes an `IDamageable` interface:
```csharp
public interface IDamageable
{
    void TakeDamage(float damage);
}
```

Any object that should take damage from bullets should implement this interface.

## Migration from Old System

1. Replace `Bullet.cs` with `ModularBullet.cs` on bullet prefabs
2. Remove `GenericBulletPool` from the scene
3. Add `WeaponSystem` to the player
4. Update `PlayerShooting` to use the new system (already done)
5. Create weapon prefabs with `BulletWeapon` components

## Benefits

- **Modular**: Each weapon is self-contained
- **Swappable**: Easy to change weapons at runtime
- **Configurable**: Rich configuration options per weapon
- **Scalable**: Easy to add new weapon types
- **Performance**: Each weapon manages its own pool efficiently
- **Flexible**: Supports various firing patterns (single, burst, spread) 