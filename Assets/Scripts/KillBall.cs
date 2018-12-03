using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBall : MonoBehaviour
{
    //If the ball goes out of bounds, kill it
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="killBall")
        {
            Destroy(gameObject);
        }
    }

}
