using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingDummy : MonoBehaviour
{
    [SerializeField] GameObject trainingDummyPrefab;

    public void Respawn()
    {
        Instantiate(trainingDummyPrefab);
        Destroy(gameObject);
    }
}
