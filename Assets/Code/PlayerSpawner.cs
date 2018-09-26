using DarkRift;
using DarkRift.Client;
using DarkRift.Client.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour {
    // Serialized Fields
    [SerializeField] public UnityClient client;
    [SerializeField] GameObject controllablePrefab;
    [SerializeField] GameObject networkPrefab;

    // Methods
    void Awake() {
        if (client == null) {
            Debug.LogError("Client unassigned in PlayerSpawner.cs");
            Application.Quit();
        }

        if (controllablePrefab == null) {
            Debug.LogError("Controllable Prefab unassigned in PlayerSpawner.cs");
            Application.Quit();
        }

        if (networkPrefab == null) {
            Debug.LogError("Network Prefab unassigned in PlayerSpawner.cs");
            Application.Quit();
        }

        client.MessageReceived += ReceiveMessage;
    }

    // Events
    void ReceiveMessage(object sender, MessageReceivedEventArgs e) {
        // Store message
        Message message = e.GetMessage();
        DarkRiftReader reader = message.GetReader();

        // Filter through tags
        switch (message.Tag) {
            case Tags.SpawnPlayer:
                SpawnPlayer(reader);
                break;

        }
    }
    void SpawnPlayer(DarkRiftReader reader) {
        while (reader.Position < reader.Length) {
            // Read packet
            ushort id = reader.ReadUInt16();
            Vector3 position = new Vector3(reader.ReadSingle(), reader.ReadSingle());

            // Spawn player
            GameObject obj;
            if (id == client.ID) {
                obj = Instantiate(controllablePrefab, position, Quaternion.identity) as GameObject;

                Player player = obj.GetComponent<Player>();
                player.client = client;
            }
            else
                obj = Instantiate(networkPrefab, position, Quaternion.identity) as GameObject;
        }
    }
}