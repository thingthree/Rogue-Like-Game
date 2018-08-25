using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    // although there is no heirarchy for calling the camera as of yet,
    // the camera will rudimentarily follow players currently based
    // on setting the camera's target via the GameManager's player cycle 
    //variable, which is neat

    // right now I just use a rigidbody and addforce to move the camera, because
    // I'm not sure what a good function is to get the camera to follow the player
    // and look pretty smooth
    public static GameObject cameraTarget = null;

    public void SmoothFollow(Transform follower, Transform followed)
    {
        Debug.Log("MovementManager: SmoothFollow - follower = " + follower + " & followed = " + followed);

        float speed = 2f;
        float interpolation = speed * Time.deltaTime;
        if (Mathf.Abs(follower.position.x - followed.position.x) < .1 && Mathf.Abs(follower.position.y - followed.position.y) < .1)
        {
            return;
        }
        else
        {
            Vector3 position = follower.position;
            position.x = Mathf.Lerp(follower.position.x, followed.position.x, interpolation);
            position.y = Mathf.Lerp(follower.position.y, followed.position.y, interpolation);
            follower.position = position;
        }

       
    }
    public void Teleport(GameObject toTeleport, Transform endLocation)
    {
        toTeleport.transform.position = new Vector3(endLocation.transform.position.x, endLocation.transform.position.y);
    }
}