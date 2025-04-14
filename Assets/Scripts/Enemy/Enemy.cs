using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public abstract class Enemy : MonoBehaviour, IDamagable
{
    [SerializeField] private float _speed = 5f;
    [SerializeField] private int _health;
    [SerializeField] private int _reward;
    
    private static readonly int Speed = Animator.StringToHash("speed");
    
    private Faction _faction = Faction.Enemy;
    private NavMeshAgent _agent;
    private Coroutine _moveCoroutine;
    private Animator _animator;

    protected NavMeshAgent Agent => _agent;

    public event Action<Enemy, int> Died; 
    
    protected void OnEnable()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = _speed;
        _animator = GetComponent<Animator>();
        StartCoroutine(Animate());
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
        _health -= damage;

        if (_health <= 0)
        {
            Died?.Invoke(this, _reward);
            Destroy(gameObject);
        }
    }

    private IEnumerator Animate()
    {
        while (enabled)
        {
            _animator.SetFloat(Speed, _agent.speed);
            yield return null;
        }
    }
    
    protected abstract IEnumerator Fight(Character character);
}