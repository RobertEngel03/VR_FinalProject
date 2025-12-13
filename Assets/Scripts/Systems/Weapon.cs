using System;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Guid WeaponID => Guid.NewGuid();
    public WeaponSO Data;

    public void Initialize(WeaponSO data)
    {
        Data = data;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Hit {other.name}");
        if (other.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.DealDamage(new DamageContext(null, Data.Damage, 0));
        }
    }
}
