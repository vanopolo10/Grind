using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private int _damage = 35;
    [SerializeField] private float _distance = 10;
    [SerializeField] private float _speed = 10f;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out Character character))
            character.TakeDamage(_damage);
        else if (other.gameObject.TryGetComponent<Enemy>(out _) == false)
            Destroy(gameObject);
    }

    public void BeginFly(Vector3 direction)
    {
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
            
            transform.position += direction.normalized * step;
            traveledDistance += step;

            yield return null;
        }
        
        Destroy(gameObject);
    }
}
