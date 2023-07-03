using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject _bullet_container;
    [SerializeField] private GameObject _enemy_container;
    [SerializeField] private GameObject _enemy_blob;
    [SerializeField] private GameObject _player;

    private static int _enemy_count = 0;
    private static float _spawn_radius = 7f;
    private static int _level = 0;
    private IEnumerator _spawner;

    void Start()
    {
        _spawner = Spawner(10f);
        StartCoroutine(_spawner);
    }

    void Update()
    {
        if (_enemy_count < 1)
        {
            _level++;
            SpawnEnemy(_level);
        }
    }


    private float GenerateSpawnCoordinate()
    {
        return _spawn_radius * Random.Range(0.5f, 1);
    }
    public GameObject GetBulletContainer()
    {
        return _bullet_container;
    }
    
    public GameObject GetEnemyContainer()
    {
        return _enemy_container;
    }

    private void SpawnEnemy()
    {
        Vector3 relative_spawn_position = new Vector3(GenerateSpawnCoordinate(), GenerateSpawnCoordinate(), 0);
        SpawnEnemy(_player.transform.position + relative_spawn_position);
    }

    private void SpawnEnemy(Vector3 spawn_position)
    {
        GameObject enemy = Instantiate(_enemy_blob, spawn_position, Quaternion.identity);
        enemy.transform.SetParent(_enemy_container.transform);
        _enemy_count++;
    }

    private void SpawnEnemy(int count)
    {
        for (int i = 0; i < count; i++)
        {
            SpawnEnemy();
        }
    }

    private IEnumerator Spawner(float time_seconds)
    {
        while (true)
        {
            yield return new WaitForSeconds(time_seconds);
            SpawnEnemy(1);
        }
    }
    public static void EnemyDeath()
    {
        _enemy_count--;
    }
}
