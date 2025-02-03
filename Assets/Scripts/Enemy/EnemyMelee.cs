using System.Collections;
using UnityEngine;

public class EnemyMelee : Enemy
{
    [SerializeField] private int _damage = 25;
    [SerializeField] private float _attackCooldown = 0.7f;
    private bool _canAttack = true;

    protected override IEnumerator Fight(Character character)
    {
        while (character != null)
        {
            _agent.SetDestination(character.transform.position);
            yield return null;
        }
    }

    private void OnCollisionStay(Collision other)
    {
        if (_canAttack && other.gameObject.TryGetComponent(out Character character))
        {
            StartCoroutine(Attack(character));
        }
    }

    private IEnumerator Attack(Character character)
    {
        _canAttack = false;
        character.TakeDamage(_damage);
        yield return new WaitForSeconds(_attackCooldown);
        _canAttack = true;
    }
}