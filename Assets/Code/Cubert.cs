using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cubert : MonoBehaviour {
    // Variables
    public int GoalCountHost = 0;
    public int GoalCountGuest = 0;
    public Text PointsHost;
    public Text PointsGuest;

    // Check for goals
    void OnCollisionEnter(Collision col) {
        string colname = col.gameObject.name;

        Debug.Log("Cubert has collided with " + colname + "\n");
        if (colname == "GoalHost") {
            GoalCountGuest++;

            gameObject.transform.position = new Vector3(0f, 0f, -7f);
            ResetVelocity();
        }
        else if (colname == "GoalGuest") {
            GoalCountHost++;
            
            gameObject.transform.position = new Vector3(0f, 0f, -7f);
            ResetVelocity();
        }
    }

    void ResetVelocity() {
        float sx = Random.Range(0, 2) == 0 ? -1 : 1;
        float sy = Random.Range(0, 2) == 0 ? -1 : 1;

        GetComponent<Rigidbody>().velocity = new Vector3(5f * sx, 5f * sy, 0f);
    }

	// Use this for initialization
	void Start () {
        ResetVelocity();
    }
	
	// Update is called once per frame
	void Update () {
        PointsHost.text = GoalCountHost.ToString();
        PointsGuest.text = GoalCountGuest.ToString();
    }
}