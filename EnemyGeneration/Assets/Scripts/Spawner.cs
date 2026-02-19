using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    private const float Delay = 2;

    [SerializeField] private Enemy _enemy;
    [SerializeField] private int _poolCapacity = 5;
    [SerializeField] private int _poolMaxSize = 5;
    [SerializeField] private GameObject _startPoint;
    [SerializeField] private Target _target;

    private ObjectPool<Enemy> _pool;

    private bool _isOpen = true;

    private WaitForSeconds _wait = new WaitForSeconds(Delay);

    private void Awake() 
    {
    _pool = new ObjectPool<Enemy>(
        createFunc: () => { Enemy enemy = Instantiate(_enemy);
        return enemy;},
        actionOnGet: (enemy) => PrepareObject(enemy),
        actionOnRelease: (enemy) => enemy.gameObject.SetActive(false),
        actionOnDestroy: (enemy) => Destroy(enemy.gameObject),
        collectionCheck: true,
        defaultCapacity: _poolCapacity,
        maxSize: _poolMaxSize);
    }

    private void Start()
    {
        StartCoroutine(SpawnObject());
    }

    private void PrepareObject(Enemy enemy)
    {
        enemy.ResetPosition();
        enemy.gameObject.SetActive(true);
    }

    private void EstablishSpawnPoint(Enemy enemy)
    {
        enemy.transform.position = _startPoint.transform.position;
    }

    private void ReturnEnemyPool(Enemy enemy)
    {
        enemy.LifeTimeEnded -= ReturnEnemyPool;
        _pool.Release(enemy);
    }

    private IEnumerator SpawnObject()
    {
        while (_isOpen)
        {
            if(_startPoint != null)
            {
                Enemy enemy = _pool.Get();
                EstablishSpawnPoint(enemy);
                enemy.EstablishTarget(_target.transform);
                enemy.LifeTimeEnded += ReturnEnemyPool;
            }

            yield return _wait;
        }
    }
}