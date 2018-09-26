using DarkRift;
using DarkRift.Client;
using DarkRift.Client.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPlayer : MonoBehaviour {
    // Serialized Fields
    [SerializeField] UnityClient client;

	// Use this for initialization
	void Awake() {
        client.MessageReceived += MessageReceived;
	}
    void MessageReceived(object sender, MessageReceivedEventArgs e) {
        Message message = e.GetMessage();

        if (message.Tag == Tags.MovePlayer) {
            DarkRiftReader reader = message.GetReader();
            float x = reader.ReadSingle();
            float y = reader.ReadSingle();

            transform.position = new Vector3(x, y);
        }
    }
}
