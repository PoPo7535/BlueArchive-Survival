using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.UI;

public class App : SimulationBehaviour
{
    // Start is called before the first frame update
    private NetworkRunner runner;
    public Button startBtn;
    public static App I;
    void Start()
    {
        Debug.Log(GameManager.I.gameObject);
        I = this;
        runner = GetComponent<NetworkRunner>();
        startBtn.onClick.AddListener(StartGame);
    }

    private async void StartGame()
    {
        var result = await runner.StartGame(new StartGameArgs()
        {
            GameMode = GameMode.AutoHostOrClient,
            SessionName = "test",
            IsOpen = true,
            SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>(),
            ObjectProvider = gameObject.AddComponent<NetworkObjectProviderDefault>() 
            
        });
        if (result.Ok)
        {
            if (runner.GameMode == GameMode.Host)
                await runner.LoadScene("1.TestScene");
        }
        else
            Debug.Log(result.ErrorMessage);
    }
}

