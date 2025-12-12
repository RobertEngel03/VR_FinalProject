using UnityEngine;

public class WeaponButtonSpawner : MonoBehaviour
{
    [Header("Assign your 4 weapon prefabs here (size must be 4)")]
    public GameObject[] weaponPrefabs = new GameObject[4];

    [Header("Where the weapon spawns")]
    public Transform spawnPoint;

    [Header("Optional: destroy previous spawned weapon")]
    public bool destroyPrevious = true;

    private GameObject _lastSpawned;

    public void SpawnWeapon(int index)
    {
        if (weaponPrefabs == null || weaponPrefabs.Length < 4)
        {
            Debug.LogError("weaponPrefabs must have 4 entries.");
            return;
        }

        if (spawnPoint == null)
        {
            Debug.LogError("SpawnPoint is not assigned.");
            return;
        }

        if (index < 0 || index >= weaponPrefabs.Length || weaponPrefabs[index] == null)
        {
            Debug.LogError($"Weapon prefab at index {index} is missing.");
            return;
        }

        if (destroyPrevious && _lastSpawned != null)
            Destroy(_lastSpawned);

        _lastSpawned = Instantiate(weaponPrefabs[index], spawnPoint.position, spawnPoint.rotation);
    }

    // Convenience methods for the 4 buttons:
    public void Spawn0() => SpawnWeapon(0);
    public void Spawn1() => SpawnWeapon(1);
    public void Spawn2() => SpawnWeapon(2);
    public void Spawn3() => SpawnWeapon(3);
}
