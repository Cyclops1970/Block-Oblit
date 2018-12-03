using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour {

    public GameObject blockKilledParticle;

    //What to do when something hits the killzone
    private void OnTriggerEnter(Collider other)
    {
        //scoring block
        if (other.tag == "block")
        {
            Instantiate(blockKilledParticle, other.gameObject.transform.position, Quaternion.identity);

            other.gameObject.GetComponent<Renderer>().enabled = false; //hide it from view
            Destroy(other);//remove collider and rigidbody 
            Destroy(other.gameObject.GetComponent<Rigidbody>());
            Destroy(other.gameObject,2); // destroy in x seconds to allow everything to finish before it is deleted
        }
    }
}
