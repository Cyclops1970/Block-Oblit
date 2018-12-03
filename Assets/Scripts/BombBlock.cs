using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBlock : MonoBehaviour {

    public GameObject bombParticle;
    
    float power = 5000;
    float radius = 5;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag=="Player")
        {
            Vector3 explosionPos = transform.position;
            //get the colliders within the bomb radius
            Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
            foreach(Collider hit in colliders)
            {
                Rigidbody rb = hit.GetComponent<Rigidbody>();

                if (rb != null)
                {
                    rb.AddExplosionForce(power, explosionPos, radius, 3.0F, ForceMode.Force);
                }
            }

            Instantiate(bombParticle, gameObject.transform.position, Quaternion.identity);

            //take the bomb out of play
            gameObject.GetComponent<Renderer>().enabled = false; //hide it from view
            Destroy(gameObject.GetComponent<Collider>());//remove collider and rigidbody 
            Destroy(gameObject.GetComponent<Rigidbody>());
            Destroy(gameObject, 3); // destroy in x seconds to allow everything to finish before it is deleted
        }
    }
}
