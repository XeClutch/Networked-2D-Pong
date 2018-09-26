using DarkRift;
using DarkRift.Client.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bumpers : MonoBehaviour {
    // Fields
    public float speed = 5f;

	// Events
	void Start() {
		
	}
	void Update() {
        // Move bumper
        transform.Translate(0f, Input.GetAxis("Vertical") * speed * Time.deltaTime, 0f);
	}
}