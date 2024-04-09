using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendListManager : MonoBehaviour
{
    [SerializeField] private GameObject _steamFriendItem;
    [SerializeField] private Transform _friendListContainer;
    [SerializeField] private List<FriendItem> _friendList = new List<FriendItem>();

    private Dictionary<CSteamID, FriendItem> friendDictionary = new Dictionary<CSteamID, FriendItem>();


    [SerializeField] private float timeToRefreshList = 30f;
    float timer;

    bool initialized = false;
    private void Start()
    {

    }
    private void Update()
    {
        if (SteamManager.Initialized)
        {
            if (!initialized)
            {
                GetSteamFriends();
                initialized = true;
                return;
            }

            timer += Time.deltaTime;
            if (timer > timeToRefreshList)
            {
                GetSteamFriends();
                timer = 0;
            }
        }
    }

    private void GetSteamFriends()
    {
        if (_friendList.Count > 0)
        {
            foreach (FriendItem friend in _friendList)
            {
                Destroy(friend.gameObject);
            }
            _friendList.Clear();
        }

        int friendsCount = SteamFriends.GetFriendCount(EFriendFlags.k_EFriendFlagImmediate);
        int currentOnlineCount = 0;

        if (friendsCount == -1)
        {
            Debug.LogError("Friend count returned at -1, the user is not logged in");
            friendsCount = 0;
        }

        //ONLINE FIRST
        for (int i = 0; i < friendsCount; i++)
        {
            CSteamID friendSteamID = SteamFriends.GetFriendByIndex(i, EFriendFlags.k_EFriendFlagImmediate);
            string friendName = SteamFriends.GetFriendPersonaName(friendSteamID);
            EPersonaState friendState = SteamFriends.GetFriendPersonaState(friendSteamID);

            bool isOnline = false;

            if (friendState == EPersonaState.k_EPersonaStateOffline)
                continue;
            else
                isOnline = true;

            FriendItem friendItem = Instantiate(_steamFriendItem, _friendListContainer).GetComponent<FriendItem>();
            friendItem.InitializeFriendItem(friendName, friendSteamID.m_SteamID, isOnline);
            _friendList.Add(friendItem);
            currentOnlineCount++;
        }

        //OFFLINE LAST
        for (int i = 0; i < friendsCount; i++)
        {
            CSteamID friendSteamID = SteamFriends.GetFriendByIndex(i, EFriendFlags.k_EFriendFlagImmediate);
            string friendName = SteamFriends.GetFriendPersonaName(friendSteamID);
            EPersonaState friendState = SteamFriends.GetFriendPersonaState(friendSteamID);

            bool isOnline = false;

            if (friendState == EPersonaState.k_EPersonaStateOffline)
                isOnline = false;
            else
                continue;

            FriendItem friendItem = Instantiate(_steamFriendItem, _friendListContainer).GetComponent<FriendItem>();
            friendItem.InitializeFriendItem(friendName, friendSteamID.m_SteamID, isOnline);
            _friendList.Add(friendItem);
        }
    }
}
