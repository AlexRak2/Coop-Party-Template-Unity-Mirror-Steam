using Steamworks;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FriendItem : MonoBehaviour
{
    [SerializeField] private Image _iconImage;
    [SerializeField] private Image _statusImage;
    [SerializeField] private TMP_Text _usernameText;

/*    [SerializeField] private Color onlineColor;
    [SerializeField] private Color offlineColor;*/
    private string username;
    private CSteamID steamID;
    private bool isOnline;

    private void Start()
    {
        avatarImageLoaded = Callback<AvatarImageLoaded_t>.Create(OnAvatarImageLoaded);
    }
    public void InitializeFriendItem(string name, ulong id, bool status)
    {
        username = name;
        steamID = new CSteamID(id);
        isOnline = status;

        _usernameText.text = username;
        //_statusImage.color = isOnline ? onlineColor : offlineColor;

        //_iconImage.sprite = SteamHelper.GetAvatar(steamID);
        GetIcon();

    }

    public void InviteFriend()
    {
        SteamMatchmaking.InviteUserToLobby(SteamLobby.LobbyID, steamID);
    }


    Sprite icon;
    protected Callback<AvatarImageLoaded_t> avatarImageLoaded;

    private void OnAvatarImageLoaded(AvatarImageLoaded_t callback)
    {
        if (callback.m_steamID != steamID) return;
        GetIcon();
    }

    void GetIcon() 
    {
        Texture2D tex = SteamHelper.GetAvatar(steamID);
        if (tex)
        {
            icon = SteamHelper.ConvertTextureToSprite(tex);
            _iconImage.sprite = icon;
        }
    }
}
