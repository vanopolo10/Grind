using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private float _distance = 10;
    [SerializeField] private float _speed = 10f;

    private Faction _shooterFaction;
    private int _damage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<IDamagable>(out var target) && target.GetFaction() != _shooterFaction)
        {
            target.TakeDamage(_damage);
            Destroy(gameObject);
        }
    }

    public void BeginFly(Vector3 direction, int damage, Faction shooterFaction)
    {
        _shooterFaction = shooterFaction;
        _damage = damage;
        StartCoroutine(Fly(direction));
    }
    
    private IEnumerator Fly(Vector3 direction)
    {
        float traveledDistance = 0f;

        while (traveledDistance < _distance)
        {
            float step = _speed * Time.deltaTime;
            float remainingDistance = _distance - traveledDistance;
            step = Mathf.Min(step, remainingDistance);
        
            transform.position += direction * step;
            traveledDistance += step;

            yield return null;
        }

        Destroy(gameObject);
    }
}
