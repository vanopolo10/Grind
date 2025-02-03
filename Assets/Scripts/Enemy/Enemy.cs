using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected float _speed = 5f;
    [SerializeField] protected float _lifeTime = 5f;

    protected NavMeshAgent _agent;
    private Coroutine _moveCoroutine;
    
    protected virtual void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = _speed;
    }
    
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    
    // private void OnCollisionEnter(Collision other)
    // {
    //     if (other.gameObject.CompareTag("Player"))
    //         Destroy(gameObject);
    // }
    
    public void StartFight(Character character)
    {
        if (_moveCoroutine != null)
            StopCoroutine(_moveCoroutine);

        StartCoroutine(Decay());
        _moveCoroutine = StartCoroutine(Fight(character));
    }

    private IEnumerator Decay()
    {
        yield return new WaitForSeconds(_lifeTime);
        Destroy(gameObject);
    }

    protected abstract IEnumerator Fight(Character character);
}