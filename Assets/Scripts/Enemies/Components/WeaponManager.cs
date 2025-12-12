using System.Collections;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    private Agent _agent;

    private EnemyAttackState _currentAttack;

    public EnemyHurtbox WeaponHurtbox;

    public float GlobalCooldown = 2f;
    [SerializeField] private float _timer;

    [field: SerializeField] public bool ready { get; private set; }

    private void Awake()
    {
        _agent = GetComponent<Agent>();
    }

    private void Start()
    {
        _timer = GlobalCooldown;    
    }

    private void Update()
    {
        _timer += Time.deltaTime;

        if (_timer >= GlobalCooldown) ready = true;
        else ready = false;    
    }

    public void PlayAttack(EnemyAttackState attackSO, float duration)
    {
        _timer = 0f;

        _currentAttack = attackSO;
        StartCoroutine(ToggleHurtbox(duration));
    }

    private IEnumerator ToggleHurtbox(float duration)
    {
        WeaponHurtbox.Enable(new DamageContext(_agent, _currentAttack.damage, _currentAttack.knockbackForce));

        yield return new WaitForSeconds(duration);

        WeaponHurtbox.Disable();
    }
}
