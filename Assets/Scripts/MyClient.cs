using Mirror;
using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct PlayerInfoData 
{
    public string username;
    public ulong steamId;

    public PlayerInfoData(string username, ulong steamId)
    {
        this.username = username;
        this.steamId = steamId;
    }
}
public class MyClient : NetworkBehaviour
{
    [SyncVar(hook = nameof(PlayerInfoUpdate))]
    public PlayerInfoData playerInfo;

    [SyncVar(hook = nameof(IsReadyUpdate))]
    public bool IsReady;

    [Header("Controller")]
    [SerializeField] private GameObject controllerObj;
    [SerializeField] private GameObject meshObj;
    [SerializeField] private GameObject camHolder;
    [SerializeField] private Behaviour[] controllerComponents;

    public Sprite icon { get; private set; }
    public CharacterSkinElement characterInstance { get; set; }

    #region Steam PFP
    protected Callback<AvatarImageLoaded_t> avatarImageLoaded;
    private void OnAvatarImageLoaded(AvatarImageLoaded_t callback)
    {
        Debug.Log("Avatar loaded " + callback.m_steamID);
        if (callback.m_steamID.m_SteamID != playerInfo.steamId) return;
        SetIcon(callback.m_steamID);
    }

    void SetIcon(CSteamID steamId)
    {
        Texture2D tex = SteamHelper.GetAvatar(steamId);
        if (tex)
            icon = SteamHelper.ConvertTextureToSprite(tex);
    }
    #endregion

    private void Start()
    {
        ((MyNetworkManager)NetworkManager.singleton).allClients.Add(this);

        if(CharacterSkinHandler.instance) CharacterSkinHandler.instance.SpawnCharacterMesh(this);
        avatarImageLoaded = Callback<AvatarImageLoaded_t>.Create(OnAvatarImageLoaded);

        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            ToggleController(true);
        }
        else
        { 
            ToggleController(false);
        }
    }

    void ToggleController(bool value) 
    {
        controllerObj.SetActive(value);
        meshObj.SetActive(value ? !isLocalPlayer : false);
        camHolder.SetActive(value ? isLocalPlayer : false);

        if (!isLocalPlayer) 
            value = false;

        
        foreach (var component in controllerComponents)
        {
            component.enabled = value;
        }

        GetComponent<CharacterController>().enabled = value;
    }

    #region Ready Up
    public void ToggleReady() => Cmd_ToggleReady();

    [Command]
    private void Cmd_ToggleReady() 
    {
        IsReady = !IsReady;
    }
    #endregion

    #region SyncVar Hooks
    private void PlayerInfoUpdate(PlayerInfoData _, PlayerInfoData data)
    {
        if (characterInstance)
            characterInstance.Initialize(this, IsReady);

            SetIcon(new CSteamID(data.steamId));
    }

    public void IsReadyUpdate(bool _, bool value) 
    {
        if (characterInstance)
            characterInstance.Initialize(this, value);
        if (isLocalPlayer) 
        {
            MainMenu.instance.UpdateReadyButton(value);
        }
    }

    #endregion



    private void OnDestroy()
    {
        if (this && ((MyNetworkManager)NetworkManager.singleton))
            ((MyNetworkManager)NetworkManager.singleton).allClients.Remove(this);

        if (characterInstance && !isLocalPlayer)
        {
            CharacterSkinHandler.instance.DestroyCharacterMesh(this);
        }
    }
}
