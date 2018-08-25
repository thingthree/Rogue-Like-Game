using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollision : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision Enter");
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("Stay Occcuring");
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Collision Exit");
    }
}
