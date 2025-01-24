using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;

    private Coroutine _moveCoroutine;
    
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
            Destroy(gameObject);
    }
    
    public void StartFight(Transform targetTransform)
    {
        if (_moveCoroutine != null)
            StopCoroutine(_moveCoroutine);
        
        _moveCoroutine = StartCoroutine(Fight(targetTransform));
    }

    private IEnumerator Fight(Transform targetTransform)
    {
        bool isActive = true;
        
        while (isActive)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetTransform.position, _speed * Time.deltaTime);
            yield return null;
        }
    }
}