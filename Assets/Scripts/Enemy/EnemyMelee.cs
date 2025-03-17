using System;
using System.Collections;
using UnityEngine;

public class EnemyMelee : Enemy
{
    [SerializeField] private int _damage = 10;
    [SerializeField] private float _attackCooldown = 0.7f;
    private bool _canAttack = true;

    protected override int Reward => 5;
    protected override int Health { get; set; } = 100;

    private void OnCollisionStay(Collision other)
    {
        if (_canAttack && other.gameObject.TryGetComponent(out Character character))
            StartCoroutine(Attack(character));
    }
    
    protected override IEnumerator Fight(Character character)
    {
        while (character != null)
        {
            Agent.SetDestination(character.transform.position);
            yield return null;
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