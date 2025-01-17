using System;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer), typeof(Collider))]
public class Door : MonoBehaviour
{
    private MeshRenderer _meshRenderer;
    private Collider _collider;
    
    public event Action HeroEntered;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _collider = GetComponent<Collider>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
            HeroEntered?.Invoke();
    }

    public void Activate()
    {
        _meshRenderer.enabled = true;
        _collider.enabled = true;
    }

    public void Deactivate()
    {
        _meshRenderer.enabled = false;
        _collider.enabled = false;
    }
}
