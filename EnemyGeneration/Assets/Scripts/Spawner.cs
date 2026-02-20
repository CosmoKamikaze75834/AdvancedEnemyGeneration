using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    private const float Delay = 2;

    [SerializeField] private Enemy _enemy;
    [SerializeField] private int _poolCapacity = 5;
    [SerializeField] private int _poolMaxSize = 5;
    [SerializeField] private StartPoint _startPoint;
    [SerializeField] private Target _target;

    private ObjectPool<Enemy> _pool;

    private bool _isOpen = true;

    private WaitForSeconds _wait = new WaitForSeconds(Delay);

    private void Awake() 
    {
    _pool = new ObjectPool<Enemy>(
        createFunc: () => Instantiate(_enemy),
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
        enemy.gameObject.SetActive(true);
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
                enemy.Initialize(_target.transform, _startPoint.transform);
                enemy.LifeTimeEnded += ReturnEnemyPool;
            }

            yield return _wait;
        }
    }
}