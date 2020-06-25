using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.tag == "Robot") {
            Robot robot = collider.GetComponent<Robot>();
            robot.GoalReached = true;
        }
    }
}
