using Mirror;
using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterSkinHandler : MonoBehaviour
{
    public static CharacterSkinHandler instance;

    [SerializeField] private GameObject characterSkinPrefab;
    [SerializeField] private Transform[] spawnPositions;
    public CharacterSkinElement[] clientsCharacters;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        clientsCharacters = new CharacterSkinElement[NetworkManager.singleton.maxConnections];

        StartCoroutine(WaitTillSteamInitialized());
    }

    IEnumerator WaitTillSteamInitialized() 
    {
        while (!SteamManager.Initialized)
            yield return new WaitForEndOfFrame();

        SpawnCharacterMesh(null);
    }
    public void SpawnCharacterMesh(MyClient client) 
    {
        int index = GetNextPlatformIndex(client);

        if(client && client.isLocalPlayer)
        {
            client.characterInstance = clientsCharacters[0];
            client.characterInstance.Initialize(client, client.IsReady);

            return;
        }

        clientsCharacters[index] = Instantiate(characterSkinPrefab, spawnPositions[index]).GetComponent<CharacterSkinElement>();
        clientsCharacters[index].Initialize(client, client ? client.IsReady : false);


        if (client)
            client.characterInstance = clientsCharacters[index];
    }

    public int GetNextPlatformIndex(MyClient client)
    {
        if (client == null || client.isLocalPlayer)
            return 0;

        for (int i = 0; i < clientsCharacters.Length; i++)
        {
            if(clientsCharacters[i] == null)
                return i;
        }
        return 0;
    }

    public void DestroyCharacterMesh(MyClient client) 
    {
        foreach (var character in clientsCharacters) 
        {
            if(character == null)
                continue;

            if (character.client == client)
            {
                if (character == clientsCharacters[0]) 
                {
                    clientsCharacters[0].Initialize(client, false);
                    MainMenu.instance.UpdateReadyButton(false);
                }
                else
                    Destroy(character.gameObject);

                break;
            }
        }
    }
}
