using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Character : MonoBehaviour, IDamagable
{
    [SerializeField] private int _maxHealth = 10;
    [SerializeField] private int _damage = 1;
    [SerializeField] private float _attackRadius = 2;
    [SerializeField] private Projectile _projectilePrefab;
    [SerializeField] private float _attackCooldown = 1f;

    private int _health;
    private int _attackIncreaseBy = 1;
    private int _healthIncreaseBy = 2;
    private float _attackCooldownDecreaseBy = 0.01f;
    
    private bool _canAttack = true;

    public int Damage => _damage;
    public int Health => _health;
    public int MaxHealth => _maxHealth;
    public float AttackSpeed => _attackCooldown;

    public event Action<int> HealthChanged;
    public event Action Attacked;

    private void Awake()
    {
        _health = _maxHealth;
    }

    private void Start()
    {
        StartCoroutine(Fight());
    }

    public Faction GetFaction() => Faction.Player;

    public void TakeDamage(int damage)
    {
        _health -= damage;
        
        if (_health <= 0)
        {
            HealthChanged?.Invoke(0);
            Die();
        }
        
        HealthChanged?.Invoke(_health);
    }

    public void IncreaseStat(Upgrades upgrade)
    {
        switch (upgrade)
        {
            case Upgrades.Damage:
                _damage += _attackIncreaseBy;
                break;
            
            case Upgrades.Health:
                UpgradeHealth();
                break;
            
            case Upgrades.AttackSpeed:
                _attackCooldown -= _attackCooldownDecreaseBy;
                break;
            
            default:
                print("Unknown upgrade");
                break;
        }
    }

    private void UpgradeHealth()
    {
        _maxHealth += _healthIncreaseBy;
        _health += _healthIncreaseBy;
        HealthChanged?.Invoke(_health);
    }

    private IEnumerator Fight()
    {
        while (enabled)
        {
            if (_canAttack)
            {
                Enemy closestEnemy = FindClosestEnemy();
                
                if (closestEnemy)
                {
                    StartCoroutine(Shoot(closestEnemy.transform.position));
                }
            }
            
            yield return null;
        }
    }
    
    private Enemy FindClosestEnemy()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _attackRadius);
    
        Enemy closestEnemy = null;
        float closestDistance = float.MaxValue;

        foreach (Collider collider in colliders)
        {
            if (collider.TryGetComponent(out Enemy enemy))
            {
                float distance = Vector3.Distance(transform.position, collider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = enemy;
                }
            }
        }
        
        return closestEnemy;
    }
    
    private IEnumerator Shoot(Vector3 targetPosition)
    {
        _canAttack = false;
        Projectile projectile = Instantiate(_projectilePrefab, transform.position, new Quaternion());
        projectile.BeginFly((targetPosition - transform.position).normalized, _damage, GetFaction());
        Attacked?.Invoke();
        yield return new WaitForSeconds(_attackCooldown);
        _canAttack = true;
    }

    private void Die()
    {
        gameObject.SetActive(false);
    }
}
