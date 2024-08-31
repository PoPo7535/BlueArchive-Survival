using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class App : SimulationBehaviour, IPlayerJoined
{
    // Start is called before the first frame update
    private NetworkRunner runner;
    public Button startBtn;

    void Start()
    {
        Debug.Log(GameManager.I.gameObject);
        runner = GetComponent<NetworkRunner>();
        startBtn.onClick.AddListener(StartG);
    }

    private async void StartG()
    {
        var result = await runner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.Shared,
            SessionName = "test",
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>(),
            ObjectProvider = gameObject.AddComponent<NetworkObjectProviderDefault>() 
            
        });
        if (result.Ok)
        {
            await runner.LoadScene("1.TestScene");
        }
        else
            Debug.Log(result.ErrorMessage);
    }
    public void PlayerJoined(PlayerRef player)
    {
        Debug.Log("asdasdasdasdasdasdasdasdasdasdassasdadsasdasdasdasdasdasdsda");
        if (player == Runner.LocalPlayer)
        {
            var obj = Runner.Spawn(
                GameManager.I.kayoko, 
                new Vector3(0, 1, 0), 
                Quaternion.identity,player);
            GameManager.I.player = obj.gameObject;
        }
    }
}
