using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyController : MonoBehaviour
{
    public static LobbyController instance;

    private void Awake()
    {
        instance = this;
    }


    public void StartGameWithParty() 
    {
        if (AllPlayersReady()) 
        {
            StartGame();
        }
    }


    public void StartGameSolo()
    {
        StartCoroutine(StartSinglePlayer());
    }
    IEnumerator StartSinglePlayer() 
    {
        NetworkManager.singleton.StartHost();

        while(NetworkClient.localPlayer == null)
            yield return new WaitForEndOfFrame();

        ((MyNetworkManager)NetworkManager.singleton).SetMultiplayer(false);
        StartGame();
    }

    private void StartGame()
    {
        NetworkManager.singleton.ServerChangeScene("Stage_1");
    }

    private bool AllPlayersReady() 
    {
        foreach (MyClient client in ((MyNetworkManager)NetworkManager.singleton).allClients)
            if (!client.IsReady)
                return false;
        return true;
    }
}
