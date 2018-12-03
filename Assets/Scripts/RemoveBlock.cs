using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveBlock : MonoBehaviour {

    public GameObject removeBlockParticle;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Instantiate(removeBlockParticle, gameObject.transform.position, Quaternion.identity);

            //take the bomb out of play
            gameObject.GetComponent<Renderer>().enabled = false; //hide it from view
            Destroy(gameObject.GetComponent<Collider>());//remove collider and rigidbody 
            Destroy(gameObject.GetComponent<Rigidbody>());
            Destroy(gameObject, 1   ); // destroy in x seconds to allow everything to finish before it is deleted
        }
    }
}
