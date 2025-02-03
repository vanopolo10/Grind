using System.Collections;
using UnityEngine;

public class RangedEnemy : Enemy
{
    [SerializeField] private float _stoppingDistance = 10f;
    [SerializeField] private float _retreatDistance = 5f;
    [SerializeField] private Projectile _projectilePrefab;
    [SerializeField] private float _attackCooldown = 0.7f;

    private bool _canAttack = true;

    protected override IEnumerator Fight(Character character)
    {
        while (enabled)
        {
            float distance = Vector3.Distance(transform.position, character.transform.position);

            if (distance > _stoppingDistance)
            {
                _agent.SetDestination(character.transform.position);
            }
            else if (distance < _retreatDistance)
            {
                Vector3 retreatDirection = transform.position - character.transform.position;
                retreatDirection.Normalize();

                Vector3 retreatTarget = transform.position + retreatDirection * _stoppingDistance;
                _agent.SetDestination(retreatTarget);
            }
            else
            {
                _agent.ResetPath();

                if (_canAttack)
                {
                    StartCoroutine(Shoot(character.transform.position));
                }
            }

            yield return null;
        }
    }

    private IEnumerator Shoot(Vector3 characterPosition)
    {       
        _canAttack = false;
        Projectile projectile = Instantiate(_projectilePrefab, transform.position, new Quaternion());
        projectile.BeginFly(characterPosition - transform.position);
        yield return new WaitForSeconds(_attackCooldown);
        _canAttack = true;
    }
}