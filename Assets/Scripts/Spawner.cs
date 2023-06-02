using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Object _spawningObject;

    private Transform _transform;
    private List<Transform> _spawnLocations = new List<Transform>();
    private float _timeBetweenSpawn = 2;
    private float _timeSinceLastSpawn = 0;
    private int _currentSpawnPoint = 0;

    private void Start()
    {
        _transform= GetComponent<Transform>();

        foreach (Transform chield in transform)
        {
            if(chield.GetComponent(typeof(SpawnLocation)))
                _spawnLocations.Add(chield);
        }
    }

    private void Update()
    {
        _timeSinceLastSpawn += Time.deltaTime;

        if(_timeBetweenSpawn < _timeSinceLastSpawn)
        {
            _timeSinceLastSpawn = 0;
            Spawn(_spawnLocations[_currentSpawnPoint++ % _spawnLocations.Count].position);
        }
    }

    private void Spawn(Vector3 position)
    {
        Instantiate(_spawningObject, position, Quaternion.identity);
    }
}
