using System.Collections;
using UnityEngine;

public class SpawnFeedback : MonoBehaviour
{
    [SerializeField] private GameObject feedbackObject;
    private Animator _animator;
    private Coroutine _spawnCoroutine;
    private WaitForSeconds _spawnDelay;
    private Transform _objectTransform;
    private bool _coroutineRunning = false;

    private void Awake()
    {
        Debug.Assert(feedbackObject, "Movement Feedback Object is not set.");
        _spawnDelay = new WaitForSeconds(.3f); // Length of animation
        _objectTransform = Instantiate(feedbackObject, new Vector3(500, 500, 500), Quaternion.identity).transform;
        _animator = _objectTransform.GetComponent<Animator>();
    }

    public void Spawn(Vector3 position, Vector3 normal)
    {
        if(_coroutineRunning)
        {
            StopCoroutine(_spawnCoroutine);
            _spawnCoroutine = StartCoroutine(_Spawn(position, normal));
        }
        else
        {
            _spawnCoroutine = StartCoroutine(_Spawn(position, normal));
        }
    }

    private IEnumerator _Spawn(Vector3 position, Vector3 normal)
    {
        _coroutineRunning = true;
        _objectTransform.position = position;
        _objectTransform.rotation = Quaternion.FromToRotation(Vector3.up, normal);
        _animator.Play("MovementFeedback", -1, 0f);
        yield return _spawnDelay;
        Despawn();
    }

    private void Despawn()
    {
        _objectTransform.position = new Vector3(500, 500, 500);
        _coroutineRunning = false;
    }
}