using Mirror;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum MenuState { Home, InParty}
public class MainMenu : MonoBehaviour
{
    public static MainMenu instance;

    public MenuState state = MenuState.Home;
    [SerializeField] private GameObject homeUI, partyUI;

    [Header("Ready Button")]
    [SerializeField] private Image readyButton_Image;
    [SerializeField] private TMP_Text readyButton_Text;
    public Color readyColor, notReadyColor;
    private void Awake()
    {
        instance = this;
    }

    public void SetMenuState(MenuState state) 
    {   
        this.state = state;

        homeUI.SetActive(state == MenuState.Home);
        partyUI.SetActive(state == MenuState.InParty);
    }

    public void CreateParty() 
    {

        PopupManager.instance.Popup_Show("Creating Party");

        ((MyNetworkManager)NetworkManager.singleton).SetMultiplayer(true);
        SteamLobby.instance.CreateLobby();    
    }

    public void StartSinglePlayer()
    {
        LobbyController.instance.StartGameSolo();
    }

    public void LeaveParty() 
    {
        if (!NetworkClient.active) return;

        if(NetworkClient.localPlayer.isServer)
            NetworkManager.singleton.StopHost();
        else
            NetworkManager.singleton.StopClient();

        SteamLobby.instance.Leave();
    }

    public void FindMatch() 
    {
        SteamLobby.instance.FindMatch();
    }

    public void StartGame() 
    {
        LobbyController.instance.StartGameWithParty();
    }

    public void StartLocalClient()
    {
        ((MyNetworkManager)NetworkManager.singleton).SetMultiplayer(true);
        NetworkManager.singleton.StartClient();
    }

    public void StartLocalHost()
    {
        ((MyNetworkManager)NetworkManager.singleton).SetMultiplayer(true);
        NetworkManager.singleton.StartHost();
    }

    public void ToggleReady() 
    {
        if (!NetworkClient.active) return;

        NetworkClient.localPlayer.GetComponent<MyClient>().ToggleReady(); 
    }

    public void UpdateReadyButton(bool value) 
    {
        readyButton_Text.text = value ? "Ready" : "Not Ready";
        readyButton_Image.color = value ? readyColor : notReadyColor;
    }
}
