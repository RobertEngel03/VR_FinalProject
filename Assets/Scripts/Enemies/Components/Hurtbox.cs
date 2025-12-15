using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    private new BoxCollider collider;
    private DamageContext _currentContext;

    private void Awake()
    {
        collider = GetComponent<BoxCollider>();
    }

    public void Enable(DamageContext context)
    {
        _currentContext = context;
        collider.enabled = true;
    }
    public void Disable() => collider.enabled = false;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Hit {other.name}");
        if (other.TryGetComponent<IDamageable>(out var damageable))
        {
            // TEMPORARY FIX
            if (_currentContext == null) _currentContext = new DamageContext(null, 5, 0);
            damageable.DealDamage(_currentContext);
        }
    }
}
