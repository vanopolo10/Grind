using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class Spawner : MonoBehaviour
{
    private MeshRenderer _meshRenderer;
    private Vector3 _highestSurfaceCenter;

    public Vector3 EnemySpawnPoint => _highestSurfaceCenter;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _highestSurfaceCenter = GetHighestSurfaceCenter(_meshRenderer);
    }
    
    private Vector3 GetHighestSurfaceCenter(MeshRenderer meshRenderer)
    {
        Bounds bounds = meshRenderer.bounds;
        return new Vector3(bounds.center.x, bounds.max.y, bounds.center.z);
    }
}