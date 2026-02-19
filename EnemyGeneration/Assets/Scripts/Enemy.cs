using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed;

    private Rigidbody _rigidbody;
    private Transform _target;
    private int _lifeTime = 5;

    public event Action<Enemy> LifeTimeEnded;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _target.position, _speed * Time.deltaTime);
    }

    private void OnEnable()
    {
        StartCoroutine(WaitForLifeTimeEnd());
    }

    public void ResetPosition()
    {
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        _rigidbody.rotation = Quaternion.identity;
    }

    public void EstablishTarget(Transform target)
    {
        _target = target;
    }

    private IEnumerator WaitForLifeTimeEnd()
    {
        WaitForSeconds _wait = new WaitForSeconds(_lifeTime);

        yield return _wait;

        LifeTimeEnded?.Invoke(this);
    }
}