using DarkRift;
using DarkRift.Client.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    // Serialized Fields
    [SerializeField] public UnityClient client;
    [SerializeField] float moveDistance = 0.05f;

    // Fields
    Vector3 lastPosition;

    // Events
	void Awake() {
		
	}
	void Update() {
		// Check to see if we need to send an update
        if (Vector3.Distance(lastPosition, transform.position) > moveDistance) {
            // Create message to send to server
            DarkRiftWriter writer = DarkRiftWriter.Create();
            writer.Write(transform.position.x);
            writer.Write(transform.position.y);

            // Send message to server
            Message message = Message.Create(Tags.MovePlayer, writer);
            client.SendMessage(message, SendMode.Unreliable);

            // Save current position for later checks
            lastPosition = transform.position;
        }
	}
}
