using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed;

    private Transform _target;
    private int _lifeTime = 5;

    public event Action<Enemy> LifeTimeEnded;

    private void OnEnable()
    {
        StartCoroutine(WaitForLifeTimeEnd());
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _target.position, _speed * Time.deltaTime);
    }

    public void Initialize(Transform target, Transform startPoint)
    {
        _target = target;
        transform.position = startPoint.position;
    }

    private IEnumerator WaitForLifeTimeEnd()
    {
        WaitForSeconds _wait = new WaitForSeconds(_lifeTime);

        yield return _wait;

        LifeTimeEnded?.Invoke(this);
    }
}