using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Spawner : MonoBehaviour
{
    public List<GameObject> spawnPoints = new List<GameObject>();
    public GameObject obj;
    private float spawnDelay = 0;
    public float spawnDelayMax = 2;

    // Update is called once per frame
    void Update()
    {
        spawnDelay += Time.deltaTime;
        if (spawnDelayMax < spawnDelay)
        {
            spawnDelay = 0;
            var randomIndex = Random.Range(0, spawnPoints.Count);
            Instantiate(obj, spawnPoints[randomIndex].transform.position, Quaternion.identity);

        }
    }
}
