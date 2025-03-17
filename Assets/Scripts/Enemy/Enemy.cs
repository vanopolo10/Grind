using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public abstract class Enemy : MonoBehaviour, IDamagable
{
    [SerializeField] private float _speed = 5f;
    
    private Faction _faction = Faction.Enemy;
    private NavMeshAgent _agent;
    private Coroutine _moveCoroutine;

    protected NavMeshAgent Agent => _agent;
    protected abstract int Reward { get; }
    protected abstract int Health { get; set; }

    public event Action<Enemy, int> Died; 

    protected virtual void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = _speed;
    }
    
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    
    public Faction GetFaction() => _faction;
    
    public void StartFight(Character character)
    {
        if (_moveCoroutine != null)
            StopCoroutine(_moveCoroutine);

        _moveCoroutine = StartCoroutine(Fight(character));
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;

        if (Health <= 0)
        {
            Died?.Invoke(this, Reward);
            Destroy(gameObject);
        }
    }
    
    protected abstract IEnumerator Fight(Character character);
}