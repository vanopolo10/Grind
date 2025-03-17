using System;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer), typeof(Collider))]
public class Gates : MonoBehaviour
{
    [SerializeField] private Collider _colliderSolid;
    [SerializeField] private Collider _colliderTrigger;
    
    private MeshRenderer _meshRenderer;
    
    public event Action HeroEntered;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Character>(out _))
            HeroEntered?.Invoke();
    }

    public void Activate()
    {
        _meshRenderer.enabled = true;
        _colliderSolid.enabled = true;
        _colliderTrigger.enabled = false;
    }

    public void Deactivate()
    {
        _meshRenderer.enabled = false;
        _colliderSolid.enabled = false;
        _colliderTrigger.enabled = true;
    }
}
