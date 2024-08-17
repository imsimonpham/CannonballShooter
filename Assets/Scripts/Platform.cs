using UnityEngine;


public class Platform : MonoBehaviour
{
    /*[SerializeField] private Transform _spawnPoint;
    [SerializeField] private Box _boxPrefab;
    [SerializeField] private int _maxSpawnCountPerRow;
    private float _spacing;*/

    /*private void Start()
    {
        // Get the size of the box prefab
        if (_boxPrefab.TryGetComponent<Renderer>(out Renderer renderer))
        {
            Vector3 objectSize = renderer.bounds.size;
            float objectLength = objectSize.z;
            _spacing = objectLength + 0.02f;
        }

        for (var i = 0; i < _maxSpawnCountPerRow; i++)
        {
            Vector3 spawnPosition = _spawnPoint.position + new Vector3(0, 0, i * _spacing);
            Box spawnedBox = Instantiate(_boxPrefab, spawnPosition, Quaternion.Euler(0, -20, 0));
        }
    }*/
}
