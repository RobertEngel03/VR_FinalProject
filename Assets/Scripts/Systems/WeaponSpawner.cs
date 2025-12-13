using UnityEngine;

public class WeaponSpawner : MonoBehaviour
{
    public Transform SpawnLocation;

    public void SpawnWeapon(WeaponSO weaponSO)
    {
        GameObject spawnedWeapon = Instantiate(weaponSO.Prefab, SpawnLocation);
        var weapon = spawnedWeapon.AddComponent<Weapon>();
        weapon.Initialize(weaponSO);
    }
}
