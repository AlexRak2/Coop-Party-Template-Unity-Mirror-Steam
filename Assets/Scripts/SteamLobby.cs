using Mirror;
using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Lobby
{
    public CSteamID lobbyID;
    public string name;

    public Lobby(CSteamID lobbyID, string name)
    {
        this.lobbyID = lobbyID;
        this.name = name;
    }
}

public class SteamLobby : MonoBehaviour
{
    private const string HOST_ADDRESS_KEY = "HostAddress";
                      
    public static SteamLobby instance;
    public static CSteamID LobbyID;

    public List<Lobby> allLobbies = new List<Lobby>();

    //CallBacks
    protected Callback<LobbyCreated_t> lobbyCreated;
    protected Callback<GameLobbyJoinRequested_t> joinRequested;
    protected Callback<LobbyEnter_t> lobbyEntered;
    protected Callback<LobbyMatchList_t> lobbyMatchList;


    private void Awake()
    {
        if (instance == null)
            instance = this;

            lobbyMatchList = Callback<LobbyMatchList_t>.Create(OnLobbyMatchList);
    }

    private void Start()
    {
        if (!SteamManager.Initialized) return;

        lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
        joinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnJoinRequest);
        lobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);

        ReloadLobbyList();
    }


    #region Steam Lobby
    public void ReloadLobbyList()
    {        
        allLobbies.Clear();

        SteamMatchmaking.AddRequestLobbyListDistanceFilter(ELobbyDistanceFilter.k_ELobbyDistanceFilterWorldwide);
        SteamMatchmaking.AddRequestLobbyListStringFilter("displayable", "true", ELobbyComparison.k_ELobbyComparisonEqual);
        SteamMatchmaking.RequestLobbyList();
    }
    void OnLobbyMatchList(LobbyMatchList_t param)
    {
        for (int i = 0; i < param.m_nLobbiesMatching; i++)
        {                        
            CSteamID lobbyID = SteamMatchmaking.GetLobbyByIndex(i);
            CSteamID ownerID = SteamMatchmaking.GetLobbyOwner(lobbyID);

            allLobbies.Add(new Lobby(lobbyID, SteamMatchmaking.GetLobbyData(lobbyID, "name")));
        }

        allLobbies.Sort((a, b) => SteamMatchmaking.GetNumLobbyMembers(b.lobbyID).CompareTo(SteamMatchmaking.GetNumLobbyMembers(a.lobbyID)));

        /*for (int i = 0; i < allLobbies.Count; i++)
        {

            if (SteamMatchmaking.GetLobbyData(allLobbies[i].lobbyID, "displayable") == "true")
            {
                var lobbyElement = Instantiate(MainMenu.instance.lobbyElementPrefab, MainMenu.instance.lobbyListContainer).GetComponent<LobbyElement>();
                lobbyElement.Initialize(allLobbies[i]);
                allLobbies[i].listElement = lobbyElement.gameObject;
            }
        }*/
    }


    public void CreateLobby()
    {
        SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypePublic, ((MyNetworkManager)NetworkManager.singleton).maxConnections);
    }


    public void JoinLobby(CSteamID lobby)
    {

        SteamMatchmaking.JoinLobby(lobby);
    }


    private void OnLobbyCreated(LobbyCreated_t callback)
    {
        if (callback.m_eResult != EResult.k_EResultOK) { return; }

        string lobbyName = SteamFriends.GetFriendPersonaName(SteamUser.GetSteamID());

        LobbyID = new CSteamID(callback.m_ulSteamIDLobby);


        ((MyNetworkManager)NetworkManager.singleton).StartHost();

        SteamMatchmaking.SetLobbyData(LobbyID, HOST_ADDRESS_KEY, SteamUser.GetSteamID().ToString());
        SteamMatchmaking.SetLobbyData(LobbyID, "name", lobbyName);
        SteamMatchmaking.SetLobbyData(LobbyID, "displayable", "true");

        SetLobbyLocation();
    }


    private void OnJoinRequest(GameLobbyJoinRequested_t callback)
    {
        PopupManager.instance.Popup_Show("Joining Party");
        SteamMatchmaking.JoinLobby(callback.m_steamIDLobby);
    }

    private void OnLobbyEntered(LobbyEnter_t callback)
    {
        //if lobby full leave
        if (callback.m_bLocked)
        {
            SteamMatchmaking.LeaveLobby((CSteamID)callback.m_ulSteamIDLobby);
            PopupManager.instance.Popup_Close();
            return;
        }

        Debug.Log($"Entered Lobby {LobbyID}");
        LobbyID = new CSteamID(callback.m_ulSteamIDLobby);


        if (NetworkServer.active)
            return;
        ((MyNetworkManager)NetworkManager.singleton).SetMultiplayer(true);

        ((MyNetworkManager)NetworkManager.singleton).networkAddress = SteamMatchmaking.GetLobbyData(new CSteamID(LobbyID.m_SteamID), HOST_ADDRESS_KEY);
        ((MyNetworkManager)NetworkManager.singleton).StartClient();
    }
    #endregion

    public void Leave()
    {
        SteamMatchmaking.LeaveLobby(LobbyID);
    }


    public static void SetLobbyLocation()
    {
        SteamNetworkingUtils.GetLocalPingLocation(out SteamNetworkPingLocation_t pingLocation);
        SteamNetworkingUtils.ConvertPingLocationToString(ref pingLocation, out string result, 1024);
        SteamMatchmaking.SetLobbyData(LobbyID, "location", result);
    }

    public void FindMatch() 
    {
        StartCoroutine(FindMatchRoutine());
    }

    IEnumerator FindMatchRoutine() 
    {
        PopupManager.instance.Popup_Show("Finding Match..");
        bool foundMatch = false;

        while (!foundMatch) 
        {
            ReloadLobbyList();
            yield return new WaitForSeconds(1);

            foreach (var lobby in allLobbies)
            {
                if (SteamMatchmaking.GetNumLobbyMembers(lobby.lobbyID) < NetworkManager.singleton.maxConnections) 
                {
                    JoinLobby(lobby.lobbyID);
                    foundMatch = true;
                }
            }
        }
        PopupManager.instance.Popup_Close();

    }
}
                   