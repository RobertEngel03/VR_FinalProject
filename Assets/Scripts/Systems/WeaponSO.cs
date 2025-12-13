using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Scriptables/Weapon")]
public class WeaponSO : ScriptableObject
{
    public string WeaponName;
    public float Damage;

    public GameObject Prefab;
}
