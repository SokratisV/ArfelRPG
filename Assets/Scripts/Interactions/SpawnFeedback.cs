using System.Collections;
using UnityEngine;

public class SpawnFeedback : MonoBehaviour
{
    [SerializeField] GameObject feedbackObject;
    Animator animator;
    Coroutine spawnCoroutine;
    WaitForSeconds spawnDelay;
    Transform objectTransform;
    bool coroutineRunning = false;

    private void Awake()
    {
        Debug.Assert(feedbackObject, "Movement Feedback Object is not set.");
        spawnDelay = new WaitForSeconds(.3f); // Length of animation
        objectTransform = Instantiate(feedbackObject, new Vector3(500, 500, 500), Quaternion.identity).transform;
        animator = objectTransform.GetComponent<Animator>();
    }
    public void Spawn(Vector3 position, Vector3 normal)
    {
        if (coroutineRunning)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = StartCoroutine(_Spawn(position, normal));
        }
        else
        {
            spawnCoroutine = StartCoroutine(_Spawn(position, normal));
        }
    }
    private IEnumerator _Spawn(Vector3 position, Vector3 normal)
    {
        coroutineRunning = true;
        objectTransform.position = position;
        objectTransform.rotation = Quaternion.FromToRotation(Vector3.up, normal);
        animator.Play("MovementFeedback", -1, 0f);
        yield return spawnDelay;
        Despawn();
    }
    private void Despawn()
    {
        objectTransform.position = new Vector3(500, 500, 500);
        coroutineRunning = false;
    }
}
