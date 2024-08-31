using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class Spawner : SimulationBehaviour, IPlayerJoined
{
    public List<GameObject> spawnPoints = new List<GameObject>();
    public GameObject obj;
    private float spawnDelay = 0;
    public float spawnDelayMax = 2;

    // Update is called once per frame
    void Update()
    {
        // spawnDelay += Time.deltaTime;
        // if (spawnDelayMax < spawnDelay)
        // {
        //     spawnDelay = 0;
        //     var randomIndex = Random.Range(0, spawnPoints.Count);
        //     Instantiate(obj, spawnPoints[randomIndex].transform.position, Quaternion.identity);
        //
        // }
    }

    public void PlayerJoined(PlayerRef player)
    {
        Debug.Log("asdasdasdasdasdasdasdasdasdasdassasdadsasdasdasdasdasdasdsda");
        if (player == Runner.LocalPlayer)
        {
            var obj = Runner.Spawn(GameManager.I.kayoko, new Vector3(0, 1, 0), Quaternion.identity);
            GameManager.I.player = obj.gameObject;
        }
    }
}
