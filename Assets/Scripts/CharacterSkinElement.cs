using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterSkinElement : MonoBehaviour
{
     public Transform nametagPos;

    public MyClient client { get; private set; }
    public CSteamID steamId { get; private set; }
    public NametagMarker nametagMarker { get; set; }

    bool initialized = false;
    Sprite icon;
    protected Callback<AvatarImageLoaded_t> avatarImageLoaded;

    private void OnAvatarImageLoaded(AvatarImageLoaded_t callback)
    {
        if (callback.m_steamID != steamId) return;

        Texture2D tex = SteamHelper.GetAvatar(steamId);
        if(tex)
            icon = SteamHelper.ConvertTextureToSprite(tex);
        nametagMarker.UpdatePFP(icon);
    }

    public void Initialize(MyClient client, bool _isReady) 
    {
        string username = SteamFriends.GetPersonaName();
        bool isReady = _isReady;

        steamId = client ? new CSteamID(client.playerInfo.steamId) : SteamUser.GetSteamID();

        if (nametagMarker == null)
            nametagMarker = (NametagMarker)MarkerHandler.instance.SpawnMarker(0, nametagPos.position, null);

        if (client != null)
        {
            this.client = client;
            username = client.playerInfo.username;
            icon = client.icon;
        }

        if (!initialized) 
        {
            initialized = true;
            avatarImageLoaded = Callback<AvatarImageLoaded_t>.Create(OnAvatarImageLoaded);

            Texture2D tex = SteamHelper.GetAvatar(steamId);
            if (tex)
                icon = SteamHelper.ConvertTextureToSprite(tex); nametagMarker.UpdatePFP(icon);        
        }

        nametagMarker.UpdateTag(username, isReady);
        nametagMarker.UpdatePFP(icon);
    }

    private void OnDestroy()
    {
        if (nametagMarker)
            nametagMarker.DestroyMarker();
    }

}
